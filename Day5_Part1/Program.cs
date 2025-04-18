using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {

        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day5.txt");
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File {filePath} not found!");
            return;
        }

        string[] input = File.ReadAllLines(filePath);

        // Split rules and updates
        int separatorIndex = Array.FindIndex(input, line => string.IsNullOrWhiteSpace(line));
        var rules = input.Take(separatorIndex).ToList();
        var updates = input.Skip(separatorIndex + 1).ToList();

        // Add rules into a dictionary
        var precedenceRules = new List<(int Before, int After)>();
        foreach (var rule in rules)
        {
            var parts = rule.Split('|').Select(int.Parse).ToArray();
            precedenceRules.Add((Before: parts[0], After: parts[1]));
        }

        // Process each update
        int middleSum = 0;
        foreach (var update in updates)
        {
            var pages = update.Split(',').Select(int.Parse).ToList();

            if (IsValidUpdate(pages, precedenceRules))
            {
                int middlePage = pages[pages.Count / 2];
                middleSum += middlePage;
            }
        }

        Console.WriteLine($"The Sum of middle pages in valid updatesis : {middleSum}");
    }

    static bool IsValidUpdate(List<int> pages, List<(int Before, int After)> rules)
    {
        var pageIndices = pages.Select((page, index) => new { page, index })
                               .ToDictionary(x => x.page, x => x.index);

        foreach (var rule in rules)
        {
            if (pageIndices.ContainsKey(rule.Before) && pageIndices.ContainsKey(rule.After))
            {
                if (pageIndices[rule.Before] >= pageIndices[rule.After])
                {
                    return false;
                }
            }
        }
        return true;
    }
}
