using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DEgg: DataItem
    {

        public string Name{ get;protected set; }

        public int Rank{ get;protected set; }

        public int RankUpRatio{ get;protected set; }

        public int RankUpID{ get;protected set; }

        public int MintTime{ get;protected set; }

        public List<string> MonsterRatio{ get; protected set; }

        public string Icon{ get;protected set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Name = tempStrs[count++];
            Rank = int.Parse(tempStrs[count++]);
            RankUpRatio = int.Parse(tempStrs[count++]);
            RankUpID = int.Parse(tempStrs[count++]);
            MintTime = int.Parse(tempStrs[count++]);
            string[] tempArr = tempStrs[count++].Split(',');
            foreach(string str in tempArr)
            {
                MonsterRatio.Add(str);
            };
            Icon = tempStrs[count++];
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            Name = rd.ReadString();
            Rank = rd.ReadInt32();
            RankUpRatio = rd.ReadInt32();
            RankUpID = rd.ReadInt32();
            MintTime = rd.ReadInt32();
            int count = rd.ReadInt16();
            MonsterRatio = new List<string>();
            for(int i = 0; i < count; i++)
            {
                MonsterRatio.Add(rd.ReadString());
            }
            
            Icon = rd.ReadString();        
        }
    }
}   
