using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignals
{
    public static class HarmonicSignal
    {
        public struct HarmonicSignalParams
        {
            public float B1 { get; set; }
            public float B2 { get; set; }
            public float N { get; set; }
        }

        private static Dictionary<int, double> parabolaDegree4DividorMap = new Dictionary<int, double>() {
            {5, 231 },
            {9, 429 },
            {11, 429 },
            {13, 2431 },
        };
        private static Dictionary<int, float> parabolaDegree4Points11= new Dictionary<int, float>() {
            {5, 18 },
            {4, -45 },
            {3, -10 },
            {2, 60 },
            {1, 120 },
            {0, 143 }
        };

        public static List<double> GenerateHarmonicSignal(HarmonicSignalParams signalParams, int size)
        {
            Random rand = new Random();
            return Enumerable.Range(0, size)
                .Select(i => signalParams.B1 * MathF.Sin(2 * MathF.PI * i / signalParams.N) +
                    Enumerable.Range(50, 20)
                    .Select(j => signalParams.B2 * Math.Pow(-1, rand.Next(0, 2)) * MathF.Sin(2 * MathF.PI * i * j / signalParams.N))
                    .Sum())
                .ToList();
        }

        public static List<double> MedianFilter(List<double> signal, int size)
        {
            return signal.Select((x, i) => signal.GetRange(Math.Max(i - size / 2, 0), Math.Min(signal.Count - i, size / 2 + 1) + Math.Min(i - size / 2, 0) + size / 2).Average())
                .ToList();
        }

        public static List<double> MedianAverage(List<double> signal, int size)
        {
            return signal.Select((x, i) =>
            {
                var range = signal.GetRange(Math.Max(i - size / 2, 0), Math.Min(signal.Count - i, size / 2 + 1) + Math.Min(i - size / 2, 0) + size / 2);
                range.Sort();
                return range.GetRange(range.Count / 4, range.Count - 2 * (range.Count / 4)).Average();
            }).ToList();
        }

        public static List<double> ParabolaDegree4(List<double> signal)
        {
            return signal.Select((x, i) =>
            {
                double filteredValue = 0;
                for (int j = -5; j <= 5; j++)
                {
                    if (i + j < 0 || i + j >= signal.Count) continue;
                    filteredValue += parabolaDegree4Points11[Math.Abs(j)] * signal[i + j];
                }
                return filteredValue / parabolaDegree4DividorMap[11];
            }).ToList();
        }

        public static List<Complex> FourierTransform(int N, List<Complex> signal, bool isInvers = false)
        {
            double[] tSin = Enumerable.Range(0, N).Select(value => Math.Sin(2 * Math.PI * value / N)).ToArray();
            var result = new Complex[N];
            Parallel.For(0, N, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, (j) =>
            {
                for (int i = 0; i < N; i++)
                {
                    result[j] += signal[i] * new Complex(tSin[(i * j + N / 4) % N], (isInvers ? -1 : 1) * tSin[i * j % N]);
                }
            });
            return result.ToList();
        }
    }
}
