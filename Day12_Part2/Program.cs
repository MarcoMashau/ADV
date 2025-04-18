using System;
using System.Collections.Generic;
using System.IO;

public class Program
{
    private readonly char[,] grid;

    public int Height { get; }
    public int Width { get; }

    public Program(string input)
    {
        var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        Height = lines.Length;
        Width = lines[0].Length;
        grid = new char[Height, Width];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                grid[y, x] = lines[y][x];
            }
        }
    }

    public char this[int y, int x]
    {
        get => grid[y, x];
    }
}

public class Day12Solution
{
    public static void Main(string[] args)
    {
        // Reading input from the file
        string input = File.ReadAllText("day12.txt");

        var price = 0;
        var grid = new Program(input);
        var visited = new bool[grid.Width * grid.Height];
        var queue = new Queue<(int X, int Y)>(128);
        var fences = new HashSet<(int X, int Y, int Direction)>(256);

        int Walk(Program grid, int x2, int y2, int direction, char plant)
        {
            if (x2 < 0 || x2 >= grid.Width || y2 < 0 || y2 >= grid.Height || grid[y2, x2] != plant)
            {
                fences.Add((x2, y2, direction));
                return 1;
            }

            if (!visited[y2 * grid.Width + x2])
            {
                queue.Enqueue((x2, y2));
            }

            return 0;
        }

        for (var y = 0; y < grid.Height; y++)
        {
            for (var x = 0; x < grid.Width; x++)
            {
                if (visited[y * grid.Width + x])
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
                    var hash = n.Y * grid.Width + n.X;

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

        
                price += area * sides;
            }
        }

        Console.WriteLine($" The new total price of fencing all regions on the map is  R {price}.00");
    }
}
