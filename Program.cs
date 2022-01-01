using System.Linq;

namespace AdventOfCode
{
    public class Program
    {
        public static int Main()
        {

            Console.WriteLine("Advent of Code101: " + AdventOfCode101());
            Console.WriteLine("Advent of Code102: " + AdventOfCode102());
            Console.WriteLine("Advent of Code201: " + AdventOfCode201());
            Console.WriteLine("Advent of Code202: " + AdventOfCode202());
            Console.WriteLine("Advent of Code301: " + AdventOfCode301());
            Console.WriteLine("Advent of Code302: " + AdventOfCode302());

            Console.WriteLine("Advent of Code401: " + Day04Solver.Run(part1:true));
            Console.WriteLine("Advent of Code402: " + Day04Solver.Run(part1:false)); // for part 2

            Console.WriteLine("Advent of Code501: " + Day05Solver.Run()); 
            Console.WriteLine("Advent of Code502: " + Day05Solver.Run(includeDiagonal:true)); 

            return 1;
        }

        public static int AdventOfCode101()
        {
            var data = File.ReadAllLines(@"C:\test projects\csharp\data\day101.txt").Select(x => int.Parse(x)).ToList();

            int max = data.First();

            int countBiggerThanPrevious = 0;
            for (int i = 0; i < data.Count; i++)
            {

                if (i == 0)
                    continue;

                if (data[i] > data[i - 1])
                {
                    countBiggerThanPrevious++;
                }
            }

            return countBiggerThanPrevious;
        }

        public static int AdventOfCode102()
        {
            var data = File.ReadAllLines(@"C:\test projects\csharp\data\day102.txt").Select(x => int.Parse(x)).ToList();
            var sumsOfThrees = new List<int>();
            for (int i = 0; i < data.Count; i++)
            {
                if (i > data.Count - 3)
                    continue;
                var sum = data[i] + data[i + 1] + data[i + 2];
                sumsOfThrees.Add(sum);
            }

            int countBiggerThanPrevious = 0;
            for (int i = 0; i < sumsOfThrees.Count; i++)
            {
                if (i == 0)
                    continue;

                if (sumsOfThrees[i] > sumsOfThrees[i - 1])
                {
                    countBiggerThanPrevious++;
                }
            }

            return countBiggerThanPrevious;
        }

        public static int AdventOfCode201()
        {
            var data = File.ReadAllLines(@"C:\test projects\csharp\data\day201.txt").ToList();
            var depth = 0;
            var horizontal = 0;

            var commands = new string[]{
            "forward ",
            "down ",
            "up "
        };

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                int value = 0;
                var foundCommand = "";
                foreach (var command in commands)
                {
                    if (item.Contains(command))
                    {
                        value = int.Parse(item.Replace(command, ""));
                        foundCommand = command;
                        break;
                    }
                }
                if (foundCommand.Contains("forward"))
                    horizontal += value;
                else if (foundCommand.Contains("down"))
                    depth += value;
                else if (foundCommand.Contains("up"))
                    depth -= value;
            }

            return depth * horizontal;
        }

        public static int AdventOfCode202()
        {
            var data = File.ReadAllLines(@"C:\test projects\csharp\data\day202.txt").ToList();

            var depth = 0;
            var horizontal = 0;
            var aim = 0;

            var commands = new string[]{
            "forward ",
            "down ",
            "up "
        };

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                int value = 0;
                var foundCommand = "";
                foreach (var command in commands)
                {
                    if (item.Contains(command))
                    {
                        value = int.Parse(item.Replace(command, ""));
                        foundCommand = command;
                        break;
                    }
                }
                if (foundCommand.Contains("forward"))
                {
                    horizontal = value + horizontal;
                    depth = depth + (aim * value);
                }
                else if (foundCommand.Contains("down"))
                {
                    aim += value;
                }
                else if (foundCommand.Contains("up"))
                {
                    aim -= value;
                }
            }

