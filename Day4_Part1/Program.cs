using System;
using System.IO;

class Program
{
    static void Main()
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day4.txt");
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File {filePath} not found!");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        int xmasCount = 0;

        // word search dimensions 
        int rows = lines.Length;
        int cols = lines[0].Length;

        // the directions on how to search 
        int[][] directions = new int[][]
        {
            new int[] {0, 1},   // Horizontal right
            new int[] {1, 0},   // Vertical down
            new int[] {1, 1},   // Diagonal down-right
            new int[] {1, -1},  // Diagonal down-left
            new int[] {0, -1},  // Horizontal left
            new int[] {-1, 0},  // Vertical up
            new int[] {-1, -1}, // Diagonal up-left
            new int[] {-1, 1}   // Diagonal up-right
        };

        string target = "XMAS";

        // Iterate over each character in the grid
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                // Check in all 8 directions
                foreach (var dir in directions)
                {
                    int r = row, c = col;
                    int match = 0;

                    // an attempt to match the word
                    for (int k = 0; k < target.Length; k++)
                    {
                        if (r >= 0 && r < rows && c >= 0 && c < cols && lines[r][c] == target[k])
                        {
                            match++;
                            r += dir[0];
                            c += dir[1];
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (match == target.Length)
                    {
                        xmasCount++;
                    }
                }
            }
        }
        Console.WriteLine($"The word 'XMAS' appears {xmasCount} times.");
    }
}
