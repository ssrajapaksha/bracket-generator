namespace BracketGenerator.Domain.Models
{
    public class Game
    {
        public Bracket? BracketNode { get; set; }
        public Game(Team? teamX, Team? teamY, Bracket? bracket)
        {
            TeamX = teamX;
            TeamY = teamY;

            BracketNode = bracket;

            AddGameToTeams();
        }

        public Team? TeamX { get; set; }
        public Team? TeamY { get; set; }

        public Team? Winner { get; set; }

        public void SetWinner(Team team)
        {
            if (TeamX == null || TeamY == null)
            {
                throw new InvalidOperationException("Opponent not assigned yet.");
            }

            if (Winner != null)
            {
                throw new InvalidOperationException("Winner already set for particular game.");
            }

            Winner = team;

            UpdateTailBracket(Winner);
        }

        private void UpdateTailBracket(Team team)
        {
            var tailBracket = BracketNode?.Tail;
            if (tailBracket == null)
            {
                return;
            }

            if (tailBracket.Game == null)
            {
                tailBracket.Game = new Game(
                    team, null, tailBracket);
            }
            else if (tailBracket.Game.TeamY == null)
            {
                tailBracket.Game.TeamY = team;
                team.Games.Add(tailBracket.Game);
            }
        }

        private void AddGameToTeams()
        {
            TeamX?.Games.Add(this);
            TeamY?.Games.Add(this);
        }

        public override string ToString()
        {
            return $"{TeamX?.Name ?? "<Unassigned>"} Vs. {TeamY?.Name ?? "<Unassigned>"}";
        }
    }
}
