using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public class LoginManager : MonoBehaviour
    {
        public enum RegisterState { AlreadyExists, Success }
        public enum LoginState { NoSuchUserName, WrongPassword, Success }

        Player currentPlayer;

        private void Awake()
        {
            currentPlayer = GameObject.Find("Player").GetComponent<Player>();
        }

        public RegisterState Register(string name, string password)
        {
            if (!Directory.Exists(Settings.GeneratePersistentPath("Player")))
                Directory.CreateDirectory(Settings.GeneratePersistentPath("Player"));
            XmlDocument playerDocument = new XmlDocument();
            if (!File.Exists(Settings.PlayersPath()))
            {
                XmlElement root = playerDocument.CreateElement("Players");
                playerDocument.AppendChild(root);
                playerDocument.Save(Settings.PlayersPath());
            }
            playerDocument.Load(Settings.PlayersPath());
            var playerList = playerDocument.SelectSingleNode("Players").ChildNodes;
            foreach (XmlElement player in playerList)
            {
                if (player.GetAttribute("name") == name)
                {
                    return RegisterState.AlreadyExists;
                }
            }

            XmlElement newPlayer = playerDocument.CreateElement("Player");
            newPlayer.SetAttribute("name", name);
            newPlayer.SetAttribute("password", Settings.EncodeByMd5(password));
            XmlElement perkNodeList = playerDocument.CreateElement("Perks");
            perkNodeList.SetAttribute("currentPerkName", "MechanicMaster");
            var perkNameList = new List<string>
            {
                "MechanicMaster", "MadBomber", "FireRanger", "ThunderSpirit", "BaseballCoach"
            };
            foreach (var perkName in perkNameList)
            {
                XmlElement perkNode = playerDocument.CreateElement("Perk");
                perkNode.SetAttribute("name", perkName);
                perkNode.SetAttribute("level", "0");
                perkNode.SetAttribute("exp", "0");
                for (int i = 1; i < 11; i++)
                {
                    XmlElement skillNode = playerDocument.CreateElement("Skill");
                    skillNode.SetAttribute("chosen", "false");
                    perkNode.AppendChild(skillNode);
                }
                perkNodeList.AppendChild(perkNode);
            }
            newPlayer.AppendChild(perkNodeList);
            playerDocument.SelectSingleNode("Players").AppendChild(newPlayer);
            playerDocument.Save(Settings.PlayersPath());
            return RegisterState.Success;
        }

        public LoginState Login(string name, string password)
        {
            XmlDocument document = new XmlDocument();
            document.Load(Settings.PlayersPath());
            XmlNodeList playerList = document.SelectSingleNode("Players").ChildNodes;
            foreach (XmlElement player in playerList)
            {
                if (player.GetAttribute("name").Equals(name))
                {
                    if (player.GetAttribute("password").Equals(Settings.EncodeByMd5(password)))
                    {
                        #region Read Player data from file
                        currentPlayer.currentPerkName = (player.ChildNodes[0] as XmlElement).GetAttribute("currentPerkName");
                        var perkNodeList = player.ChildNodes[0].ChildNodes;
                        for (int i=0; i<5; i++)
                        {
                            currentPlayer.perkInfo[i].currentLevel = Int32.Parse((perkNodeList[i] as XmlElement).GetAttribute("level"));
                            currentPlayer.perkInfo[i].currentExp = Int32.Parse((perkNodeList[i] as XmlElement).GetAttribute("exp"));
                            var skillList = currentPlayer.perkInfo[i].skillChosen;
                            for (int j = 0; j < 10; j++)
                            {
                                skillList[j] = Boolean.Parse((perkNodeList[i].ChildNodes[j] as XmlElement).GetAttribute("chosen"));
                            }
                        }
                        currentPlayer.userName = name;
                        #endregion
                        currentPlayer.InitPerks();
                        return LoginState.Success;
                    }
                    else
                        return LoginState.WrongPassword;
                }
            }
            return LoginState.NoSuchUserName;
        }
    }
}