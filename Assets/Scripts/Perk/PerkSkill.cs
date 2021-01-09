using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public abstract class PerkSkill
    {
        [HideInInspector] public string uniquePerkSkillName;
        [HideInInspector] public GameManager gameManager;
        [HideInInspector] public Perk perk;
        public abstract string Description();
        public abstract void OnStart();
        public abstract void OnExit();

        public PerkSkill(Perk perk)
        {
            this.perk = perk;
        }
    }
}