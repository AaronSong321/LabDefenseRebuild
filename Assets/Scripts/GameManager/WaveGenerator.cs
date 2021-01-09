using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public class WaveGenerator : MonoBehaviour
    {
        List<GameObject> EnemyPrefabs;

        protected virtual void Awake()
        {
            EnemyPrefabs = new List<GameObject>
            {
                Resources.Load<GameObject>("Prefabs/Enemy/Elfin/Elfin"),
                Resources.Load<GameObject>("Prefabs/Enemy/Crawler/Crawler"),
                Resources.Load<GameObject>("Prefabs/Enemy/Zombie/Zombie")
            };
        }

        public static void Reshuffle<T>(List<T> list)
        {
            int num = list.Count;
            List<T> answer = new List<T>();
            System.Random c = new System.Random();
            for (int i = 0; i < num; i++)
            {
                int random_index = c.Next(0, list.Count);
                answer.Add(list[random_index]);
                list.RemoveAt(random_index);
            }
            for (int i = 0; i < num; i++)
            {
                list.Add(answer[i]);
            }
        }

        public List<Wave> GenerateAllWaves()
        {
            string fileName = Settings.GenerateStreamingPath("System/Difficulty/Hypothetical/Waves.xml");
            XmlDocument document = new XmlDocument();
            document.Load(fileName);
            XmlNodeList waveList = document.SelectSingleNode("Waves").ChildNodes;
            List<Wave> ans = new List<Wave>();

            for (int i = 0; i < waveList.Count; i++)
            {
                ans.Add(Wave.ReadWave(waveList[i], EnemyPrefabs));
            }
            return ans;
        }
    }
}
