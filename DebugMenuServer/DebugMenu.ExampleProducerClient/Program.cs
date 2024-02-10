// See https://aka.ms/new-console-template for more information

using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using DebugMenu.ExampleProducerClient;
using DebugMenu.ProducerClientSdk;
using DebugMenu.Silo.Web.RunningInstances;
using DebugMenu.Silo.Web.RunningInstances.Requests.CreateRunningInstance;

Console.WriteLine("Hello, World!");

var id = "8c57f858-7d30-4b34-b32c-df88e677a267";
// var producer = new Producer("https://localhost:8082", "hej");
//
// //var success = await producer.Start(new Dictionary<string, string>());
// var success = await producer.Rejoin($"wss://localhost:8082/ws/room/{id}", id);
//
// Console.WriteLine(success);
//
// var controller = new Controller($"wss://localhost:8082/ws/room/{producer.SessionId}/controller", "hej");

var token = "6FE3EA8B0F8E39FCBE6B95EA17D487A687BB57E0DA6591FD";
var metadata = new Dictionary<string, string>();

var url = "https://localhost:8082";
var client = new HttpClient();
var body = JsonContent.Create(new CreateRunningInstanceRequest() {
    Token = token,
    Metadata = metadata
});
var response = await client.PostAsync(new Uri(url + "/api/instances"), body);

JsonSerializerOptions options = new () {
    PropertyNameCaseInsensitive = true
};
var responseJson = await response.Content.ReadAsStringAsync();
var instance = JsonSerializer.Deserialize<RunningInstanceDto>(responseJson, options)!;

var producerHandler = new DebugMenuWebSocketClient(instance.WebsocketUrl!+"/instance", token, metadata);
producerHandler.ReceivedJson += message => {
    Console.WriteLine($"Producer received ({message.channel}): {message.payload.ToString()}");
};

producerHandler.ConnectedAfterHandshake += SendApi;

// var controllerHandler = new DebugMenuWebSocketClient($"wss://localhost:8082/ws/room/{id}/controller");
// controllerHandler.ReceivedJson += message => {
//     Console.WriteLine($"Controller received ({message.channel}): {message.payload.ToString()}");
// };

var random = new Random();
var delayMax = 5000;
while (true) {
    try {
        // Console.WriteLine("Send from producer: ");
        // var line = Console.ReadLine();
        // await producerHandler.SendJson("hello", new {
        //     data = line
        // }, CancellationToken.None);
        //
        // Console.WriteLine("Send from controller: ");
        // line = Console.ReadLine();
        // await controllerHandler.SendJson("test", new {
        //     data = line
        // }, CancellationToken.None);

        var log = FakeLogGenerator.Generate();
        await producerHandler.SendJson("log", new
        {
            text = log.text,
            details = log.details,
            type = log.type
        }, CancellationToken.None);

        await Task.Delay(random.Next() % delayMax + 10);

        if (Console.KeyAvailable) {
            var key = Console.ReadKey();
            delayMax = key.KeyChar switch {
                '1' => 10,
                '2' => 100,
                '3' => 1000,
                '4' => 5000,
                _ => delayMax
            };
            switch (key.KeyChar) {
              case 'a':
                SendApi();
                break;
            }

            Console.WriteLine($"Delay max set to {delayMax}");
        }
    }
    catch (Exception e) {
        Console.WriteLine($"boom {e.Message}");
    }
}

const string api1 = """"
  {
  "asyncapi": "2.6.0",
"info": {
  "title": "Example AsyncAPI specification",
  "version": "0.1.0"
},
"channels": {
  "log": {
    "subscribe": {
      "tags": [
      {
        "name": "log"
      }
      ],
      "message": {
        "payload": {
          "type": "object",
          "properties": {
            "text": {
              "type": "string"
            },
            "details": {
              "type": "string"
            },
            "type": {
              "type": "string"
            }
          }
        }
      }
    }
  },
  "gameplay/spawn": {
    "publish": {
      "description": "asdf",
      "tags": [
      {
        "name": "button"
      }
      ],
      "message": {
        "payload": {
          "type": "object",
          "properties": {
            "exampleField": {
              "type": "string",
              "description": "This is an example text field"
            },
            "exampleNumber": {
              "type": "number"
            },
            "exampleDate": {
              "type": "string",
              "format": "date-time"
            }
          }
        }
      }
    }
  },
  "gameplay/restart": {
    "publish": {
      "tags": [
      {
        "name": "button"
      }
      ]
    }
  },
  "progression/level-up": {
    "publish": {
      "tags": [
      {
        "name": "button"
      }
      ]
    }
  },
  "progression/reset": {
    "publish": {
      "tags": [
      {
        "name": "button"
      }
      ]
    }
  },
  "progression/add-xp": {
    "publish": {
      "tags": [
      {
        "name": "button"
      }
      ]
    }
  }
}
}
"""";

Task SendApi() {
  return producerHandler.SendBytes("__internal/api",
    Encoding.UTF8.GetBytes(api1),
    CancellationToken.None);
}
