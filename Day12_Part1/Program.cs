using System;
using System.IO;
using System.Collections.Generic;

namespace AdventOfCode;

public class Program
{
    public Solution Solve(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var height = lines.Length;
        var width = lines[0].Length;

        var grid = new char[height, width];
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                grid[y, x] = lines[y][x];
            }
        }

        var price = 0;
        var visited = new bool[width * height];
        var queue = new Queue<(int X, int Y)>(128);
        var fences = new HashSet<(int X, int Y, int Direction)>(256);

        int Walk(char[,] grid, int x2, int y2, int direction, char plant)
        {
            if (x2 < 0 || y2 < 0 || x2 >= width || y2 >= height || grid[y2, x2] != plant)
            {
                fences.Add((x2, y2, direction));
                return 1;
            }

            if (!visited[y2 * width + x2])
            {
                queue.Enqueue((x2, y2));
            }

            return 0;
        }

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (visited[y * width + x])
                {
                    continue;
                }

                var plant = grid[y, x];
                var area = 0;
                var perimeter = 0;
                var sides = 0;

                fences.Clear();
                queue.Enqueue((x, y));

                while (queue.TryDequeue(out var n))
                {
                    var hash = n.Y * width + n.X;

                    if (visited[hash])
                    {
                        continue;
                    }

                    visited[hash] = true;

                    area++;
                    perimeter += Walk(grid, n.X + 1, n.Y, 1, plant);
                    perimeter += Walk(grid, n.X - 1, n.Y, 2, plant);
                    perimeter += Walk(grid, n.X, n.Y + 1, 3, plant);
                    perimeter += Walk(grid, n.X, n.Y - 1, 4, plant);
                }

                foreach (var cur in fences)
                {
                    sides++;

                    var (x2, y2, dir) = cur;
                    fences.Remove(cur);

                    var d = 0;
                    while (fences.Remove((x2 + ++d, y2, dir))) { }

                    d = 0;
                    while (fences.Remove((x2 - ++d, y2, dir))) { }

                    d = 0;
                    while (fences.Remove((x2, y2 + ++d, dir))) { }

                    d = 0;
                    while (fences.Remove((x2, y2 - ++d, dir))) { }
                }

                price += area * perimeter;
            }
        }

        return new(price.ToString());
    }

    public static void Main()
    {
        // Read input from the file
        string input = File.ReadAllText("day12.txt");

        // Create an instance of Day12 and solve the problem
        Program day12 = new Program();
        Solution solution = day12.Solve(input);
        Console.WriteLine($"the total price of fencing all regions in the map is R {solution.price}.00");    
    }
}

public record Solution(string price);
