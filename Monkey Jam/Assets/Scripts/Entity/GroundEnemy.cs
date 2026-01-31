using System;

namespace MonkeyJam.Entities
{
    public class GroundEnemy : EnemyBase
    {
        
        
        private void Start()
        {
            SetupStats(Data.Stats);
        }
    }
}