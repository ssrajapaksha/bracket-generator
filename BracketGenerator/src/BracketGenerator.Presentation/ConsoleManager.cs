using BracketGenerator.Domain;
using BracketGenerator.Domain.Models;
using BracketGenerator.Enums;
using ConsoleTables;

namespace BracketGenerator
{
    public static class ConsoleManager
    {
        public static GenerationType Start()
        {
            var input = GenerationType.WithoutGroups;
            var isValidInput = false;
            var options = new[]
            {
                GenerationType.WithGroups,
                GenerationType.WithoutGroups
            };

            Console.WriteLine("Welcome to tournament bracket generator. Please select an option to proceed. Default option is generate without group stage (1).");

            do
            {
                Console.WriteLine("1. Generate bracket without group stage.");
                Console.WriteLine("2. Generate bracket with group stage.\n");
                Console.Write("Bracket generation option: ");

                try
                {
                    var inputOption = Console.ReadLine();
                    input = string.IsNullOrEmpty(inputOption)
                        ? GenerationType.WithoutGroups
                        : (GenerationType)Convert.ToInt16(inputOption);

                    if (!options.Contains(input))
                    {
                        throw new InvalidOperationException();
                    }

                    isValidInput = true;
                }
                catch
                {
                    isValidInput = false;
                    Console.WriteLine("\nInvalid option. Please try again.\n");
                }

            } while (!isValidInput);

            return input;
        }

