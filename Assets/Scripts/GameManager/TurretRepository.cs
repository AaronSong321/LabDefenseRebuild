using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public class TurretRepository : MonoBehaviour
    {
        GameManager gameManager;

        public class TurretData
        {
            public int cost;

            public string uniqueTurretName;

            public int attackSpeed;
            public float minRange;
            public float maxRange;
            public Turret.TargetFlyable targetFlyable;
            public Turret.TargetCount targetCount;

            public int damage;
            public Turret.DamageType damageType;
            public int ballVelocity;
            public EnemyBuff enemyBuff;

            public GameObject turretPrefab;
            public GameObject bulletPrefab;
        }

        public List<List<TurretData>> turrets;

        private void Awake()
        {
            ReadTurretData();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameManager.viewSoloGame.ReadTurretFromRepository();
        }

        private void ReadTurretData()
        {
            string fileName = Settings.GenerateStreamingPath("System/Turret/Turrets.xml");
            XmlDocument document = new XmlDocument();
            document.Load(fileName);
            var turretsInfo = document.SelectSingleNode("Turrets").ChildNodes;
            turrets = new List<List<TurretData>>();
            for (int i = 0; i < turretsInfo.Count; i++)
            {
                turrets.Add(new List<TurretData>());
                var levelCount = turretsInfo[i].ChildNodes.Count;
                for (int j = 0; j < levelCount; j++)
                {
                    var levelInfo = turretsInfo[i].ChildNodes[j];
                    var basic = levelInfo.ChildNodes[0] as XmlElement;
                    var attack = levelInfo.ChildNodes[1] as XmlElement;
                    var newTurretData = new TurretData
                    {
                        uniqueTurretName = (turretsInfo[i] as XmlElement).GetAttribute("name"),
                        cost = Int32.Parse(basic.GetAttribute("cost")),
                        minRange = Int32.Parse(basic.GetAttribute("minRange")),
                        maxRange = Int32.Parse(basic.GetAttribute("maxRange")),
                        targetFlyable = Turret.GetTargetFlyable(basic.GetAttribute("targetFlyable")),
                        targetCount = Turret.GetTargetCount(basic.GetAttribute("targetCount")),
                        attackSpeed = Int32.Parse(basic.GetAttribute("attackSpeed")),
                        damage = Int32.Parse(attack.GetAttribute("damage")),
                        damageType = Turret.GetDamageType(attack.GetAttribute("damageType")),
                        ballVelocity = Int32.Parse(attack.GetAttribute("ballVelocity"))
                    };
                    newTurretData.damageType = Turret.GetDamageType(attack.GetAttribute("damageType"));
                    newTurretData.turretPrefab = Resources.Load<GameObject>($"Prefabs/Turrets/{newTurretData.uniqueTurretName}/Level{j + 1}/Turret");
                    newTurretData.bulletPrefab = Resources.Load<GameObject>($"Prefabs/Turrets/{newTurretData.uniqueTurretName}/Level{j + 1}/Bullet");

                    var enemyBuff = levelInfo.ChildNodes[2] as XmlElement;
                    var buffName = enemyBuff.GetAttribute("name");
                    EnemyBuff buff = null;
                    switch (buffName)
                    {
                        case "": break;
                        case "Explosive":
                            buff = new ExplosiveEffect
                            {
                                radius = Int32.Parse(enemyBuff.GetAttribute("radius")),
                                damage = Int32.Parse(enemyBuff.GetAttribute("damage")),
                                damageType = Turret.GetDamageType(enemyBuff.GetAttribute("damageType"))
                            };
                            break;
                        case "Decelerate":
                            buff = new DecelerateEffect
                            {
                                duration = Single.Parse(enemyBuff.GetAttribute("duration")),
                                ratio = Single.Parse(enemyBuff.GetAttribute("ratio"))
                            };
                            break;
                        case "Stun":
                            buff = new StunEffect
                            {
                                duration = Single.Parse(enemyBuff.GetAttribute("duration"))
                            };
                            break;
                        case "ContinuousDamage":
                            buff = new ContinuousDamage
                            {
                                duration = Single.Parse(enemyBuff.GetAttribute("duration")),
                                damagePerSecond = Int32.Parse(enemyBuff.GetAttribute("damagePerSecond")),
                                damageType = Turret.GetDamageType(enemyBuff.GetAttribute("damageType"))
                            };
                            break;
                        default: throw new UnityException($"Cannot recognize EnemyBuff.buffName {buffName}");
                    }
                    if (buff != null)
                        newTurretData.enemyBuff = buff;
                    turrets[i].Add(newTurretData);
                }
            }
        }
    }
}