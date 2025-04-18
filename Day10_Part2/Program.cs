using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day10.txt");
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File {filePath} not found!");
                return;
            }

            string input = File.ReadAllText("day10.txt");

            // Parse the input into a 2D grid
            var grid = ParseGrid(input);


            Program scores = new Program();
            var solution = scores.Solve(grid);
            Console.WriteLine($"the ratings of all trailheads is : {solution.tailhead_rating}");
        }

        public Solution Solve(char[,] grid)
        {
            var sample = 0;
            var visited = new HashSet<int>(128);

            int height = grid.GetLength(0);
            int width = grid.GetLength(1);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var c = grid[y, x];

                    if (c != '0')
                    {
                        continue;
                    }

                    visited.Clear();
                    var a = FollowTrail(grid, 0, x, y, c, visited);
                 
                    sample += a;
                }
            }

            return new Solution(sample.ToString());
        }

        private int FollowTrail(char[,] grid, int currentScore, int x, int y, char current, HashSet<int> visited)
        {
            var tailhead = 0;

            foreach (var (dx, dy) in Coord.Directions)
            {
                int xx = x + dx;
                int yy = y + dy;

                if (xx < 0 || yy < 0 || yy >= grid.GetLength(0) || xx >= grid.GetLength(1))
                {
                    continue; // Boundary check
                }

                var c = grid[yy, xx];

                if (c - current != 1)
                {
                    continue;
                }

                if (c == '9')
                {
                    visited.Add(xx * 10000 + yy); // This ensures unique trailheads
                    tailhead += 1;
                    continue;
                }

                tailhead += FollowTrail(grid, currentScore + 1, xx, yy, c, visited);
            }

            return tailhead;
        }


        private static char[,] ParseGrid(string input)
        {
            var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int height = lines.Length;
            int width = lines[0].Length;

            var grid = new char[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[y, x] = lines[y][x];
                }
            }

            return grid;
        }
    }

    public record Solution(string tailhead_rating);

    public static class Coord
    {
        public static readonly (int dx, int dy)[] Directions = { (0, -1), (0, 1), (-1, 0), (1, 0) };
    }
}
