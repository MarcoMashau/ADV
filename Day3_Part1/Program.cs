using System;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day3.txt");

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"The file '{filePath}' does not exist. Please check the file path.");
            return;
        }

        string corruptedMemory = File.ReadAllText(filePath);

        int sum = 0;

        // Regex was an easier option to get matching cases. Loops would take forever. no pun-intended
        string pattern = @"mul\((\d+),(\d+)\)";
        Regex regex = new Regex(pattern);

        MatchCollection matches = regex.Matches(corruptedMemory);

        foreach (Match match in matches)
        {
            // Extract numbers X and Y
            int x = int.Parse(match.Groups[1].Value);
            int y = int.Parse(match.Groups[2].Value);

            // Multiply and add to the sum
            sum += x * y;
        }

        Console.WriteLine($"The sum of valid multiplications is: {sum}");
    }
}
