using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DSpirit: DataItem
    {

        public string Name{ get;private set; }

        public int GroupID{ get;private set; }

        public EnumElement Element{ get; private set; }

        public List<int> Attribute{ get; private set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Name = tempStrs[count++];
            GroupID = int.Parse(tempStrs[count++]);
            Element = (EnumElement)int.Parse(tempStrs[count++]);
            string[] tempArr = tempStrs[count++].Split(',');
            foreach(string str in tempArr)
            {
                Attribute.Add(int.Parse(str));
            };
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            Name = rd.ReadString();
            GroupID = rd.ReadInt32();
            Element = (EnumElement)rd.ReadInt16();
            int count = rd.ReadInt16();
            Attribute = new List<int>();
            for(int i = 0; i < count; i++)
            {
                Attribute.Add(rd.ReadInt32());
            }
                    
        }
    }
}   
