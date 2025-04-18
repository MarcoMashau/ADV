﻿using System;
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
        var antinodes = new HashSet<(int, int)>(); //store in unique positions

        // Iterate through the grid to find antennas
        for (int y1 = 0; y1 < rows; y1++)
        {
            for (int x1 = 0; x1 < cols; x1++)
            {
                char freq = input[y1][x1];
                if (freq == '.') continue;

                // Check cells in the grid for the same frequency
                for (int y2 = 0; y2 < rows; y2++)
                {
                    for (int x2 = 0; x2 < cols; x2++)
                    {
                        if ((x1 == x2 && y1 == y2) || input[y2][x2] != freq) continue;
                        MarkAntinodes(x1, y1, x2, y2, antinodes, rows, cols);
                    }
                }

                // Add the antenna itself if it's in line with another of the same frequency
                antinodes.Add((y1, x1));
            }
        }

        Console.WriteLine($"Unique antinode locations: {antinodes.Count}");
    }

    static void MarkAntinodes(int x1, int y1, int x2, int y2, HashSet<(int, int)> antinodes, int rows, int cols)
    {
        int dx = x2 - x1;
        int dy = y2 - y1;

        // Normalize direction
        int gcd = GetGreatestCommonDivisor(Math.Abs(dx), Math.Abs(dy));
        dx /= gcd;
        dy /= gcd;

        // Traverse in both directions to mark antinodes
        int x = x1, y = y1;
        while (IsValid(x, y, rows, cols))
        {
            antinodes.Add((y, x));
            x -= dx;
            y -= dy;
        }

        x = x2; y = y2;
        while (IsValid(x, y, rows, cols))
        {
            antinodes.Add((y, x));
            x += dx;
            y += dy;
        }
    }

    static bool IsValid(int x, int y, int rows, int cols)
    {
        return x >= 0 && x < cols && y >= 0 && y < rows;
    }

    static int GetGreatestCommonDivisor(int a, int b)
    {
        return b == 0 ? a : GetGreatestCommonDivisor(b, a % b);
    }
}
