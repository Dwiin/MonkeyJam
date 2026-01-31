using MonkeyJam.Managers;
using UnityEngine;


namespace MonkeyJam.Entities
{    
    public abstract class EnemyBase : EntityBase
    {
        [SerializeField] protected bool isPatroling;
        [SerializeField] protected Transform[] waypoints;

        public override void TakeDamage(int amount, EntityBase source = null) {
            _stats.Health -= amount;

            if (_stats.Health <= 0) {
                //Fucking oofed
                EventManager.Instance.EnemyDied(this);
                Destroy(gameObject);
            }
        }
    }
}