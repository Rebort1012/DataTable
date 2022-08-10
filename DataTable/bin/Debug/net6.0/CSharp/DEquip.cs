using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DEquip: DataItem
    {

        public string Name{ get;protected set; }

        public string SpritePath{ get;protected set; }

        public EnumEquip Equip{ get; protected set; }

        public int IconID{ get;protected set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Name = tempStrs[count++];
            SpritePath = tempStrs[count++];
            Equip = (EnumEquip)int.Parse(tempStrs[count++]);
            IconID = int.Parse(tempStrs[count++]);
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            Name = rd.ReadString();
            SpritePath = rd.ReadString();
            Equip = (EnumEquip)rd.ReadInt16();
            IconID = rd.ReadInt32();        
        }
    }
}   
