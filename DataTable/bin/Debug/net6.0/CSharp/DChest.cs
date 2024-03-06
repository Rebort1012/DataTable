using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DChest: DataItem
    {

        public string Name{ get;protected set; }

        public int ChestType{ get;protected set; }

        public List<string> ChestRatio{ get; protected set; }

        public string ChoosenChestRatio{ get;protected set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Name = tempStrs[count++];
            ChestType = int.Parse(tempStrs[count++]);
            string[] tempArr = tempStrs[count++].Split(',');
            foreach(string str in tempArr)
            {
                ChestRatio.Add(str);
            };
            ChoosenChestRatio = tempStrs[count++];
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            Name = rd.ReadString();
            ChestType = rd.ReadInt32();
            int count = rd.ReadInt16();
            ChestRatio = new List<string>();
            for(int i = 0; i < count; i++)
            {
                ChestRatio.Add(rd.ReadString());
            }
            
            ChoosenChestRatio = rd.ReadString();        
        }
    }
}   
