namespace BracketGenerator.Domain
{
    public static class Utilities
    {
        private static int _seedIndex = 0;

        public static int SeedIndex => _seedIndex++;

        public static void ResetSeedIndex()
        {
            _seedIndex = 0;
        }

        public static int GetNumberOfRounds(int numberOfTeams)
        {
            if (!ValidateTeamCount(numberOfTeams))
            {
                throw new ArgumentException("Invalid number of teams");
            }

            return Convert.ToInt16(Math.Log2(numberOfTeams));
        }

        public static bool ValidateTeamCount(int numberOfTeams)
        {
            var numberOfRounds = Math.Log2(numberOfTeams);

            return !(double.IsInfinity(numberOfRounds)
                || double.IsNaN(numberOfRounds)
                || numberOfRounds == 0
                || numberOfRounds % 1 != 0);
        }

        public static bool ValidateGroupCount(int numberOfGroups)
        {
            var numberOfRounds = Math.Log2(numberOfGroups * 2);

            return !(numberOfGroups < 2
                || double.IsInfinity(numberOfRounds)
                || double.IsNaN(numberOfRounds)
                || numberOfRounds == 0
                || numberOfRounds % 1 != 0);
        }
    }
}
