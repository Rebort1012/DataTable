using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DHeroSprite: DataItem
    {

        public string Name{ get;protected set; }

        public string Path{ get;protected set; }

        public EnumHeroSprite HeroSprite{ get; protected set; }

        public int IconID{ get;protected set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Name = tempStrs[count++];
            Path = tempStrs[count++];
            HeroSprite = (EnumHeroSprite)int.Parse(tempStrs[count++]);
            IconID = int.Parse(tempStrs[count++]);
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            Name = rd.ReadString();
            Path = rd.ReadString();
            HeroSprite = (EnumHeroSprite)rd.ReadInt16();
            IconID = rd.ReadInt32();        
        }
    }
}   
