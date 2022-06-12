using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DBuff: DataItem
    {

        public string Name{ get;private set; }

        public EnumBuff Buff{ get; private set; }

        public float Duration{ get;private set; }

        public float Rate{ get;private set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Name = tempStrs[count++];
                Buff = (EnumBuff)int.Parse(tempStrs[count++]);
            Duration = float.Parse(tempStrs[count++]);
            Rate = float.Parse(tempStrs[count++]);
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            using (BinaryReader rd = new BinaryReader(ms))
            {
                Id = rd.ReadInt32();
                Name = rd.ReadString();
                Buff = (EnumBuff)rd.ReadInt16();
                Duration = rd.ReadSingle();
                Rate = rd.ReadSingle();
            }
        }
    }
}   
