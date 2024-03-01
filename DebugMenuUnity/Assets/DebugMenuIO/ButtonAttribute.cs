using System;

#nullable enable

namespace DebugMenuIO {
    public class ButtonAttribute : Attribute {
        public string? Path { get; set; } = null;
    }

    public class ControllerAttribute : Attribute {
        public string? Path { get; set; } = null;
    }

    public class ToggleAttribute : Attribute {
        public string? Path { get; set; } = null;
    }
}
