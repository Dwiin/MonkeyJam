using MonkeyJam.Managers;
using UnityEngine;


namespace MonkeyJam.Entities {

    public interface IDamageable {
        public void TakeDamage(int amount, EntityBase source);

        public ResistanceData[] GetResistances();

    }

    [System.Serializable]
    public struct EntityStats {
        [Min(1)] public int MaxHealth;
        public float MoveSpeed;
        [Range(0f, 1f)]public float Armour; //How much to reduce incoming dmg by as a %.
        [HideInInspector] public int Health;
        public ResistanceData[] Resistances;
    }

    public abstract class EntityBase : MonoBehaviour, IDamageable {
        [SerializeField] protected EntityStats _stats;
        [SerializeField] protected Rigidbody2D _rb;
        [SerializeField] protected Animator _animator;

        [Space]
        [SerializeField] protected Transform wayPoint1;
        [SerializeField] protected Transform wayPoint2;

        public ResistanceData[] GetResistances() {
            return _stats.Resistances;
        }

        public void TakeDamage(int amount, EntityBase source = null) {
            Debug.Log("Oof");
           
        }

        protected virtual void SetupStats(EntityStats stats)
        {
            _stats.Armour = stats.Armour;
            _stats.MaxHealth = stats.MaxHealth;
            _stats.MoveSpeed = stats.MoveSpeed;
            _stats.Resistances = stats.Resistances;

            _stats.Health = stats.MaxHealth;
        }
    }
}