using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Aaron.LabDefenseRebuild
{
    public abstract class Perk : MonoBehaviour
    {
        public string uniquePerkName;
        public List<PerkSkill> perkAttributes;
        public List<PerkSkill> perkSkills;
        [HideInInspector] public bool[] skillChosen;

        [HideInInspector] public int currentLevel;
        [HideInInspector] public int maxLevel;
        protected bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (isActive == false)
                {
                    isActive = true;
                    foreach (var attribute in perkAttributes)
                        attribute.OnStart();
                    foreach (var skill in perkSkills)
                        skill.OnStart();
                }
                else
                {
                    isActive = false;
                    foreach (var attribute in perkAttributes)
                        attribute.OnExit();
                    foreach (var skill in perkSkills)
                        skill.OnExit();
                }
            }
        }

        protected GameManager gameManager;
        protected Player player;

        protected virtual void Awake()
        {
            player = GameObject.Find("Player").GetComponent<Player>();
            var sceneName = SceneManager.GetActiveScene().name;
            if (sceneName != "Welcome" && sceneName != "ChoosingPerk")
                gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            perkAttributes = new List<PerkSkill>();
            perkSkills = new List<PerkSkill>();
            skillChosen = new bool[10];
            Init();
            ReadFromPlayer();
            isActive = false;
        }

        protected abstract void ReadFromPlayer();
        protected abstract void Init();
        
        public string AttributeDesciption()
        {
            var ans = "";
            foreach (var attribute in perkAttributes)
                ans += $"{attribute.Description()}\n";
            return ans;
        }
    }
}