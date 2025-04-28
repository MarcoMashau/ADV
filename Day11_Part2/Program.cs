using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Program
    {
        static readonly int[] Powers = { 0, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000 };

        public static void Main(string[] args)
        {

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day11.txt");
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File {filePath} not found!");
                return;
            }

            string input = File.ReadAllText("day11.txt");

            // Create an instance of Day11 and solve the puzzle
            Program day11 = new Program();
            var solution = day11.Solve(input);


            Console.WriteLine($"after blinking 75 times, you will have {solution.stones} stones.");
        }

        public Solution Solve(string input)
        {
            var span = input.AsSpan();
            var stones1 = new Dictionary<long, long>();
            var stones2 = new Dictionary<long, long>(); // Removed use of 'Capacity'

            foreach (var range in input.Split(' '))
            {
                var stone = long.Parse(range);
                Set(stones1, stone, 1);
            }

            for (var day = 26; day <= 75; day++) Blink(ref stones1, ref stones2);
            var no_of_stones = stones1.Values.Sum();

            return new Solution(no_of_stones.ToString());
        }

        static void Blink(ref Dictionary<long, long> stones1, ref Dictionary<long, long> stones2)
        {
            foreach (var (stone, count) in stones1)
            {
                if (stone == 0)
                {
                    Set(stones2, 1, count);
                    continue;
                }

                var digits = Digits(stone);

                if (digits % 2 != 0)
                {
                    Set(stones2, stone * 2024, count);
                    continue;
                }

                var (a, b) = Math.DivRem(stone, Powers[digits / 2]);
                Set(stones2, a, count);
                Set(stones2, b, count);
            }

            (stones1, stones2) = (stones2, stones1);
            stones2.Clear();
        }

        static void Set(Dictionary<long, long> stoneCounts, long key, long count)
        {
            if (!stoneCounts.TryGetValue(key, out var c))
            {
                stoneCounts[key] = count;
            }
            else
            {
                stoneCounts[key] = c + count;
            }
        }

        static int Digits(long num)
        {
            var i = 0;

            while (num > 0)
            {
                num /= 10;
                ++i;
            }

            return i;
        }
    }

    public record Solution(string stones);
}
