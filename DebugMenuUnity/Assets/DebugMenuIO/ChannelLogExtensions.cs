#nullable enable
using System;

public static class ChannelLogExtensions {
    [Serializable]
    private class LogMessage {
        public string text = string.Empty;
        public string level = "info";
        public string details = string.Empty;
    }

    public static void SendLog(this IChannel channel, string text, string logLevel, string details) {
        channel.SendJson(new LogMessage() {
            text = text,
            details = details,
            level = logLevel.ToString().ToLowerInvariant()
        });
    }
}
