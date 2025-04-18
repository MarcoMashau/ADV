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
        int correctedMiddleSum = 0;
        foreach (var update in updates)
        {
            var pages = update.Split(',').Select(int.Parse).ToList();

            if (!IsValidUpdate(pages, precedenceRules))
            {
                var correctedOrder = CorrectOrder(pages, precedenceRules);
                int middlePage = correctedOrder[correctedOrder.Count / 2];
                correctedMiddleSum += middlePage;
            }
        }

        Console.WriteLine($"The Sum of middle pages in corrected updates is : {correctedMiddleSum}");
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

    static List<int> CorrectOrder(List<int> pages, List<(int Before, int After)> rules)
    {
        var precedenceGraph = new Dictionary<int, List<int>>();
        var inDegrees = new Dictionary<int, int>();

        foreach (var page in pages)
        {
            precedenceGraph[page] = new List<int>();
            inDegrees[page] = 0;
        }

        foreach (var rule in rules)
        {
            if (pages.Contains(rule.Before) && pages.Contains(rule.After))
            {
                precedenceGraph[rule.Before].Add(rule.After);
                inDegrees[rule.After]++;
            }
        }

        // removing nodes with no dependencies
        var queue = new Queue<int>(inDegrees.Where(kv => kv.Value == 0).Select(kv => kv.Key));
        var sortedPages = new List<int>();

        while (queue.Count > 0)
        {
            int current = queue.Dequeue();
            sortedPages.Add(current);

            foreach (var neighbor in precedenceGraph[current])
            {
                inDegrees[neighbor]--;
                if (inDegrees[neighbor] == 0)
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        return sortedPages;
    }
}
