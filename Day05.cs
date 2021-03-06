using System.Text;
using System.Linq;
namespace AdventOfCode
{
    public class Day05Solver
    {
        public static int Run(bool includeDiagonal=false)
        {
            var data = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, @"data\day501.txt")).ToList();

            var paths = data.Select(x =>
            {
                var path = x.Split("->").Select(x => x.Trim().Split(",").Select(int.Parse).ToArray()).ToArray();
                return (path[0], path[1]);
            }).ToList();

            var maxX = paths.SelectMany(x => new[] { x.Item1[0], x.Item2[0] }).Max();
            var maxY = paths.SelectMany(x => new[] { x.Item1[1], x.Item2[1] }).Max();

            var map = new int[maxX + 1, maxY + 1];

            foreach (var (start, end) in paths)
            {
                var x1 = start[0];
                var x2 = end[0];
                var y1 = start[1];
                var y2 = end[1];

                if (x1 == x2)
                {
                    if (y1 < y2)
                    {
                        for (var y = y1; y <= y2; y++)
                        {
                            map[x1, y]++;
                        }
                    }
                    else
                    {
                        for (var y = y2; y <= y1; y++)
                        {
                            map[x1, y]++;
                        }
                    }
                }
                else if (y1 == y2)
                {
                    if (x1 < x2)
                    {
                        for (var x = x1; x <= x2; x++)
                        {
                            map[x, y1]++;
                        }
                    }
                    else
                    {
                        for (var x = x2; x <= x1; x++)
                        {
                            map[x, y1]++;
                        }
                    }
                }
                else if (includeDiagonal)
                {
                    var length = Math.Abs(x1 - x2);
                    var x = x1;
                    var y = y1;
                    while (length >= 0)
                    {
                        map[x, y]++;
                        if (x2 > x)
                        {
                            x++;
                        }
                        else
                        {
                            x--;
                        }
                        if (y2 > y)
                        {
                            y++;
                        }
                        else
                        {
                            y--;
                        }
                        length--;
                    }
                }

                if (paths.Count < 20)
                {
                    Console.WriteLine("--------------------");
                    for (var x = 0; x <= maxX; x++)
                    {
                        var line = "";
                        for (var y = 0; y <= maxY; y++)
                        {
                            line = map[y, x] == 0 ? $"{line}." : $"{line}{map[y, x]}";
                        }
                        Console.WriteLine(line);
                    }
                    Console.WriteLine("--------------------");
                }
            }

            var overlaps = 0;
            for (var x = 0; x <= maxX; x++)
            {
                for (var y = 0; y <= maxY; y++)
                {
                    if (map[x, y] > 1)
                    {
                        overlaps++;
                    }
                }
            }

            return overlaps;
        }
    }
}