#nullable enable
using System.Linq;
using System.Reflection;
using DebugMenuIO;
using DebugMenuIO.Schema;
using Newtonsoft.Json.Linq;

namespace DebugMenu {
    public class TextFieldDebugMenuChannelHandler : DebugMenuChannelHandler {
        public override string Type => "text-field";

        private readonly object _instance;
        private readonly MethodInfo _methodInfo;
        private readonly TextFieldAttribute _attribute;

        public TextFieldDebugMenuChannelHandler(string channel, object instance, MethodInfo methodInfo,
            TextFieldAttribute attribute) : base(channel) {
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
                                Type = "string",
                                MaxLength = _attribute.MaxLength
                            }
                        }
                    }
                },
                Subscribe = new Payload() {
                    Type = "object",
                    Properties = new() {
                        {
                            "value", new() {
                                Type = "string",
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

            var value = token.Value<string>();

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
