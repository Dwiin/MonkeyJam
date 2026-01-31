using UnityEngine;


namespace MonkeyJam.Entities
{
    public abstract class EnemyBase : EntityBase
    {
        [field: SerializeField] public EnemyData Data { get; private set; }
    }
}