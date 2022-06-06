using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    [Serializable]
    public class Hero
    {

        public int Id{ get;private set; }

        public string Name{ get;private set; }

        public string PerfabName{ get;private set; }

        public EnumJob Job{ get; private set; }

        public int Icon{ get;private set; }

        public int MP{ get;private set; }

        public int Exp{ get;private set; }

        public int Rank{ get;private set; }

        public int AttackID{ get;private set; }

        public int SkillID{ get;private set; }

        public bool IsHero{ get;private set; }

        public int TalentID{ get;private set; }

        public void Parse(string data)
        {
        
        }
    }
}