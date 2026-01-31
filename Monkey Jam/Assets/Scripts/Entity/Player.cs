
using System.Collections;
using MonkeyJam.Managers;
using UnityEngine;

namespace MonkeyJam.Entities {
    public class Player : EntityBase
    {
        [SerializeField] private EnemyBase _toPosess; //Debug only, will be removed for actual implementation
        
        private void Start() {
            DamageManager.HandleDamage(new DamageInfo() { Target = this, Damage = 10, DamageType = DamageType.Physical});
            StartCoroutine(TestPosession());
        }

        private IEnumerator TestPosession()
        {
            yield return new WaitForSeconds(2);
            Posess(_toPosess);
        }
        
        public void Posess(EnemyBase enemy)
        {
            SetupStats(enemy.Data.Stats); 
            _animator.runtimeAnimatorController = enemy.Data.AnimationController;
        }
    }
}