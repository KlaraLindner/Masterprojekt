using World.Structure;

namespace World.Agents.Plants
{
    public class Grass : FloraAgent
    {
        public Grass(Ground ground) : base(ground)
        {
        
        }

        public override string LN()
        {
            return "Grass";
        }
    }
}
