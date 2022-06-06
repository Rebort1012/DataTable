using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    [Serializable]
    public class Skill
    {

        public int SkillId{ get;private set; }

        public string SkillName{ get;private set; }

        public string Describe{ get;private set; }

        public string SkillType{ get;private set; }

        public int AttackNum{ get;private set; }

        public int Icon{ get;private set; }

        public string FxPerfab{ get;private set; }

        public string HitPerfab{ get;private set; }

        public string AudioName{ get;private set; }

        public float Damage{ get;private set; }

        public float HitTimes{ get;private set; }

        public float Cost{ get;private set; }

        public string BuffType{ get;private set; }

        public string BuffPerfab{ get;private set; }

        public float BuffRatio{ get;private set; }

        public float BuffDamage{ get;private set; }

        public float BuffDuration{ get;private set; }

        public string BuffAnima{ get;private set; }

        public bool IsDebuff{ get;private set; }

        public void Parse(string data)
        {
        
        }
    }
}