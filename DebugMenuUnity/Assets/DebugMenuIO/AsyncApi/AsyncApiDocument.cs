using System.Collections.Generic;

namespace DebugMenuIO.AsyncApi {
    public class Document {
        public string Asyncapi { get; set; }
        public Info Info { get; set; }
        public Dictionary<string, Channel> Channels { get; set; }
    }

    public class Info {
        public string Title { get; set; }
        public string Version { get; set; }
    }

    public class Channel {
        public Subscribe Subscribe { get; set; }
        public Publish Publish { get; set; }
    }

    public class Subscribe {
        public string Description { get; set; }
        public List<Tag> Tags { get; set; }
        public Message Message { get; set; }
    }

    public class Message {
        public Payload Payload { get; set; }
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

    public class Publish {
        public string Description { get; set; }
        public List<Tag> Tags { get; set; }
        public Message Message { get; set; }
    }

    public class Tag {
        public string Name { get; set; }
    }
}
