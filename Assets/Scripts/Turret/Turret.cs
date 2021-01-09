using System.Collections.Generic;
using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public class Turret : MonoBehaviour
    {
        public enum TargetFlyable { Ground, Air, GroundAir }
        public enum TargetCount { One, All }
        public enum DamageType { Bullet, Explosive, Flame, Tesla, Nuclear, Microwave }

        public static TargetFlyable GetTargetFlyable(string s)
        {
            switch (s)
            {
                case "Ground":return TargetFlyable.Ground;
                case "Air":return TargetFlyable.Air;
                case "GroundAir":return TargetFlyable.GroundAir;
                default:throw new UnityException($"Cannot get Turret.TargetFlyable for string \"{s}\"");
            }
        }
        public static TargetCount GetTargetCount(string s)
        {
            switch (s)
            {
                case "One":return TargetCount.One;
                case "All":return TargetCount.All;
                default:throw new UnityException($"Cannot get Turret.TargetCount for string \"{s}\"");
            }
        }
        public static DamageType GetDamageType(string s)
        {
            switch (s)
            {
                case "Bullet":return DamageType.Bullet;
                case "Explosive":return DamageType.Explosive;
                case "Flame":return DamageType.Flame;
                case "Tesla":return DamageType.Tesla;
                case "Nuclear":return DamageType.Nuclear;
                case "Microwave":return DamageType.Microwave;
                default:throw new UnityException($"Cannot get Turret.DamageType for string \"{s}\"");
            }
        }

        [HideInInspector] public int currentLevel;
        [HideInInspector] public int maxLevel;
        public bool CanUpgrade => currentLevel < maxLevel;
        public string uniqueTurretName;

        #region Read from TurretData
        [HideInInspector] private int attackSpeed;
        [HideInInspector] private float minRange;
        [HideInInspector] private float maxRange;
        [HideInInspector] public TargetFlyable targetFlyable;
        [HideInInspector] public TargetCount targetCount;
        [HideInInspector] private Dictionary<DamageType, bool> ignoreResistance;

        public float MinRange
        {
            get { return minRange; }
            set
            {
                if (minCollider != null)
                    Destroy(minCollider);
                minRange = value;
                minCollider = gameObject.AddComponent<CapsuleCollider>();
                minCollider.isTrigger = true;
                minCollider.center = new Vector3(0, 0.5f, 0);
                minCollider.radius = minRange / GameSettings.lengthFactor;
                minCollider.height = 25 + minCollider.radius;
            }
        }
        public float MaxRange
        {
            get { return maxRange; }
            set
            {
                if (maxCollider != null)
                    Destroy(maxCollider);
                maxRange = value;
                maxCollider = gameObject.AddComponent<CapsuleCollider>();
                maxCollider.isTrigger = true;
                maxCollider.center = new Vector3(0, 0.5f, 0);
                maxCollider.radius = maxRange / GameSettings.lengthFactor;
                maxCollider.height = 25 + maxCollider.radius;
            }
        }
        public int AttackSpeed
        {
            get { return attackSpeed; }
            set
            {
                attackSpeed = value;
                attackRateTime = 100.0f / attackSpeed;
            }
        }
        public float DamageFactor
        {
            get => damageFactor; 
            set
            {
                damageFactor = value;
                //if (enemyBuff != null)
                    foreach (var eb in enemyBuff)
                        eb.damageFactor = value;
            }
        }
        public int Damage
        {
            get => damage;
        }
        public bool HasDirectHitDamage
        {
            get => bDirectHitDamage;
            set => value = bDirectHitDamage;
        }

        [HideInInspector] private int damage;
        [HideInInspector] public DamageType damageType;
        private int ballVelocity;
        private bool bDirectHitDamage;
        protected GameObject bulletPrefab;
        [HideInInspector] public List<EnemyBuff> enemyBuff;
        #endregion

        protected float attackRateTime;
        protected float attackTimer;
        [HideInInspector] private float damageFactor;
        protected List<Enemy> enemiesNearby;
        protected List<Enemy> enemiesCantAttack;
        [SerializeField] protected Transform firePosition;
        [HideInInspector] private CapsuleCollider minCollider;
        [HideInInspector] private CapsuleCollider maxCollider;

        protected GameManager gameManager;

        public delegate void AttackHandler(Turret turret, Bullet bullet, Enemy enemy);
        public event AttackHandler AttackEvent;

        protected virtual void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            enemiesNearby = new List<Enemy>();
            enemiesCantAttack = new List<Enemy>();
            ignoreResistance = new Dictionary<DamageType, bool>
            {
                { DamageType.Bullet, false },
                {DamageType.Explosive,false },
                {DamageType.Flame,false },
                {DamageType.Microwave,false },
                {DamageType.Nuclear,false },
                {DamageType.Tesla,false }
            };
            bDirectHitDamage = true;
            GetDataFromRepository();
            attackTimer = 0;
            damageFactor = 1;
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            var category = collider.gameObject.GetComponent<MonoBehaviour>();
            if (category is Enemy)
            {
                var enemy = category as Enemy;
                if (enemiesNearby.Contains(enemy))
                {
                    enemiesNearby.Remove(enemy);
                    enemiesCantAttack.Add(enemy);
                }
                else
                {
                    if (CanAttack(enemy))
                    {
                        enemiesNearby.Add(enemy);
                    }
                }
            }
        }

        protected virtual void OnTriggerExit(Collider collider)
        {
            var category = collider.gameObject.GetComponent<MonoBehaviour>();
            if (category is Enemy)
            {
                var enemy = category as Enemy;
                if (enemiesNearby.Contains(enemy))
                {
                    enemiesNearby.Remove(enemy);
                }
                else
                {
                    enemiesNearby.Add(enemy);
                    enemiesCantAttack.Remove(enemy);
                    if (CanAttack(enemy))
                    {
                        enemiesNearby.Add(enemy);
                    }
                }
            }
        }

        private bool CanAttack(Enemy e)
        {
            switch (e.flyable)
            {
                case Enemy.Flyable.Ground:return targetFlyable == TargetFlyable.Ground || targetFlyable == TargetFlyable.GroundAir;
                case Enemy.Flyable.Air:return targetFlyable == TargetFlyable.Air || targetFlyable == TargetFlyable.GroundAir;
                default:return true;
            }
        }

        protected virtual void GetDataFromRepository()
        {
            var turrets = gameManager.turretRepository.turrets;
            TurretRepository.TurretData c = null;
            foreach(var turretInfo in turrets)
            {
                if (turretInfo[0].uniqueTurretName.Equals(uniqueTurretName))
                {
                    maxLevel = turretInfo.Count;
                    c = turretInfo[currentLevel];
                    break;
                }
            }
            attackSpeed = c.attackSpeed;
            MinRange = c.minRange;
            MaxRange = c.maxRange;
            targetFlyable = c.targetFlyable;
            targetCount = c.targetCount;
            damage = c.damage;
            damageType = c.damageType;
            ballVelocity = c.ballVelocity;
            bulletPrefab = c.bulletPrefab;
            enemyBuff = new List<EnemyBuff>();
            if (c.enemyBuff != null)
                enemyBuff.Add(c.enemyBuff);
        }

        protected virtual void Update()
        {
            if (!Mathf.Approximately(attackTimer, 0))
            {
                var decreaseTimer = Time.deltaTime * gameManager.gameMode.GameSpeed;
                decreaseTimer = decreaseTimer > attackTimer ? attackTimer : decreaseTimer;
                attackTimer -= decreaseTimer;
                if (Mathf.Approximately(attackTimer, 0))
                    ReadyToAttack();
            }
            if (enemiesNearby.Count>0 && Mathf.Approximately(attackTimer,0) && gameManager.gameMode.bPause == false)
            {
                attackTimer = attackRateTime;
                Attack();
            }
        }

        protected virtual void Attack()
        {
            UpdateEnemies();
            if (enemiesNearby.Count > 0)
            {
                switch (targetCount)
                {
                    case TargetCount.All:
                        for (int i = 0; i < enemiesNearby.Count; i++)
                        {
                            if (i == 0)
                            {
                                var bullet = GenerateBullet(enemiesNearby[i]);
                                AttackEvent?.Invoke(this, bullet, enemiesNearby[i]);
                            }
                            else GenerateBullet(enemiesNearby[i]);
                        }
                        attackTimer = attackRateTime;
                        break;
                    case TargetCount.One:
                        var b = GenerateBullet(enemiesNearby[0]);
                        AttackEvent?.Invoke(this, b, enemiesNearby[0]);
                        attackTimer = attackRateTime;
                        break;
                }
            }
            else attackTimer = 0;
        }

        protected virtual void UpdateEnemies()
        {
            List<int> emptyIndex = new List<int>();
            for (int i = 0; i < enemiesNearby.Count; i++)
                if (enemiesNearby[i] == null)
                    emptyIndex.Add(i);
            for (int i = 0; i < emptyIndex.Count; i++)
                enemiesNearby.RemoveAt(emptyIndex[i] - i);
        }

        protected virtual Bullet GenerateBullet(Enemy enemy)
        {
            GameObject @object = Instantiate(bulletPrefab, firePosition.position, firePosition.rotation);
            var bullet = @object.GetComponent<Bullet>();
            if (bDirectHitDamage)
                bullet.damage = (int)(damage * damageFactor);
            else
                bullet.damage = 0;
            bullet.damageType = damageType;
            bullet.ballVelocity = ballVelocity / GameSettings.lengthFactor;
            bullet.target = enemy;
            bullet.uniqueTurretName = uniqueTurretName;
            bullet.turretLevel = currentLevel;
            bullet.gameManager = gameManager;

            bullet.ignoreResistance = new Dictionary<DamageType, bool>();
            foreach (var pair in ignoreResistance)
            {
                bullet.ignoreResistance.Add(pair.Key, pair.Value);
            }
            bullet.enemyBuff = new List<EnemyBuff>();
            bullet.ignoreResistance = ignoreResistance;
            if (enemyBuff != null)
                foreach (var eb in enemyBuff)
                    switch (eb)
                    {
                        case null:bullet.enemyBuff = null;break;
                        case ExplosiveEffect ee: bullet.enemyBuff.Add(new ExplosiveEffect(ee)); break;
                        case ContinuousDamage cd:bullet.enemyBuff.Add(new ContinuousDamage(cd));break;
                        case DecelerateEffect de:bullet.enemyBuff.Add(new DecelerateEffect(de));break;
                        case StunEffect se:bullet.enemyBuff.Add(new StunEffect(se));break;
                    }
            return bullet;
        }

        protected virtual void ReadyToAttack()
        {

        }

        public EnemyBuff ContainsBuff(string uniqueBuffName)
        {
            foreach (var buff in enemyBuff)
            {
                if (buff.uniqueBuffName.Equals(uniqueBuffName))
                    return buff;
            }
            return null;
        }
        public bool DeleteBuff(string uniqueBuffName)
        {
            for (int i = 0; i < enemyBuff.Count; i++)
                if (enemyBuff[i].uniqueBuffName.Equals(uniqueBuffName))
                {
                    enemyBuff.RemoveAt(i);
                    return true;
                }
            return false;
        }
        public void AppendBuff(EnemyBuff buff)
        {
            foreach(var onebuff in enemyBuff)
            {
                if (onebuff.uniqueBuffName == buff.uniqueBuffName)
                    return;
            }
            enemyBuff.Add(buff);
        }

        public void SetIgnoreResistance(DamageType dt, bool ignore)
        {
            ignoreResistance.Remove(dt);
            ignoreResistance.Add(dt, ignore);
        }
        public bool GetIgnoreResistance(DamageType dt)
        {
            ignoreResistance.TryGetValue(dt, out var ans);
            return ans;
        }
    }
}