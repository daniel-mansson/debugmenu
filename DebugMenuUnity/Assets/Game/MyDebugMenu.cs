using System.Collections.Generic;
using System.Threading;
using DebugMenu;
using UnityEngine;

namespace Game {
    [DebugMenuIO.Controller]
    public class MyDebugMenu : MonoBehaviour {
        [SerializeField] private string token;
        [SerializeField] private string url;

        private DebugMenuClient _debugMenuClient;

        private async void Start() {
            _debugMenuClient = new DebugMenuClient(url, token, new Dictionary<string, string>());
            var instance = await _debugMenuClient.Run(CancellationToken.None);

            _debugMenuClient.RegisterController(this);
            _debugMenuClient.RegisterHandler(Reset);
            _debugMenuClient.RegisterHandler<int>(SetGold);
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
    }
}
