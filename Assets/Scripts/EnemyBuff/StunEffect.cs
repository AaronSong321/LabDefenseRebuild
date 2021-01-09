using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public class StunEffect : EnemyBuff
    {
        public float possibility;
        private bool bSuccessStun;

        public StunEffect():base()
        {
            buffName = "StunEffect";
            uniqueBuffName = "StunEffect";
            damageFactor = 1;
            bInstant = false;
            startBuff += (enemy) =>
            {
                if (Random.value <= possibility)
                    bSuccessStun = true;
                else bSuccessStun = false;
                if (bSuccessStun)
                    enemy.bStun = true;
            };
            inBuff += (enemy, deltaTime) =>
            {
                if (bSuccessStun)
                    enemy.bStun = true;
            };
            quitBuff += (enemy) =>
            {
                if (bSuccessStun)
                    enemy.bStun = false;
            };
        }
        public StunEffect(StunEffect se) : base(se)
        {
            possibility = se.possibility;
            bSuccessStun = se.bSuccessStun;
        }
    }
}