using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Database
{
    [Serializable]
    public class Skill: DataItem
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

        public float BuffRatio{ get;private set; }

        public float BuffDamage{ get;private set; }

        public float BuffDuration{ get;private set; }

        public string BuffAnima{ get;private set; }

        public bool IsDebuff{ get;private set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            
            SkillId = int.Parse(tempStrs[count++]);
            SkillName = tempStrs[count++];
            Describe = tempStrs[count++];
            SkillType = tempStrs[count++];
            AttackNum = int.Parse(tempStrs[count++]);
            Icon = int.Parse(tempStrs[count++]);
            FxPerfab = tempStrs[count++];
            HitPerfab = tempStrs[count++];
            AudioName = tempStrs[count++];
            Damage = float.Parse(tempStrs[count++]);
            HitTimes = float.Parse(tempStrs[count++]);
            Cost = float.Parse(tempStrs[count++]);
            BuffType = tempStrs[count++];
            BuffRatio = float.Parse(tempStrs[count++]);
            BuffDamage = float.Parse(tempStrs[count++]);
            BuffDuration = float.Parse(tempStrs[count++]);
            BuffAnima = tempStrs[count++];
            IsDebuff = int.Parse(tempStrs[count++]) != 0;
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            using (BinaryReader rd = new BinaryReader(ms))
            {
                
                SkillId = rd.ReadInt32();
                SkillName = rd.ReadString();
                Describe = rd.ReadString();
                SkillType = rd.ReadString();
                AttackNum = rd.ReadInt32();
                Icon = rd.ReadInt32();
                FxPerfab = rd.ReadString();
                HitPerfab = rd.ReadString();
                AudioName = rd.ReadString();
                Damage = rd.ReadSingle();
                HitTimes = rd.ReadSingle();
                Cost = rd.ReadSingle();
                BuffType = rd.ReadString();
                BuffRatio = rd.ReadSingle();
                BuffDamage = rd.ReadSingle();
                BuffDuration = rd.ReadSingle();
                BuffAnima = rd.ReadString();
                IsDebuff = rd.ReadBoolean();
            }
        }
    }
}   
