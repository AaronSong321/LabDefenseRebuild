using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public class ExplosiveEffect : EnemyBuff
    {
        public float radius;
        public Turret.DamageType damageType;
        public int damage;

        public ExplosiveEffect() : base()
        {
            buffName = "ExplosiveEffect";
            uniqueBuffName = "ExplosiveEffect";
            duration = 0;
            damageFactor = 1;
            bInstant = true;
            startBuff += (enemy) =>
            {
                Collider[] collider = Physics.OverlapSphere(enemy.transform.position, radius / GameSettings.lengthFactor, 1 << LayerMask.NameToLayer("Enemy"));
                foreach (var col in collider)
                {
                    ignoreResistance.TryGetValue(damageType, out var b);
                    col.GetComponent<Enemy>().TakeDamage(damage, damageType, b);
                }
            };
            inBuff = null;
            quitBuff = null;
        }
        public ExplosiveEffect(ExplosiveEffect ee) : base(ee)
        {
            radius = ee.radius;
            damageType = ee.damageType;
            damage = ee.damage;
        }
    }
}