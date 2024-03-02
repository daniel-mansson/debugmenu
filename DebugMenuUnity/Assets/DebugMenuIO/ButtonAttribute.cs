using System;

#nullable enable

namespace DebugMenuIO {
    public class DebugMenuChannelAttribute : Attribute {
        public string? Name { get; set; } = null;
        public string? Path { get; set; } = null;
    }

    public class ControllerAttribute : Attribute {
        public string? Path { get; set; } = null;
    }

    public class ToggleAttribute : DebugMenuChannelAttribute {
    }

    public class ButtonAttribute : DebugMenuChannelAttribute {
    }
}
