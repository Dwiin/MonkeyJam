using MonkeyJam.Managers;
using System;
using UnityEngine;
using UnityEngine.Animations;

namespace MonkeyJam.Entities
{
    [System.Serializable]
    public struct AttackData {
        public float Cooldown;
        public int Damage;
        public DamageType DamageType;
        public int AttackColliderIndex;
        public bool IsRanged;
    }

    [CreateAssetMenu(menuName = "MonkeyJam/New Enemy", fileName = "New Enemy")]
    public class EnemyData : ScriptableObject
    {
        public string Name;
        public EntityStats Stats;
        public RuntimeAnimatorController AnimationController;
        public AttackData[] Attacks;

        private void OnEnable()
        {
            if (Name == null) Name = this.name;
        }
    }
}