using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DTest: DataItem
    {

        public float Hp{ get;private set; }

        public string Name{ get;private set; }

        public bool IsHero{ get;private set; }

        public List<int> Params{ get; private set; }

        public List<string> Descrip{ get; private set; }

        public Dictionary<string,float> Attribute{ get; private set; }

        public Vector3 Pos{ get; private set; }

        public Color OldCol{ get; private set; }

        public EnumJob Job{ get; private set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Hp = float.Parse(tempStrs[count++]);
            Name = tempStrs[count++];
            IsHero = int.Parse(tempStrs[count++]) != 0;
            string[] tempArr = tempStrs[count++].Split(',');
            foreach(string str in tempArr)
            {
                Params.Add(int.Parse(str));
            };
            tempArr = tempStrs[count++].Split(',');
            foreach(string str in tempArr)
            {
                Descrip.Add(str);
            };
            tempArr = tempStrs[count++].Split(',');
            Pos = new Vector3(float.Parse(tempArr[0]),float.Parse(tempArr[1]),float.Parse(tempArr[2]));
            tempArr = tempStrs[count++].Split(',');
            OldCol = new Color(float.Parse(tempArr[0]),float.Parse(tempArr[1]),float.Parse(tempArr[2]),float.Parse(tempArr[3]));
                Job = (EnumJob)int.Parse(tempStrs[count++]);
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            using (BinaryReader rd = new BinaryReader(ms))
            {
                Id = rd.ReadInt32();
                Hp = rd.ReadSingle();
                Name = rd.ReadString();
                IsHero = rd.ReadBoolean();
                int count = rd.ReadInt16();
                for(int i = 0; i < count; i++)
                {
                    Params.Add(rd.ReadInt32());
                }
            
                count = rd.ReadInt16();
                for(int i = 0; i < count; i++)
                {
                    Descrip.Add(rd.ReadString());
                }
            
                Pos = new Vector3(rd.ReadSingle(),rd.ReadSingle(),rd.ReadSingle());
                OldCol = new Color(rd.ReadSingle(),rd.ReadSingle(),rd.ReadSingle(),rd.ReadSingle());
                Job = (EnumJob)rd.ReadInt16();
            }
        }
    }
}   
