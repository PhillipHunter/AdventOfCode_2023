using System.Configuration;
using System.Diagnostics;
using System.Text;

namespace AOC2023.Puzzles
{
    public class Day1Part2 : IAdventPuzzle
    {
        public string Name { get; }

        private const string FILENAME = "Day1_Part2Ex.txt";
        private Dictionary<string, int> numbers = new Dictionary<string, int>();

        public Day1Part2()
        {
            Name = "Day 1 Part 2: Trebuchet?!";

            numbers["ONE"] = 1;
            numbers["TWO"] = 2;
            numbers["THREE"] = 3;
            numbers["FOUR"] = 4;
            numbers["FIVE"] = 5;
            numbers["SIX"] = 6;
            numbers["SEVEN"] = 7;
            numbers["EIGHT"] = 8;
            numbers["NINE"] = 9;
        }

        public PuzzleOutput GetOutput()
        {
            Stopwatch stopwatch  = Stopwatch.StartNew();

            #region Puzzle
            int sum = 0;

            string[] currPuzzleFileLines = File.ReadAllLines(Path.Combine(ConfigurationManager.AppSettings["PuzzleInputDirectory"], FILENAME));

            for(int i = 0; i < currPuzzleFileLines.Length; i++)
            {
                string currLine = currPuzzleFileLines[i].ToUpper();
                string originalLine = new string(currLine);

                AOC.Log($"\nLine {i + 1} {currLine}");
                
                bool done = false;
                while (done != true)
                {
                    for (int lineCharIndex = 0; lineCharIndex < currLine.Length; lineCharIndex++)
                    {
                        Debug.WriteLine($"Start: lineCharIndex: {lineCharIndex} currLineLength: {currLine.Length}");
                        bool foundInner = false;
                        if (foundInner) break;

                        string checking = currLine.Substring(0, lineCharIndex + 1);
                        AOC.Log($"Checking {checking}");

                        foreach (KeyValuePair<string, int> numericalKP in numbers)
                        {
                            if (checking.Contains(numericalKP.Key))
                            {
                                checking = $"{checking.Replace(numericalKP.Key, numericalKP.Value.ToString())}{currLine.Substring(lineCharIndex + 1)}";
                                AOC.Log($"Found number. Restart with {checking}");
                                currLine = checking;
                                foundInner = true;
                                break;
                            }
                        }
                        Debug.WriteLine($"End: lineCharIndex: {lineCharIndex} currLineLength: {currLine.Length}");
                        if (!foundInner && lineCharIndex == currLine.Length - 1)
                        {
                            done = true;
                            AOC.Log("Done with replacement");
                        }
                    }
                }
                

                AOC.Log($"Replaced line: {currLine}");
                AOC.Log($"Original line: {originalLine}");

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
