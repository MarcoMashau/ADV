using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day3.txt");

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"The file {filePath} was not found, please check your DIR.");
            return;
        }

        string input = File.ReadAllText(filePath);
        Console.WriteLine($"The enabled multiplications sum is : {MemoryCheck(input)}");
    }

    public static object MemoryCheck(string input) => Solve(input, @"mul\((\d{1,3}),(\d{1,3})\)|don't\(\)|do\(\)");

    static long Solve(string input, string rx)
    {
        var matches = Regex.Matches(input, rx, RegexOptions.Multiline); // Find pieces of text matching the pattern.

        return matches.Aggregate( // Combine results step by step.
            (enabled: true, res: 0L), // Start with calculations enabled and result at 0.
            (acc, m) => { // For each match, update the result based on the match value.
                var currentMatch = m.Value;
                if (currentMatch == "don't()")
                    return (false, acc.res); // Stop calculating.
                else if (currentMatch == "do()")
                    return (true, acc.res); // Resume calculating.
                else if (acc.enabled) // Only calculate when enabled.
                {
                    var factor1 = int.Parse(m.Groups[1].Value); // Get the first number from the match.
                    var factor2 = int.Parse(m.Groups[2].Value); // Get the second number from the match.
                    return (true, acc.res + factor1 * factor2); // Add their product to the result.
                }
                return acc; 
            },
            acc => acc.res // After processing all matches, return the final result.
        );
    }
}
