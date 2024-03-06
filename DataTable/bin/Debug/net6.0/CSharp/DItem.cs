using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DItem: DataItem
    {

        public string Name{ get;protected set; }

        public int Rank{ get;protected set; }

        public int ItemType{ get;protected set; }

        public int CanStack{ get;protected set; }

        public int Value{ get;protected set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Name = tempStrs[count++];
            Rank = int.Parse(tempStrs[count++]);
            ItemType = int.Parse(tempStrs[count++]);
            CanStack = int.Parse(tempStrs[count++]);
            Value = int.Parse(tempStrs[count++]);
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            Name = rd.ReadString();
            Rank = rd.ReadInt32();
            ItemType = rd.ReadInt32();
            CanStack = rd.ReadInt32();
            Value = rd.ReadInt32();        
        }
    }
}   
