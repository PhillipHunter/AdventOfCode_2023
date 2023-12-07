using AOC2023;
using AOC2023.Models.Day5Part2;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AOC2023.Puzzles
{
    public class Day5Part2 : IAdventPuzzle
    {
        public string Name { get; }
        public string? Solution { get; }

        private const string FILENAME = "Day5.txt";
        
        public Day5Part2()
        {
            Name = "Day 5 Part 2: If You Give A Seed A Fertilizer";
            Solution = "84206669"; // Final runtime was 20126583 milliseconds, or 5.6 hours :)
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

            long lastInitial = 0;
            for(int currSeedInputIndex = 0; currSeedInputIndex < seedInput.Length; currSeedInputIndex++)
            {
                string currSeedInput = seedInput[currSeedInputIndex];
                bool initial = (currSeedInputIndex % 2 == 0); // If true, its iniital number, rather than range.
                
                if(initial)
                {
                    lastInitial = long.Parse(currSeedInput);
                }
                else
                {
                    almanac.SeedRanges.Add(new SeedRange()
                    {
                        SeedNumber = lastInitial,
                        Range = long.Parse(currSeedInput)
                    });
                }
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

            object progressLock = new object();
            object minSeedLock = new object();
            
            long? minSeedLocation = null;
            long progress = 0;
            long total = almanac.SeedRanges.Sum(s => s.Range);

            List<Task> seedLocationTasks = new List<Task>();

            foreach (SeedRange currSeedRange in almanac.SeedRanges)
            {
                Task currSeedLocationTask = Task.Run(() =>
                {
                    for (int i = 0; i < currSeedRange.Range; i++)
                    {
                        lock (progressLock)
                        {
                            progress++;
                            if (progress % 1000000 == 0)
                            {
                                AOC.Log($"Completed {progress} / {total}. {(progress / (float)total) * 100.0f}%");
                            }
                        }

                        long currSeedLocation = GetLocationForSeed(currSeedRange.SeedNumber + i);
                        lock (minSeedLock)
                        {
                            if (minSeedLocation == null || currSeedLocation < minSeedLocation)
                            {
                                minSeedLocation = currSeedLocation;
                            }
                        }
                    }
                });

                seedLocationTasks.Add(currSeedLocationTask);
            }

            Task.WaitAll(seedLocationTasks.ToArray());

            #endregion Puzzle

            stopwatch.Stop();

            PuzzleOutput puzzleOutput = new PuzzleOutput();
            puzzleOutput.Result = (minSeedLocation.GetValueOrDefault(-1)).ToString();
            puzzleOutput.CompletionTime = stopwatch.ElapsedMilliseconds;

            return puzzleOutput;
        }
    }
}

namespace AOC2023.Models.Day5Part2
{
    public class Almanac
    {
        public List<SeedRange> SeedRanges { get; set; } = new List<SeedRange>();
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

    public class SeedRange
    {
        public long SeedNumber { get; set; }
        public long Range { get; set; }
    }
}