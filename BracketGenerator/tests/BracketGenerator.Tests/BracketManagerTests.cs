using BracketGenerator.Domain;
using BracketGenerator.Tests.Helpers;
using System;
using System.Linq;
using Xunit;

namespace BracketGenerator.Tests
{
    public class BracketManagerTests
    {


        [Fact]
        public void SeedTeamTest()
        {
            var bracketGenerator = TestsHelper.GetSeededBracketManager();

            Assert.Equal(16, bracketGenerator.Teams.Count);
        }

        [Fact]
        public void SeedTeamFailTest()
        {
            var bracketGenerator = TestsHelper.GetSeededBracketManager();
            var existingTem = bracketGenerator.Teams.First();
            Assert.Throws<InvalidOperationException>(() =>
            {
                bracketGenerator.SeedTeam(existingTem.Seed, existingTem.Name);
            });
        }

        [Fact]
        public void CreateTournamentBracketTest()
        {
            Utilities.ResetSeedIndex();

            var bracketGenerator = TestsHelper.GetSeededBracketManager();
            bracketGenerator.CreateTournamentBracket();

            Assert.NotNull(bracketGenerator.TournamentBracket);
        }

        [Fact]
        public void AdvanceTeamAndGetTournamentWinnerTest()
        {
            Utilities.ResetSeedIndex();

            var bracketGenerator = TestsHelper.GetSeededBracketManager();
            bracketGenerator.CreateTournamentBracket();

            var teamAdvanceEvents = TestsHelper.GetTeamAdvanceEventsData();

            foreach (var teamAdvanceEvent in teamAdvanceEvents?.Events ?? new string[] { })
            {
                bracketGenerator.AdvanceTeam(teamAdvanceEvent);
            }

            Assert.Equal("Argentina", bracketGenerator.TournamentBracket?.Game?.Winner?.Name);
        }

        [Fact]
        public void AdvanceTeamFailsWhenWinnerAlreadyAssignedTest()
        {
            Utilities.ResetSeedIndex();

            var bracketGenerator = TestsHelper.GetSeededBracketManager();
            bracketGenerator.CreateTournamentBracket();

            var teamAdvanceEvents = TestsHelper.GetTeamAdvanceEventsData();

            foreach (var teamAdvanceEvent in teamAdvanceEvents?.Events ?? new string[] { })
            {
                bracketGenerator.AdvanceTeam(teamAdvanceEvent);
            }

            Assert.Throws<InvalidOperationException>(() => bracketGenerator.AdvanceTeam("France"));
        }

        [Fact]
        public void AdvanceTeamFailsWhenTeamNotFoundTest()
        {
            Utilities.ResetSeedIndex();

            var bracketGenerator = TestsHelper.GetSeededBracketManager();
            bracketGenerator.CreateTournamentBracket();

            Assert.Throws<InvalidOperationException>(() => bracketGenerator.AdvanceTeam("NotExistingTeam"));
        }

        [Fact]
        public void GetTournamentWinnerFailsWhenWinnerNotAssignedTest()
        {
            Utilities.ResetSeedIndex();

            var bracketGenerator = TestsHelper.GetSeededBracketManager();
            bracketGenerator.CreateTournamentBracket();

            Assert.Throws<InvalidOperationException>(() => bracketGenerator.GetTournamentWinner());
        }

        [Fact]
        public void PathToVictoryTest()
        {
            Utilities.ResetSeedIndex();

            var bracketGenerator = TestsHelper.GetSeededBracketManager();
            bracketGenerator.CreateTournamentBracket();

            var teamAdvanceEvents = TestsHelper.GetTeamAdvanceEventsData();

            foreach (var teamAdvanceEvent in teamAdvanceEvents?.Events ?? new string[] { })
            {
                bracketGenerator.AdvanceTeam(teamAdvanceEvent);
            }

            var a = bracketGenerator.PathToVictory();

            Assert.Equal(
                "Argentina Vs. Australia --> Netherlands Vs. Argentina --> Croatia Vs. Argentina --> Argentina Vs. France",
                bracketGenerator.PathToVictory());
        }
    }
}
