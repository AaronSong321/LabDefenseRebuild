using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Aaron.LabDefenseRebuild
{
    public class VChoosingPerk : MonoBehaviour
    {
        Button BOverview;
        Button BPerk;
        Button BQuit;

        Button BStart;
        Image IPerk1;
        Text XPerk1;
        Text XName1;

        CanvasGroup CGOverview;

        CanvasGroup CGPerk;
        Text XPerkAttributes;
        List<Button> BSkills;
        List<Text> XSkills;

        CanvasGroup CGCurrent;
        Player player;

        [SerializeField] Color skillEnableColor;
        [SerializeField] Color skillDisableColor;

        private void Awake()
        {
            BOverview = GameObject.Find("CMain/CGButton/BOverview").GetComponent<Button>();
            BPerk = GameObject.Find("CMain/CGButton/BPerk").GetComponent<Button>();
            BQuit = GameObject.Find("CMain/CGButton/BQuit").GetComponent<Button>();
            BOverview.onClick.AddListener(() =>
            {
                if (CGCurrent != CGOverview)
                {
                    CGCurrent = CGOverview;
                    CGOverview.gameObject.SetActive(true);
                    CGPerk.gameObject.SetActive(false);
                }
            });
            BPerk.onClick.AddListener(() =>
            {
                if (CGCurrent != CGPerk)
                {
                    CGCurrent = CGPerk;
                    CGPerk.gameObject.SetActive(true);
                    CGOverview.gameObject.SetActive(false);
                }
                RefreshSkillChosen();
            });
            BQuit.onClick.AddListener(() =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });

            BStart = GameObject.Find("CMain/CGSquad/BStart").GetComponent<Button>();
            BStart.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("BasicMap");
            });

            IPerk1 = GameObject.Find("CMain/CGSquad/CGPlayer1/IPerk").GetComponent<Image>();
            XPerk1 = GameObject.Find("CMain/CGSquad/CGPlayer1/XPerk").GetComponent<Text>();
            XName1 = GameObject.Find("CMain/CGSquad/CGPlayer1/XName").GetComponent<Text>();

            CGOverview = GameObject.Find("CMain/CGOverview").GetComponent<CanvasGroup>();

            #region CanvasGroup Perk Skill
            player = GameObject.Find("Player").GetComponent<Player>();
            player.viewChoosingPerk = this;
            BSkills = new List<Button>();
            XSkills = new List<Text>();
            for (int i = 1 ; i <= 10; i++)
            {
                BSkills.Add(GameObject.Find($"CMain/CGPerk/CGPerkSkill/BSkill{i.ToString()}").GetComponent<Button>());
                XSkills.Add(GameObject.Find($"CMain/CGPerk/CGPerkSkill/BSkill{i.ToString()}/Text").GetComponent<Text>());
            }
            BSkills[0].onClick.AddListener(() => player.ChoosePerkSkill(0));
            BSkills[1].onClick.AddListener(() => player.ChoosePerkSkill(1));
            BSkills[2].onClick.AddListener(() => player.ChoosePerkSkill(2));
            BSkills[3].onClick.AddListener(() => player.ChoosePerkSkill(3));
            BSkills[4].onClick.AddListener(() => player.ChoosePerkSkill(4));
            BSkills[5].onClick.AddListener(() => player.ChoosePerkSkill(5));
            BSkills[6].onClick.AddListener(() => player.ChoosePerkSkill(6));
            BSkills[7].onClick.AddListener(() => player.ChoosePerkSkill(7));
            BSkills[8].onClick.AddListener(() => player.ChoosePerkSkill(8));
            BSkills[9].onClick.AddListener(() => player.ChoosePerkSkill(9));
            CGPerk = GameObject.Find("CMain/CGPerk").GetComponent<CanvasGroup>();
            XPerkAttributes = GameObject.Find("CMain/CGPerk/Background/XPerkAttributes").GetComponent<Text>();
            foreach (var perkName in GameSettings.perkNameList)
            {
                var image = GameObject.Find($"CMain/CGPerk/CGPerks/CG{perkName}/Image").GetComponent<Image>();
                image.sprite = Resources.Load<Sprite>($"Icons/Perks/{perkName}/small");
                var text = GameObject.Find($"CMain/CGPerk/CGPerks/CG{perkName}/Text").GetComponent<Text>();
                text.text = $"{perkName} Level{player.GetPerkInfo(perkName).currentLevel}";
                var button = GameObject.Find($"CMain/CGPerk/CGPerks/CG{perkName}").GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    if (!player.currentPerkName.Equals(perkName))
                    {
                        player.ChangePerk(perkName);
                        RefreshSquad();
                        RefreshSkillChosen();
                    }
                    player.WriteToFile();
                });
            }
            CGPerk.gameObject.SetActive(false);
            #endregion

            CGCurrent = CGOverview;
            RefreshSquad();
            RefreshSkillChosen();
        }

        public void RefreshSquad()
        {
            IPerk1.sprite = Resources.Load<Sprite>($"Icons/Perks/{player.currentPerkName}/small");
            XPerk1.text = $"Level{player.CurrentLevelOfPerk} {player.currentPerkName}";
            XName1.text = $"{player.userName}";
        }

        public void RefreshSkillChosen()
        {
            var index = GameSettings.perkNameList.IndexOf(player.currentPerkName);
            for (int i = 0; i < 10; i++)
            {
                XSkills[i].text = (player.currentPerk.perkSkills[i] as PerkSkill).Description();
                if (player.perkInfo[index].skillChosen[i])
                {
                    BSkills[i].gameObject.GetComponent<Image>().color = skillEnableColor;
                }
                else
                {
                    BSkills[i].gameObject.GetComponent<Image>().color = skillDisableColor;
                }
            }
            XPerkAttributes.text = player.currentPerk.AttributeDesciption();
        }
    }
}