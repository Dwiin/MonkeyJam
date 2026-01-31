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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Vector3 detectRangeVec = transform.position;
            detectRangeVec.x += detectRange;
            Gizmos.DrawLine(transform.position, detectRangeVec);
        }
    }
}