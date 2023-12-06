using AOC2023;
using AOC2023.Models.Day5Part1;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AOC2023.Puzzles
{
    public class Day5Part1 : IAdventPuzzle
    {
        public string Name { get; }
        public string? Solution { get; }

        private const string FILENAME = "Day5.txt";
        
        public Day5Part1()
        {
            Name = "Day 5 Part 1: If You Give A Seed A Fertilizer";
            Solution = "388071289";
        }

        public PuzzleOutput GetOutput()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            #region Puzzle
            string[] currPuzzleFileLines = File.ReadAllLines(Path.Combine(ConfigurationManager.AppSettings["PuzzleInputDirectory"], FILENAME));

            Almanac almanac = new Almanac();

            string headerRegexPattern = @"(\w+)-to-(\w+) map:";
            Regex headerRegex = new Regex(headerRegexPattern);

            List<string> plantingProcessOrder = new List<string>();
            plantingProcessOrder.Add("seed");
            plantingProcessOrder.Add("soil");
            plantingProcessOrder.Add("fertilizer");
            plantingProcessOrder.Add("water");
            plantingProcessOrder.Add("light");
            plantingProcessOrder.Add("temperature");
            plantingProcessOrder.Add("humidity");
            plantingProcessOrder.Add("location");

            #region Parse
            // Parse Seeds
            string[] seedInput = (currPuzzleFileLines[0].Split(':')[1]).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string currSeedInput in seedInput)
            {
                almanac.Seeds.Add(long.Parse(currSeedInput));
            }
            
            string? headerSourceName = null;
            string? headerDestName = null;
            
            // Parse Maps
            for (int row = 1; row < currPuzzleFileLines.Length; row++)
            {
                string currPuzzleFileLine = currPuzzleFileLines[row];
                
                if (string.IsNullOrWhiteSpace(currPuzzleFileLine))
                    continue;

                AOC.Log($"Current line: {row + 1} / {currPuzzleFileLines.Length}");

                // Parse Possible Header Line
                Match possibleHeaderMatch = headerRegex.Match(currPuzzleFileLine);
                if(possibleHeaderMatch.Success) // If we're on a header line, set source/dest
                {
                    headerSourceName = possibleHeaderMatch.Groups[1].Value;
                    headerDestName = possibleHeaderMatch.Groups[2].Value;

                    continue; // Finish parsing this header line, move on to mapping
                }

                // Below this point we know this is a mapping number line

                Debug.Assert(headerSourceName != null && headerDestName != null);

                Mapping currMapping = new Mapping();
                currMapping.SourceName = headerSourceName;
                currMapping.DestinationName = headerDestName;

                string[] currMappingInput = currPuzzleFileLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                currMapping.SourceRangeStart = long.Parse(currMappingInput[1]);
                currMapping.DestRangeStart = long.Parse(currMappingInput[0]);
                currMapping.Range = long.Parse(currMappingInput[2]);

                almanac.Mappings.Add(currMapping);
            }

            #endregion Parse

            AOC.Log(almanac.ToString());

            long GetLocationForSeed(long seed)
            {
                long eventualLocation = seed;
                for(int i = 0; i < plantingProcessOrder.Count - 1; i++)
                {
                    eventualLocation = almanac.MapLookUp(plantingProcessOrder[i], plantingProcessOrder[i + 1], eventualLocation);
                }
                return eventualLocation;
            }

            List<long> seedLocations = new List<long>();
            foreach (long currSeed in almanac.Seeds)
            {
                AOC.Log($"Location for seed: {currSeed}: {GetLocationForSeed(currSeed)}");
                seedLocations.Add(GetLocationForSeed(currSeed));
            }

            #endregion Puzzle

            stopwatch.Stop();

            PuzzleOutput puzzleOutput = new PuzzleOutput();
            puzzleOutput.Result = seedLocations.Min().ToString();
            puzzleOutput.CompletionTime = stopwatch.ElapsedMilliseconds;

            return puzzleOutput;
        }
    }
}

namespace AOC2023.Models.Day5Part1
{
    public class Almanac
    {
        public List<long> Seeds { get; set; } = new List<long>();
        public List<Mapping> Mappings { get; set; } = new List<Mapping>();

        /// <returns>Destination Number</returns>
        public long MapLookUp(string sourceName, string destName, long sourceNumber)
        {
            List<Mapping> sourceDestMappings = Mappings.Where(m => 
                                                         m.SourceName == sourceName
                                                      && m.DestinationName == destName)
                                                        .ToList();

            Mapping? correctMapping = sourceDestMappings.Where(m => sourceNumber >= m.SourceRangeStart && sourceNumber < (m.SourceRangeStart + m.Range))
                                      .OrderBy(m => m.SourceRangeStart).FirstOrDefault();

            if (correctMapping == null)
                return sourceNumber;

            return (sourceNumber - correctMapping.SourceRangeStart) + correctMapping.DestRangeStart;

        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public class Mapping
    {
        public long SourceRangeStart { get; set; }
        public long DestRangeStart { get; set; }
        public string SourceName { get; set; } = string.Empty;
        public string DestinationName { get; set; } = string.Empty;
        public long Range { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}