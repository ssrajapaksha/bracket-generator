namespace BracketGenerator.Domain.Models
{
    public class Group
    {
        public IList<Team> SelectedTeams { get; set; } = new List<Team>();

        public Group(BracketManager bracketManager, string name)
        {
            BracketManager = bracketManager;
            Name = name;
        }

        public string Name { get; set; }
        public BracketManager BracketManager { get; set; }
    }
}
