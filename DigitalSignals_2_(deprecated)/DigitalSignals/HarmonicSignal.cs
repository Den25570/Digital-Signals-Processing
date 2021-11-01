using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignals
{
    public static class HarmonicSignal
    {
        public static float GetHarmonicSignal(float n, float N)
        {
            return MathF.Sin(2 * MathF.PI  * n / N);
        }
        public static float GetHarmonicSignalPhase(float n, float N, float phase)
        {
            return MathF.Sin(2 * MathF.PI * n / N + phase);
        }

        public static List<float> GetSignalRange(int N, int M)
        {
            var range = new List<float>();
            for (int i = 0; i < M; i++)
            {
                range.Add(GetHarmonicSignal(i, N));
            }
            return range;
        }
        public static List<float> GetSignalRangePhase(int N, int M, float phase)
        {
            var range = new List<float>();
            for (int i = 0; i < M; i++)
            {
                range.Add(GetHarmonicSignalPhase(i, N, phase));
            }
            return range;
        }

        public static float RootMeanSquare(List<float> values)
        {
            float result = 0;
            for (int i = 0; i < values.Count; i++)
            {
                result += values[i] * values[i];
            }
            result = MathF.Sqrt(result / values.Count);
            return result;
        }

        public static float RootMeanSquareDeviation(List<float> values, float rms)
        {
            float result = 0;
            for (int i = 0; i < values.Count; i++)
            {
                result += (values[i] - rms) * (values[i] - rms);
            }
            result = MathF.Sqrt(result / values.Count);
            return result;
        }

        public static Tuple<List<float>, List<float>> DiscreteFourierTransform(List<float> values)
        {
            var ReX = new List<float>();
            var ImX = new List<float>();
            var N = values.Count;
            for (int k = 0; k < N; k++)
            {
                float sumRe = 0;
                float sumIm = 0;
                for (int n = 0; n < N; n++)
                {
                    sumRe += MathF.Cos(2 * MathF.PI * k * n / N) * values[n];
                    sumIm += -MathF.Sin(2 * MathF.PI * k * n / N) * values[n];
                }
                ReX.Add(sumRe);
                ImX.Add(sumIm);
            }
            return new Tuple<List<float>, List<float>>(ReX, ImX);
        }

        public static List<float> DiscreteFourierTransformAmplitude(Tuple<List<float>, List<float>> values)
        {
            var amplitudes = new List<float>();
            for (int i = 0; i < values.Item1.Count; i++)
            {
                var abs = MathF.Sqrt(values.Item1[i] * values.Item1[i] + values.Item2[i] * values.Item2[i]);
                amplitudes.Add(abs / values.Item1.Count);
            }
            return amplitudes;
        }
    }
}