            return depth * horizontal;
        }

        public static int AdventOfCode301()
        {
            var data = File.ReadAllLines(@"C:\test projects\csharp\data\day301.txt").ToList();
            var width = data.First().Length;

            Dictionary<int, List<byte>> storage = new Dictionary<int, List<byte>>();
            for(int i = 0; i < width;i++)
            {
                storage.Add(i, new List<byte>());
            }

            foreach(var item in data)
            {
                var array = item.Select(x=>Convert.ToByte(x.ToString())).ToArray();
                int index = 0;
                 while(index < width)
                {
                    storage[index].Add(array[index++]);
                }
            }
            var mostCommon = "";
            var leastCommon = "";
            for(int i = 0; i < width;i++)
            {
                mostCommon = mostCommon + GetMaxByte(storage[i]);
            }
             for(int i = 0; i < width;i++)
            {
                leastCommon = leastCommon + GetMaxByte(storage[i],false);
            }
            
            var numberMostCommon = Convert.ToInt32(mostCommon,2);
            var numberLeastCommon = Convert.ToInt32(leastCommon,2);

            return numberMostCommon * numberLeastCommon;
        }

        public static int AdventOfCode302()
        {
            var data = File.ReadAllLines(@"C:\test projects\csharp\data\day301.txt").ToList();
            var width = data.First().Length;

            var storageColumns = new Dictionary<int, List<byte>>();
            var storageRows = new List<List<byte>>();
            for(int i = 0; i < width;i++)
            {
                storageColumns.Add(i, new List<byte>());
            }

            foreach(var item in data)
            {
                var array = item.Select(x=>Convert.ToByte(x.ToString())).ToArray();
                int indexColumns = 0;
                while(indexColumns < width)
                {
                    storageColumns[indexColumns].Add(array[indexColumns++]);
                }
                storageRows.Add(array.ToList());
            }

            int positionToProcess = 0;
            var workingsetOxygen = storageRows;
            var workingsetCo2 = storageRows;
            do
            {
                
                workingsetOxygen = ReduceArrayBy(workingsetOxygen,positionToProcess,true,isCo2: false);
                workingsetCo2 = ReduceArrayBy(workingsetCo2,positionToProcess,false, isCo2: true);
                
                positionToProcess++;

                if(workingsetOxygen.Count == 1 && workingsetCo2.Count == 1)
                    break;
            
            }while(true);
            
            var oxygenRating = GetIntFromByteArray(workingsetOxygen[0]);
            var co2Rating = GetIntFromByteArray(workingsetCo2[0]);

            return co2Rating * oxygenRating;
        }

        public static int GetIntFromByteArray(List<byte> bytes)
        {
            string value = "";
            foreach(var item in bytes)
            {
                value = value + item.ToString();
            }
            return Convert.ToInt32(value,2);
        }

        public static List<List<byte>> ReduceArrayBy(List<List<byte>> data, int position, bool mostCommon, bool isCo2)
        {
            List<byte> positionCollection = new List<byte>();
            if(data.Count == 1)
                return data;
            foreach(var item in data)
            {
                positionCollection.Add(item[position]);
            }

            var maxByte = Convert.ToByte(GetByteForC02Oxygen(positionCollection,mostCommon,isCo2));

            List<List<byte>> newCollection = new List<List<byte>>();

            foreach(var item in data)
            {
                if(item[position] == maxByte)
                    newCollection.Add(item);
            }

            return newCollection;
        }

        public static byte GetByteForC02Oxygen(List<byte> array, bool mostCommon, bool isCo2)
        {
            var zeroes = array.Where(x=> x == 0).Count();
            var ones = array.Where(x=>x == 1).Count();

            if(zeroes == ones)
            {
                if(isCo2)
                    return 0;
                else
                    return 1;
            }

            return Convert.ToByte(GetMaxByte(array,mostCommon));
        }
        
        public static string GetMaxByte(List<byte> array, bool mostCommon = true)
        {
            var zeroes = array.Where(x=> x == 0).Count();
            var ones = array.Where(x=>x == 1).Count();
            
            if(mostCommon)
                return zeroes > ones ? "0": "1"; 
            else
                return zeroes < ones ? "0": "1"; 

        }
    }
}