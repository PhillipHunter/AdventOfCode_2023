using AOC2023;
using AOC2023.Models.Day4Part2;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AOC2023.Puzzles
{
    public class Day4Part2 : IAdventPuzzle
    {
        public string Name { get; }
        public string? Solution { get; }

        private const string FILENAME = "Day4.txt";
        
        public Day4Part2()
        {
            Name = "Day 4 Part 2: Scratchcards";
            Solution = "9924412";
        }

        public PuzzleOutput GetOutput()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            #region Puzzle
            string[] currPuzzleFileLines = File.ReadAllLines(Path.Combine(ConfigurationManager.AppSettings["PuzzleInputDirectory"], FILENAME));

            string initialCapturePattern = @"Card\s*(\d+):(.*?)\|(.*)";
            Regex initialCaptureRegex = new Regex(initialCapturePattern);

            Game game = new Game();

            for (int row = 0; row < currPuzzleFileLines.Length; row++)
            {
                string currPuzzleFileLine = currPuzzleFileLines[row];
                AOC.Log($"Current line: {row + 1} / {currPuzzleFileLines.Length}");

                Match match = initialCaptureRegex.Match(currPuzzleFileLine);

                Debug.Assert(match.Success);
                
                ScratchCard scratchCard = new ScratchCard();
                scratchCard.Id = int.Parse(match.Groups[1].Value);

                string[] winningNumbersInput = match.Groups[2].Value.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach(string currWinNumberInput in winningNumbersInput)
                    scratchCard.WinningNumbers.Add(int.Parse(currWinNumberInput));

                string[] playerNumbersInput = match.Groups[3].Value.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (string currPlayerNumberInput in playerNumbersInput)
                    scratchCard.PlayerNumbers.Add(int.Parse(currPlayerNumberInput));

                // Scores
                foreach(int currWinningNumber in scratchCard.WinningNumbers)
                {
                    if (scratchCard.PlayerNumbers.Contains(currWinningNumber))
                    {
                        scratchCard.CorrectCount++;
                    }
                }

                scratchCard.Score = (int)Math.Pow(2, scratchCard.CorrectCount - 1);

                game.ScratchCards.Add(scratchCard);
            }

            // Instance Counts
            foreach(ScratchCard currScratchCard in game.ScratchCards)
            {
                AOC.Log($"Processing instance counts for {currScratchCard.Id}. Matching numbers: {currScratchCard.CorrectCount}");

                List<ScratchCard> scratchCardsToIncrement = game.ScratchCards.Where(sc => sc.Id > currScratchCard.Id)
                                                                             .OrderBy(sc => sc.Id)
                                                                             .Take(currScratchCard.CorrectCount)
                                                                             .ToList();

                foreach(ScratchCard currIncrementScratchCard in scratchCardsToIncrement)
                {
                    AOC.Log($"Incrementing {currIncrementScratchCard.Id}");
                    currIncrementScratchCard.InstanceCount += 1 * currScratchCard.InstanceCount;
                }
            }
            
            AOC.Log(game.ToString());
            int sum = game.ScratchCards.Sum(sc => sc.InstanceCount);

            #endregion Puzzle

            stopwatch.Stop();

            PuzzleOutput puzzleOutput = new PuzzleOutput();
            puzzleOutput.Result = sum.ToString();
            puzzleOutput.CompletionTime = stopwatch.ElapsedMilliseconds;

            return puzzleOutput;
        }
    }
}

namespace AOC2023.Models.Day4Part2
{
    public class Game
    {
        public List<ScratchCard> ScratchCards { get; set; } = new List<ScratchCard>();

        public int TotalScore { get { return ScratchCards.Sum(g => g.Score); } }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public class ScratchCard
    {
        public int Id { get; set; }
        public HashSet<int> WinningNumbers { get; set; } = new HashSet<int>();
        public HashSet<int> PlayerNumbers { get; set; } = new HashSet<int>();

        public int CorrectCount { get; set; } = 0;
        public int Score { get; set; } = 0;
        public int InstanceCount { get; set; } = 1;
    }
}