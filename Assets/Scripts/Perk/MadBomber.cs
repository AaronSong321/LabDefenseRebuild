using static Aaron.LabDefenseRebuild.Settings;
using System;
using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public class MadBomber : Perk
    {
        protected override void Awake()
        {
            uniquePerkName = "MadBomber";
            base.Awake();
        }

        protected override void ReadFromPlayer()
        {

        }

        protected override void Init()
        {
            perkAttributes.Add(new Attribute1(this));
            perkAttributes.Add(new Attribute2(this));
            //perkAttributes.Add(new Attribute3(this));
        }

        public class Attribute1 : PerkSkill
        {
            public float factor;

            public BuildManager.ModifyTurretHandler handler;

            public Attribute1(Perk perk) : base(perk)
            {
                uniquePerkSkillName = $"{nameof(MadBomber)}.Attribute1";
                handler += (turret, cash) =>
                {
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.DamageFactor += perk.currentLevel * factor;
                    }
                };
            }

            public override string Description() => $"Increase direct hit and explosive damage of {RichTextModify("SharpnelThrower", 2)}, {RichTextModify("PatriotMissile", 2)} and " +
                $"{RichTextModify("RocketLauncher", 2)} {RichTextModify(factor)} per level.";

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.DamageFactor -= perk.currentLevel * factor;
                    }
                }
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.DamageFactor += perk.currentLevel * factor;
                    }
                }
            }
        }

        public class Attribute2 : PerkSkill
        {
            public float factor;

            public BuildManager.ModifyTurretHandler handler;

            public Attribute2(Perk perk) : base(perk)
            {
                uniquePerkSkillName = $"{nameof(MadBomber)}.{nameof(Attribute2)}";
                handler += (turret, cash) =>
                {
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.MaxRange = (int)(turret.MaxRange * (1 + factor * perk.currentLevel));
                    }
                };
            }

            public override string Description() => $"Increase max range of {RichTextModify("SharpnelThrower", 2)}, {RichTextModify("PatriotMissile", 2)} and " +
                $"{RichTextModify("RocketLauncher", 2)} {RichTextModify(factor)} per level.";

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.MaxRange = (int)(turret.MaxRange / (1 + factor * perk.currentLevel));
                    }
                }
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.MaxRange = (int)(turret.MaxRange * (1 + factor * perk.currentLevel));
                    }
                }
            }
        }

        public class Attribute3 : PerkSkill
        {
            public float factor;

            public EnemySpawner.SpawnEnemyHandler handler;

            public Attribute3(Perk perk) : base(perk)
            {
                uniquePerkSkillName = $"{nameof(MadBomber)}.{nameof(Attribute3)}";
                handler += (enemy) =>
                {
                    enemy.rewardExp = (int)((1 + factor) * enemy.rewardExp);
                };
            }

            public override string Description() => $"Experience gained increase {RichTextModify(factor)}.";

            public override void OnExit()
            {
                gameManager.enemySpawner.SpawnEnemyEvent -= handler;
            }

            public override void OnStart()
            {
                gameManager.enemySpawner.SpawnEnemyEvent += handler;
            }
        }

        public class Skill1 : PerkSkill
        {
            public float deltaSpeedFactor;
            public BuildManager.ModifyTurretHandler handler;

            public Skill1(Perk perk) : base(perk)
            {
                uniquePerkSkillName = $"Speed Loader";
                handler += (turret, cash) =>
                {
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.AttackSpeed = (int)((1 + deltaSpeedFactor) * turret.AttackSpeed);
                    };
                };
            }

            public override string Description() => $"{uniquePerkSkillName}: Increase fire rate of {RichTextModify("SharpnelThrower", 2)}, {RichTextModify("PatriotMissile", 2)}, " +
                $"{RichTextModify("RocketLauncher", 2)} {deltaSpeedFactor}.";

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.AttackSpeed = (int)(turret.AttackSpeed / (1 + deltaSpeedFactor));
                    };
                }
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.AttackSpeed = (int)((1 + deltaSpeedFactor) * turret.AttackSpeed);
                    };
                }
            }
        }

        public class Skill2 : PerkSkill
        {
            public float rangeFactor;
            public float damageFactor;

            public BuildManager.ModifyTurretHandler handler;

            public Skill2(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "Aggregation";
                handler += (turret, cash) =>
                {
                    if (turret is PatriotMissile || turret is RocketLauncher)
                    {
                        var buff = turret.ContainsBuff("ExplosiveEffect") as ExplosiveEffect;
                        turret.DamageFactor += damageFactor;
                        turret.MaxRange = (int)(turret.MaxRange * (1 - rangeFactor));
                    }
                };
            }

            public override string Description() => $"{uniquePerkSkillName}:Increase damage of {RichTextModify("PatriotMissile", 2)}, {RichTextModify("RocketLauncher", 2)} {RichTextModify(damageFactor)}" +
                $", but decrease explosion range of them {RichTextModify(rangeFactor)}.";

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret is PatriotMissile || turret is RocketLauncher)
                    {
                        var buff = turret.ContainsBuff("ExplosiveEffect") as ExplosiveEffect;
                        turret.DamageFactor -= damageFactor;
                        turret.MaxRange = (int)(turret.MaxRange / (1 - rangeFactor));
                    }
                }
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += handler;
                foreach(var turret in gameManager.player.turrets)
                {
                    if (turret is PatriotMissile || turret is RocketLauncher)
                    {
                        var buff = turret.ContainsBuff("ExplosiveEffect") as ExplosiveEffect;
                        turret.DamageFactor += damageFactor;
                        turret.MaxRange = (int)(turret.MaxRange * (1 - rangeFactor));
                    }
                }
            }
        }

        public class Skill3 : PerkSkill
        {
            public float factor;

            public BuildManager.ModifyTurretHandler handler;

            public Skill3(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "High explosion round";
                handler += (turret, cash) =>
                {
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.DamageFactor += factor;
                    }
                };
            }

            public override string Description() => $"{uniquePerkSkillName}: Increase the damage of {RichTextModify("SharpnelThrower", 2)}, {RichTextModify("PatriotMissile", 2)} and " +
                $"{RichTextModify("RocketLauncher", 2)} {RichTextModify(factor)}.";

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.DamageFactor -= factor;
                    }
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += handler;
                foreach (var turret in gameManager.player.turrets)
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.DamageFactor += factor;
                    }
            }
        }

        public class Skill4 : PerkSkill
        {
            public float stunDuration;

            public BuildManager.ModifyTurretHandler handler;

            public Skill4(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "Back Off";
                handler += (turret, cash) =>
                {
                    if (turret is PatriotMissile || turret is RocketLauncher)
                    {
                        var stunBuff = new StunEffect { duration = stunDuration, possibility = 1 };
                        stunBuff.uniqueBuffName = stunBuff.buffName + $":MadBomber.{uniquePerkSkillName}";
                        turret.AppendBuff(new StunEffect { duration = stunDuration, possibility = 1 });
                    }
                };
            }
            public override string Description() => $"{uniquePerkSkillName}: The direct hit of {RichTextModify("PatriotMissile", 2)} and {RichTextModify("RocketLauncher", 2)} can stun enemy" +
                $" in tiny, normal, giant size for {RichTextModify($"{stunDuration}s", 0)}.";

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret is PatriotMissile || turret is RocketLauncher)
                        turret.DeleteBuff($"StunEffect:MadBomber.{uniquePerkSkillName}");
                }
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret is PatriotMissile || turret is RocketLauncher)
                    {
                        var stunBuff = new StunEffect { duration = stunDuration, possibility = 1 };
                        stunBuff.uniqueBuffName = stunBuff.buffName + $":MadBomber.{uniquePerkSkillName}";
                        turret.AppendBuff(new StunEffect { duration = stunDuration, possibility = 1 });
                    }
                }
            }
        }

        public class Skill5 : PerkSkill
        {
            public float range;
            string uniqueBuffName;

            public BuildManager.ModifyTurretHandler handler;

            public Skill5(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "A New Type";
                uniqueBuffName = $"ExplosiveEffect:MadBomber.{uniquePerkSkillName}";
                handler += (turret, cash) =>
                {
                    if (turret.uniqueTurretName == "MachineGun" || turret.uniqueTurretName == "PillBox")
                    {
                        var ee = new ExplosiveEffect { damage = turret.Damage, radius = range, damageType = Turret.DamageType.Explosive, uniqueBuffName = uniqueBuffName };
                        ee.damageFactor = 0;
                        turret.AppendBuff(ee);
                        var temp = turret.DamageFactor;
                        turret.DamageFactor = ee.damage;
                        ee.damageFactor = temp;
                    }
                };
            }
            public override string Description() => $"{uniquePerkSkillName}:{RichTextModify("MachineGun", 2)}, {RichTextModify("PillBox", 2)} deals explosive damage with a range " +
                $"of {RichTextModify(range.ToString(), 0)}.";

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret.uniqueTurretName == "MachineGun" || turret.uniqueTurretName == "PillBox")
                    {
                        var ee = turret.ContainsBuff(uniqueBuffName) as ExplosiveEffect;
                        ee.damageFactor = 0;
                        var temp = turret.DamageFactor;
                        turret.DamageFactor = ee.damage;
                        ee.damageFactor = temp;
                        turret.DeleteBuff(uniqueBuffName);
                    }
                }
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret.uniqueTurretName == "MachineGun" || turret.uniqueTurretName == "PillBox")
                    {
                        var ee = new ExplosiveEffect { damage = turret.Damage, radius = range, damageType = Turret.DamageType.Explosive, uniqueBuffName = uniqueBuffName };
                        turret.AppendBuff(ee);
                        var temp = turret.DamageFactor;
                        turret.DamageFactor = ee.damage;
                        ee.damageFactor = temp;
                    }
                }
            }
        }

        public class Skill6 : PerkSkill
        {
            private float radius;
            private string uniqueBuffName;
            private BuildManager.ModifyTurretHandler handler;

            public Skill6(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "Explosion Type";
                uniqueBuffName = "ExplosiveEffect:ExplosionType";
                handler += (turret, cash) =>
                {
                    if (turret.uniqueTurretName == "MachineGun" || turret.uniqueTurretName == "PillBox")
                    {
                        turret.HasDirectHitDamage = false;
                        turret.AppendBuff(new ExplosiveEffect
                        {
                            damage = turret.Damage,
                            damageType = Turret.DamageType.Explosive,
                            radius = radius,
                            uniqueBuffName = uniqueBuffName
                        });
                    }
                };
            }

            public override string Description() => $"{uniquePerkSkillName}";

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret.uniqueTurretName == "MachineGun" || turret.uniqueTurretName == "PillBox")
                    {
                        turret.HasDirectHitDamage = true;
                        turret.DeleteBuff(uniqueBuffName);
                    }
                }
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret.uniqueTurretName == "MachineGun" || turret.uniqueTurretName == "PillBox")
                    {
                        turret.HasDirectHitDamage = false;
                        turret.AppendBuff(new ExplosiveEffect
                        {
                            damage = turret.Damage,
                            damageType = Turret.DamageType.Explosive,
                            radius = radius,
                            uniqueBuffName = uniqueBuffName
                        });
                    }
                }
            }
        }

        public class Skill7 : PerkSkill
        {
            private float factor;
            private BuildManager.ModifyTurretHandler handler;

            public Skill7(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "Ranged Strike";
                handler += (turret, cash) =>
                {
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.MaxRange = (int)((1 + factor) * turret.MaxRange);
                    }
                };
            }

            public override string Description() => $"{uniquePerkSkillName}: Increase the range of {RichTextModify("SharpnelThrower", 2)}, {RichTextModify("PatriotMissile", 2)} and " +
                $"{RichTextModify("RocketLauncher", 2)} {RichTextModify(factor)}.";

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.MaxRange = (int)(turret.MaxRange / (1 + factor));
                    }
                }
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.MaxRange = (int)((1 + factor) * turret.MaxRange);
                    }
                }
            }
        }

        public class Skill8 : PerkSkill
        {
            private class ExplosiveEffectBully : ExplosiveEffect
            {
                public ExplosiveEffectBully() : base()
                {
                    startBuff = null;
                    startBuff += (enemy) =>
                    {
                        Collider[] collider = Physics.OverlapSphere(enemy.transform.position, radius / GameSettings.lengthFactor, 1 << LayerMask.NameToLayer("Enemy"));
                        foreach (var col in collider)
                        {
                            var e = col.GetComponent<Enemy>();
                            if (e.type == Enemy.Size.Normal || e.type == Enemy.Size.Tiny)
                            {
                                e.TakeDamage(damage, damageType, true);
                            }
                            else
                            {
                                ignoreResistance.TryGetValue(damageType, out var b);
                                col.GetComponent<Enemy>().TakeDamage(damage, damageType, b);
                            }
                        }
                    };
                }
            }

            private float radius;
            private string uniqueBuffName;
            private readonly BuildManager.ModifyTurretHandler handler;

            public Skill8(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "Bully";
                uniqueBuffName = "ExplosiveEffect:Bully";
                handler += (turret, cash) =>
                {
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                        turret.AppendBuff(new ExplosiveEffectBully
                        {
                            radius = radius,
                            uniqueBuffName = uniqueBuffName,
                            damage = turret.Damage,
                            damageType = Turret.DamageType.Explosive
                        });
                };
            }
            public override string Description() => $"{uniquePerkSkillName}: {RichTextModify("SharpnelThrower", 2)}, {RichTextModify("PatriotMissile", 2)} and " +
                $"{RichTextModify("RocketLauncher", 2)} ignore the explosive resistance of enemies typed Tiny or Small.";

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.DeleteBuff(uniqueBuffName);
                    }
                }
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret is SharpnelThrower || turret is PatriotMissile || turret is RocketLauncher)
                    {
                        turret.AppendBuff(new ExplosiveEffectBully
                        {
                            radius = radius,
                            uniqueBuffName = uniqueBuffName,
                            damage = turret.Damage,
                            damageType = Turret.DamageType.Explosive
                        });
                    }
                }
            }
        }

        public class Skill9 : PerkSkill
        {
            public Skill9(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "Final Core";
            }
            public override string Description()
            {
                throw new NotImplementedException();
            }

            public override void OnExit()
            {
                throw new NotImplementedException();
            }

            public override void OnStart()
            {
                throw new NotImplementedException();
            }
        }
    }
}