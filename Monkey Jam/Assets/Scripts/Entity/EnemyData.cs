using System;
using UnityEngine;
using UnityEngine.Animations;

namespace MonkeyJam.Entities
{
    [CreateAssetMenu(menuName = "MonkeyJam/New Enemy", fileName = "New Enemy")]
    public class EnemyData : ScriptableObject
    {
        public string Name;
        public EntityStats Stats;
        public RuntimeAnimatorController AnimationController;

        private void OnEnable()
        {
            if (Name == null) Name = this.name;
        }
    }
}