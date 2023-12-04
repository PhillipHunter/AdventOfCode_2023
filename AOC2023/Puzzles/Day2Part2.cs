﻿using AOC2023;
using AOC2023.Models.Day2Part2;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.Text;

namespace AOC2023.Puzzles
{
    public class Day2Part2 : IAdventPuzzle
    {
        public string Name { get; }

        private const string FILENAME = "Day2.txt";
        
        private Dictionary<string, int> totalBagCounts = new Dictionary<string, int>() 
        { 
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 } 
        };

        public Day2Part2()
        {
            Name = "Day 2 Part 2: Cube Conundrum";
        }

        public PuzzleOutput GetOutput()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            #region Puzzle
            string[] currPuzzleFileLines = File.ReadAllLines(Path.Combine(ConfigurationManager.AppSettings["PuzzleInputDirectory"], FILENAME));
            int sum = 0;

            for(int i = 0; i < currPuzzleFileLines.Length; i++)
            {
                string currPuzzleFileLine = currPuzzleFileLines[i];
                AOC.Log($"Current line: {i + 1} / {currPuzzleFileLines.Length}");
                Game currGame = Game.Parse(currPuzzleFileLine);

                sum += currGame.GetMinimumPower();
            }

            #endregion Puzzle

            stopwatch.Stop();

            PuzzleOutput puzzleOutput = new PuzzleOutput();
            puzzleOutput.Result = sum.ToString();
            puzzleOutput.CompletionTime = stopwatch.ElapsedMilliseconds;

            return puzzleOutput;
        }
    }
}

namespace AOC2023.Models.Day2Part2
{
    public class Game
    {
        public int Id { get; set; }
        public List<Set> Sets { get; set; } = new List<Set>();

        private Dictionary<string, int> minimums = new Dictionary<string, int>();

        public int GetMinimumPower()
        {
            int power = 1;
            foreach(KeyValuePair<string, int> minimumsKP in minimums)
            {
                power *= minimumsKP.Value;
            }

            return power;
        }

        public static Game Parse(string input)
        {
            Game game = new Game();

            int id = int.Parse((input.Split(":")[0]).Split(" ")[1]);
            game.Id = id;

            string inputGameSetsAll = input.Split(":")[1];
            string[] inputGameSets = inputGameSetsAll.Split(";");

            List<Set> sets = new List<Set>();

            foreach(string currInputGameSet in inputGameSets)
            {
                Set set = new Set();
                Dictionary<string, int> colorCounts = new Dictionary<string, int>();

                string[] currInputGameSetColors = currInputGameSet.Split(",");
                foreach(string currInputColor in currInputGameSetColors)
                {
                    string[] colorSplit = currInputColor.Trim().Split(" ");
                    int count = int.Parse(colorSplit[0]);
                    string color = colorSplit[1];
                    colorCounts[color] = count;

                    if((!game.minimums.ContainsKey(color)) || (game.minimums.ContainsKey(color) &&
                        count > game.minimums[color]
                        ))
                    {
                        game.minimums[color] = count;
                    }
                }

                set.ColorCounts = colorCounts;
                sets.Add(set);
            }

            game.Sets = sets;

            return game;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public class Set
    {
        public Dictionary<string, int> ColorCounts { get; set; } = new Dictionary<string, int>();
    }
}