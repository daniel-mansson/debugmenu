#nullable enable
using System.Linq;
using System.Reflection;
using DebugMenuIO;
using DebugMenuIO.Schema;
using Newtonsoft.Json.Linq;

namespace DebugMenu {
    public class ToggleDebugMenuChannelHandler : DebugMenuChannelHandler {
        public override string Type => "toggle";

        private readonly object _instance;
        private readonly MethodInfo _methodInfo;
        private readonly ToggleAttribute _attribute;

        public ToggleDebugMenuChannelHandler(string channel, object instance, MethodInfo methodInfo,
            ToggleAttribute attribute) : base(channel) {
            _instance = instance;
            _methodInfo = methodInfo;
            _attribute = attribute;
        }

        public override Channel GetSchema() {
            return new Channel() {
                Type = Type,
                Name = _attribute.Name,
                Publish = new Payload() {
                    Type = "object",
                    Properties = new() {
                        {
                            "value", new() {
                                Type = "boolean",
                            }
                        }
                    }
                },
                Subscribe = new Payload() {
                    Type = "object",
                    Properties = new() {
                        {
                            "value", new() {
                                Type = "boolean",
                            }
                        }
                    }
                }
            };
        }

        public override object? HandleMessage(JObject payload) {
            if(!payload.TryGetValue("value", out var token)) {
                return null;
            }

            bool value = token.Value<bool>();

            var parameters = _methodInfo.GetParameters()
                .Select(p => (object)value)
                .ToArray();

            var returnValue = _methodInfo.Invoke(_instance, parameters);
            if(returnValue != null) {
                return new { value = returnValue };
            }

            return null;
        }
    }
}
