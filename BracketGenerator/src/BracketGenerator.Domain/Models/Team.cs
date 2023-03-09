namespace BracketGenerator.Domain.Models
{
    public class Team
    {
        public IList<Game> Games { get; set; } = new List<Game>();

        public Team(string name, string seed)
        {
            Name = name;
            Seed = seed;
        }

        public string Name { get; set; }
        public string Seed { get; set; }
    }
}
