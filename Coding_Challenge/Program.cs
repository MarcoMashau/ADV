using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {

        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "day1.txt");
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File {filePath} not found!");
            return;
        }
        var rows = File.ReadAllLines(filePath);

        //loading the data into the two lists 
        var leftList = new List<int>();
        var rightList = new List<int>();

        foreach (var line in rows)
        {
            var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                leftList.Add(int.Parse(parts[0]));
                rightList.Add(int.Parse(parts[1]));
            }
        }

        // Sort both lists
        leftList.Sort();
        rightList.Sort();

        // actual distance calcualtion
        int totalDistance = 0;
        for (int i = 0; i < leftList.Count; i++)
        {
            totalDistance += Math.Abs(leftList[i] - rightList[i]);
        }

        Console.WriteLine($"The Total Distance is : {totalDistance} ");
    }
}
