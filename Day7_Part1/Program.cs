using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day7.txt");
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File {filePath} not found!");
            return;
        }
        string input = File.ReadAllText(filePath);
        Console.WriteLine("The Total Caliration Result is : " + Calculate(input, CalculateCalibration));

    }

    static long Calculate(string input, Func<long, long, List<long>, bool> checker)
    {
        return input.Split('\n')
            .Select(line => Regex.Matches(line, @"\d+").Select(m => long.Parse(m.Value)).ToList())
            .Where(parts => parts.Count > 1 && checker(parts[0], parts[1], parts.Skip(2).ToList()))
            .Sum(parts => parts[0]);
    }

    static bool CalculateCalibration(long target, long acc, List<long> nums)
    {
        if (!nums.Any()) return target == acc;
        return CalculateCalibration(target, acc + nums[0], nums.Skip(1).ToList()) ||
               CalculateCalibration(target, acc * nums[0], nums.Skip(1).ToList());
    }
}
   