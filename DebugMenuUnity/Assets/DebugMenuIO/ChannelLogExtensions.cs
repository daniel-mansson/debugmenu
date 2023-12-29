#nullable enable
using System;
using UnityEditor.PackageManager;

public static class ChannelLogExtensions {

    [Serializable]
    class LogMessage {
        public string text = String.Empty;
        public string level = "info";
        public string details = String.Empty;
    }
    
    public static void SendLog(this IChannel channel, string text, LogLevel logLevel, string details) {
        channel.SendJson(new LogMessage() {
            text = text,
            details = details,
            level = logLevel.ToString().ToLowerInvariant()
        });
    }
}