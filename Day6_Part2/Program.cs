using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Numerics;

class Logic
{
    Complex Up = Complex.ImaginaryOne;
    Complex TurnRight = -Complex.ImaginaryOne;

    public object CalculatePosition(string input)
    {
        var (map, start) = Parse(input);
        return Walk(map, start).positions
            .AsParallel()
            .Count(pos => Walk(map.SetItem(pos, '#'), start).isLoop);
    }

    (IEnumerable<Complex> positions, bool isLoop) Walk(ImmutableDictionary<Complex, char> map, Complex pos)
    {
        var visited = new HashSet<(Complex pos, Complex dir)>();
        var dir = Up;
        while (map.ContainsKey(pos) && !visited.Contains((pos, dir)))
        {
            visited.Add((pos, dir));
            if (map.GetValueOrDefault(pos + dir) == '#')
            {
                dir *= TurnRight;
            }
            else
            {
                pos += dir;
            }
        }
        return (
            positions: visited.Select(s => s.pos).Distinct(),
            isLoop: visited.Contains((pos, dir))
        );
    }

    (ImmutableDictionary<Complex, char> map, Complex start) Parse(string input)
    {
        var lines = input.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        // ensuring that  all lines are of equal length
        int rowLength = lines.FirstOrDefault()?.Length ?? 0;

        var map = (
            from y in Enumerable.Range(0, lines.Length)
            from x in Enumerable.Range(0, rowLength)
            where x < lines[y].Length // Bounds check
            select new KeyValuePair<Complex, char>(-Up * y + x, lines[y][x])
        ).ToImmutableDictionary();

        var start = map.First(x => x.Value == '^').Key;

        return (map, start);
    }



    class Program
    {
        static void Main()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day6.txt");
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: File {filePath} not found!");
                return;
            }
            string input = File.ReadAllText("day6.txt");

            Logic solution = new Logic();
            Console.WriteLine($"You can choose from {solution.CalculatePosition(input)} different positions");

        }
    }


}
