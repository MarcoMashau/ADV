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
        Console.WriteLine("The Total Calibration Result is : " + Calculate(input, CalculateCalibration));
    }

    static long Calculate(string input, Func<long, long, List<long>, bool> checker)
    {
        long sum = 0;

        // Split the input into lines
        string[] lines = input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
           //extract only the numbers 
            MatchCollection matches = Regex.Matches(line, @"\d+");
            List<long> parts = new List<long>();
            foreach (Match match in matches)
            {
                parts.Add(long.Parse(match.Value));
            }

            // Check if there are more than one number and if the checker returns true
            if (parts.Count > 1 && checker(parts[0], parts[1], parts.GetRange(2, parts.Count - 2)))
            {
                sum += parts[0];
            }
        }

        return sum;
    }

    static bool CalculateCalibration(long target, long currentTotal, List<long> nums)
    {
        if (nums.Count == 0) return target == currentTotal;

        // Recursive calls for addition and multiplication
        long firstNum = nums[0];
        List<long> remainingNums = nums.GetRange(1, nums.Count - 1);
        return CalculateCalibration(target, currentTotal + firstNum, remainingNums) ||
               CalculateCalibration(target, currentTotal * firstNum, remainingNums);
    }
}
