using BracketGenerator;
using BracketGenerator.Domain;

var generateOption = ConsoleManager.Start();

var bracketManager = new BracketManager();

if (generateOption == BracketGenerator.Enums.GenerationType.WithoutGroups)
{
    ConsoleManager.SeedTeams(bracketManager);
}
else
{
    var groups = ConsoleManager.SeedGroups();

    foreach (var group in groups)
    {
        group.BracketManager.CreateTournamentBracket();

        Console.WriteLine($"\nBracket for {group.Name}");
        ConsoleManager.DisplayBracket(group.BracketManager);
    }

    ConsoleManager.HandleBracketOperationsForGroups(groups);

    foreach (var selectedTeam in groups.SelectMany(x => x.SelectedTeams).ToList())
    {
        bracketManager.SeedTeam(selectedTeam.Seed, selectedTeam.Name);
    }
}

bracketManager.CreateTournamentBracket();

ConsoleManager.DisplayBracket(bracketManager);
ConsoleManager.HandleBracketOperations(bracketManager);