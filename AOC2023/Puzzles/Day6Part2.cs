using AOC2023;
using AOC2023.Models.Day6Part2;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AOC2023.Puzzles
{
    public class Day6Part2 : IAdventPuzzle
    {
        public string Name { get; }
        public string? Solution { get; }

        private const string FILENAME = "Day6.txt";
        
        public Day6Part2()
        {
            Name = "Day 6 Part 2: Wait For It";
            Solution = "43364472";
        }

        public PuzzleOutput GetOutput()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            #region Puzzle
            string[] currPuzzleFileLines = File.ReadAllLines(Path.Combine(ConfigurationManager.AppSettings["PuzzleInputDirectory"], FILENAME));
            RaceSet raceSet = new RaceSet();

            #region Parse

            string[] timesInput = ((currPuzzleFileLines[0].Split(':'))[1]).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            raceSet.Times.Add(long.Parse(string.Join("", timesInput)));

            string[] distsInput = ((currPuzzleFileLines[1].Split(':'))[1]).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            raceSet.Distances.Add(long.Parse(string.Join("", distsInput)));

            AOC.Log(raceSet.ToString());

            #endregion Parse

            #endregion Puzzle

            stopwatch.Stop();

            PuzzleOutput puzzleOutput = new PuzzleOutput();
            puzzleOutput.Result = raceSet.GetNumberOfWaysToWinForRace(0).ToString();
            puzzleOutput.CompletionTime = stopwatch.ElapsedMilliseconds;

            return puzzleOutput;
        }
    }
}

namespace AOC2023.Models.Day6Part2
{
    public class RaceSet
    {
        public List<long> Times { get; set; } = new List<long>();
        public List<long> Distances { get; set; } = new List<long>();

        public long GetDistanceWhenHeld(int raceId, long msHeld)
        {
            return (Times[raceId] - msHeld) * msHeld;
        }

        public long GetNumberOfWaysToWinForRace(int raceId)
        {
            long winningHoldTimes = 0;
            for (int i = 0; i <= Times[raceId]; i++)
            {
                long distanceForCurrHoldTime = GetDistanceWhenHeld(raceId, i);

                if (distanceForCurrHoldTime > Distances[raceId])
                    winningHoldTimes++;
            }

            return winningHoldTimes;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}