        public static void SeedTeams(BracketManager bracketManager)
        {
            var currentTeamNumber = 1;
            var numberOfTeams = 0;

            var nameInput = string.Empty;
            var seedInput = string.Empty;

            while (true)
            {
                Console.Write("\nNumber of teams to input: ");
                var numberOfTeamsInput = Console.ReadLine();
                if (!int.TryParse(numberOfTeamsInput, out numberOfTeams))
                {
                    Console.WriteLine("\nInvalid input.");
                }
                else if (!Utilities.ValidateTeamCount(numberOfTeams))
                {
                    Console.WriteLine("\nNumber of teams should be a power of two.");
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("\nInput team details. Leave an empty line and press enter to finish inserting team details.");

            while (currentTeamNumber <= numberOfTeams)
            {
                Console.Write($"\nTeam {currentTeamNumber} name: ");
                nameInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nameInput))
                {
                    continue;
                }

                Console.Write($"Team {currentTeamNumber} seed: ");
                seedInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(seedInput))
                {
                    continue;
                }

                try
                {
                    bracketManager.SeedTeam(seedInput.Trim(), nameInput.Trim());
                    currentTeamNumber++;
                }
                catch (InvalidOperationException exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }

        public static IList<Group> SeedGroups()
        {
            var currentGroupNumber = 1;
            var numberOfGoups = 0;

            var nameInput = string.Empty;

            while (true)
            {
                Console.Write("\nNumber of groups to input: ");
                var numberOfGroupsInput = Console.ReadLine();
                if (!int.TryParse(numberOfGroupsInput, out numberOfGoups))
                {
                    Console.WriteLine("\nInvalid input.");
                }
                else if (!Utilities.ValidateGroupCount(numberOfGoups))
                {
                    Console.WriteLine("\nNumber of teams qualified for tournament should be a power of two.");
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("\nInput team details for groups. Each group should have 4 teams.");

            var groupLetter = (int)'A';
            var groups = new List<Group>();

            while (currentGroupNumber <= numberOfGoups)
            {
                var group = new Group(new BracketManager(), $"Group {(char)groupLetter}");

                Console.WriteLine($"\nInput teams for Group {group.Name}");

                for (int i = 0; i < 4; i++)
                {
                    Console.Write($"Team {i + 1} name: ");
                    nameInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(nameInput))
                    {
                        i--;
                        continue;
                    }

                    try
                    {
                        group.BracketManager.SeedTeam($"{i + 1}{(char)groupLetter}", nameInput);
                    }
                    catch (InvalidOperationException exception)
                    {
                        Console.WriteLine(exception.Message);
                        i--;
                    }
                }

                groups.Add(group);

                groupLetter++;
                currentGroupNumber++;
            }

            return groups;
        }

        public static void HandleBracketOperationsForGroups(IList<Group> groups)
        {
            Console.WriteLine("\nEvery team should have 2 winners  to continue with the tournament.");

            var groupNumberInput = string.Empty;
            var groupNumber = 0;

            do
            {
                var i = 1;
                Console.WriteLine("\nChoose a group to select winners.\n");
                foreach (var group in groups)
                {
                    Console.WriteLine($"{i}. {group.Name}");
                    i++;
                }

                Console.Write("\nGroup number (eg. 1): ");
                groupNumberInput = Console.ReadLine();

                if (!int.TryParse(groupNumberInput, out groupNumber))
                {
                    Console.WriteLine("Invalid group name.");
                    continue;
                }

                try
                {
                    var selectedGroup = groups.ElementAt(groupNumber - 1);
                    var selectedTeam = AdvanceTeamAsWinner(selectedGroup.BracketManager);

                    if (selectedTeam != null)
                    {
                        selectedGroup.SelectedTeams.Add(selectedTeam);
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid group name.");
                }
            } while (!IsGroupStageFinished(groups));

            Console.Write("\nGroup stage matches are done.");
        }

        public static void HandleBracketOperations(BracketManager bracketManager)
        {
            BracketOperations input = BracketOperations.AdvanceTeamAsWinner;
            var options = new[]
            {
                BracketOperations.AdvanceTeamAsWinner,
                BracketOperations.GetTournamentWinner,
                BracketOperations.GetPathToVictory,
                BracketOperations.DisplayBracket,
                BracketOperations.Exit
            };

            Console.WriteLine("\nTournament bracket has been created. You may choose any operation below. Default operation is advance a team as winner (1).");

            do
            {
                Console.WriteLine("\n1. Advance a team as winner.");
                Console.WriteLine("2. Get tournament winner.");
                Console.WriteLine("3. Get path to victory of the winner.");
                Console.WriteLine("4. Display tournament bracket.");
                Console.WriteLine("5. Exit");
                Console.Write("\nBracket operation: ");

                try
                {
                    var inputOption = Console.ReadLine();
                    input = string.IsNullOrEmpty(inputOption)
                        ? BracketOperations.AdvanceTeamAsWinner
                        : (BracketOperations)Convert.ToInt16(inputOption);

                    if (!options.Contains(input))
                    {
                        throw new InvalidOperationException();
                    }

                    PerformBracketOperation(input, bracketManager);
                }
                catch
                {
                    Console.WriteLine("\nInvalid operation. Please try again.\n");
                }

            } while (true);
        }

        public static void DisplayBracket(BracketManager bracketManager)
        {
            Console.WriteLine("\n");

            var numberOfRounds = Utilities.GetNumberOfRounds(bracketManager.Teams.Count);
            var table = new ConsoleTable(new ConsoleTableOptions
            {
                EnableCount = false
            });

            IList<KeyValuePair<string, string>> matches = new List<KeyValuePair<string, string>>();
            FillTableData(bracketManager.TournamentBracket, matches, numberOfRounds);

            var groupedMatches = matches.GroupBy(x => x.Key).Reverse();
            table.AddColumn(groupedMatches.Select(x => x.Key).ToList());

            for (int i = 0; i < Math.Pow(2, numberOfRounds) / 2; i++)
            {
                table.AddRow(groupedMatches.Select(x => x.ElementAtOrDefault(i).Value).ToArray());
            }

            table.Write();
        }

        private static bool IsGroupStageFinished(IList<Group> groups)
        {
            return groups.All(x => x.SelectedTeams.Count == 2);
        }

        private static void PerformBracketOperation(BracketOperations operation, BracketManager bracketManager)
        {
            switch (operation)
            {
                case BracketOperations.AdvanceTeamAsWinner:
                    AdvanceTeamAsWinner(bracketManager);
                    break;
                case BracketOperations.GetTournamentWinner:
                    GetTournamentWinner(bracketManager);
                    break;
                case BracketOperations.GetPathToVictory:
                    GetPathToVictory(bracketManager);
                    break;
                case BracketOperations.DisplayBracket:
                    DisplayBracket(bracketManager);
                    break;
                case BracketOperations.Exit:
                    Environment.Exit(0);
                    break;
            }
        }

        private static Team AdvanceTeamAsWinner(BracketManager bracketManager)
        {
            var input = string.Empty;
            Team? selectedTeam = null;

            while (true)
            {
                Console.Write("\nTeam name to advance as winner: ");
                input = Console.ReadLine();

                try
                {
                    if (string.IsNullOrEmpty(input))
                    {
                        return null;
                    }

                    selectedTeam = bracketManager.AdvanceTeam(input);

                    Console.WriteLine($"\n{input} has been advanced as winner");

                    break;
                }
                catch (InvalidOperationException exception)
                {
                    Console.WriteLine($"\n{exception.Message}");
                }
            }

            DisplayBracket(bracketManager);

            return selectedTeam;
        }

        private static void GetPathToVictory(BracketManager bracketManager)
        {
            try
            {
                var pathToVictory = bracketManager.PathToVictory();
                Console.WriteLine($"\nPath to victory for the winner:");
                Console.WriteLine($"\n{pathToVictory}");
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine($"\n{exception.Message}");
            }
        }

        private static void GetTournamentWinner(BracketManager bracketManager)
        {
            try
            {
                var winner = bracketManager.GetTournamentWinner();
                Console.WriteLine($"\nWinner of the tournament is {winner.Name} !!!");
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine($"\n{exception.Message}");
            }
        }

        private static void FillTableData(
            Bracket? bracket, IList<KeyValuePair<string, string>> matches, int numberOfRounds)
        {
            ArgumentNullException.ThrowIfNull(bracket);

            matches.Add(new KeyValuePair<string, string>($"Round {numberOfRounds}", bracket.Game?.ToString() ?? "No teams assigned"));

            if (bracket.Top == null || bracket.Bottom == null)
            {
                return;
            }

            FillTableData(bracket.Top, matches, numberOfRounds - 1);
            FillTableData(bracket.Bottom, matches, numberOfRounds - 1);
        }
    }
}
