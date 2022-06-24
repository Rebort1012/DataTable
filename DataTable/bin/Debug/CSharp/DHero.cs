using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DHero: DataItem
    {

        public string Name{ get;protected set; }

        public EnumElement Element{ get; protected set; }

        public EnumWeapon Weapon{ get; protected set; }

        public int Rank{ get;protected set; }

        public int MatrixID{ get;protected set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Name = tempStrs[count++];
            Element = (EnumElement)int.Parse(tempStrs[count++]);
            Weapon = (EnumWeapon)int.Parse(tempStrs[count++]);
            Rank = int.Parse(tempStrs[count++]);
            MatrixID = int.Parse(tempStrs[count++]);
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            Name = rd.ReadString();
            Element = (EnumElement)rd.ReadInt16();
            Weapon = (EnumWeapon)rd.ReadInt16();
            Rank = rd.ReadInt32();
            MatrixID = rd.ReadInt32();        
        }
    }
}   
