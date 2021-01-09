using static Aaron.LabDefenseRebuild.Settings;
using System;
using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public class MechanicMaster : Perk
    {
        public class Attribute1 : PerkSkill
        {
            public float factor;

            public BuildManager.ModifyTurretHandler handler;

            public Attribute1(Perk perk) : base(perk)
            {
                uniquePerkSkillName = $"MechanicMaster.Attribute1";
                handler += (turret, cash) =>
                {
                    if (turret.uniqueTurretName.Equals("MachineGun") || turret.uniqueTurretName.Equals("PillBox"))
                    {
                        turret.DamageFactor += perk.currentLevel * factor;
                    }
                };
            }

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret.uniqueTurretName.Equals("MachineGun") || turret.uniqueTurretName.Equals("PillBox"))
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
                    if (turret.uniqueTurretName.Equals("MachineGun") || turret.uniqueTurretName.Equals("PillBox"))
                    {
                        turret.DamageFactor += perk.currentLevel * factor;
                    }
                }
            }

            public override string Description() => $"Increase {RichTextModify("MachineGun", 2)},{RichTextModify("PillBox", 2)} base damage" +
                    $"{RichTextModify(factor)} per level.";
        }

        public class Attribute2 : PerkSkill
        {
            public float factor;

            public BuildManager.ModifyTurretHandler handler;

            public Attribute2(Perk perk) : base(perk)
            {
                uniquePerkSkillName = $"MechanicMaster.Attribute1: Sniper and CrossbowHunter basic damage increase";
                handler += (turret, cash) =>
                {
                    if (turret.uniqueTurretName.Equals("Sniper") || turret.uniqueTurretName.Equals("CrossbowHunter"))
                    {
                        turret.DamageFactor += perk.currentLevel * factor;
                    }
                };
            }

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret.uniqueTurretName.Equals("Sniper") || turret.uniqueTurretName.Equals("CrossbowHunter"))
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
                    if (turret.uniqueTurretName.Equals("Sniper") || turret.uniqueTurretName.Equals("CrossbowHunter"))
                    {
                        turret.DamageFactor += perk.currentLevel * factor;
                    }
                }
            }

            public override string Description() => $"Increase {RichTextModify("Sniper", 2)},{RichTextModify("CrossbowHunter", 2)} base damage" +
                    $"{RichTextModify($"{Mathf.RoundToInt(factor)}%", 0)} per level.";
        }

        public class Attribute3 : PerkSkill
        {
            public int deltaSpeed;

            public BuildManager.ModifyTurretHandler handler;

            public Attribute3(Perk perk) : base(perk)
            {
                uniquePerkSkillName = $"MechanicMaster.Attribute1: Sniper and CrossbowHunter fire rate increase";
                handler += (turret, cash) =>
                {
                    if (turret.uniqueTurretName.Equals("Sniper") || turret.uniqueTurretName.Equals("CrossbowHunter"))
                    {
                        turret.AttackSpeed = turret.AttackSpeed + perk.currentLevel / 10 * deltaSpeed;
                    }
                };
            }

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret.uniqueTurretName.Equals("Sniper") || turret.uniqueTurretName.Equals("CrossbowHunter"))
                    {
                        turret.AttackSpeed = turret.AttackSpeed - perk.currentLevel / 10 * deltaSpeed;
                    }
                }
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret.uniqueTurretName.Equals("Sniper") || turret.uniqueTurretName.Equals("CrossbowHunter"))
                    {
                        turret.AttackSpeed = turret.AttackSpeed + perk.currentLevel / 10 * deltaSpeed;
                    }
                }
            }

            public override string Description() => $"Increase {RichTextModify("Sniper", 2)},{RichTextModify("CrossbowHunter", 2)} fire rate" +
                    $"{RichTextModify($"{deltaSpeed}", 0)} per 10 level.";
        }

        public class Attribute4 : PerkSkill
        {
            public float factor;

            public GameMode.GainWaveCashHandler handler;

            public Attribute4(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "MechanicMaster.Attribute4: Initialized cash bonus";
                handler += (wave, cash) =>
                {
                    if (gameManager.gameMode.currentWave == 1)
                        gameManager.gameMode.currentCash += (int)(cash * factor);
                };
            }

            public override void OnExit()
            {
                gameManager.gameMode.GainWaveCashEvent -= handler;
            }

            public override void OnStart()
            {
                gameManager.gameMode.GainWaveCashEvent += handler;
            }

            public override string Description() => $"Increase Initialized cash bonus in wave 1 by {RichTextModify($"{Mathf.RoundToInt(factor * 100)}%", 0)}.";
        }

        public class Skill1 : PerkSkill
        {
            public float factor;

            public BuildManager.ModifyTurretHandler handler;

            public Skill1(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "Armour Piercing Bullet";
                handler += (turret, cash) =>
                {
                    var utn = turret.uniqueTurretName;
                    if (utn.Equals("MachineGun") || utn.Equals("CrossbowHunter") || utn.Equals("PillBox") || utn.Equals("SniPer"))
                    {
                        turret.DamageFactor += factor;
                    }
                };
            }

            public override string Description() => $"{uniquePerkSkillName}: Increase the damage of {RichTextModify("MachineGun", 2)}, {RichTextModify("CrossbowHunter", 2)}" +
                $", {RichTextModify("PillBox", 2)}, {RichTextModify("Sniper", 2)} by {RichTextModify(factor)}.";

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    var utn = turret.uniqueTurretName;
                    if (utn.Equals("MachineGun") || utn.Equals("CrossbowHunter") || utn.Equals("PillBox") || utn.Equals("SniPer"))
                    {
                        turret.DamageFactor -= factor;
                    }
                };
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    var utn = turret.uniqueTurretName;
                    if (utn.Equals("MachineGun") || utn.Equals("CrossbowHunter") || utn.Equals("PillBox") || utn.Equals("SniPer"))
                    {
                        turret.DamageFactor += factor;
                    }
                };
            }
        }

        public class Skill2 : PerkSkill
        {
            public float factor;

            public EnemySpawner.SpawnEnemyHandler handler;
            
            public Skill2(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "Cash Bonus";
                handler += (enemy) =>
                {
                    enemy.EnemyKilledEvent += (e) =>
                    {
                        e.rewardcash = (int)(e.rewardcash * (1 + factor));
                    };
                };
            }

            public override string Description() => $"{uniquePerkSkillName}: Every enemy killed provides an extra cash bonus of {RichTextModify(factor)}.";

            public override void OnExit()
            {
                gameManager.enemySpawner.SpawnEnemyEvent -= handler;
            }

            public override void OnStart()
            {
                gameManager.enemySpawner.SpawnEnemyEvent += handler;
            }
        }

        public class Skill3 : PerkSkill
        {
            public Skill3(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "Never Fly Away";
                handler += (turret, cash) =>
                {
                    if (turret.uniqueTurretName.Equals("MachineGun") || turret.uniqueTurretName.Equals("PillBox"))
                        turret.targetFlyable = Turret.TargetFlyable.GroundAir;
                };
            }

            public BuildManager.ModifyTurretHandler handler;

            public override string Description() => $"{uniquePerkSkillName}: Enable {RichTextModify("MachineGun", 0)}, {RichTextModify("PillBox", 0)} to attack flyaing targets.";

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                    if (turret.uniqueTurretName.Equals("MachineGun") || turret.uniqueTurretName.Equals("PillBox"))
                        turret.targetFlyable = Turret.TargetFlyable.Ground;
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += handler;
                foreach (var turret in gameManager.player.turrets)
                    if (turret.uniqueTurretName.Equals("MachineGun") || turret.uniqueTurretName.Equals("PillBox"))
                        turret.targetFlyable = Turret.TargetFlyable.GroundAir;
            }
        }

        public class Skill4 : PerkSkill
        {
            public int deltaSpeed;

            public BuildManager.ModifyTurretHandler handler;

            public Skill4(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "Ramrod and Reload";
                handler += (turret, cash) =>
                {
                    if (turret.uniqueTurretName.Equals("CrossbowHunter") || turret.uniqueTurretName.Equals("SniPer"))
                    {
                        turret.AttackSpeed = turret.AttackSpeed + deltaSpeed;
                    }
                };
            }

            public override string Description() => $"{uniquePerkSkillName}: Increase the fire rate of {RichTextModify("CrossbowHunter", 2)} and {RichTextModify("SniPer", 2)}" +
                $" {RichTextModify($"{deltaSpeed}", 0)}.";

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret.uniqueTurretName.Equals("CrossbowHunter") || turret.uniqueTurretName.Equals("SniPer"))
                    {
                        turret.AttackSpeed = turret.AttackSpeed - deltaSpeed;
                    }
                };
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += handler;
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret.uniqueTurretName.Equals("CrossbowHunter") || turret.uniqueTurretName.Equals("SniPer"))
                    {
                        turret.AttackSpeed = turret.AttackSpeed + deltaSpeed;
                    }
                };
            }
        }

        public class Skill5 : PerkSkill
        {
            public float factor1;
            public float factor2;
            public float factor3;
            public float duration1;
            public string uniqueBuffName;

            public BuildManager.ModifyTurretHandler handler;

            public Skill5(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "Bullets and Shells";
                uniqueBuffName = $"ExplosiveEffect:{perk.uniquePerkName}.{uniquePerkSkillName}";

                handler += (turret, cash) =>
                {
                    if (turret.uniqueTurretName.Equals("MachineGun") || turret.uniqueTurretName.Equals("PillBox"))
                    {
                        if (turret.ContainsBuff(uniqueBuffName) != null)
                        {
                            var neb = new DecelerateEffect
                            {
                                ratio = factor1,
                                duration = duration1,
                                uniqueBuffName = uniqueBuffName
                            };
                            switch (turret.currentLevel)
                            {
                                case 1:neb.ratio = factor1;break;
                                case 2:neb.ratio = factor2;break;
                                case 3:neb.ratio = factor3;break;
                            }
                            turret.enemyBuff.Add(neb);
                        }
                    }
                };
            }

            public override string Description() => $"{uniquePerkSkillName}: {RichTextModify("MachineGun", 2)} and {RichTextModify("PillBox", 2)} slows enemies by " +
                $"{RichTextModify(1 - factor1)}/{RichTextModify(1 - factor2)}/" +
                $"{RichTextModify(1 - factor3)}, depneding on the turret level. The duration is {RichTextModify($"{duration1}",0)}";

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                    if (turret.uniqueTurretName.Equals("MachineGun") || turret.uniqueTurretName.Equals("PillBox"))
                        turret.DeleteBuff(uniqueBuffName);
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += handler;
                foreach (var turret in gameManager.player.turrets)
                    if (turret.uniqueTurretName.Equals("MachineGun") || turret.uniqueTurretName.Equals("PillBox"))
                    {
                        if (turret.ContainsBuff(uniqueBuffName) != null)
                        {
                            var neb = new DecelerateEffect
                            {
                                ratio = factor1,
                                duration = duration1,
                                uniqueBuffName = uniqueBuffName
                            };
                            switch (turret.currentLevel)
                            {
                                case 1:neb.ratio = factor1;break;
                                case 2:neb.ratio = factor2;break;
                                case 3:neb.ratio = factor3;break;
                            }
                            turret.enemyBuff.Add(neb);
                        }
                    }
            }
        }

        public class Skill6 : PerkSkill
        {
            public override string Description() => $"";

            public override void OnExit()
            {

            }

            public override void OnStart()
            {

            }

            public Skill6(Perk perk) : base(perk)
            {

            }
        }

        public class Skill7 : PerkSkill
        {
            public int deltaSpeed1;
            public int deltaSpeed2;
            public int deltaSpeed3;

            public override string Description() => $"{uniquePerkSkillName}: Increase the fire rate of {RichTextModify("MachineGun", 2)}, {RichTextModify("PillBox", 2)} by" +
                    $" {RichTextModify($"{deltaSpeed1}", 0)}/{RichTextModify($"{deltaSpeed2}", 0)}/{RichTextModify($"{deltaSpeed3}", 0)}/.";

            public Skill7(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "Crazy Boy";
            }

            public override void OnExit()
            {
                foreach (var turret in gameManager.player.turrets)
                {
                    if (turret.uniqueTurretName.Equals("MachineGun") || turret.uniqueTurretName.Equals("PillBox"))
                    {
                        switch (turret.currentLevel)
                        {
                            case 1: turret.AttackSpeed -= deltaSpeed1; break;
                            case 2: turret.AttackSpeed -= deltaSpeed2; break;
                            case 3: turret.AttackSpeed -= deltaSpeed3; break;
                        }
                    }
                };
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += (turret, cash) =>
                {
                    if (turret.uniqueTurretName.Equals("MachineGun") || turret.uniqueTurretName.Equals("PillBox"))
                    {
                        switch (turret.currentLevel)
                        {
                            case 1: turret.AttackSpeed += deltaSpeed1; break;
                            case 2: turret.AttackSpeed += deltaSpeed2; break;
                            case 3: turret.AttackSpeed += deltaSpeed3; break;
                        }
                    }
                };
            }
        }

        public class Skill8 : PerkSkill
        {
            public float factor;

            public EnemySpawner.SpawnEnemyHandler handler;

            public Skill8(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "Millionare";
                handler += (enemy) =>
                {
                    enemy.EnemyKilledEvent += (e) =>
                    {
                        e.rewardcash = (int)(e.rewardcash * (1 + factor));
                    };
                };
            }

            public override string Description() => $"{uniquePerkSkillName}: Every giant enemy killed provides an extra cash bonus of {RichTextModify(factor)}." +
                $" This bonus stacks with the bonus of Cash Bonus.";

            public override void OnExit()
            {
                gameManager.enemySpawner.SpawnEnemyEvent -= handler;
            }

            public override void OnStart()
            {
                gameManager.enemySpawner.SpawnEnemyEvent += handler;
            }
        }

        public class Skill9 : PerkSkill
        {
            public float factor;
            public int upperBound;

            public Turret.AttackHandler handler;

            public Skill9(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "Fat Penalty";
                handler += (t, b, e) =>
                {
                    b.damage += (int)(e.initHealth * factor);
                };
            }

            public override string Description() => $"{uniquePerkSkillName}: Every shot from {RichTextModify("SniPer", 2)} deals an extra damage equals to {RichTextModify(factor)} of the maximum" +
                $"health of the target. The maximum extra damage is {RichTextModify($"{upperBound}", 0)}.";

            public override void OnExit()
            {
                foreach (var turret in gameManager.player.turrets)
                    if (turret.uniqueTurretName.Equals("SniPer"))
                    {
                        turret.AttackEvent -= handler;
                    }
            }

            public override void OnStart()
            {
                foreach (var turret in gameManager.player.turrets)
                    if (turret.uniqueTurretName.Equals("SniPer"))
                    {
                        turret.AttackEvent += handler;
                    }
            }
        }

        public class Skill10 : PerkSkill
        {
            public float factor1;
            public float factor2;
            public float factor3;

            private BuildManager.ModifyTurretHandler handler;

            public Skill10(Perk perk) : base(perk)
            {
                uniquePerkSkillName = "Dud Versus Head";
                handler += (turret, cash) =>
                {
                    if (turret is RocketLauncher)
                    {
                        if (turret.ContainsBuff("ExplosiveEffect") is ExplosiveEffect ee)
                            ee.bEnable = false;
                        switch (turret.currentLevel)
                        {
                            case 1: turret.DamageFactor += factor1; break;
                            case 2: turret.DamageFactor += factor2; break;
                            case 3: turret.DamageFactor += factor3; break;
                        }
                    }
                };
            }

            public override string Description() => $"{uniquePerkSkillName}: {RichTextModify("RocketLauncher", 2)} can shoot enemy in any distance; instead of exploding, the bullet deals " +
                $"{RichTextModify(factor1.ToString(), 0)}/{RichTextModify(factor2.ToString(), 0)}/{RichTextModify(factor3.ToString(), 0)} times of direct hit damage.";

            public override void OnExit()
            {
                gameManager.buildManager.ModifyTurretEvent -= handler;
                foreach (var turret in gameManager.player.turrets)
                    if (turret is RocketLauncher)
                    {
                        if (turret.ContainsBuff("ExplosiveBuff") is ExplosiveEffect ee)
                            ee.bEnable = true;
                        switch (turret.currentLevel)
                        {
                            case 1: turret.DamageFactor -= factor1; break;
                            case 2: turret.DamageFactor -= factor2; break;
                            case 3: turret.DamageFactor -= factor3; break;
                        }
                    }
            }

            public override void OnStart()
            {
                gameManager.buildManager.ModifyTurretEvent += handler;
                foreach (var turret in gameManager.player.turrets)
                    if (turret is RocketLauncher)
                    {
                        if (turret.ContainsBuff("ExplosiveEffect") is ExplosiveEffect ee)
                            ee.bEnable = false;
                        switch (turret.currentLevel)
                        {
                            case 1: turret.DamageFactor += factor1; break;
                            case 2: turret.DamageFactor += factor2; break;
                            case 3: turret.DamageFactor += factor3; break;
                        }
                    }
            }
        }

        protected override void Init()
        {
            perkAttributes.Add(new Attribute1(this));
            perkAttributes.Add(new Attribute2(this));
            perkAttributes.Add(new Attribute3(this));
            perkAttributes.Add(new Attribute4(this));

            perkSkills.Add(new Skill1(this));
            perkSkills.Add(new Skill2(this));
            perkSkills.Add(new Skill3(this));
            perkSkills.Add(new Skill4(this));
            perkSkills.Add(new Skill5(this));
            perkSkills.Add(new Skill6(this));
            perkSkills.Add(new Skill7(this));
            perkSkills.Add(new Skill8(this));
            perkSkills.Add(new Skill9(this));
            perkSkills.Add(new Skill10(this));
        }

        protected override void Awake()
        {
            uniquePerkName = "MechanicMaster";
            base.Awake();
        }

        private void Start()
        {
            if (isActive)
                foreach (var skill in perkAttributes)
                    skill.OnStart();
        }
        
        protected override void ReadFromPlayer()
        {
            foreach (var perk in player.perkInfo) 
                if (perk.uniquePerkName.Equals("MechanicMaster"))
                {
                    currentLevel = perk.currentLevel;
                    maxLevel = perk.maxLevel;

                    var att1 = perkAttributes[0] as Attribute1;
                    if (perk.attributeData[0].TryGetValue("factor", out string tempString))
                        att1.factor = Single.Parse(tempString);
                    var att2 = perkAttributes[1] as Attribute2;
                    if (perk.attributeData[1].TryGetValue("factor", out tempString))
                        att2.factor = Single.Parse(tempString);
                    var att3 = perkAttributes[2] as Attribute3;
                    if (perk.attributeData[2].TryGetValue("deltaSpeed", out tempString))
                        att3.deltaSpeed = Int32.Parse(tempString);
                    var att4 = perkAttributes[3] as Attribute4;
                    if (perk.attributeData[3].TryGetValue("factor", out tempString))
                        att4.factor = Single.Parse(tempString);

                    var skill1 = perkSkills[0] as Skill1;
                    if (perk.skillData[0].TryGetValue("factor", out tempString))
                        skill1.factor = Single.Parse(tempString);
                    var skill2 = perkSkills[1] as Skill2;
                    if (perk.skillData[1].TryGetValue("factor", out tempString))
                        skill2.factor = Single.Parse(tempString);
                    var skil4 = perkSkills[3] as Skill4;
                    if (perk.skillData[3].TryGetValue("deltaSpeed", out tempString))
                        skil4.deltaSpeed = Int32.Parse(tempString);
                    var skill5 = perkSkills[4] as Skill5;
                    if (perk.skillData[4].TryGetValue("factor1", out tempString))
                        skill5.factor1 = Single.Parse(tempString);
                    if (perk.skillData[4].TryGetValue("factor2", out tempString))
                        skill5.factor2 = Single.Parse(tempString);
                    if (perk.skillData[4].TryGetValue("factor3", out tempString))
                        skill5.factor3 = Single.Parse(tempString);
                    if (perk.skillData[4].TryGetValue("duration1", out tempString))
                        skill5.duration1 = Single.Parse(tempString);
                    var skill7 = perkSkills[6] as Skill7;
                    if (perk.skillData[6].TryGetValue("deltaSpeed1", out tempString))
                        skill7.deltaSpeed1 = Int32.Parse(tempString);
                    if (perk.skillData[6].TryGetValue("deltaSpeed2", out tempString))
                        skill7.deltaSpeed2 = Int32.Parse(tempString);
                    if (perk.skillData[6].TryGetValue("deltaSpeed3", out tempString))
                        skill7.deltaSpeed3 = Int32.Parse(tempString);
                    var skill8 = perkSkills[7] as Skill8;
                    if (perk.skillData[7].TryGetValue("factor", out tempString))
                        skill8.factor = Single.Parse(tempString);
                    var skill9 = perkSkills[8] as Skill9;
                    if (perk.skillData[8].TryGetValue("factor", out tempString))
                        skill9.factor = Single.Parse(tempString);
                    if (perk.skillData[8].TryGetValue("upperBound", out tempString))
                        skill9.upperBound = Int32.Parse(tempString);
                    var skill10 = perkSkills[9] as Skill10;
                    if (perk.skillData[9].TryGetValue("factor1", out tempString))
                        skill10.factor1 = Single.Parse(tempString);
                    if (perk.skillData[9].TryGetValue("factor2", out tempString))
                        skill10.factor2 = Single.Parse(tempString);
                    if (perk.skillData[9].TryGetValue("factor3", out tempString))
                        skill10.factor3 = Single.Parse(tempString);

                    break;
                }
        }
    }
}