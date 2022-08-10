using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DWeapon: DataItem
    {

        public string Name{ get;protected set; }

        public bool IsBoom{ get;protected set; }

        public EnumWeapon Weapon{ get; protected set; }

        public int IconID{ get;protected set; }

        public int Rank{ get;protected set; }

        public EnumElement Element{ get; protected set; }

        public int Range{ get;protected set; }

        public string SpriteName{ get;protected set; }

        public string Path{ get;protected set; }

        public string SpriteName2{ get;protected set; }

        public string BloomCol{ get;protected set; }

        public float Rate{ get;protected set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Name = tempStrs[count++];
            IsBoom = int.Parse(tempStrs[count++]) != 0;
            Weapon = (EnumWeapon)int.Parse(tempStrs[count++]);
            IconID = int.Parse(tempStrs[count++]);
            Rank = int.Parse(tempStrs[count++]);
            Element = (EnumElement)int.Parse(tempStrs[count++]);
            Range = int.Parse(tempStrs[count++]);
            SpriteName = tempStrs[count++];
            Path = tempStrs[count++];
            SpriteName2 = tempStrs[count++];
            BloomCol = tempStrs[count++];
            Rate = float.Parse(tempStrs[count++]);
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            Name = rd.ReadString();
            IsBoom = rd.ReadBoolean();
            Weapon = (EnumWeapon)rd.ReadInt16();
            IconID = rd.ReadInt32();
            Rank = rd.ReadInt32();
            Element = (EnumElement)rd.ReadInt16();
            Range = rd.ReadInt32();
            SpriteName = rd.ReadString();
            Path = rd.ReadString();
            SpriteName2 = rd.ReadString();
            BloomCol = rd.ReadString();
            Rate = rd.ReadSingle();        
        }
    }
}   
