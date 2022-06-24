using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DUpPool: DataItem
    {

        public int PoolGroup{ get;protected set; }

        public int Rank{ get;protected set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            PoolGroup = int.Parse(tempStrs[count++]);
            Rank = int.Parse(tempStrs[count++]);
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            PoolGroup = rd.ReadInt32();
            Rank = rd.ReadInt32();        
        }
    }
}   
