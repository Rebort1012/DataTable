using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DGlobal: DataItem
    {

        public string Value1{ get;protected set; }

        public string Value2{ get;protected set; }

        public int Value3{ get;protected set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Value1 = tempStrs[count++];
            Value2 = tempStrs[count++];
            Value3 = int.Parse(tempStrs[count++]);
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            Value1 = rd.ReadString();
            Value2 = rd.ReadString();
            Value3 = rd.ReadInt32();        
        }
    }
}   
