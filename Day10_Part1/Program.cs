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
            var grid = ParseGrid(input);


            Program scores = new Program();

            var solution = scores.Solve(grid);
            Console.WriteLine($"The sum of the scores of all trailheads on the topographic map is: {solution.tailheads_sum}");
        }

        public Solution Solve(char[,] grid)
        {
            var sample = 0; 
            var visited = new HashSet<int>(128); // initialized to avoid frequent memory reallocation.

            int height = grid.GetLength(0);
            int width = grid.GetLength(1);

            // Iterates through each cell in the grid.
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var c = grid[y, x]; // Current cell value.
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

            //move in all possible directions in my grid
            foreach (var (dx, dy) in Coord.Directions)
            {
              
                int xx = x + dx;
                int yy = y + dy;

                // Check boundaries to ensure the new position is within the grid.
                if (xx < 0 || yy < 0 || yy >= grid.GetLength(0) || xx >= grid.GetLength(1))
                {
                    continue;
                }

                var c = grid[yy, xx];

                // Checks if the new cell's value is not sequentially increasing.
                if (c - current != 1)
                {
                    continue;
                }

                // If the new cell is the endpoint ('9'), it marks the trailhead.
                if (c == '9')
                {
                    tailhead += visited.Add(xx * 10000 + yy) ? 1 : 0; // Ensures uniqueness of the trailhead.
                    continue;
                }

                tailhead += FollowTrail(grid, currentScore + 1, xx, yy, c, visited);
            }
            return tailhead;
        }

        private static char[,] ParseGrid(string input)
        {
            // Splits the input into lines while removing empty lines.
            var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            int height = lines.Length;
            int width = lines[0].Length;

            // populate the grid with input data
            var grid = new char[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[y, x] = lines[y][x];
                }
            }
            return grid; // my newly constructed grid 
        }
    }
    public record Solution(string tailheads_sum);

    public static class Coord
    {
        // all possible directions to move in the grid 
        public static readonly (int dx, int dy)[] Directions = { (0, -1), (0, 1), (-1, 0), (1, 0) };
    }
}
