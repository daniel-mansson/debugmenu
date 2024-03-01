#nullable enable
using System;
using System.Reflection;
using DebugMenuIO;
using DebugMenuIO.Schema;
using Newtonsoft.Json.Linq;

namespace DebugMenu {
    public abstract class DebugMenuChannelHandler {
        public string Channel { get; }
        public abstract string Type { get; }

        protected DebugMenuChannelHandler(string channel) {
            Channel = channel;
        }

        public abstract bool Register(object controller, Type type);
        public abstract object HandleMessage(JObject payload);
    }

    public class ButtonDebugMenuChannelHandler : DebugMenuChannelHandler {
        public override string Type => "button";

        public ButtonDebugMenuChannelHandler(string channel, object instance, MethodInfo methodInfo,
            ButtonAttribute attribute) : base(channel) {
        }

        public Channel GetSchema() {
            return new Channel();
        }

        public override bool Register(object controller, Type type) {
            throw new NotImplementedException();
        }

        public override object HandleMessage(JObject payload) {
            throw new NotImplementedException();
        }
    }
}
