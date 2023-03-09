using BracketGenerator.Domain;
using System;
using Xunit;

namespace BracketGenerator.Tests;

public class UtilitiesTests
{
    [Fact]
    public void SeddIndexTest()
    {
        Utilities.ResetSeedIndex();
        var actualCollection = new int[]
        {
            Utilities.SeedIndex, Utilities.SeedIndex, Utilities.SeedIndex
        };

        Assert.Collection(
            actualCollection,
            x => Assert.Equal(0, x),
            y => Assert.Equal(1, y),
            z => Assert.Equal(2, z));
    }

    [Fact]
    public void ResetSeedIndexTest()
    {
        Utilities.ResetSeedIndex();
        Assert.Equal(0, Utilities.SeedIndex);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    public void GetNumberOfRoundsInvalidTest(int numberOfTeams)
    {
        Assert.Throws<ArgumentException>(() => Utilities.GetNumberOfRounds(numberOfTeams));
    }

    [Theory]
    [InlineData(4, 2)]
    [InlineData(16, 4)]
    public void GetNumberOfRoundsTest(int numberOfTeams, int expectedNumberOfRounds)
    {
        var numberOfRounds = Utilities.GetNumberOfRounds(numberOfTeams);
        Assert.Equal(expectedNumberOfRounds, numberOfRounds);
    }

    [Theory]
    [InlineData(-4, false)]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(3, false)]
    [InlineData(4, true)]
    public void ValidateTeamCountTest(int numberOfTeams, bool expectedValidity)
    {
        var isValid = Utilities.ValidateTeamCount(numberOfTeams);
        Assert.Equal(expectedValidity, isValid);
    }

    [Theory]
    [InlineData(-4, false)]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(3, false)]
    [InlineData(4, true)]
    public void ValidateGroupCountTest(int numberOfGoups, bool expectedValidity)
    {
        var isValid = Utilities.ValidateGroupCount(numberOfGoups);
        Assert.Equal(expectedValidity, isValid);
    }
}