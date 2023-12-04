using AOC2023.Puzzles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023
{
    public class AOC
    {
        private static IAdventPuzzle[] adventPuzzles = new IAdventPuzzle[2];

        public static IAdventPuzzle[] GetAdventPuzzles()
        {
            adventPuzzles[0] = new Day1Part1();
            adventPuzzles[1] = new Day1Part2();
            return adventPuzzles;
        }

        public static void Log(string message)
        {
#if DEBUG
            Debug.WriteLine(message);
#endif
        }
    }
}
