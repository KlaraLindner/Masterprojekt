using NaughtyAttributes;
using UnityEngine;

namespace World.Agents.Modifier
{
    public abstract class Disease: AgentModifier
    {    
        public TextAsset barkBeetleData { get; set; }
        
        [MaxValue(1)]
        [MinValue(0)]
        public float progress =0;
        
        public Vector2 HumidityRage{get;set;}
        
        public Vector2 TemperatureRange{get;set;}
        
        public Vector2 SeasonRange{get;set;}
        protected Disease(string n):base(n)
        {
       
        }
    }
}