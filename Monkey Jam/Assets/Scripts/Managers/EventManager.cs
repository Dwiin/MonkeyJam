using UnityEngine;
using System;
using MonkeyJam.Entities;


namespace MonkeyJam.Managers {
    public class  EventManager {
        private static EventManager _instance;
        public static EventManager Instance { get { if (_instance == null) _instance = new EventManager(); return _instance; }  private set { } }


        public Action<string> OnSceneTransitionBegin;
        public Action<SceneHandler> OnSceneTransitionEnd;
        public Action<Player> OnPlayerSpawned;
        public Action<EnemyBase> OnEnemyDied;

        public void BeginSceneTransition(string scene) {
            OnSceneTransitionBegin?.Invoke(scene);
        }

        public void EndSceneTransition(SceneHandler data) {
            OnSceneTransitionEnd?.Invoke(data);
        }

        public void SpawnPlayer(Player player) {
            OnPlayerSpawned?.Invoke(player);
        }

        public void EnemyDied(EnemyBase enemy) {
            OnEnemyDied?.Invoke(enemy);
        }
    }
}