using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public sealed class Wave
    {
        public int initCash;
        public float waveInterval;
        public float enemyInterval;
        public List<GameObject> enemies;

        public static Wave ReadWave(XmlNode node, List<GameObject> enemyPrefabs)
        {
            Wave ans = new Wave();
            var waveInfo = node.ChildNodes;

            var init = waveInfo[0] as XmlElement;
            ans.initCash = Int32.Parse(init.GetAttribute("dosh"));

            var interval = waveInfo[1] as XmlElement;
            ans.waveInterval = Single.Parse(interval.GetAttribute("wave"));
            ans.enemyInterval = Single.Parse(interval.GetAttribute("enemy"));

            var enemiesInfo = waveInfo[2].ChildNodes;
            ans.enemies = new List<GameObject>();
            for (int i = 0; i < enemiesInfo.Count; i++)
            {
                var enemyCount = Int32.Parse((enemiesInfo[i] as XmlElement).GetAttribute("count"));
                for (int j = 0; j < enemyCount; j++)
                    ans.enemies.Add(enemyPrefabs[i]);
            }
            WaveGenerator.Reshuffle(ans.enemies);
            return ans;
        }
    }
}