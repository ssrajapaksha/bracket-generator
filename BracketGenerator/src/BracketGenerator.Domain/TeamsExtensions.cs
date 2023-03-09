using BracketGenerator.Domain.Models;

namespace BracketGenerator.Domain
{
    public static class TeamsExtensions
    {
        public static IList<Team> ToSeededList(this IList<Team> teams)
        {
            var sortedList = new List<Team>();
            teams = teams.OrderBy(x => x.Seed).ToList();

            for (int i = 0; i < teams.Count; i++)
            {
                var normalizedIndex = i < teams.Count / 2 ? i : i - teams.Count / 2;

                if (i % 2 == 0)
                {
                    sortedList.Add(teams[normalizedIndex + (i < teams.Count / 2 ? 0 : teams.Count / 2)]);
                }
                else
                {
                    sortedList.Add(teams[normalizedIndex + (i < teams.Count / 2 ? teams.Count / 2 : 0)]);
                }
            }

            return sortedList;
        }
    }
}
