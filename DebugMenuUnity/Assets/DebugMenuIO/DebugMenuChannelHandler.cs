#nullable enable
using DebugMenuIO.Schema;
using Newtonsoft.Json.Linq;

namespace DebugMenu {
    public abstract class DebugMenuChannelHandler {
        public string Channel { get; }
        public abstract string Type { get; }

        protected DebugMenuChannelHandler(string channel) {
            Channel = channel;
        }

        public abstract Channel GetSchema();
        public abstract object? HandleMessage(JObject payload);
    }
}
