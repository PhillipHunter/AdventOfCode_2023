using AOC2023;
using AOC2023.Models.Day8Part1;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AOC2023.Puzzles
{
    public class Day8Part1 : IAdventPuzzle
    {
        public string Name { get; }
        public string? Solution { get; }

        private const string FILENAME = "Day8.txt";
        
        public Day8Part1()
        {
            Name = "Day 8 Part 1: Haunted Wasteland";
            Solution = "16531";
        }

        public PuzzleOutput GetOutput()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            #region Puzzle
            string[] currPuzzleFileLines = File.ReadAllLines(Path.Combine(ConfigurationManager.AppSettings["PuzzleInputDirectory"], FILENAME));

            #region Parse

            Map map = new Map();

            map.Instructions = currPuzzleFileLines[0].ToCharArray();

            Regex nodeRegex = new Regex(@"(\w+) = \((\w+),\s+(\w+)\)");

            for (int row = 2; row < currPuzzleFileLines.Length; row++)
            {
                string currPuzzleFileLine = currPuzzleFileLines[row];
                AOC.Log($"Current line: {row + 1} / {currPuzzleFileLines.Length}");

                Match nodeRegexMatch = nodeRegex.Match(currPuzzleFileLine);
                Debug.Assert(nodeRegexMatch.Success);

                InstructionSet instructionSet = new InstructionSet();
                instructionSet.Left = nodeRegexMatch.Groups[2].Value;
                instructionSet.Right = nodeRegexMatch.Groups[3].Value;

                map.Nodes.Add(nodeRegexMatch.Groups[1].Value, instructionSet);

                AOC.Log(currPuzzleFileLine);
            }

            AOC.Log($"\nMap:\n{map}");

            #endregion Parse

            int instrPos = 0;
            int stepCounter = 0;
            string currNode = "AAA";

            void IncrInstrPos()
            {
                instrPos++;
                if (instrPos >= map.Instructions.Length)
                    instrPos = 0;
            }

            while(currNode != "ZZZ")
            {
                stepCounter++;
                if (currNode == "ZZZ")
                {
                    currNode = "ZZZ";
                    continue;
                }

                if (map.Instructions[instrPos] == 'L')
                {
                    IncrInstrPos();

                    currNode = map.Nodes[currNode].Left;
                    continue;
                }
                else if (map.Instructions[instrPos] == 'R')
                {
                    IncrInstrPos();

                    currNode = map.Nodes[currNode].Right;
                    continue;
                }

                throw new InvalidOperationException("Invalid instruction character.");
            }

            AOC.Log($"Curr Node: {currNode} Count: {stepCounter}");

            #endregion Puzzle

            stopwatch.Stop();

            PuzzleOutput puzzleOutput = new PuzzleOutput();
            puzzleOutput.Result = stepCounter.ToString();
            puzzleOutput.CompletionTime = stopwatch.ElapsedMilliseconds;

            return puzzleOutput;
        }
    }
}

namespace AOC2023.Models.Day8Part1
{
    public class Map
    {
        public char[]? Instructions { get; set; }

        public Dictionary<string, InstructionSet> Nodes { get; set; } = new Dictionary<string, InstructionSet>();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public class InstructionSet
    {
        public string Left { get; set; } = string.Empty;
        public string Right { get; set; } = string.Empty;
    }
}