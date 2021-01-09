using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public class RocketLauncher : Turret
    {
        [SerializeField] private GameObject bullet;

        protected override void Attack()
        {
            base.Attack();
            bullet.SetActive(false);
        }

        protected override void ReadyToAttack()
        {
            base.ReadyToAttack();
            bullet.SetActive(true);
        }
    }
}