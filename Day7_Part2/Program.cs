using System;
using System.Collections.Generic;
using System.IO;
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
        Console.WriteLine($"The total calibration is {Calculate(input, CalculateCalibration)}");
    }

    static long Calculate(string input, Func<long, long, List<long>, bool> checker)
    {
        long total = 0;
        string[] lines = input.Split('\n');

        foreach (string line in lines)
        {
            List<long> parts = new List<long>();
            foreach (Match match in Regex.Matches(line, @"\d+"))
            {
                parts.Add(long.Parse(match.Value));
            }

            if (parts.Count > 1 && checker(parts[0], parts[1], parts.GetRange(2, parts.Count - 2)))
            {
                total += parts[0];
            }
        }
        return total;
    }

    static bool CalculateCalibration(long target, long currentTotal, List<long> nums)
    {
        if (currentTotal > target) return false;
        if (nums.Count == 0) return target == currentTotal;

        List<long> remainingNums = nums.GetRange(1, nums.Count - 1);
        return CalculateCalibration(target, currentTotal + nums[0], remainingNums) ||
               CalculateCalibration(target, currentTotal * nums[0], remainingNums) ||
               CalculateCalibration(target, long.Parse(currentTotal.ToString() + nums[0]), remainingNums);
    }
}
