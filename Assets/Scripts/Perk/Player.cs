using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aaron.LabDefenseRebuild
{
    public class Player : MonoBehaviour
    {
        public class PerkInfo
        {
            [HideInInspector] public string uniquePerkName;
            [HideInInspector] public int[] levelupExp;
            [HideInInspector] public int maxLevel;
            public List<Dictionary<string, string>> attributeData;
            public List<Dictionary<string, string>> skillData;

            [HideInInspector] public bool[] skillChosen;
            [HideInInspector] public int currentLevel;
            [HideInInspector] public int currentExp;

            public PerkInfo()
            {
                attributeData = new List<Dictionary<string, string>>();
                skillData = new List<Dictionary<string, string>>();
                skillChosen = new bool[10];
            }
        }

        [HideInInspector] public string currentPerkName;
        [HideInInspector] public List<PerkInfo> perkInfo;
        [HideInInspector] public List<Turret> turrets;
        [HideInInspector] public List<int> expBuffer;
        [HideInInspector] public string userName;
        protected List<int> expRequire;

        GameManager gameManager;
        [HideInInspector] public VChoosingPerk viewChoosingPerk;

        [HideInInspector] public MechanicMaster mm;
        [HideInInspector] public MadBomber mb;
        [HideInInspector] public Perk currentPerk;

        public void InitPerks()
        {
            mm = GameObject.Find("Player/MechanicMaster").AddComponent<MechanicMaster>();
            mb = GameObject.Find("Player/MadBomber").AddComponent<MadBomber>();
            switch (currentPerkName)
            {
                case "MechanicMaster":currentPerk = mm;break;
                case "MadBomber":currentPerk = mb;break;
            }
        }

        public void ChangePerk(string nextPerkName)
        {
            if (currentPerkName.Equals(nextPerkName))
                return;
            else
            {
                switch (nextPerkName)
                {
                    case "MechanicMaster":
                        currentPerk = mm;
                        break;
                    case "MadBomber":
                        currentPerk = mb;
                        break;
                }
                currentPerkName = nextPerkName;
            }
        }

        public void GetGameManager(GameManager gm)
        {
            gameManager = gm;
            foreach (var attribute in mm.perkAttributes)
                attribute.gameManager = gm;
            foreach (var skill in mm.perkSkills)
                skill.gameManager = gm;
            foreach (var attribute in mb.perkAttributes)
                attribute.gameManager = gm;
            InitWithGameManager();
        }

        private void InitWithGameManager()
        {
            if (SceneManager.GetActiveScene().name.Equals("BasicMap"))
            {
                gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                gameManager.enemySpawner.SpawnEnemyEvent += (enemy) =>
                {
                    enemy.EnemyKilledEvent += (e) =>
                    {
                        expBuffer[IndexOfCurrentPerk] += e.rewardExp;
                    };
                };
                gameManager.gameMode.VictoryEvent += WriteToFile;
                gameManager.gameMode.DefeatEvent += WriteToFile;
            }
        }

        private void Awake()
        {
            perkInfo = new List<PerkInfo>();
            turrets = new List<Turret>();
            DontDestroyOnLoad(gameObject);
            expBuffer = new List<int> { 0, 0, 0, 0, 0 };
            expRequire = new List<int>();
            ReadSystemData();
        }

        private void ReadSystemData()
        {
            #region Read ExpRequire from system data
            var expRequireReader = XmlReader.Create(Settings.ExpRequirePath(), new XmlReaderSettings { IgnoreComments = true });
            var expRequireDocument = new XmlDocument();
            expRequireDocument.Load(expRequireReader);
            var expList = expRequireDocument.SelectSingleNode("ExpRequire").ChildNodes;
            for (int i = 0; i < expList.Count; i++)
            {
                expRequire.Add(Int32.Parse((expList[i] as XmlElement).GetAttribute("exp")));
            }
            #endregion

            #region Read MechanicMaster From System Data
            XmlDocument document = new XmlDocument();
            document.Load(Settings.GenerateStreamingPath($"System/Perk/MechanicMaster/data.xml"));
            var attributeList = document.SelectSingleNode("Perk").ChildNodes[0];

            var newPerk = new PerkInfo
            {
                uniquePerkName = GameSettings.perkNameList[0],
                maxLevel = 50
            };
            perkInfo.Add(newPerk);

            newPerk.attributeData.Add(new Dictionary<string, string>
            {
                { "factor", (attributeList.ChildNodes[0] as XmlElement).GetAttribute("factor") }
            });
            newPerk.attributeData.Add(new Dictionary<string, string>
            {
                { "factor",(attributeList.ChildNodes[1] as XmlElement).GetAttribute("factor") }
            });
            newPerk.attributeData.Add(new Dictionary<string, string>
            {
                { "deltaSpeed", (attributeList.ChildNodes[2] as XmlElement).GetAttribute("deltaSpeed") }
            });
            newPerk.attributeData.Add(new Dictionary<string, string>
            {
                { "factor",(attributeList.ChildNodes[3] as XmlElement).GetAttribute("factor") }
            });

            var skillList = document.SelectSingleNode("Perk").ChildNodes[1];
            newPerk.skillData.Add(new Dictionary<string, string>
            {
                { "factor", (skillList.ChildNodes[0] as XmlElement).GetAttribute("factor") }
            });
            newPerk.skillData.Add(new Dictionary<string, string>
            {
                { "factor", (skillList.ChildNodes[1] as XmlElement).GetAttribute("factor") }
            });
            newPerk.skillData.Add(null);
            newPerk.skillData.Add(new Dictionary<string, string>
            {
                { "deltaSpeed", (skillList.ChildNodes[3] as XmlElement).GetAttribute("deltaSpeed") }
            });
            newPerk.skillData.Add(new Dictionary<string, string>
            {
                { "factor1", (skillList.ChildNodes[4] as XmlElement).GetAttribute("factor1") },
                { "factor2", (skillList.ChildNodes[4] as XmlElement).GetAttribute("factor2") },
                { "factor3", (skillList.ChildNodes[4] as XmlElement).GetAttribute("factor3") },
                { "duration1", (skillList.ChildNodes[4] as XmlElement).GetAttribute("duration1") }
            });
            newPerk.skillData.Add(null);
            newPerk.skillData.Add(new Dictionary<string, string>
            {
                { "deltaSpeed1", (skillList.ChildNodes[6] as XmlElement).GetAttribute("deltaSpeed1") },
                { "deltaSpeed2", (skillList.ChildNodes[6] as XmlElement).GetAttribute("deltaSpeed2") },
                { "deltaSpeed3", (skillList.ChildNodes[6] as XmlElement).GetAttribute("deltaSpeed3") }
            });
            newPerk.skillData.Add(new Dictionary<string, string>
            {
                { "factor", (skillList.ChildNodes[7] as XmlElement).GetAttribute("factor") }
            });
            newPerk.skillData.Add(new Dictionary<string, string>
            {
                { "factor", (skillList.ChildNodes[8] as XmlElement).GetAttribute("factor") },
                { "upperBound", (skillList.ChildNodes[8] as XmlElement).GetAttribute("upperBound") }
            });
            newPerk.skillData.Add(new Dictionary<string, string>
            {
                { "factor1", (skillList.ChildNodes[9] as XmlElement).GetAttribute("factor1") },
                { "factor2", (skillList.ChildNodes[9] as XmlElement).GetAttribute("factor2") },
                { "factor3", (skillList.ChildNodes[9] as XmlElement).GetAttribute("factor3") },
            });

            #endregion

            #region Read MadBomber from System Data
            document.Load(Settings.GenerateStreamingPath("System/Perk/MadBomber/Data.xml"));
            attributeList = document.SelectSingleNode("Perk").ChildNodes[0];
            newPerk = new PerkInfo
            {
                uniquePerkName = GameSettings.perkNameList[1],
                maxLevel = 50
            };
            perkInfo.Add(newPerk);

            newPerk.attributeData.Add(new Dictionary<string, string>
            {

            });

            #endregion
            for (int i = 2; i <= 4; i++)
            {
                perkInfo.Add(new PerkInfo
                {
                    uniquePerkName = GameSettings.perkNameList[i]
                });
            }
        }

        public int IndexOfCurrentPerk => GameSettings.perkNameList.IndexOf(currentPerkName);
        public int CurrentLevelOfPerk => perkInfo[IndexOfCurrentPerk].currentLevel;
        public PerkInfo GetPerkInfo(string perkName)
        {
            foreach (var perk in perkInfo)
            {
                if (perk.uniquePerkName == perkName)
                    return perk;
            }
            return null;
        }
        public PerkInfo GetCurrentPerkInfo => GetPerkInfo(currentPerkName);

        public void WriteToFile()
        {
            var document = new XmlDocument();
            document.Load(Settings.PlayersPath());
            foreach (XmlElement playerInfo in document.SelectSingleNode("Players").ChildNodes)
            {
                if (playerInfo.GetAttribute("name").Equals(userName))
                {
                    var perkList = playerInfo.ChildNodes[0].ChildNodes;
                    for (int i = 0; i < GameSettings.perkNameList.Count; i++)
                    {
                        var exp = perkInfo[i].currentExp + expBuffer[i];
                        while (perkInfo[i].currentLevel < perkInfo[i].maxLevel && exp >= expRequire[perkInfo[i].currentLevel])
                        {
                            exp -= expRequire[perkInfo[i].currentLevel];
                            perkInfo[i].currentLevel++;
                        }
                        expBuffer[i] = 0;
                        var perk = perkList[i] as XmlElement;
                        perk.SetAttribute("level", perkInfo[i].currentLevel.ToString());
                        perk.SetAttribute("exp", exp.ToString());
                        var skillList = perk.ChildNodes;
                        for (int j = 0; j < perkInfo[i].skillChosen.Length; j++)
                            (skillList[j] as XmlElement).SetAttribute("chosen", perkInfo[i].skillChosen[j].ToString());
                    }
                }
            }
            document.Save(Settings.PlayersPath());
        }

        public void ChoosePerkSkill(int index)
        {
            if (index % 2 == 1)
            {
                if (CurrentLevelOfPerk >= (index / 2 + 1 ) * 10 && !GetCurrentPerkInfo.skillChosen[index])
                {
                    GetCurrentPerkInfo.skillChosen[index] = true;
                    GetCurrentPerkInfo.skillChosen[index - 1] = false;
                    viewChoosingPerk?.RefreshSkillChosen();
                    WriteToFile();
                }
            }
            else
            {
                if (CurrentLevelOfPerk >= (index / 2 + 1) * 10 && !GetCurrentPerkInfo.skillChosen[index])
                {
                    GetCurrentPerkInfo.skillChosen[index] = true;
                    GetCurrentPerkInfo.skillChosen[index + 1] = false;
                    viewChoosingPerk?.RefreshSkillChosen();
                    WriteToFile();
                }
            }
        }
    }
}