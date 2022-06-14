using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DSkill: DataItem
    {

        public string Name{ get;private set; }

        public int Icon{ get;private set; }

        public string Desp{ get;private set; }

        public EnumElement Element{ get; private set; }

        public List<int> Attribute{ get; private set; }

        public float DamageRate{ get;private set; }

        public int Cost{ get;private set; }

        public int TargetNum{ get;private set; }

        public List<int> BuffList{ get; private set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Name = tempStrs[count++];
            Icon = int.Parse(tempStrs[count++]);
            Desp = tempStrs[count++];
            Element = (EnumElement)int.Parse(tempStrs[count++]);
            string[] tempArr = tempStrs[count++].Split(',');
            foreach(string str in tempArr)
            {
                Attribute.Add(int.Parse(str));
            };
            DamageRate = float.Parse(tempStrs[count++]);
            Cost = int.Parse(tempStrs[count++]);
            TargetNum = int.Parse(tempStrs[count++]);
            tempArr = tempStrs[count++].Split(',');
            foreach(string str in tempArr)
            {
                BuffList.Add(int.Parse(str));
            };
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            Name = rd.ReadString();
            Icon = rd.ReadInt32();
            Desp = rd.ReadString();
            Element = (EnumElement)rd.ReadInt16();
            int count = rd.ReadInt16();
            Attribute = new List<int>();
            for(int i = 0; i < count; i++)
            {
                Attribute.Add(rd.ReadInt32());
            }
            
            DamageRate = rd.ReadSingle();
            Cost = rd.ReadInt32();
            TargetNum = rd.ReadInt32();
            count = rd.ReadInt16();
            BuffList = new List<int>();
            for(int i = 0; i < count; i++)
            {
                BuffList.Add(rd.ReadInt32());
            }
                    
        }
    }
}   
