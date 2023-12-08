using AOC2023;
using AOC2023.Models.Day7Part1;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AOC2023.Puzzles
{
    public class Day7Part1 : IAdventPuzzle
    {
        public string Name { get; }
        public string? Solution { get; }

        private const string FILENAME = "Day7.txt";
        
        public Day7Part1()
        {
            Name = "Day 7 Part 1: Camel Cards";
            Solution = "248836197";
        }

        public PuzzleOutput GetOutput()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            #region Puzzle
            string[] currPuzzleFileLines = File.ReadAllLines(Path.Combine(ConfigurationManager.AppSettings["PuzzleInputDirectory"], FILENAME));

            #region Parse

            Game game = new Game();

            for (int row = 0; row < currPuzzleFileLines.Length; row++)
            {
                string currPuzzleFileLine = currPuzzleFileLines[row];

                HandBid handBid = new HandBid();
                string[] handBidInput = currPuzzleFileLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                handBid.Hand.Cards = handBidInput[0];
                handBid.Bid = int.Parse(handBidInput[1]);

                game.HandBids.Add(handBid);
            }

            AOC.Log(game.ToString());

            #endregion Parse

            game.HandBids = game.HandBids.OrderBy(a => a.Hand).ToList();

            List<int> winnings = new List<int>();
            for (int handBidIndex = 0; handBidIndex < game.HandBids.Count; handBidIndex++)
            {
                winnings.Add((handBidIndex + 1) * game.HandBids[handBidIndex].Bid);
            }

            #endregion Puzzle

            stopwatch.Stop();

            PuzzleOutput puzzleOutput = new PuzzleOutput();
            puzzleOutput.Result = winnings.Sum().ToString();
            puzzleOutput.CompletionTime = stopwatch.ElapsedMilliseconds;

            return puzzleOutput;
        }
    }
}

namespace AOC2023.Models.Day7Part1
{
    public class Game
    {
        public List<HandBid> HandBids { get; set; } = new List<HandBid>();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public class HandBid
    {
        public Hand Hand { get; set; } = new Hand();
        public int Bid { get; set; }
    }

    public class Hand : IComparable<Hand>
    {
        public string Cards { get; set; } = string.Empty;

        public enum SpecialHand { NONE, FIVE_KIND, FOUR_KIND, FULL_HOUSE, THREE_KIND, TWO_PAIR, ONE_PAIR, HIGH_CARD }

        private List<SpecialHand> SpecialHandRankings = new List<SpecialHand>() { SpecialHand.HIGH_CARD, SpecialHand.ONE_PAIR, SpecialHand.TWO_PAIR, SpecialHand.THREE_KIND, SpecialHand.FULL_HOUSE, SpecialHand.FOUR_KIND, SpecialHand.FIVE_KIND, SpecialHand.NONE };

        public static readonly List<char> CardStrengths = new List<char>()
        {
            '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A'
        };

        public SpecialHand GetSpecial()
        {
            Dictionary<char, int> charCounts = new Dictionary<char, int>();

            // Parse characters into counts
            foreach(char c in Cards)
            {
                if(charCounts.ContainsKey(c))
                {
                    charCounts[c]++;
                }
                else
                {
                    charCounts.Add(c, 1);
                }
            }

            // Five of a kind
            if (charCounts.Max(c => c.Value) == 5)
            {
                return SpecialHand.FIVE_KIND;
            }
            // Four of a Kind
            else if (charCounts.Max(c => c.Value) == 4)
            {
                return SpecialHand.FOUR_KIND;
            }
            // Full House
            else if (charCounts.Max(c => c.Value) == 3 && (charCounts.Where(c => c.Value == 2).Count() > 0))
            {
                return SpecialHand.FULL_HOUSE;
            }
            // Three of a Kind
            else if (charCounts.Max(c => c.Value) == 3)
            {
                return SpecialHand.THREE_KIND;
            }
            // Two Pair
            else if (charCounts.Where(c => c.Value == 2).Count() == 2) 
            {
                return SpecialHand.TWO_PAIR;
            }
            // One Pair
            else if (charCounts.Where(c => c.Value == 2).Count() == 1 && charCounts.Max(c => c.Value) == 2)
            {
                return SpecialHand.ONE_PAIR;
            }
            // High Card
            else if (charCounts.Max(c => c.Value) == 1)
            {
                return SpecialHand.HIGH_CARD;
            }

            return SpecialHand.NONE;
        }

        private int GetStrength(char card)
        {
            return CardStrengths.IndexOf(card);
        }

        public int CompareTo(Hand? other)
        {
            Debug.Assert(other != null);

            SpecialHand thisSpecial = GetSpecial();
            SpecialHand otherSpecial = other.GetSpecial();

            if (SpecialHandRankings.IndexOf(thisSpecial) > SpecialHandRankings.IndexOf(otherSpecial))
            {
                return 1;
            }
            if (SpecialHandRankings.IndexOf(thisSpecial) < SpecialHandRankings.IndexOf(otherSpecial))
            {
                return -1;
            }
            else
            {
                for(int i = 0; i < Cards.Length; i++)
                {
                    if (GetStrength(Cards[i]) > GetStrength(other.Cards[i]))
                        return 1;
                    else if (GetStrength(Cards[i]) < GetStrength(other.Cards[i]))
                        return -1;
                }

                return 0;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Hand && Cards.Equals(((Hand)obj).Cards);
        }

        public override string ToString()
        {
            return Cards;
        }

        public override int GetHashCode()
        {
            return Cards.GetHashCode();
        }
    }
}