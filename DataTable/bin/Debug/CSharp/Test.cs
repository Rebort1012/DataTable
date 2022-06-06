using System;
using System.Collections.Generic;
using UnityEngine;

namespace Database
{
    [Serializable]
    public class Test
    {
        public int Id{ get;private set; }

        public float Hp{ get;private set; }

        public string Name{ get;private set; }

        public bool IsHero{ get;private set; }

        public List<int> Params{ get; private set; }

        public List<string> Descrip{ get; private set; }

        public Dictionary<string,float> Attribute{ get; private set; }

        public Vector3 Pos{ get; private set; }

        public Color OldCol{ get; private set; }

        public EnumJob Job{ get; private set; }

        public void Parse(string data)
        {
        
        }
    }
}