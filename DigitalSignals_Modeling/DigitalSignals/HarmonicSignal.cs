using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignals
{
    public static class HarmonicSignal
    {
        public static float GetHarmonicSignal(float n, float amplitude, float N, float phase, float frequency)
        {
            return amplitude * MathF.Sin(2 * MathF.PI * frequency * n / N + phase);
        }

        public static float GetPolyHarmonicSignal(float n, List<float> amplitudes, float N, List<float> phases, List<float> frequencies)
        {
            float result = 0;
            for (int i = 0; i < amplitudes.Count; i++)
            {
                result += amplitudes[i] * MathF.Sin(2 * MathF.PI * frequencies[i] * n / N + phases[i]);
            }
            return result;
        }
    }
}
