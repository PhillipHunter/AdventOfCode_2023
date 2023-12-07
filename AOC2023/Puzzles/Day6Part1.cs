using AOC2023;
using AOC2023.Models.Day6Part1;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AOC2023.Puzzles
{
    public class Day6Part1 : IAdventPuzzle
    {
        public string Name { get; }
        public string? Solution { get; }

        private const string FILENAME = "Day6.txt";
        
        public Day6Part1()
        {
            Name = "Day 6 Part 1: Wait For It";
        }

        public PuzzleOutput GetOutput()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            #region Puzzle
            string[] currPuzzleFileLines = File.ReadAllLines(Path.Combine(ConfigurationManager.AppSettings["PuzzleInputDirectory"], FILENAME));
            RaceSet raceSet = new RaceSet();

            #region Parse

            string[] timesInput = ((currPuzzleFileLines[0].Split(':'))[1]).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach(string currTimesInput in timesInput)
            {
                raceSet.Times.Add(int.Parse(currTimesInput));
            }

            string[] distsInput = ((currPuzzleFileLines[1].Split(':'))[1]).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string currDistsInput in distsInput)
            {
                raceSet.Distances.Add(int.Parse(currDistsInput));
            }

            AOC.Log(raceSet.ToString());

            #endregion Parse


            List<int> waysToWin = new List<int>();
            for (int i = 0; i < raceSet.Times.Count; i++)
            {
                int currWaysToWin = raceSet.GetNumberOfWaysToWinForRace(i);
                AOC.Log($"There are {currWaysToWin} ways to win race {i}");
                
                waysToWin.Add(currWaysToWin);
            }

            

            #endregion Puzzle

            stopwatch.Stop();

            PuzzleOutput puzzleOutput = new PuzzleOutput();
            puzzleOutput.Result = waysToWin.Aggregate((a, b) =>  a * b).ToString();
            puzzleOutput.CompletionTime = stopwatch.ElapsedMilliseconds;

            return puzzleOutput;
        }
    }
}

namespace AOC2023.Models.Day6Part1
{
    public class RaceSet
    {
        public List<int> Times { get; set; } = new List<int>();
        public List<int> Distances { get; set; } = new List<int>();

        public int GetDistanceWhenHeld(int raceId, int msHeld)
        {
            return (Times[raceId] - msHeld) * msHeld;
        }

        public int GetNumberOfWaysToWinForRace(int raceId, bool optimize = false)
        {
            List<int> winningHoldTimes = new List<int>();
            for (int i = 0; i <= Times[raceId]; i++)
            {
                int distanceForCurrHoldTime = GetDistanceWhenHeld(raceId, i);

                if (distanceForCurrHoldTime > Distances[raceId])
                    winningHoldTimes.Add(i);
            }

            return winningHoldTimes.Count;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}