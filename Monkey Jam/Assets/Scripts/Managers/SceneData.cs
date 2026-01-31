using UnityEngine;


namespace MonkeyJam.Managers {
    public class SceneData : MonoBehaviour {
        [field:SerializeField] public Transform SpawnLocation { get; private set; }

        private void Start() {
            EventManager.Instance.EndSceneTransition(this);
        }
    }
}