using System;
using System.Collections.Generic;
using System.Threading;
using DebugMenu;
using DebugMenuIO.AsyncApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Serialization;
using Channel = DebugMenuIO.AsyncApi.Channel;
using Random = UnityEngine.Random;

namespace Game {
    [DebugMenuIO.Controller]
    public class MyDebugMenu : MonoBehaviour {
        [SerializeField] private string token;
        [SerializeField] private string url;

        public Rigidbody body;
        public Material[] materials;

        [Header("Editor only")] [SerializeField]
        private bool reuseInstance;

        private DebugMenuClient _debugMenuClient;


        private async void Start() {
            _debugMenuClient = new DebugMenuClient(url, token, new Dictionary<string, string>());

            DebugMenuClient.RunningInstance instance = null;
#if UNITY_EDITOR
            if(reuseInstance) {
                instance = JsonConvert.DeserializeObject<DebugMenuClient.RunningInstance>(
                    UnityEditor.EditorPrefs.GetString("DebugMenuInstance", ""));
            }
#endif
            instance = await _debugMenuClient.Run(CancellationToken.None, instance);
#if UNITY_EDITOR
            UnityEditor.EditorPrefs.SetString("DebugMenuInstance", JsonConvert.SerializeObject(instance));
#endif
            _debugMenuClient.RegisterController(this);

            body.GetComponent<MeshRenderer>().material = materials[0];

            Application.logMessageReceivedThreaded += OnLogMessage;
            _debugMenuClient.AddExplicitSchema("log/unity", new DebugMenuIO.Schema.Channel() {
                Name = "Unity Log",
                Category = "Logs",
                Type = "log",
                Subscribe = new DebugMenuIO.Schema.Payload() {
                    Type = "object",
                    Properties = new Dictionary<string, DebugMenuIO.Schema.Property> {
                        {
                            "timestamp", new DebugMenuIO.Schema.Property {
                                Type = "long"
                            }
                        }, {
                            "text", new DebugMenuIO.Schema.Property {
                                Type = "string"
                            }
                        }, {
                            "details", new DebugMenuIO.Schema.Property {
                                Type = "string"
                            }
                        }, {
                            "type", new DebugMenuIO.Schema.Property {
                                Type = "string"
                            }
                        }
                    }
                }
            });
        }

        private void OnLogMessage(string message, string stacktrace, LogType type) {
            _debugMenuClient.SendLog("log/unity", message, type.ToString().ToLower(), stacktrace,
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }

        private void OnDestroy() {
            _debugMenuClient?.Dispose();
            _debugMenuClient = null;
        }

        [DebugMenuIO.Button]
        public void Reset() {
            Debug.Log("Reset was called");
            body.position = Vector3.up * 2;
            body.rotation = Quaternion.identity;
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
        }

        [DebugMenuIO.Button]
        public void Push() {
            Debug.Log("Push");
            body.AddForce((Vector3.up * 7f + Random.insideUnitSphere) * Random.Range(1f, 3f) - body.position,
                ForceMode.Impulse);
            body.AddRelativeTorque(Random.insideUnitSphere * Random.Range(1f, 2f), ForceMode.Impulse);
        }

        [DebugMenuIO.Button]
        public void SetGold(int gold) {
            Debug.Log($"Set gold to {gold}");
        }

        [DebugMenuIO.Toggle]
        public bool DoubleXp(bool value) {
            Debug.Log($"Set DoubleXp to {value}");
            body.GetComponent<MeshRenderer>().material = materials[value ? 1 : 0];
            return value;
        }

        [DebugMenuIO.TextField(MaxLength = 13)]
        public string Name(string name) {
            Debug.Log($"Set Name to {name}");
            return name.Substring(0, Mathf.Min(3, name.Length));
        }

        [DebugMenuIO.TextField]
        public void SomeText(string value) {
            Debug.Log($"Set SomeText to {value}");
        }

        [DebugMenuIO.Button]
        public void SetSomething(string id, int value) {
            Debug.Log($"Set {id} => {value}");
        }

        private void OnGUI() {
            if(GUILayout.Button("log")) {
                Debug.Log("here is a log message", gameObject);
            }

            if(GUILayout.Button("warning")) {
                Debug.LogWarning("here is a warning message", gameObject);
            }

            if(GUILayout.Button("error")) {
                Debug.LogError("here is an error message", gameObject);
            }

            if(GUILayout.Button("exception")) {
                var ex = new NotFiniteNumberException("ex message");
                Debug.LogException(ex, gameObject);
            }

            if(GUILayout.Button("assertion")) {
                Debug.LogAssertion("here is an assertion message", gameObject);
            }
        }

        [ContextMenu("json")]
        private void TestJson() {
            var doc = new Document() {
                Asyncapi = "2.6.0",
                Info = new Info {
                    Title = "Test",
                    Version = "1.0.0"
                },
                Channels = new Dictionary<string, Channel> {
                    {
                        "progression/addXp", new Channel {
                            Publish = new Publish {
                                Tags = new List<Tag> {
                                    new() {
                                        Name = "button"
                                    }
                                },
                                Message = new Message {
                                    Payload = new Payload {
                                        Type = "object",
                                        Properties = new Dictionary<string, Property> {
                                            {
                                                "xp", new Property() {
                                                    Type = "number"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var settings = new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            var json = JsonConvert.SerializeObject(doc, settings);
            Debug.Log(json);
        }
    }
}
