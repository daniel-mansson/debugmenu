using System.Diagnostics;
using System.Numerics;
using System.Text.Json;

namespace DebugMenu.ExampleProducerClient;

public class Outside {
    public Outside(TestController c) {

        c.Teleport += v => {
            Console.WriteLine(v);
        };
    }
}

public class TestController  {
    public event Action<Vector3>? Teleport;
    
    public void Log(string text) {
        throw new NotImplementedException();
    }
}


public class Message {
    public string channel;
    public JsonElement payload;
}

public class Raw {


    public Raw() {
        
    }

    public void OnMessageReceived(Message message) {

    }
    
    
}

public class Consumer {
    public event Action<Message> MessageReceived;

    public void SendMessage(Message message) {
        
    }
}