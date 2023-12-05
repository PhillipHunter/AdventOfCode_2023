using System.Configuration;
using System.Diagnostics;
using System.Text;

namespace AOC2023.Puzzles
{
    public class Day1Part1 : IAdventPuzzle
    {
        public string Name { get; }
        public string? Solution { get; }

        private const string FILENAME = "Day1.txt";

        public Day1Part1()
        {
            Name = "Day 1 Part 1: Trebuchet?!";
            Solution = "55621";
        }
        
        public PuzzleOutput GetOutput()
        {
            Stopwatch stopwatch  = Stopwatch.StartNew();

            #region Puzzle
            int sum = 0;

            string[] currPuzzleFileLines = File.ReadAllLines(Path.Combine(ConfigurationManager.AppSettings["PuzzleInputDirectory"], FILENAME));

            for(int i = 0; i < currPuzzleFileLines.Length; i++)
            {
                string currLine = currPuzzleFileLines[i];
                AOC.Log($"Line {i + 1} {currLine}");
                StringBuilder currLineNumbers = new StringBuilder();
                string finalLine = string.Empty;

                foreach (char currChar in currLine)
                {
                    int value;
                    bool parsed = int.TryParse(currChar.ToString(), out value);

                    if(parsed)
                    {
                        currLineNumbers.Append(value.ToString());
                    }
                }
                
                string currLineNumbersString = currLineNumbers.ToString();
                if (currLineNumbers.Length > 2)
                {
                    finalLine = $"{currLineNumbersString[0]}{currLineNumbersString[currLineNumbersString.Length - 1]}";
                }
                // I thought if it was one digit it only wanted that added, so crammed this in last second.
                else if (currLineNumbers.Length == 1)
                {
                    finalLine = $"{currLineNumbersString[0]}{currLineNumbersString[0]}";
                }
                else
                {
                    finalLine = currLineNumbersString;
                }

                AOC.Log(finalLine);
                
                try
                {
                    sum += int.Parse(finalLine);
                }
                catch(FormatException ex)
                {
                    AOC.Log("No numbers found in this line.");
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
