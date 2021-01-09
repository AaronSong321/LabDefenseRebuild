namespace Aaron.LabDefenseRebuild
{
    public class ContinuousDamage : EnemyBuff
    {
        public int damagePerSecond;
        public Turret.DamageType damageType;

        public ContinuousDamage():base()
        {
            buffName = "ContinuousDamage";
            uniqueBuffName = "ContinuousDamage";
            bInstant = false;
            damageFactor = 1;
            startBuff = null;
            inBuff += (enemy, deltaTime) =>
            {
                ignoreResistance.TryGetValue(damageType, out var b);
                enemy.TakeDamage((int)(damagePerSecond * damageFactor * deltaTime), damageType, b);
            };
            quitBuff = null;
        }
        public ContinuousDamage(ContinuousDamage cd) : base(cd)
        {
            damagePerSecond = cd.damagePerSecond;
            damageType = cd.damageType;
        }
    }
}