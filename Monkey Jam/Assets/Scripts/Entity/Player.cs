
using MonkeyJam.Managers;

namespace MonkeyJam.Entities {
    public class Player : EntityBase {

        private void Start() {
            DamageManager.HandleDamage(new DamageInfo() { Target = this, Damage = 10, DamageType = DamageType.Physical});
        }
    }
}