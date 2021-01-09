using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Aaron.LabDefenseRebuild
{
    public static class Settings
    {
        public static string GenerateStreamingPath(string fileName) => Application.streamingAssetsPath + "/" + fileName;
        public static string GeneratePersistentPath(string fileName) => Application.persistentDataPath + "/" + fileName;

        public static string RichTextModify(string raw, int mode)
        {
            switch (mode)
            {
                case 0: return $"<color=yellow>{raw}</color>";
                case 1: return $"<color=red>{raw}</color>";
                case 2: return $"<color=blue>{raw}</color>";
                case 3: return $"<color=pink>{raw}</color>";
                default: return raw;
            }
        }

        public static string PlayersPath() => GeneratePersistentPath("Player/Players.xml");
        public static string ExpRequirePath() => GenerateStreamingPath("System/Perk/ExpRequire.xml");

        public static string RichTextModify(float percentage) => $"<color=yellow>{Mathf.RoundToInt(percentage * 100)}%</color>";

        public static string EncodeByMd5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            for (int i = 0; i < s.Length; i++)
                pwd = pwd + s[i].ToString("X");
            return pwd;
        }
    }
}