using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DDragon: DataItem
    {

        public string Icon{ get;protected set; }

        public int MaxExp{ get;protected set; }

        public int MaxLevel{ get;protected set; }

        public int EggRank{ get;protected set; }

        public int BoxRewardInterval{ get;protected set; }

        public string ItemValue{ get;protected set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Icon = tempStrs[count++];
            MaxExp = int.Parse(tempStrs[count++]);
            MaxLevel = int.Parse(tempStrs[count++]);
            EggRank = int.Parse(tempStrs[count++]);
            BoxRewardInterval = int.Parse(tempStrs[count++]);
            ItemValue = tempStrs[count++];
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            Icon = rd.ReadString();
            MaxExp = rd.ReadInt32();
            MaxLevel = rd.ReadInt32();
            EggRank = rd.ReadInt32();
            BoxRewardInterval = rd.ReadInt32();
            ItemValue = rd.ReadString();        
        }
    }
}   
