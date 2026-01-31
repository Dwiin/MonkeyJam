using UnityEngine;
using UnityEngine.SceneManagement;
using MonkeyJam.Entities;


namespace MonkeyJam.Managers {
    public class SceneHandler : MonoBehaviour {
        [SerializeField] private Player _player;
        [SerializeField] private Transform _spawnLocation;
        

        private void Awake() {
            //Bind events
            EventManager.Instance.OnSceneTransitionBegin += OnSceneTransition;
            EventManager.Instance.OnSceneTransitionEnd += OnSceneTransitionEnd;
            EventManager.Instance.OnPlayerSpawned += OnPlayerSpawned;
        }

        private void Start() {
            EventManager.Instance.EndSceneTransition(this);
        }

        private void OnSceneTransition(string scene) {
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive); //Add a ui transition maybe, who knows
        }

        private void OnPlayerSpawned(Player player) {
            if (player.gameObject.scene != gameObject.scene) return;
            _player = player;
            _player.transform.position = _spawnLocation.position;
        }

        private void OnSceneTransitionEnd(SceneHandler handler) {
            if (handler == this) return;
            EventManager.Instance.OnSceneTransitionEnd -= OnSceneTransitionEnd;
            SceneManager.MoveGameObjectToScene(_player.gameObject, handler.gameObject.scene);
            EventManager.Instance.SpawnPlayer(_player);

            SceneManager.UnloadSceneAsync(gameObject.scene);
        }

        private void OnDisable() {
            EventManager.Instance.OnSceneTransitionEnd -= OnSceneTransitionEnd;
            EventManager.Instance.OnSceneTransitionBegin -= OnSceneTransition;
            EventManager.Instance.OnPlayerSpawned -= OnPlayerSpawned;
        }
    }
}