using System;
using System.Collections;
using MonkeyJam.Managers;
using UnityEngine;


namespace MonkeyJam.Entities
{    
    public abstract class EnemyBase : EntityBase
    {
        [SerializeField] protected bool isPatroling = false;
        [SerializeField] protected Transform[] waypoints;

        [Range(0f,10f), SerializeField] protected float attackRange;
        [Range(0f, 10f), SerializeField] protected float detectRange;
        protected float _attackDebounceDuration = 0.2f;
        protected bool _attDebounce = false;

        //[field: SerializeField] public EnemyData Data { get; private set; }

        public override void TakeDamage(int amount, EntityBase source = null) {
            _stats.Health -= amount;
            
            if (_stats.Health <= 0) {
                //Fucking oofed
                if (Data.SoundData.DeathSound.Clip != null)
                {
                    EventManager.Instance.RequestSound(Data.SoundData.DeathSound.Clip, transform, Data.SoundData.DeathSound.Volume);
                }
                EventManager.Instance.EnemyDied(this);
                Destroy(gameObject);
            }
            else
            {
                if (Data.SoundData.DamageSound.Clip != null)
                {
                    EventManager.Instance.RequestSound(Data.SoundData.DamageSound.Clip, transform, Data.SoundData.DamageSound.Volume);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (_attDebounce) return;
            EntityBase entity = other.gameObject.GetComponent<EntityBase>();
            if (entity == null) return;
            StartCoroutine(HandleAttackDebounce());
            DamageManager.HandleDamage(new DamageInfo(){Damage = _attackUsing.Damage, DamageType = _attackUsing.DamageType, Source = this, Target = entity});
        }

        private IEnumerator HandleAttackDebounce()
        {
            _attDebounce = true;
            yield return new WaitForSeconds(_attackDebounceDuration);
            _attDebounce = false;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Vector3 detectRangeVec = transform.position;
            //detectRangeVec.x += detectRange;
            detectRangeVec.y -= detectRange;
            Gizmos.DrawLine(transform.position, detectRangeVec);
        }
    }
}