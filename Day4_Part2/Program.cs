using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Map = System.Collections.Immutable.ImmutableDictionary<(int x, int y), char>;

class Program
{
    (int x, int y) Up = (0, -1);
    (int x, int y) Down = (0, 1);
    (int x, int y) Left = (-1, 0);
    (int x, int y) Right = (1, 0);

    public object GetCount(string input)
    {
        var mat = GetMap(input);
        return (
            from pt in mat.Keys
            where
                Matches(mat, Add(pt, Add(Up, Left)), Add(Down, Right), "MAS") &&
                Matches(mat, Add(pt, Add(Down, Left)), Add(Up, Right), "MAS")
            select 1
        ).Count();
    }

    bool Matches(Map map, (int x, int y) pt, (int x, int y) dir, string pattern)
    {
        var chars = Enumerable.Range(0, pattern.Length)
            .Select(i => map.GetValueOrDefault(Add(pt, Multiply(dir, i))))
            .ToArray();
        return
            Enumerable.SequenceEqual(chars, pattern) ||
            Enumerable.SequenceEqual(chars, pattern.Reverse());
    }

    Map GetMap(string input)
    {
        var map = input.Split("\n");
        return (
            from y in Enumerable.Range(0, map.Length)
            from x in Enumerable.Range(0, map[y].Length)
            select new KeyValuePair<(int x, int y), char>((x, y), map[y][x])
        ).ToImmutableDictionary();
    }

    (int x, int y) Add((int x, int y) a, (int x, int y) b)
    {
        return (a.x + b.x, a.y + b.y);
    }

    (int x, int y) Multiply((int x, int y) a, int scalar)
    {
        return (a.x * scalar, a.y * scalar);
    }

    public static void Main(string[] args)
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day4.txt");
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File {filePath} not found!");
            return;
        }

        string input = File.ReadAllText("day4.txt");
        Program solution = new Program();
        var results = solution.GetCount(input);
        Console.WriteLine($"XMAS appears : {results} times");
    }
}
