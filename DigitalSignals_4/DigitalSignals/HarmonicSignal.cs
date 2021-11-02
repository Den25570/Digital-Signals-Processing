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
            public int N { get; set; }
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
            return Enumerable.Range(-maxDelay * 2, maxDelay * 4).Select(delay =>
            {
                double sxy = 0;
                double mx = x.Average();
                double my = y.Average();
                for (int i = 0; i < x.Count; i++)
                {
                    double yVal = i + delay < 0 || i + delay >= x.Count ? 0 : y[i + delay];
                    sxy += (x[i] - mx) * (yVal - my);
                }
                return sxy / Math.Sqrt(x.Sum(xi => (xi - mx) * (xi - mx)) * y.Sum(yi => (yi - my) * (yi - my)));
            }).ToList();
        }

        public static double[] AutoCrossCorrelationFFT(List<double> x, int maxDelay, int N)
        {
            var x_c = new double[x.Count * 2];
            double[] tSin = new double[x_c.Length];
            for (int i = 0; i < x_c.Length; i++) tSin[i] = Math.Sin(2 * Math.PI * i / x_c.Length);
            for (int i = 0; i < x.Count; i++)  x_c[i] = x[i];
            var xfft = FFT(x_c.Length, x_c, tSin);
            var xyfft = new Complex[xfft.Length];
            for (int i = 0; i < xfft.Length; i++) xyfft[i] = xfft[i] * Complex.Conjugate(xfft[i]) / N;
            var result = IFFT(xfft.Length, xyfft, tSin);

            return result;
        }

        public static double[] CrossCorrelationFFT(List<double> x, List<double> y, int maxDelay, int N)
        {
            var x_c = new double[x.Count * 2];
            var y_c = new double[y.Count * 2];
            for (int i = 0; i < x.Count; i++)
            {
                x_c[i] = x[i];
                y_c[i] = y[i];
            }

            double[] tSin = new double[x_c.Length];
            for (int i = 0; i < x_c.Length; i++) tSin[i] = Math.Sin(2 * Math.PI * i / x_c.Length);
            var xfft = FFT(x_c.Length, x_c, tSin);
            var yfft = FFT(x_c.Length, y_c, tSin);
            for (int i = 0; i < yfft.Length; i++) yfft[i] = Complex.Conjugate(yfft[i]);

            var xyfft = new Complex[xfft.Length];
            for (int i = 0; i < xfft.Length; i++) xyfft[i] = xfft[i] * yfft[i] / N;
            var result = IFFT(xyfft.Length, xyfft, tSin);

            return result;
        }

        public static Complex[] FFT(int N, double[] signal, double[] tSin)
        {
            var result = new Complex[N];
            Parallel.For(0, N, (j) =>
            {
                for (int i = 0; i < N; i++)
                {
                    result[j] += signal[i] * new Complex(tSin[(i * j + N / 4) % N], tSin[i * j % N]);
                }
            });
            return result;
        }

        public static double[] IFFT(int N, Complex[] signal, double[] tSin)
        {
            double[] result = new double[signal.Length];
            Parallel.For(0, N, (n) =>
            {
                for (int R = 0; R < signal.Length; R++)
                {
                    result[(n + N / 2) % N] += signal[R].Real * tSin[(n * R + N / 4) % N] - signal[R].Imaginary * tSin[n * R % N];
                }
                result[(n + N / 2) % N] /= N;
            });
            return result;
        }
    }
}
