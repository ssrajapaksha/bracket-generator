namespace BracketGenerator.Domain.Models
{
    public class Bracket
    {
        public Game? Game { get; set; }

        public Bracket? Top { get; set; }
        public Bracket? Bottom { get; set; }

        public Bracket? Tail { get; set; }

        public Bracket(IList<Team> teams, int numberOfRounds, Bracket? tail)
        {
            CreateNodes(teams, numberOfRounds);
            Tail = tail;
        }

        private void CreateNodes(IList<Team> teams, int numberOfRounds)
        {
            if (numberOfRounds > 1)
            {
                Top = new(teams, numberOfRounds - 1, this);
                Bottom = new(teams, numberOfRounds - 1, this);
            }
            else if (numberOfRounds == 1)
            {
                Game = new Game(
                    teams.ElementAt(Utilities.SeedIndex),
                    teams.ElementAt(Utilities.SeedIndex),
                    this);
            }
        }
    }
}
