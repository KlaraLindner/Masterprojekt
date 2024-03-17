using System;
using UnityEngine;

namespace World.Agents.Modifier.Diseases
{
    public class BarkBeetle: Disease
    {
        public BarkBeetle() : base("BarkBeetle")
        {
            
        }
        
        public override void OnInit(Agent sender, EventArgs e)
        {   //TODO: Change to Barkbeetle Data!
            barkBeetleData =(TextAsset)Resources.Load("SomeData");
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