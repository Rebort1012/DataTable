using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class Hero: DataItem
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

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Name = tempStrs[count++];
            PerfabName = tempStrs[count++];
                Job = (EnumJob)int.Parse(tempStrs[count++]);
            Icon = int.Parse(tempStrs[count++]);
            MP = int.Parse(tempStrs[count++]);
            Exp = int.Parse(tempStrs[count++]);
            Rank = int.Parse(tempStrs[count++]);
            AttackID = int.Parse(tempStrs[count++]);
            SkillID = int.Parse(tempStrs[count++]);
            IsHero = int.Parse(tempStrs[count++]) != 0;
            TalentID = int.Parse(tempStrs[count++]);
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            using (BinaryReader rd = new BinaryReader(ms))
            {
                Id = rd.ReadInt32();
                Name = rd.ReadString();
                PerfabName = rd.ReadString();
                Job = (EnumJob)rd.ReadInt16();
                Icon = rd.ReadInt32();
                MP = rd.ReadInt32();
                Exp = rd.ReadInt32();
                Rank = rd.ReadInt32();
                AttackID = rd.ReadInt32();
                SkillID = rd.ReadInt32();
                IsHero = rd.ReadBoolean();
                TalentID = rd.ReadInt32();
            }
        }
    }
}   
