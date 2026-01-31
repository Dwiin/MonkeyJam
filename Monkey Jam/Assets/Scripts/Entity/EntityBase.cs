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
        [SerializeField] private Collider2D[] _attackColliders;

        public ResistanceData[] GetResistances() {
            return _stats.Resistances;
        }

        public void TakeDamage(int amount, EntityBase source = null) {
            Debug.Log("Oof");
           
        }

        /// <summary>
        /// Called through animation events to tell the entity that they can start doing the attack logic for whatever cunting thing they're doing
        /// </summary>
        public void ValidateAttack(int colliderIndex) {
            #region Old AnimationEvent BS
            //float x = 0;
            //float y = 0;
            //string numVal = "";
            //bool editX = true;
            //foreach(char c in collisionSize) {
            //    if (char.IsDigit(c)) {
            //        if (editX) numVal += c.ToString();
            //        else numVal += c.ToString();
            //    }
            //    else {
            //        editX = false;
            //        if (numVal == string.Empty) continue;
            //        x = int.Parse(numVal);
            //        numVal = "";
            //    }
            //}
            //y = int.Parse(numVal);

            //_attackCollider.size = new Vector2(x, y);
            //_attackCollider.offset = new Vector2(x * 0.5f, 0);
            //_attackCollider.enabled = true;
            #endregion
            _attackColliders[colliderIndex].enabled = true;
        }

        public void InvalidateAttack(int colliderIndex) {
            _attackColliders[colliderIndex].enabled = false;
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