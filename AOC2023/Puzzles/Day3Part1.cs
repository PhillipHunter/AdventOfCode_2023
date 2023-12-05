using AOC2023;
using AOC2023.Models.Day3Part1;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Drawing;

namespace AOC2023.Puzzles
{
    public class Day3Part1 : IAdventPuzzle
    {
        public string Name { get; }
        public string? Solution { get; }

        private const string FILENAME = "Day3.txt";
        
        public Day3Part1()
        {
            Name = "Day 3 Part 1: Gear Ratios";
            Solution = "507214";
        }

        public PuzzleOutput GetOutput()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            #region Puzzle
            string[] currPuzzleFileLines = File.ReadAllLines(Path.Combine(ConfigurationManager.AppSettings["PuzzleInputDirectory"], FILENAME));
            int sum = 0;

            Board board = new Board();

            for(int row = 0; row < currPuzzleFileLines.Length; row++)
            {
                string currPuzzleFileLine = currPuzzleFileLines[row];
                AOC.Log($"Current line: {row + 1} / {currPuzzleFileLines.Length}");

                string possibleDigits = string.Empty;
                for (int col = 0; col < currPuzzleFileLine.Length; col++)
                {
                    int currDigit = -1;
                    if(int.TryParse(currPuzzleFileLine[col].ToString(), out currDigit))
                    {
                        possibleDigits = $"{possibleDigits}{currDigit}";
                        if (col == currPuzzleFileLine.Length - 1) // Check for literal edge case and consider number done if so.
                        {
                            board.Numbers[new Point(col - possibleDigits.Length, row)] = int.Parse(possibleDigits);
                            possibleDigits = string.Empty;
                        }
                    }
                    else
                    {
                        if (possibleDigits.Length > 0)
                            board.Numbers[new Point(col - possibleDigits.Length, row)] = int.Parse(possibleDigits);
                        possibleDigits = string.Empty;

                        if(currPuzzleFileLine[col] != '.')
                        {
                            board.Symbols[new Point(col - possibleDigits.Length, row)] = currPuzzleFileLine[col];
                        }
                    }
                }
            }

            AOC.Log($"Board Created: {board}");

            foreach(KeyValuePair<Point, int> boardNumbersKP in board.Numbers)
            {
                bool hasAdjacentSymbol = false;

                for (int testXAdd = 0; testXAdd < boardNumbersKP.Value.ToString().Length; testXAdd++)
                {
                    int checkX = boardNumbersKP.Key.X + testXAdd;
                    int checkY = boardNumbersKP.Key.Y;

                    // Up
                    if (board.Symbols.ContainsKey(new Point(checkX, checkY - 1)))
                        hasAdjacentSymbol = true;
                    // Down
                    if (board.Symbols.ContainsKey(new Point(checkX, checkY + 1)))
                        hasAdjacentSymbol = true;
                    // Left
                    if (board.Symbols.ContainsKey(new Point(checkX - 1, checkY)))
                        hasAdjacentSymbol = true;
                    // Right
                    if (board.Symbols.ContainsKey(new Point(checkX + 1, checkY)))
                        hasAdjacentSymbol = true;
                    // Top Left
                    if (board.Symbols.ContainsKey(new Point(checkX - 1, checkY -1)))
                        hasAdjacentSymbol = true;
                    // Top Right
                    if (board.Symbols.ContainsKey(new Point(checkX + 1, checkY - 1)))
                        hasAdjacentSymbol = true;
                    // Bottom Left
                    if (board.Symbols.ContainsKey(new Point(checkX - 1, checkY + 1)))
                        hasAdjacentSymbol = true;
                    // Bottom Right
                    if (board.Symbols.ContainsKey(new Point(checkX + 1, checkY + 1)))
                        hasAdjacentSymbol = true;

                    if (hasAdjacentSymbol)
                        break;
                }

                if(hasAdjacentSymbol)
                {
                    sum += boardNumbersKP.Value;
                }
                else
                {
                    AOC.Log($"{boardNumbersKP.Value} does not have an adjacent!");
                }
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

namespace AOC2023.Models.Day3Part1
{
    public class Board
    {
        public Dictionary<Point, int> Numbers { get; set; } = new Dictionary<Point, int>();
        public Dictionary<Point, char> Symbols { get; set; } = new Dictionary<Point, char>();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}