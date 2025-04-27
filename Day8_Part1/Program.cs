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

        // my container for the unique antinodes 
        var antinodes = new HashSet<(int, int)>();

        //  all possible pairs of antennas
        for (int y1 = 0; y1 < rows; y1++) // step over each row for the first antenna
        {
            for (int x1 = 0; x1 < cols; x1++) // step over each column for the first antenna
            {
                // Extract the first antenna
                char a = input[y1][x1];

                // Skip whats not an antena / freq
                if (a == '.') continue;

                for (int y2 = 0; y2 < rows; y2++) // step over each row for the second antenna
                {
                    for (int x2 = 0; x2 < cols; x2++) // step over each column for the second antenna
                    {
                        // Extract the second antenna
                        char b = input[y2][x2];

                        // Skip processing if the second antenna does not match the first or if both antennas are at the same position
                        if (b != a || (x1 == x2 && y1 == y2)) continue;

                        // Calculate the difference in X and Y coordinates between the two antennas
                        int dx = x2 - x1;
                        int dy = y2 - y1;

                        int x3 = x1 - dx; // Position in the opposite direction of the first antenna
                        int y3 = y1 - dy; // Position in the opposite direction of the first antenna
                        int x4 = x2 + dx; // Position in the extended direction from the second antenna
                        int y4 = y2 + dy; // Position in the extended direction from the second antenna

                        // if the positions are within valid boundaries, add them to the antinode set (line 25)
                        if (IsValid(x3, y3, rows, cols)) antinodes.Add((y3, x3));
                        if (IsValid(x4, y4, rows, cols)) antinodes.Add((y4, x4));
                    }
                }
            }
        }

        Console.WriteLine($"Unique antinode locations: {antinodes.Count}");
    }

    // Helper function to check whether the given coordinates are within the boundaries of the grid
    static bool IsValid(int x, int y, int rows, int cols)
    {
        return x >= 0 && x < cols && y >= 0 && y < rows;
    }
}
