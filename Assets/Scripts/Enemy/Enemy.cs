using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.AI;

namespace Aaron.LabDefenseRebuild
{
    public class Enemy : MonoBehaviour
    {
        public enum Size { Tiny, Normal, Huge, Boss }
        public enum Flyable { Ground, Air, GroundAir }
        public static Size GetSize(string s)
        {
            switch (s)
            {
                case "Tiny":return Size.Tiny;
                case "Normal":return Size.Normal;
                case "Huge":return Size.Huge;
                case "Boss":return Size.Boss;
                default:throw new UnityException($"Cannot get a corresponding Enemy.Size for string \"{s}\"");
            }
        }
        public static Flyable GetFlyable(string s)
        {
            switch (s)
            {
                case "Ground":return Flyable.Ground;
                case "Air":return Flyable.Air;
                case "GroundAir":return Flyable.GroundAir;
                default:throw new UnityException($"Cannot get a corresponding Enemy.Flyable for string \"{s}\"");
            }
        }

        protected static float lowestDistance = 2f;
        public string uniqueEnemyName;
        
        [HideInInspector] public int initHealth;
        [HideInInspector] public int currentHealth;
        [HideInInspector] public float initSpeed;
        [HideInInspector] public float currentSpeed;
        [HideInInspector] public bool bStun;
        [HideInInspector] public int initDamage;
        [HideInInspector] public int currentDamage;
        [HideInInspector] public int rewardcash;
        [HideInInspector] public int rewardExp;

        [HideInInspector] public Size type;
        [HideInInspector] public Flyable flyable;

        [HideInInspector] public Dictionary<Turret.DamageType, float> resistance;

        protected NavMeshAgent agent;
        protected Transform destination;
        public GameManager gameManager;

        public GameSettings.Difficulty difficulty;
        public List<EnemyBuff> enemyBuff;

        public delegate void EnemyKilledHandler(Enemy enemy);
        public event EnemyKilledHandler EnemyKilledEvent;
        public delegate void EnemyDiscardHandler(Enemy enemy);
        public event EnemyDiscardHandler EnemyDiscardEvent;
        public delegate void ReachDestinationHandler(Enemy enemy);
        public event ReachDestinationHandler ReachDestinationEvent;

        protected virtual void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            enemyBuff = new List<EnemyBuff>();
            difficulty = gameManager.gameMode.difficulty;
            ReadFromXml();

            currentHealth = initHealth;
            currentDamage = initDamage;
            bStun = false;

            if (flyable == Flyable.Air)
            {
                currentSpeed = initSpeed / GameSettings.lengthFactor;
                destination = gameManager.endInAir.transform;
            }
            else if (flyable== Flyable.Ground)
            {
                currentSpeed = initSpeed / GameSettings.lengthFactor;
                agent = gameObject.AddComponent<NavMeshAgent>();
                agent.avoidancePriority = 99;
                agent.speed = currentSpeed * gameManager.gameMode.GameSpeed;
                agent.destination = gameManager.endOnGround.position;
                agent.stoppingDistance = lowestDistance;
                destination = gameManager.endOnGround;
            }

            gameManager.gameMode.GameSpeedChangeEvent += () =>
            {
                if (flyable == Flyable.Air)
                {
                    currentSpeed = initSpeed / GameSettings.lengthFactor;
                }
                else if (flyable == Flyable.Ground)
                {
                    currentSpeed = initSpeed / GameSettings.lengthFactor;
                    agent = gameObject.GetComponent<NavMeshAgent>();
                    agent.speed = currentSpeed * gameManager.gameMode.GameSpeed;
                }
            };
        }

        protected virtual void Update()
        {
            if (bStun == false)
            {
                MoveTowardsDestination();
            }
            foreach (var buff in enemyBuff)
            {
                if (!buff.bInstant)
                {
                    var actualDuration = Time.deltaTime * gameManager.gameMode.GameSpeed;
                    actualDuration = actualDuration > buff.duration ? buff.duration : actualDuration;
                    buff.duration -= actualDuration;
                    buff.inBuff?.Invoke(this, actualDuration);
                    if (Mathf.Approximately(buff.duration, 0))
                    {
                        buff.quitBuff?.Invoke(this);
                        enemyBuff.Remove(buff);
                    }
                }
            }
        }

        protected virtual void MoveTowardsDestination()
        {
            if (flyable == Flyable.Ground && agent != null)
            {
                if (!agent.pathPending && agent.remainingDistance != Mathf.Infinity
                    && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= agent.stoppingDistance)
                    ReachDestination();
            }
            else if (flyable == Flyable.Air)
            {
                var direction = (destination.position - gameObject.transform.position).normalized;
                gameObject.GetComponent<Transform>().Translate(direction * currentSpeed * gameManager.gameMode.GameSpeed);
                if ((gameManager.endInAir.position - gameObject.transform.position).magnitude < lowestDistance)
                    ReachDestination();
            }
        }

