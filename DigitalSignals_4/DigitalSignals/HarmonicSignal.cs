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
            var zeroes = Enumerable.Repeat(0.0, x.Count);
            var x_padded = x.Concat(zeroes);
            var y_padded = y.Concat(zeroes);

            var x_c = x_padded.Select(x => new Complex(x, 0)).ToList();
            var y_c = y_padded.Select(y => new Complex(y, 0)).ToList();

            var xfft = FFT(x_c.Count, x_c);
            var yfft = FFT(y_c.Count, y_c).Select(c => Complex.Conjugate(c)).ToList();
            return IFFT(xfft.Select((x,i) => xfft[i] * yfft[i]).ToList());
        }

        public static List<Complex> FFT(int N, List<Complex> signal, bool isInvers = false)
        {
            double[] tSin = Enumerable.Range(0, N).Select(value => Math.Sin(2 * Math.PI * value / N)).ToArray();
            var result = new Complex[N];
            Parallel.For(0, N, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, (j) =>
            {
                for (int i = 0; i < N; i++)
                {
                    result[j] += signal[i] * new Complex(tSin[(i * j + N / 4) % N], (isInvers ? -1 : 1) * tSin[i * j % N]) / N;
                }
            });
            return result.ToList();
        }

        public static List<double> IFFT(List<Complex> signal)
        {
            var N = signal.Count;
            return Enumerable.Range(0, N).Select(n =>
                Enumerable.Range(0, N).Select(R =>
                    signal[R].Real * Math.Cos(2 * Math.PI * n * R / N) - signal[R].Imaginary * Math.Sin(2 * Math.PI * n * R / N))
                .Sum())
                .ToList();
        }
   /* return np.array([
        np.sum(
            [x_fft[R].real * np.cos(2 * np.pi * n * R / N) - x_fft[R].imag * np.sin(2 * np.pi * n * R / N) for R in range(int(N))]
        ) 
    for n in range(N)]) / N / A*/

        public static List<double> IFFT(int N, List<Complex> signal)
        {
            return FFT(N, signal, true).Select(c=> c.Real).ToList();
        }
    }
}
