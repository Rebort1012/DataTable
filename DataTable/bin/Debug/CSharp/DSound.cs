using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DSound: DataItem
    {

        public string AssetName{ get;protected set; }

        public int Priority{ get;protected set; }

        public bool Loop{ get;protected set; }

        public float Volume{ get;protected set; }

        public float SpatialBlend{ get;protected set; }

        public float MaxDistance{ get;protected set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            AssetName = tempStrs[count++];
            Priority = int.Parse(tempStrs[count++]);
            Loop = int.Parse(tempStrs[count++]) != 0;
            Volume = float.Parse(tempStrs[count++]);
            SpatialBlend = float.Parse(tempStrs[count++]);
            MaxDistance = float.Parse(tempStrs[count++]);
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            AssetName = rd.ReadString();
            Priority = rd.ReadInt32();
            Loop = rd.ReadBoolean();
            Volume = rd.ReadSingle();
            SpatialBlend = rd.ReadSingle();
            MaxDistance = rd.ReadSingle();        
        }
    }
}   