        protected void ReachDestination()
        {
            ReachDestinationEvent?.Invoke(this);
            EnemyDiscardEvent?.Invoke(this);
            Destroy(gameObject);
        }

        protected virtual void ReadFromXml()
        {
            XmlDocument document = new XmlDocument();
            document.Load(Settings.GenerateStreamingPath("System/Difficulty/Hypothetical/Enemy.xml"));
            var enemyList = document.SelectSingleNode("Enemies").ChildNodes;
            XmlElement thisEnemy = null;
            for(int i = 0; i < enemyList.Count; i++)
            {
                if ((enemyList[i] as XmlElement).GetAttribute("name").Equals(uniqueEnemyName))
                {
                    thisEnemy = enemyList[i] as XmlElement;
                    break;
                }
            }
            if (thisEnemy == null)
                throw new UnityException($"Cannot find {uniqueEnemyName} in Hypothetical difficulty.");
            var attributeList = thisEnemy.ChildNodes;

            var basic = attributeList[0] as XmlElement;
            initHealth = Int32.Parse(basic.GetAttribute("health"));
            currentHealth = initHealth;
            initSpeed = Single.Parse(basic.GetAttribute("speed"));
            currentSpeed = initSpeed;
            initDamage = Int32.Parse(basic.GetAttribute("damage"));
            currentDamage = initDamage;

            var typeInfo = attributeList[1] as XmlElement;
            type = GetSize(typeInfo.GetAttribute("size"));
            flyable = GetFlyable(typeInfo.GetAttribute("flyable"));

            var reward = attributeList[2] as XmlElement;
            rewardcash = Int32.Parse(reward.GetAttribute("dosh"));
            rewardExp = Int32.Parse(reward.GetAttribute("exp"));

            var damageFacotr = attributeList[3] as XmlElement;
            resistance = new Dictionary<Turret.DamageType, float>
            {
                { Turret.DamageType.Bullet, Single.Parse(damageFacotr.GetAttribute("bullet")) },
                { Turret.DamageType.Explosive, Single.Parse(damageFacotr.GetAttribute("explosive")) },
                { Turret.DamageType.Flame, Single.Parse(damageFacotr.GetAttribute("flame")) },
                { Turret.DamageType.Microwave, Single.Parse(damageFacotr.GetAttribute("bullet")) },
                { Turret.DamageType.Nuclear, Single.Parse(damageFacotr.GetAttribute("microwave")) },
                { Turret.DamageType.Tesla, Single.Parse(damageFacotr.GetAttribute("tesla")) }
            };
        }

        public virtual void TakeBullet(Bullet bullet)
        {
            TakeDamage(bullet.damage, bullet.damageType);
            if (bullet.enemyBuff != null)
            {
                foreach (var eb in bullet.enemyBuff)
                    if (eb.bEnable)
                    {
                        eb.startBuff?.Invoke(this);
                        if (!eb.bInstant)
                        {
                            EnemyBuff sameBuff = null;
                            foreach (var buff in enemyBuff)
                            {
                                if (buff.uniqueBuffName.Equals(eb.uniqueBuffName))
                                {
                                    sameBuff = buff;
                                }
                            }
                            if (sameBuff == null)
                                enemyBuff.Add(eb);
                            else
                            {
                                enemyBuff.Remove(sameBuff);
                                enemyBuff.Add(eb);
                            }
                        }
                    }
            }
        }

        public virtual void TakeDamage(int damage, Turret.DamageType dt, bool ignoreResistance)
        {
            if (ignoreResistance)
            {
                TakeDamage(damage, dt);
                return;
            }
            else
            {
                TakeDamage(damage);
                return;
            }
        }

        protected virtual void TakeDamage(int damage, Turret.DamageType dt)
        {
            if (currentHealth < 0 || Mathf.Approximately(currentHealth, 0))
                return;
            resistance.TryGetValue(dt, out var resis);
            var actualDamage = (int)(damage * resis);
            TakeDamage(actualDamage);
        }

        protected virtual void TakeDamage(int damage)
        {
            var actualDamage = damage > currentHealth ? currentHealth : damage;
            currentHealth -= actualDamage;
            if (currentHealth == 0) Die();
        }

        protected virtual void Die()
        {
            EnemyKilledEvent?.Invoke(this);
            Destroy(gameObject);
        }
    }
}