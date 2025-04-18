using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day8.txt");
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File {filePath} not found!");
            return;
        }

        string[] input;
        input = File.ReadAllLines("day8.txt");

        int rows = input.Length;
        int cols = input[0].Length;
        var antinodes = new HashSet<(int, int)>(); // store in unique positions

        // Iterate through all pairs of antennas
        for (int y1 = 0; y1 < rows; y1++)
        {
            for (int x1 = 0; x1 < cols; x1++)
            {
                char a = input[y1][x1];
                if (a == '.') continue;

                for (int y2 = 0; y2 < rows; y2++)
                {
                    for (int x2 = 0; x2 < cols; x2++)
                    {
                        char b = input[y2][x2];
                        if (b != a || (x1 == x2 && y1 == y2)) continue;

                        int dx = x2 - x1;
                        int dy = y2 - y1;

                        // condition: one antenna twice the distance from the other
                        int x3 = x1 - dx;
                        int y3 = y1 - dy;
                        int x4 = x2 + dx;
                        int y4 = y2 + dy;

                        if (IsValid(x3, y3, rows, cols)) antinodes.Add((y3, x3));
                        if (IsValid(x4, y4, rows, cols)) antinodes.Add((y4, x4));
                    }
                }
            }
        }

        Console.WriteLine($"Unique antinode locations: {antinodes.Count}");
    }

    static bool IsValid(int x, int y, int rows, int cols)
    {
        return x >= 0 && x < cols && y >= 0 && y < rows;
    }
}
