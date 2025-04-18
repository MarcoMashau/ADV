using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Program
{
    static void Main()
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day1.txt");
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File {filePath} not found!");
            return;
        }
        var (leftList, rightList) = ReadListsFromFile(filePath);

        int similarityScore = CalculateSimilarityScore(leftList, rightList);
        Console.WriteLine($"The similarity score between the lists is {similarityScore}");
    }

    static (List<int>, List<int>) ReadListsFromFile(string fileName)
    {
        List<int> leftList = new List<int>();
        List<int> rightList = new List<int>();

        foreach (var line in File.ReadLines(fileName))
        {
            var parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                leftList.Add(int.Parse(parts[0]));
                rightList.Add(int.Parse(parts[1]));
            }
        }

        return (leftList, rightList);
    }

    static int CalculateSimilarityScore(List<int> leftList, List<int> rightList)
    {
        // Create a dictionary to count occurrences in the right list
        Dictionary<int, int> rightCount = new Dictionary<int, int>();
        foreach (int num in rightList)
        {
            if (rightCount.ContainsKey(num))
            {
                rightCount[num]++;
            }
            else
            {
                rightCount[num] = 1;
            }
        }

        // Calculate score 
        int similarityScore = 0;
        foreach (int num in leftList)
        {
            if (rightCount.ContainsKey(num))
            {
                similarityScore += num * rightCount[num];
            }
        }
        return similarityScore;
    }
}
