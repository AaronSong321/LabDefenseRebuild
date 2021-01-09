using System;
using System.Collections.Generic;

namespace Aaron.LabDefenseRebuild
{
    public class EnemyBuff : ICloneable
    {
        public string buffName;
        public string uniqueBuffName;
        public float duration;
        public float damageFactor;
        public bool bInstant;
        public bool bEnable;
        public Dictionary<Turret.DamageType, bool> ignoreResistance;

        public delegate void InstantBuff(Enemy enemy);
        public delegate void ContinuousBuff(Enemy enemy, float time);

        public InstantBuff startBuff;
        public ContinuousBuff inBuff;
        public InstantBuff quitBuff;

        public EnemyBuff() { }
        public EnemyBuff(EnemyBuff enemyBuff)
        {
            bEnable = enemyBuff.bEnable;
            buffName = enemyBuff.buffName;
            uniqueBuffName = enemyBuff.uniqueBuffName;
            duration = enemyBuff.duration;
            damageFactor = enemyBuff.damageFactor;
            bInstant = enemyBuff.bInstant;
            startBuff = enemyBuff.startBuff;
            inBuff = enemyBuff.inBuff;
            quitBuff = enemyBuff.quitBuff;
            ignoreResistance = enemyBuff.ignoreResistance;
        }
        protected void CloneValues(EnemyBuff origin)
        {
            bEnable = origin.bEnable;
            buffName = origin.buffName;
            uniqueBuffName = origin.uniqueBuffName;
            duration = origin.duration;
            damageFactor = origin.damageFactor;
            bInstant = origin.bInstant;
            startBuff = origin.startBuff;
            inBuff = origin.inBuff;
            quitBuff = origin.quitBuff;
            ignoreResistance = origin.ignoreResistance;
        }

        public virtual EnemyBuff Clone()
        {
            EnemyBuff ans = new EnemyBuff();
            ans.CloneValues(this);
            return ans;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}