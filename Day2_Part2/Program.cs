using System;
using System.IO;

class Program
{
    static void Main()
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day2.txt");

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File {filePath} not found!");
            return;
        }

        string input = File.ReadAllText(filePath);
        string[] reportLines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        int safeCount = 0;

        foreach (string line in reportLines)
        {
            int[] levels = ReportChecker(line);
            if (IsValid(levels) || CanBeMadeSafe(levels))
            {
                safeCount++;
            }
        }

        Console.WriteLine($"Safe Reports: {safeCount}");
    }

    static int[] ReportChecker(string line)
    {
        string[] parts = line.Trim().Split(' ');
        int[] levels = new int[parts.Length];

        for (int i = 0; i < parts.Length; i++)
        {
            levels[i] = int.Parse(parts[i]);
        }

        return levels;
    }

    static bool IsValid(int[] levels)
    {
        if (levels.Length < 2)
        {
            return false; // Too short to evaluate
        }

        bool isIncreasing = levels[1] > levels[0];
        bool isDecreasing = levels[1] < levels[0];

        for (int i = 1; i < levels.Length; i++)
        {
            int difference = levels[i] - levels[i - 1];
            bool? increasingDeceiver = isIncreasing && difference < 0 ? true : false;
            bool? decreasingDeceiver = isDecreasing && difference > 0 ? true : false;

            if (difference < -3 || difference == 0 || difference > 3)
            {
                return false;
            }

            if ((bool)increasingDeceiver || (bool)decreasingDeceiver)
            {
                return false;
            }
        }

        return true;
    }

    static bool CanBeMadeSafe(int[] levels)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            int[] modifiedLevels = RemoveIndex(levels, i);
            if (IsValid(modifiedLevels))
                return true;
        }

        return false;
    }

    static int[] RemoveIndex(int[] array, int index)
    {
        int[] newArray = new int[array.Length - 1];
        for (int i = 0, j = 0; i < array.Length; i++)
        {
            if (i == index) continue;
            newArray[j++] = array[i];
        }
        return newArray;
    }
}
