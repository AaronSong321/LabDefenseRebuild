using System.Collections.Generic;
using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public class Bullet : MonoBehaviour
    {
        protected static float lowestDistance = 2f;

        [HideInInspector] public int damage;
        [HideInInspector] public Turret.DamageType damageType;
        [HideInInspector] public int ballVelocity;

        [HideInInspector] public Enemy target;
        [HideInInspector] public string uniqueTurretName;
        [HideInInspector] public int turretLevel;
        [HideInInspector] public List<EnemyBuff> enemyBuff;

        [SerializeField] protected GameObject explosionEffect;
        [HideInInspector] public GameManager gameManager;
        [HideInInspector] public Dictionary<Turret.DamageType, bool> ignoreResistance;

        protected virtual void Update()
        {
            if (target == null) Destroy(gameObject);
            else
            {
                MoveTowardsTarget();
                var distance = (target.gameObject.transform.position - gameObject.transform.position).magnitude;
                if (distance < lowestDistance)
                {
                    target.TakeBullet(this);
                    if (enemyBuff != null)
                        foreach (var eb in enemyBuff)
                            if (explosionEffect != null && eb is ExplosiveEffect)
                            {
                                var ps = Instantiate(explosionEffect, gameObject.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                                ps.Play();
                                Destroy(ps.gameObject, ps.main.duration);
                                var main = ps.main;
                                main.simulationSpeed = gameManager.gameMode.GameSpeed;
                                break;
                            }
                    Destroy(gameObject);
                }
            }
        }

        protected virtual void MoveTowardsTarget()
        {
            var direction = (target.gameObject.transform.position - gameObject.transform.position).normalized;
            gameObject.transform.Translate(direction * ballVelocity * Time.deltaTime * gameManager.gameMode.GameSpeed);
        }
    }
}