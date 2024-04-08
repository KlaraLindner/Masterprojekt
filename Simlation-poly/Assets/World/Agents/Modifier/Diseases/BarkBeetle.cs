using System;
using UnityEngine;

namespace World.Agents.Modifier.Diseases
{
    public class BarkBeetle: Disease
    {
      
        public BarkBeetle() : base("BarkBeetle")
        {
            HumidityRange = new Vector2(0, 40);
            TemperatureRange = new Vector2(16.5f, 50);
        }
        
        public override void OnInit(Agent sender, EventArgs e)
        {   
        }

        public override void OnCall(Agent sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void OnHeal(Agent sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}