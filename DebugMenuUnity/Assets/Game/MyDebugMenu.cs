using System.Collections.Generic;
using System.Threading;
using DebugMenu;
using DebugMenuIO.AsyncApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Serialization;
using Channel = DebugMenuIO.AsyncApi.Channel;

namespace Game {
    [DebugMenuIO.Controller]
    public class MyDebugMenu : MonoBehaviour {
        [SerializeField] private string token;
        [SerializeField] private string url;
        [Header("Editor only")]
        [SerializeField] private bool reuseInstance;

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
        }

        private void OnDestroy() {
            _debugMenuClient?.Dispose();
            _debugMenuClient = null;
        }

        [DebugMenuIO.Button]
        public void Reset() {
            Debug.Log("Reset was called");
        }

        [DebugMenuIO.Button]
        public void SetGold(int gold) {
            Debug.Log($"Set gold to {gold}");
        }

        [DebugMenuIO.Button]
        public void SetSomething(string id, int value) {
            Debug.Log($"Set {id} => {value}");
        }

        [ContextMenu("json")]
        void TestJson() {
            var doc = new Document() {
                Asyncapi = "2.6.0",
                Info = new() {
                    Title = "Test",
                    Version = "1.0.0"
                },
                Channels = new() {
                    {
                        "progression/addXp", new() {
                            Publish = new() {
                                Tags = new() {
                                    new Tag() {
                                        Name = "button"
                                    }
                                },
                                Message = new() {
                                    Payload = new() {
                                        Type = "object",
                                        Properties = new() {
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
