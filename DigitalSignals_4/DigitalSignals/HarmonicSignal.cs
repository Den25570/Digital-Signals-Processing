using System;
using System.Collections;
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

        public static List<double> GenerateHarmonicSignal(HarmonicSignalParams signalParams, int size)
        {
            Random rand = new Random();
            return Enumerable.Range(0, size)
                .Select(i => signalParams.B1 * Math.Sin(2 * MathF.PI * i / signalParams.N))
                .ToList();
        }

        public static List<double> GenerateHarmonicSignalWithNoise(HarmonicSignalParams signalParams, int size)
        {
            Random rand = new Random();
            return Enumerable.Range(0, size)
                .Select(i => signalParams.B1 * Math.Sin(2 * MathF.PI * i / signalParams.N) +
                    Enumerable.Range(50, 20)
                    .Select(j => signalParams.B2 * Math.Pow(-1, rand.Next(0, 2)) * MathF.Sin(2 * MathF.PI * i * j / signalParams.N))
                    .Sum())
                .ToList();
        }

        public static List<double> Noise(HarmonicSignalParams signalParams, int size)
        {
            Random rand = new Random();
            return Enumerable.Range(0, size)
                .Select(i => Enumerable.Range(50, 20)
                    .Select(j => signalParams.B2 * Math.Pow(-1, rand.Next(0, 2)) * MathF.Sin(2 * MathF.PI * i * j / signalParams.N))
                    .Sum())
                .ToList();
        }

        public static List<double> CrossCorrelation(List<double> x, List<double> y, int maxDelay)
        {
            double mx = x.Average();
            double my = y.Average();
            double sx = x.Sum(xi => (xi - mx) * (xi - mx));
            double sy = y.Sum(yi => (yi - my) * (yi - my));
            double denom = Math.Sqrt(sx * sy);

            return Enumerable.Range(-maxDelay, maxDelay * 2).Select(delay =>
            {
                double sxy = 0;
                for (int i = 0; i < x.Count; i++)
                {
                    double yVal = i + delay < 0 || i + delay >= x.Count ? 0 : y[i + delay];
                    sxy += (x[i] - mx) * (yVal - my);
                }
                return sxy / denom;
            }).ToList();
        }

        public static List<double> CrossCorrelationFFT(List<double> x, List<double> y, int maxDelay)
        {
            var x_c = x.Select(x => new Complex(x, 0)).ToList();
            var y_c = x.Select(x => new Complex(x, 0)).ToList();
            var xfft = FFT(x_c.Count, x_c);
            var yfft = FFT(y_c.Count, y_c).Select(c => Complex.Conjugate(c));
            return IFFT(x_c.Count, xfft.Select((x,i) => x * y[i] / x_c.Count).ToList());
        }

        public static List<Complex> FFT(int N, List<Complex> signal, bool isInvers = false)
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

        public static List<double> IFFT(int N, List<Complex> signal)
        {
            return FFT(N, signal, true).Select(c=> c.Real).ToList();
        }
    }
}
