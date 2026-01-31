using UnityEngine;
using UnityEngine.SceneManagement;


namespace MonkeyJam.Managers {
    public class SceneHandler : MonoBehaviour {


        private void Awake() {
            //Bind events
            EventManager.Instance.OnSceneTransitionBegin += OnSceneTransition;
            EventManager.Instance.OnSceneTransitionEnd += OnSceneTransitionEnd;
        }

        private void OnSceneTransition(string scene) {
            //Load next scene
            //Unload this scene after
            AsyncOperation loadOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }

        private void OnSceneTransitionEnd(SceneData data) {
            EventManager.Instance.OnSceneTransitionEnd -= OnSceneTransitionEnd;

            SceneManager.UnloadSceneAsync(gameObject.scene);
        }

        private void OnDisable() {
            EventManager.Instance.OnSceneTransitionEnd -= OnSceneTransitionEnd;
            EventManager.Instance.OnSceneTransitionBegin -= OnSceneTransition;
        }
    }
}