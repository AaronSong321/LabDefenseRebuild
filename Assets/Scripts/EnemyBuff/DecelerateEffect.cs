using UnityEngine.AI;

namespace Aaron.LabDefenseRebuild
{
    public class DecelerateEffect : EnemyBuff
    {
        public float ratio;

        public DecelerateEffect() : base()
        {
            buffName = "DecelerateEffect";
            uniqueBuffName = "DecelerateEffect";
            bInstant = false;
            damageFactor = 1;
            startBuff += (enemy) =>
            {
                switch (enemy.flyable)
                {
                    case Enemy.Flyable.Air:
                        enemy.currentSpeed *= ratio;
                        break;
                    case Enemy.Flyable.Ground:
                        enemy.currentSpeed *= ratio;
                        enemy.GetComponent<NavMeshAgent>().speed *= ratio;
                        break;
                }
            };
            inBuff = null;
            quitBuff += (enemy) =>
            {
                switch (enemy.flyable)
                {
                    case Enemy.Flyable.Air:
                        enemy.currentSpeed /= ratio;
                        break;
                    case Enemy.Flyable.Ground:
                        enemy.currentSpeed /= ratio;
                        enemy.GetComponent<NavMeshAgent>().speed /= ratio;
                        break;
                };
            };
        }
        public DecelerateEffect(DecelerateEffect de) : base(de)
        {
            ratio = de.ratio;
        }

        protected void CloneValues(DecelerateEffect debuff)
        {
            base.CloneValues(debuff);
            ratio = debuff.ratio;
        }
        public override EnemyBuff Clone()
        {
            DecelerateEffect ans = new DecelerateEffect();
            ans.CloneValues(this);
            return ans;
        }
    }
}