using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DLevel: DataItem
    {

        public string Name{ get;protected set; }

        public float HPAdd{ get;protected set; }

        public float AttackAdd{ get;protected set; }

        public float DefenceAdd{ get;protected set; }

        public float SpeedAdd{ get;protected set; }

        public int ExpAdd{ get;protected set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Name = tempStrs[count++];
            HPAdd = float.Parse(tempStrs[count++]);
            AttackAdd = float.Parse(tempStrs[count++]);
            DefenceAdd = float.Parse(tempStrs[count++]);
            SpeedAdd = float.Parse(tempStrs[count++]);
            ExpAdd = int.Parse(tempStrs[count++]);
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            Name = rd.ReadString();
            HPAdd = rd.ReadSingle();
            AttackAdd = rd.ReadSingle();
            DefenceAdd = rd.ReadSingle();
            SpeedAdd = rd.ReadSingle();
            ExpAdd = rd.ReadInt32();        
        }
    }
}   
