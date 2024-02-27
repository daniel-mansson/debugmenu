using System.Collections.Generic;


namespace DebugMenuIO.Schema {
    public class Api {
        public string DebugMenuApi { get; set; }
        public Dictionary<string, Channel> Channels { get; set; } = new();
    }

    public class Channel {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public Payload Subscribe { get; set; }
        public Payload Publish { get; set; }
    }

    public class Payload {
        public string Type { get; set; }
        public Dictionary<string, Property> Properties { get; set; }
    }

    public class Property {
        public string Type { get; set; }
        public string Format { get; set; }
        public string Description { get; set; }
    }
}
