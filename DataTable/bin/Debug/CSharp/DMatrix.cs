using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DMatrix: DataItem
    {

        public List<int> SpiritsType{ get; protected set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            string[] tempArr = tempStrs[count++].Split(',');
            foreach(string str in tempArr)
            {
                SpiritsType.Add(int.Parse(str));
            };
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            int count = rd.ReadInt16();
            SpiritsType = new List<int>();
            for(int i = 0; i < count; i++)
            {
                SpiritsType.Add(rd.ReadInt32());
            }
                    
        }
    }
}   
