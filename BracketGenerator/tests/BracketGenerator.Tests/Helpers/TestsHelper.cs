using BracketGenerator.Domain;
using BracketGenerator.Domain.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BracketGenerator.Tests.Helpers
{
    public static class TestsHelper
    {
        public static IList<Team> GetTeamsData()
        {
            string fileData = File.ReadAllText(@"./Data/SeedFile.json");
            var teams = JsonSerializer.Deserialize<TeamsData>(fileData);

            return teams?.R16.Select(x => new Team(x.Team, x.Seed)).ToList() ?? new List<Team>();
        }

        public static TeamAdvanceEvents? GetTeamAdvanceEventsData()
        {
            string fileData = File.ReadAllText(@"./Data/AdvanceEvents.json");
            return JsonSerializer.Deserialize<TeamAdvanceEvents>(fileData);
        }

        public static BracketManager GetSeededBracketManager()
        {
            var bracketGenerator = new BracketManager();

            var teams = TestsHelper.GetTeamsData();
            foreach (var team in teams)
            {
                bracketGenerator.SeedTeam(team.Seed, team.Name);
            }

            return bracketGenerator;
        }
    }

    public class TeamsData
    {
        public IList<R16> R16 { get; set; }
    }

    public class R16
    {
        public string Seed { get; set; }
        public string Team { get; set; }
    }

    public class TeamAdvanceEvents
    {
        public string[] Events { get; set; }
        public string Winner { get; set; }
    }
}
