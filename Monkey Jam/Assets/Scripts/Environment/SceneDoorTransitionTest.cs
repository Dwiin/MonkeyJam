using MonkeyJam.Managers;
using System.Collections;
using UnityEngine;

namespace MonkeyJam.Environment {
    public class SceneDoorTransitionTest : MonoBehaviour {
        [SerializeField] private string _nextScene;
        private bool _activated = false;

        private void OnTriggerEnter2D(Collider2D collision) {
            if (_activated) return;
            _activated = true;
            EventManager.Instance.BeginSceneTransition(_nextScene);
        }
    }
}