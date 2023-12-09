using AOC2023.Puzzles;
using System.Diagnostics;

namespace AOC2023
{
    public class AOC
    {
        private static List<IAdventPuzzle> adventPuzzles = new List<IAdventPuzzle>();

        public static IAdventPuzzle[] GetAdventPuzzles()
        {
            adventPuzzles.Add(new Day1Part1());
            adventPuzzles.Add(new Day1Part2());
            adventPuzzles.Add(new Day2Part1());
            adventPuzzles.Add(new Day2Part2());
            adventPuzzles.Add(new Day3Part1());
            adventPuzzles.Add(new Day3Part2());
            adventPuzzles.Add(new Day4Part1());
            adventPuzzles.Add(new Day4Part2());
            adventPuzzles.Add(new Day5Part1());
            adventPuzzles.Add(new Day5Part2());
            adventPuzzles.Add(new Day6Part1());
            adventPuzzles.Add(new Day6Part2());
            adventPuzzles.Add(new Day7Part1());
            adventPuzzles.Add(new Day8Part1());
            adventPuzzles.Add(new Day9Part1());

            return adventPuzzles.ToArray();
        }

        public static void Log(string message)
        {
#if DEBUG
            Debug.WriteLine(message);
#endif
        }
    }
}
