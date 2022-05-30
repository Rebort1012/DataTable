using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dadabase
{
    [Serializable]
    public class Hero
    {

        public int Id{ get;private set; }

        public string Name{ get;private set; }

        public string PerfabName{ get;private set; }

        public enum EnumJob
        {
            Sword,
            Magic,
            Bow,
        }

        public EnumJob mJob;

        public int Icon{ get;private set; }

        public int MP{ get;private set; }

        public int Exp{ get;private set; }

        public int Rank{ get;private set; }

        public int AttackID{ get;private set; }

        public int SkillID{ get;private set; }

        public bool IsHero{ get;private set; }

        public int TalentID{ get;private set; }

        public List<int> mParams;

        public Dictionary<string,float> mAttribute;

        public Vector3 mPos;

        public Color mOldCol;

        public void Parse(string data)
        {
        
        }
    }
}