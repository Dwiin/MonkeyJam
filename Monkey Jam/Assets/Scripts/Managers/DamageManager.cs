using MonkeyJam.Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace MonkeyJam.Managers {
    public enum DamageType {
        Physical,
        Magical,
        True
    }

    [System.Serializable]
    public struct ResistanceData {
        public DamageType DamageType;
        [Range(0f, 1f)] public float Resistance;
    }

    [System.Serializable]
    public struct DamageInfo {
        public IDamageable Target;
        public EntityBase Source;
        public int Damage;
        public DamageType DamageType;
    }

    public class DamageManager {


        public static void HandleDamage(DamageInfo info) {
            foreach (ResistanceData data in info.Target.GetResistances()) {
                if (data.DamageType != info.DamageType) continue;
                Debug.Log($"Damage: {info.Damage} | Resistance: {data.Resistance} | Dmg * Res: {info.Damage * data.Resistance}");
                info.Damage -= Mathf.RoundToInt(info.Damage * data.Resistance);
            }
            Debug.Log($"Current damage: {info.Damage}");
            info.Target.TakeDamage(info.Damage, info.Source);
        }
    }
}