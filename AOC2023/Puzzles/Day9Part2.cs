using AOC2023;
using AOC2023.Models.Day9Part2;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AOC2023.Puzzles
{
    public class Day9Part2 : IAdventPuzzle
    {
        public string Name { get; }
        public string? Solution { get; }

        private const string FILENAME = "Day9.txt";

        public Day9Part2()
        {
            Name = "Day 9 Part 2: Mirage Maintenance";
            Solution = "1154";
        }

        public PuzzleOutput GetOutput()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            #region Puzzle
            string[] currPuzzleFileLines = File.ReadAllLines(Path.Combine(ConfigurationManager.AppSettings["PuzzleInputDirectory"], FILENAME));

            Report report = new Report();

            #region Parse

            for (int row = 0; row < currPuzzleFileLines.Length; row++)
            {
                string currPuzzleFileLine = currPuzzleFileLines[row];

                SensorData sensorData = new SensorData();
                List<long> sensorDataNumbers = new List<long>();

                AOC.Log($"Current line: {row + 1} / {currPuzzleFileLines.Length}");
                foreach (string currNumberInput in currPuzzleFileLine.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                    sensorDataNumbers.Add(long.Parse(currNumberInput));

                sensorData.Data = sensorDataNumbers;

                report.SensorDatas.Add(sensorData);
            }

            #endregion Parse

            List<long> nextValues = new List<long>();
            foreach (SensorData sensorData in report.SensorDatas)
            {
                nextValues.Add(sensorData.GetPreviousValue());
            }

            #endregion Puzzle

            stopwatch.Stop();

            PuzzleOutput puzzleOutput = new PuzzleOutput();
            puzzleOutput.Result = nextValues.Sum().ToString();
            puzzleOutput.CompletionTime = stopwatch.ElapsedMilliseconds;

            return puzzleOutput;
        }
    }
}

namespace AOC2023.Models.Day9Part2
{
    public class Report
    {
        public List<SensorData> SensorDatas { get; set; } = new List<SensorData>();
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public class SensorData
    {
        public List<long> Data { get; set; } = new List<long>();

        private List<List<long>> nestedData = new List<List<long>>();

        public long GetPreviousValue()
        {
            AOC.Log("\n");
            List<long>? lastNestedDataFacet = null;

            while (lastNestedDataFacet == null || lastNestedDataFacet.Any(n => n != 0))
            {
                if (lastNestedDataFacet == null)
                    lastNestedDataFacet = Data;

                List<long> nestedDataFacet = new List<long>();
                for (int i = 1; i < lastNestedDataFacet.Count; i++)
                {
                    nestedDataFacet.Add(lastNestedDataFacet[i] - lastNestedDataFacet[i - 1]);
                }
                nestedData.Add(nestedDataFacet);
                lastNestedDataFacet = nestedDataFacet;
            }

            nestedData.Insert(0, Data);

            for (int i = (nestedData.Count - 1); i >= 0; i--)
            {
                List<long> currData = nestedData[i];

                if (i == (nestedData.Count - 1)) // First iteration needs 0
                {
                    currData.Insert(0, 0);
                }
                else
                {
                    AOC.Log($"{i}: {currData.First()} - {nestedData[i + 1].First()}");
                    currData.Insert(0, currData.First() - nestedData[i + 1].First());

                }

                AOC.Log($"{i} {JsonConvert.SerializeObject(currData)}");
            }

            return nestedData[0].First();
        }

        public long GetNextValue()
        {
            AOC.Log("\n");
            List<long>? lastNestedDataFacet = null;

            while (lastNestedDataFacet == null || lastNestedDataFacet.Any(n => n != 0))
            {
                if (lastNestedDataFacet == null)
                    lastNestedDataFacet = Data;

                List<long> nestedDataFacet = new List<long>();
                for (int i = 1; i < lastNestedDataFacet.Count; i++)
                {
                    nestedDataFacet.Add(lastNestedDataFacet[i] - lastNestedDataFacet[i - 1]);
                }
                nestedData.Add(nestedDataFacet);
                lastNestedDataFacet = nestedDataFacet;
            }

            nestedData.Insert(0, Data);

            for (int i = (nestedData.Count - 1); i >= 0; i--)
            {
                List<long> currData = nestedData[i];

                if (i == (nestedData.Count - 1)) // First iteration needs 0
                {
                    currData.Add(0);
                }
                else
                {
                    currData.Add(currData.Last() + nestedData[i + 1].Last());
                }

                AOC.Log($"{i} {JsonConvert.SerializeObject(currData)}");
            }

            return nestedData[0].Last();
        }

    }
}