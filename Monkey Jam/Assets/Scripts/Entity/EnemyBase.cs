using UnityEngine;


namespace MonkeyJam.Entities
{    
    public abstract class EnemyBase : EntityBase
    {
        [SerializeField] protected bool isPatroling;
        [SerializeField] protected Transform[] waypoints;


        [Range(0f,7.5f), SerializeField] protected float attackRange;

        [field: SerializeField] public EnemyData Data { get; private set; }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}