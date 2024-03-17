using System;
using UnityEngine;
using World.Environment;
using World.Structure;

namespace World.Agents.Plants
{
    public class Spruce : TreeAgent
    {
        public Color colortest;
        public Spruce(Ground ground) : base(ground)
        {
            domain = "Eukarya";
            kingdom = "Animalia";
            phylum = "Chordata";
            agentClass = "";
            order = "";
            family = "";
            genus = "";
            species = "";
            health = 200;
            waterConsumption = 10;
            heatResistance = 1;
            dryResistance =1;
           
        }

        public override string LN()
        {
            return "Pine";
        }

        public override void OnHandle(WorldController world)
        {   //TODO: Must be replaced with a raycast later!
           
            base.OnHandle(world);
            HandleEnvironment(world);
            HandleBarkbeelte();
            HandleGround();
        }

        public void HandleEnvironment( WorldController world)
        {
         
            
        }

        public void HandleGround()
        {
          colortest = GroundProperties.Instance.GetGroundInfo(transform.position);   
        }
        public void HandleBarkbeelte()
        {
            
            
        }
    }
   
}
