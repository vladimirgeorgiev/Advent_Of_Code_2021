using System.Text;
using System.Linq;
namespace AdventOfCode
{
    public class Day04Solver
    {
        public static List<int> Sanitize(string line)
        {
            return line.Split(new char[] { ' ' }).Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToInt32(x.Trim())).ToList();
        }
        public static int Run(bool part1 = true)
        {
            var data = File.ReadAllLines(@"C:\test projects\csharp\data\day401.txt").ToList();

            var drawList = data.First().Split(new char[] { ',' }).Select(x => Convert.ToInt32(x)).ToList();
            var matrices = new List<Matrix>();
            var freshData = data.Skip(1).ToList();
            var bucket = new List<string>();

            foreach (var item in freshData)
            {
                if (!string.IsNullOrEmpty(item))
                    bucket.Add(item);
                else
                {
                    if (bucket.Count > 0)
                        matrices.Add(new Matrix(bucket, Sanitize));
                    bucket = new List<string>();
                }
            }

            int sum = 0, winningNumber = 0;

            var stack = new List<int>();
            for (int i = 0; i < matrices.Count; i++)
                stack.Add(i);

            foreach (var number in drawList)
            {
                for (int i = 0; i < matrices.Count; i++)
                {
                    if (!matrices[i].PlayBingo(number))
                        continue;
                    sum = matrices[i].GetUnmarkedNumbersSum();
                    winningNumber = number;
                    stack.Remove(i);
                    if (part1 == true)
                        goto _result; // yes, yes, yes, YES!!!! Finally I am able to use it!!!
                    if (stack.Count == 0 && !part1)
                        goto _result;
                }
            }
        _result:
            return winningNumber * sum;
        }
        public static IEnumerable<T[]> GetChunk<T>(List<T> input, int size)
        {
            int i = 0;
            while (input.Count > size * i)
            {
                yield return input.Skip(size * i).Take(size).ToArray();
                i++;
            }
        }
    }
    public class Matrix
    {
        private int[,] _data = new int[0, 0];
        private bool[,] _markedNumbers = new bool[0, 0];
        private int _heightOfMatrix, _widthOfMatrix;
        public Matrix(List<string> lines, Func<string, List<int>> sanitizer)
        {
            _heightOfMatrix = lines.Count();
            _widthOfMatrix = sanitizer(lines.First()).Count();

            _data = new int[_heightOfMatrix, _widthOfMatrix];
            _markedNumbers = new bool[_heightOfMatrix, _widthOfMatrix];

            for (int i = 0; i < _heightOfMatrix; i++)
            {
                var lineData = sanitizer(lines[i]);
                for (int k = 0; k < _widthOfMatrix; k++)
                {
                    _data[i, k] = lineData[k];
                    _markedNumbers[i, k] = false;
                }
            }
        }
        public bool PlayBingo(int number)
        {
            for (int i = 0; i < _heightOfMatrix; i++)
                for (int k = 0; k < _widthOfMatrix; k++)
                    if (number == _data[i, k])
                    {
                        _markedNumbers[i, k] = true;
                        if (IsItBingo())
                            return true;
                    }
            return false;
        }
        public bool IsItBingo()
        {
            for (int i = 0; i < _heightOfMatrix; i++)
            {
                int countOfMatchedByHeight = 0, countOfMatchedByWidth = 0;
                for (int k = 0; k < _widthOfMatrix; k++)
                {
                    if (_markedNumbers[i, k] == true)
                        if (++countOfMatchedByHeight == _widthOfMatrix) // Flexing much ?
                            return true;
                    if (_markedNumbers[k, i] == true)
                        if (++countOfMatchedByWidth == _widthOfMatrix)
                            return true;
                }
            }
            return false;
        }
        public int GetUnmarkedNumbersSum()
        {
            int sum = 0;
            for (int i = 0; i < _heightOfMatrix; i++)
                for (int k = 0; k < _widthOfMatrix; k++)
                    if (_markedNumbers[i, k] != true)
                        sum += _data[i, k];
            return sum;
        }
    }
}