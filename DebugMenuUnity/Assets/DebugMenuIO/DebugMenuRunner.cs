using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;

namespace DebugMenu {
    public class DebugMenuRunner : MonoBehaviour {
        [SerializeField] private string token;
        [SerializeField] private string url;

        private DebugMenuClient _debugMenuClient;

        private async void Start() {
            _debugMenuClient = new DebugMenuClient(url, token, new Dictionary<string, string>());

            Debug.Log("Starting debug menu");

            var instance = await _debugMenuClient.Run(CancellationToken.None);

            Debug.Log(JsonConvert.SerializeObject(instance));
        }


        private void OnDestroy() {
            _debugMenuClient?.Dispose();
            _debugMenuClient = null;
        }
    }
}
