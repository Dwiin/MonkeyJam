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
        [Tooltip("Which collider to use when attacking if melee.")]public int AttackColliderIndex;
        public bool IsRanged;
        public GameObject ProjectilePrefab;
        public float AttackRange;
        [Tooltip("Which attack animation to play in the animator.")]public int AttackRangeIndex;
        [Tooltip("How much juice it costs the player to perform.")]public int StaminaCost;
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