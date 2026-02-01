using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using MonkeyJam.Entities;


namespace MonkeyJam.Managers {
    public class SceneHandler : MonoBehaviour {
        [SerializeField] private Player _player;
        [SerializeField] private Transform _spawnLocation;
        [SerializeField] private Object _mainMenuScene;
        [SerializeField, Tooltip("Only set this true for the first level")] private bool _firstScene = false;
        private string _path = "";
        public string ScenePath => _path;

#if UNITY_EDITOR
        private void OnValidate() {
            if (_mainMenuScene != null) {
                _path = AssetDatabase.GetAssetPath(_mainMenuScene);
            }
        }
#endif
        
        
        private void Awake() {
            //Bind events
            EventManager.Instance.OnSceneTransitionBegin += OnSceneTransition;
            EventManager.Instance.OnSceneTransitionEnd += OnSceneTransitionEnd;
            EventManager.Instance.OnPlayerSpawned += OnPlayerSpawned;
            EventManager.Instance.OnPlayerDied += OnPlayerDied;
        }

        private void Start()
        {
            if (_firstScene) return;
            EventManager.Instance.EndSceneTransition(this);
        }

        private void OnSceneTransition(string scene) {
            Debug.Log($"Loading {scene} scene");
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single); //Add a ui transition maybe, who knows
        }

        private void OnPlayerSpawned(Player player) {
            if (player.gameObject.scene != gameObject.scene) return;
            _player = player;
            _player.transform.position = _spawnLocation.position;
        }

        private void OnSceneTransitionEnd(SceneHandler handler) {
            if (handler == this) return;
            EventManager.Instance.OnSceneTransitionEnd -= OnSceneTransitionEnd;
            if (_player != null)
            {
                SceneManager.MoveGameObjectToScene(_player.gameObject, handler.gameObject.scene);
            }

            EventManager.Instance.SpawnPlayer(_player);
            Debug.Log("Unloading scene");
            SceneManager.UnloadSceneAsync(gameObject.scene);
        }

        private void OnPlayerDied()
        {
            SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        }

        private void OnDisable() {
            EventManager.Instance.OnSceneTransitionEnd -= OnSceneTransitionEnd;
            EventManager.Instance.OnSceneTransitionBegin -= OnSceneTransition;
            EventManager.Instance.OnPlayerSpawned -= OnPlayerSpawned;
        }
    }
}