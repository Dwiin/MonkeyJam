using System;
using MonkeyJam.Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace MonkeyJam.Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private float _projectileSpeed = 5;
        [SerializeField] private float _projectileLifetime = 3;
        [SerializeField] private Rigidbody2D _rb;
        private AttackData _data;
        private EntityBase _owner;

        public void Init(EntityBase owner, AttackData data, bool flipX)
        {
            _owner = owner;
            _data = data;
            if (_rb == null) _rb = GetComponent<Rigidbody2D>();
            
            _rb.linearVelocity = flipX ? Vector2.left * _projectileSpeed : Vector2.right * _projectileSpeed;
            Destroy(gameObject, _projectileLifetime);
            
            GetComponent<SpriteRenderer>().flipX = flipX;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            
            EntityBase entity = other.gameObject.GetComponent<EntityBase>();
            if (entity == null) return;
            if (entity == _owner) return;
            if (!_owner.CompareTag("Player") && !other.gameObject.CompareTag("Player")) return;
            DamageManager.HandleDamage(new DamageInfo(){Damage = _data.Damage, DamageType = _data.DamageType, Source = _owner, Target = entity});
            Destroy(gameObject);
        }
    }
}