using BracketGenerator.Domain.Models;

namespace BracketGenerator.Domain
{
    public class BracketManager
    {
        public IList<Team> Teams { get; set; } = new List<Team>();
        public Bracket? TournamentBracket { get; set; }

        public void SeedTeam(string seed, string team)
        {
            if (Teams.Any(x => x.Name == team))
            {
                throw new InvalidOperationException($"Team {seed} already added.");
            }

            Teams.Add(new Team(team, seed));
        }

        public Team AdvanceTeam(string team)
        {
            if (TournamentBracket?.Game?.Winner != null)
            {
                throw new InvalidOperationException("Tournament is already finished.");
            }

            var existingTeam = Teams.FirstOrDefault(x => x.Name.Equals(team, StringComparison.CurrentCultureIgnoreCase));
            if (existingTeam == null)
            {
                throw new InvalidOperationException("Team not found.");
            }

            var lastGame = existingTeam.Games.LastOrDefault();
            lastGame?.SetWinner(existingTeam);

            return existingTeam;
        }

        public Team GetTournamentWinner()
        {
            if (TournamentBracket?.Game?.Winner == null)
            {
                throw new InvalidOperationException("Tournament has not been started yet or still in progress.");
            }

            return TournamentBracket.Game.Winner;
        }

        public string PathToVictory()
        {
            var winner = GetTournamentWinner();

            return string.Join(" --> ", winner.Games.Select(x => $"{x.TeamX?.Name} Vs. {x.TeamY?.Name}").ToList());
        }

        public void CreateTournamentBracket()
        {
            var seededTeams = Teams.ToSeededList();
            var numberOfRounds = Utilities.GetNumberOfRounds(seededTeams.Count);

            TournamentBracket = new Bracket(seededTeams, numberOfRounds, null);

            Utilities.ResetSeedIndex();
        }
    }
}
