using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DAttribute: DataItem
    {

        public string Name{ get;protected set; }

        public int IsPercent{ get;protected set; }

        public float Power{ get;protected set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Name = tempStrs[count++];
            IsPercent = int.Parse(tempStrs[count++]);
            Power = float.Parse(tempStrs[count++]);
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            Name = rd.ReadString();
            IsPercent = rd.ReadInt32();
            Power = rd.ReadSingle();        
        }
    }
}   
