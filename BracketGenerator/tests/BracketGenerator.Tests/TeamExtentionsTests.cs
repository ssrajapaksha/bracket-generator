using BracketGenerator.Domain;
using BracketGenerator.Tests.Helpers;
using System.Linq;
using Xunit;

namespace BracketGenerator.Tests
{
    public class TeamExtentionsTests
    {
        [Fact]
        public void ToSeededListTest()
        {
            var teams = TestsHelper.GetTeamsData().Take(8).ToList();

            Assert.Collection(
                teams.ToSeededList(),
                x => Assert.Equal("1A", x.Seed),
                x => Assert.Equal("2B", x.Seed),
                x => Assert.Equal("1C", x.Seed),
                x => Assert.Equal("2D", x.Seed),
                x => Assert.Equal("2A", x.Seed),
                x => Assert.Equal("1B", x.Seed),
                x => Assert.Equal("2C", x.Seed),
                x => Assert.Equal("1D", x.Seed));
        }
    }
}
