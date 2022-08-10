using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Database
{
    [Serializable]
    public class DSkill: DataItem
    {

        public string Name{ get;protected set; }

        public int Icon{ get;protected set; }

        public string Desp{ get;protected set; }

        public EnumElement Element{ get; protected set; }

        public List<int> Attribute{ get; protected set; }

        public float DamageRate{ get;protected set; }

        public int Cost{ get;protected set; }

        public List<int> BuffList{ get; protected set; }

        public List<float> BuffValueList{ get; protected set; }

        public EnumAtkType AtkType{ get; protected set; }

        public string FxName{ get;protected set; }

        public string HitFxName{ get;protected set; }

        public string FireFxName{ get;protected set; }

        public EnumHitPos HitPos{ get; protected set; }

        public bool IsOneFx{ get;protected set; }

        public int AtkTimes{ get;protected set; }

        public List<float> Offsets{ get; protected set; }

        public bool IsHarmSelf{ get;protected set; }

        public EnumAtkScale AtkScale{ get; protected set; }

        public List<int> ScaleVals{ get; protected set; }

        public bool IsAtk{ get;protected set; }

        public bool IsDamage{ get;protected set; }

        public bool HasBuff{ get;protected set; }

        public float CoolDown{ get;protected set; }

        public override void ParseByString(string data)
        {
            string[] tempStrs = data.Split('-');
            int count = -1;
            Id = int.Parse(tempStrs[count++]);
            Name = tempStrs[count++];
            Icon = int.Parse(tempStrs[count++]);
            Desp = tempStrs[count++];
            Element = (EnumElement)int.Parse(tempStrs[count++]);
            string[] tempArr = tempStrs[count++].Split(',');
            foreach(string str in tempArr)
            {
                Attribute.Add(int.Parse(str));
            };
            DamageRate = float.Parse(tempStrs[count++]);
            Cost = int.Parse(tempStrs[count++]);
            tempArr = tempStrs[count++].Split(',');
            foreach(string str in tempArr)
            {
                BuffList.Add(int.Parse(str));
            };
            tempArr = tempStrs[count++].Split(',');
            foreach(string str in tempArr)
            {
                BuffValueList.Add(float.Parse(str));
            };
            AtkType = (EnumAtkType)int.Parse(tempStrs[count++]);
            FxName = tempStrs[count++];
            HitFxName = tempStrs[count++];
            FireFxName = tempStrs[count++];
            HitPos = (EnumHitPos)int.Parse(tempStrs[count++]);
            IsOneFx = int.Parse(tempStrs[count++]) != 0;
            AtkTimes = int.Parse(tempStrs[count++]);
            tempArr = tempStrs[count++].Split(',');
            foreach(string str in tempArr)
            {
                Offsets.Add(float.Parse(str));
            };
            IsHarmSelf = int.Parse(tempStrs[count++]) != 0;
            AtkScale = (EnumAtkScale)int.Parse(tempStrs[count++]);
            tempArr = tempStrs[count++].Split(',');
            foreach(string str in tempArr)
            {
                ScaleVals.Add(int.Parse(str));
            };
            IsAtk = int.Parse(tempStrs[count++]) != 0;
            IsDamage = int.Parse(tempStrs[count++]) != 0;
            HasBuff = int.Parse(tempStrs[count++]) != 0;
            CoolDown = float.Parse(tempStrs[count++]);
        }

        public override void ParseByBytes(MemoryStream ms)
        {
            BinaryReader rd = new BinaryReader(ms); 
            Id = rd.ReadInt32();
            Name = rd.ReadString();
            Icon = rd.ReadInt32();
            Desp = rd.ReadString();
            Element = (EnumElement)rd.ReadInt16();
            int count = rd.ReadInt16();
            Attribute = new List<int>();
            for(int i = 0; i < count; i++)
            {
                Attribute.Add(rd.ReadInt32());
            }
            
            DamageRate = rd.ReadSingle();
            Cost = rd.ReadInt32();
            count = rd.ReadInt16();
            BuffList = new List<int>();
            for(int i = 0; i < count; i++)
            {
                BuffList.Add(rd.ReadInt32());
            }
            
            count = rd.ReadInt16();
            BuffValueList = new List<float>();
            for(int i = 0; i < count; i++)
            {
                BuffValueList.Add(rd.ReadSingle());
            }
            
            AtkType = (EnumAtkType)rd.ReadInt16();
            FxName = rd.ReadString();
            HitFxName = rd.ReadString();
            FireFxName = rd.ReadString();
            HitPos = (EnumHitPos)rd.ReadInt16();
            IsOneFx = rd.ReadBoolean();
            AtkTimes = rd.ReadInt32();
            count = rd.ReadInt16();
            Offsets = new List<float>();
            for(int i = 0; i < count; i++)
            {
                Offsets.Add(rd.ReadSingle());
            }
            
            IsHarmSelf = rd.ReadBoolean();
            AtkScale = (EnumAtkScale)rd.ReadInt16();
            count = rd.ReadInt16();
            ScaleVals = new List<int>();
            for(int i = 0; i < count; i++)
            {
                ScaleVals.Add(rd.ReadInt32());
            }
            
            IsAtk = rd.ReadBoolean();
            IsDamage = rd.ReadBoolean();
            HasBuff = rd.ReadBoolean();
            CoolDown = rd.ReadSingle();        
        }
    }
}   
