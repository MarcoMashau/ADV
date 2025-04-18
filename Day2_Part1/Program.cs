using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class RedNosedReactor
{
    static void Main(string[] args)
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day2.txt");

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File {filePath} not found!");
            return;
        }

        string input = File.ReadAllText(filePath);
        int safeRep = ReportChecker(input);
        Console.WriteLine($"The No. Of Safe reports is : {safeRep}");

    }

    public static int ReportChecker(string input) =>
        ParseSamples(input).Count(Valid);

    static IEnumerable<int[]> ParseSamples(string input) =>
        from line in input.Split("\n", StringSplitOptions.RemoveEmptyEntries)
        let samples = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
        select samples.ToArray();

    static IEnumerable<int[]> Attenuate(int[] samples) =>
        from i in Enumerable.Range(0, samples.Length)
        let before = samples.Take(i)
        let after = samples.Skip(i + 1)
        select before.Concat(after).ToArray();

    static bool Valid(int[] samples)
    {
        var pairs = samples.Zip(samples.Skip(1), (first, second) => (First: first, Second: second));
        return
            pairs.All(p => 1 <= p.Second - p.First && p.Second - p.First <= 3) || //for the increasing set 
            pairs.All(p => 1 <= p.First - p.Second && p.First - p.Second <= 3);   // for the decreasing set 
    }
}
