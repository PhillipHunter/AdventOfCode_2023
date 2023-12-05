using AOC2023;
using AOC2023.Models.Day3Part2;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Drawing;

namespace AOC2023.Puzzles
{
    public class Day3Part2 : IAdventPuzzle
    {
        public string Name { get; }
        public string? Solution { get; }

        private const string FILENAME = "Day3.txt";
        
        public Day3Part2()
        {
            Name = "Day 3 Part 2: Gear Ratios";
            Solution = "72553319";
        }

        private static KeyValuePair<Point, int>? GetFullNumber(Board board, Point testPoint)
        {
            if (board.Symbols.ContainsKey(testPoint) || board.Periods.ContainsKey(testPoint)) // Check if we're even on a number and break otherwise (if no longer on a number).
                return null;

            List<KeyValuePair<Point, int>> numbersOnRow = board.Numbers.Where(n => n.Key.Y == testPoint.Y).ToList();
            KeyValuePair<Point, int>? targetNumber = numbersOnRow.OrderByDescending(n => n.Key.X).Where(n => n.Key.X <= testPoint.X).FirstOrDefault();

            return targetNumber;
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
                        else
                        {
                            board.Periods[new Point(col - possibleDigits.Length, row)] = currPuzzleFileLine[col];
                        }
                    }
                }
            }

            AOC.Log($"Board Created: {board}");

            foreach (KeyValuePair<Point, char> boardSymbolsKP in board.Symbols.Where(s => s.Value == '*'))
            {
                List<int> adjacentNumbers = new List<int>();

                // Check for a single digit of a number adj to this symbol
                int checkX = boardSymbolsKP.Key.X;
                int checkY = boardSymbolsKP.Key.Y;

                HashSet<Point> foundNumberPositions = new HashSet<Point>();

                ///
                /// Adds an adjacent number at the given position to the list
                ///
                void CheckPos(Point p)
                {
                    AOC.Log($"Checking pos: {p}");
                    KeyValuePair<Point, int>? possibleAdjacentNumber = GetFullNumber(board, p);
                    if (possibleAdjacentNumber.HasValue && !foundNumberPositions.Contains(possibleAdjacentNumber.Value.Key))
                    {
                        adjacentNumbers.Add(possibleAdjacentNumber.Value.Value);
                        AOC.Log($"Found adjacent at {p}: {possibleAdjacentNumber.Value.Value}");
                        foundNumberPositions.Add(possibleAdjacentNumber.Value.Key);
                    }
                }

                // Up
                CheckPos(new Point(checkX, checkY - 1));
                // Down
                CheckPos(new Point(checkX, checkY + 1));
                // Left
                CheckPos(new Point(checkX - 1, checkY));
                // Right
                CheckPos(new Point(checkX + 1, checkY));
                // Top Left
                CheckPos(new Point(checkX - 1, checkY - 1));
                // Top Right
                CheckPos(new Point(checkX + 1, checkY - 1));
                // Bottom Left
                CheckPos(new Point(checkX - 1, checkY + 1));
                // Bottom Right
                CheckPos(new Point(checkX + 1, checkY + 1));

                if (adjacentNumbers.Count == 2)
                {
                    int ratio = adjacentNumbers[0] * adjacentNumbers[1];
                    sum += ratio;
                    AOC.Log($"Gear at {boardSymbolsKP.Key} has ratio {ratio}");
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

namespace AOC2023.Models.Day3Part2
{
    public class Board
    {
        public Dictionary<Point, int> Numbers { get; set; } = new Dictionary<Point, int>();
        public Dictionary<Point, char> Symbols { get; set; } = new Dictionary<Point, char>();
        public Dictionary<Point, char> Periods { get; set; } = new Dictionary<Point, char>();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}