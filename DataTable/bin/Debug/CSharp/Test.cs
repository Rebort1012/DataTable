using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dadabase
{
    [Serializable]
    public class Test
    {

        public int Id{ get;private set; }

        public float Hp{ get;private set; }

        public string Name{ get;private set; }

        public bool IsHero{ get;private set; }

        public List<int> mParams;

        public List<string> mDescrip;

        public Dictionary<string,float> mAttribute;

        public Vector3 mPos;

        public Color mOldCol;

        public void Parse(string data)
        {
        
        }
    }
}