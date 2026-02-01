using MonkeyJam.Entities;
using MonkeyJam.Managers;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace MonkeyJam.Environment {
    public class SceneDoor : MonoBehaviour {
        [SerializeField] private Object _nextScene;
        [SerializeField] private string _sceneName;
        [Tooltip("How many enemies to kill before the door lets the player progress. We do be lovin slaughter."), SerializeField] private int _enemiesToKill;
        private int _currentEnemyDeathCount = 0;
        private bool _activated = false;
        private string _path = "";
        public string ScenePath => _path;

#if UNITY_EDITOR
        private void OnValidate() {
            if (_nextScene != null) {
                _path = AssetDatabase.GetAssetPath(_nextScene);
            }
        }
#endif

        private void Awake() {
            EventManager.Instance.OnEnemyDied += OnEnemyDied;
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (_activated || _currentEnemyDeathCount < _enemiesToKill) return;
            if (!collision.gameObject.CompareTag("Player")) return;
            _activated = true;
            EventManager.Instance.BeginSceneTransition(_sceneName);
        }

        private void OnEnemyDied(EnemyBase enemy) {
            _currentEnemyDeathCount += 1;
            _currentEnemyDeathCount = Mathf.Clamp(_currentEnemyDeathCount, 0, _enemiesToKill);
        }

        private void OnDisable() {
            EventManager.Instance.OnEnemyDied -= OnEnemyDied;
        }
    }
}