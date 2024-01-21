using World.Structure;

namespace World.Agents.Plants
{
    public class Conifer : TreeAgent
    {
        public Conifer(Ground ground) : base(ground)
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
        }
    
        private void Start()
        {
        
        }


        public override string LN()
        {
            return "Conifer";
        }
    }
}
