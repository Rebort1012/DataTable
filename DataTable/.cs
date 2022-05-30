using System;
using System.Collections.Generic;
using UnityEngine;


namespace Dadabase
{
    [Serializable]
    public class Hero
    {

        public int Id
        {
            get;
        }

        public string PerfabName
        {
            get;
        }

        public enum EnumJob
        {
            job,
            Sword,
            Magic,
            Bow,
        }

        public int Icon
        {
            get;
        }

        public int MP
        {
            get;
        }

        public int Exp
        {
            get;
        }

        public int Rank
        {
            get;
        }

        public int AttackID
        {
            get;
        }

        public int SkillID
        {
            get;
        }

        public bool IsHero
        {
            get;
        }

        public int TalentID
        {
            get;
        }

        public List<int> Params;

        public Dictionary<string, float> Attribute;

        public Vector3 Pos;

        public Color OldCol;

        public Color NewCol;

        public void Parse(string data)
        {

        }
    }
}