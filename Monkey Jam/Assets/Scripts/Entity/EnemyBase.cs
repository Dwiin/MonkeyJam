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

        [field: SerializeField] public EnemyData Data { get; private set; }

        public override void TakeDamage(int amount, EntityBase source = null) {
            _stats.Health -= amount;

            if (_stats.Health <= 0) {
                //Fucking oofed
                EventManager.Instance.EnemyDied(this);
                Destroy(gameObject);
            }
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