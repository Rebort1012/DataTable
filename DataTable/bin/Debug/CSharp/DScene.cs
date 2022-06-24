using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DScene: DataItem
    {

        public string AssetName{ get;protected set; }

        public int BackgroundMusicId{ get;protected set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            AssetName = tempStrs[count++];
            BackgroundMusicId = int.Parse(tempStrs[count++]);
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            AssetName = rd.ReadString();
            BackgroundMusicId = rd.ReadInt32();        
        }
    }
}   
