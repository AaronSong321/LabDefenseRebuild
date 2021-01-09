using System.Collections.Generic;

namespace Aaron.LabDefenseRebuild
{
    public static class GameSettings
    {
        public enum Difficulty { Hypothetical, Rival, Challenging, Desperate }

        public static int lengthFactor = 20;
        public static readonly List<string> perkNameList = new List<string>
        {
            "MechanicMaster", "MadBomber","FireRanger", "ThunderSpirit","BaseballCoach"
        };
        public static readonly List<string> turretNameList = new List<string>
        {
            "MachineGun","CrossbowHunter","PillBox","SniPer","SharpnelThrower","PatriotMissile","RocketLauncher","FlameThrower","MolotovCocktail","MicroWave","PS","TransForm","Thunder"
        };
        public static readonly List<string> enemyNameList = new List<string>
        {
            "Elfin","Crawler","Zombie","Thirsty","Butcher","D","Desolator","Mammoth","Tank","Dragon"
        };
    }
}