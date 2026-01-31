using UnityEngine;


namespace MonkeyJam.Entities
{    
    public abstract class EnemyBase : EntityBase
    {
        [SerializeField] protected bool isPatroling;
        [SerializeField] protected Transform[] waypoints;

        [field: SerializeField] public EnemyData Data { get; private set; }
    }
}