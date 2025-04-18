using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day6.txt");
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File {filePath} not found!");
            return;
        }

        string[] map = File.ReadAllLines(filePath);
        int rows = map.Length;
        int columns = map[0].Length;

        // Directions: Up, Right, Down, Left
        int[,] directions = { { -1, 0 }, { 0, 1 }, { 1, 0 }, { 0, -1 } };
        int directionIndex = 0;

        // Finding the guard's initial position
        int guardRow = 0, guardCol = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (map[i][j] == '^' || map[i][j] == '>' || map[i][j] == 'v' || map[i][j] == '<')
                {
                    guardRow = i;
                    guardCol = j;

                    // Set initial direction based on symbol
                    directionIndex = map[i][j] switch
                    {
                        '^' => 0,
                        '>' => 1,
                        'v' => 2,
                        '<' => 3,
                        _ => directionIndex
                    };

                    break;
                }
            }
        }

        HashSet<(int, int)> visitedPositions = new HashSet<(int, int)>();
        visitedPositions.Add((guardRow, guardCol));

        while (true)
        {
            int nextRow = guardRow + directions[directionIndex, 0];
            int nextCol = guardCol + directions[directionIndex, 1];

            // Check if the guard is leaving the map
            if (nextRow < 0 || nextRow >= rows || nextCol < 0 || nextCol >= columns)
                break;

            // If there's an obstacle, turn right
            if (map[nextRow][nextCol] == '#')
            {
                directionIndex = (directionIndex + 1) % 4;
            }
            else // Otherwise, move forward
            {
                guardRow = nextRow;
                guardCol = nextCol;
                visitedPositions.Add((guardRow, guardCol));
            }
        }

        Console.WriteLine($"Distinct positions visited is: {visitedPositions.Count}");
    }
}
