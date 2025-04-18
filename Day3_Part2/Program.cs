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
        var matches = Regex.Matches(input, rx, RegexOptions.Multiline);

        return matches.Aggregate(
            (enabled: true, res: 0L),
            (acc, m) =>
                (m.Value, acc.res, acc.enabled) switch
                {
                    ("don't()", _, _) => (false, acc.res),
                    ("do()", _, _) => (true, acc.res),
                    (_, var res, true) => (true, res + int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value)),
                    _ => acc
                },
            acc => acc.res
        );
    }
}
