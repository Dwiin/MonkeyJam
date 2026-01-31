using UnityEngine;
using System;


namespace MonkeyJam.Managers {
    public class  EventManager {
        private static EventManager _instance;
        public static EventManager Instance { get { if (_instance == null) _instance = new EventManager(); return _instance; }  private set { } }


        public Action<string> OnSceneTransitionBegin;
        public Action<SceneData> OnSceneTransitionEnd;

        public void BeginSceneTransition(string scene) {
            OnSceneTransitionBegin?.Invoke(scene);
        }

        public void EndSceneTransition(SceneData data) {
            OnSceneTransitionEnd?.Invoke(data);
        }

    }
}