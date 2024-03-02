#nullable enable
using System.Linq;
using System.Reflection;
using DebugMenuIO;
using DebugMenuIO.Schema;
using Newtonsoft.Json.Linq;

namespace DebugMenu {
    public class ButtonDebugMenuChannelHandler : DebugMenuChannelHandler {
        public override string Type => "button";

        private readonly object _instance;
        private readonly MethodInfo _methodInfo;
        private readonly ButtonAttribute _attribute;

        public ButtonDebugMenuChannelHandler(string channel, object instance, MethodInfo methodInfo,
            ButtonAttribute attribute) : base(channel) {
            _instance = instance;
            _methodInfo = methodInfo;
            _attribute = attribute;
        }

        public override Channel GetSchema() {
            var parameters = _methodInfo.GetParameters();
            return new Channel() {
                Type = Type,
                Name = _attribute.Name,
                Publish = new Payload() {
                    Type = "object",
                    Properties = parameters.Select(p => {
                        return (p.Name, new Property() {
                            Type = DebugMenuUtils.GetPropertyType(p.ParameterType)
                        });
                    }).ToDictionary(p => p.Name, p => p.Item2)
                }
            };
        }

        public override object? HandleMessage(JObject payload) {
            var parameters = _methodInfo.GetParameters()
                .Select(p => DebugMenuUtils.ToValue(payload, p))
                .ToArray();

            var returnValue = _methodInfo.Invoke(_instance, parameters);
            if(returnValue != null) {
                return new { value = returnValue };
            }

            return null;
        }
    }
}
