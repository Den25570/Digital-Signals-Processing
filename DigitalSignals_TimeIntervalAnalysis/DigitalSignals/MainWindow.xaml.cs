using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DigitalSignals
{
    public partial class MainWindow : Window
    {
        public SeriesCollection DeltaRTSCollection { get; set; }
        public SeriesCollection DeltaACollection { get; set; }
        public SeriesCollection PhaseDeltaCollection { get; set; }

        private const int N = 524;
        private const int K = N / 4;
        private const float Phase = MathF.PI / 2;
        private const float Step = (N / 4) / 8;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            GenerateData();
        }

        public void GenerateData()
        {
            
            var deltaRmsCollection = new ChartValues<Point>();
            var deltaACollection = new ChartValues<Point>();
            for (float M = K; M < 2*N; M += Step)
            {
                var range = HarmonicSignal.GetSignalRange(N, (int)M);
                var rms = HarmonicSignal.RootMeanSquare(range);
                var rmsd = HarmonicSignal.RootMeanSquareDeviation(range, rms);
                var dft = HarmonicSignal.DiscreteFourierTransform(range);
                var test = range.Max();
                var aRange = HarmonicSignal.DiscreteFourierTransformAmplitude(dft);
                var deltaRms = MathF.Abs(0.707f - rms);
                var deltaA = 1f - aRange.Sum();
                deltaRmsCollection.Add(new Point(M, deltaRms));
                deltaACollection.Add(new Point(M, deltaA));
            }
            DeltaRTSCollection = new SeriesCollection()
            {
                new LineSeries
                {
                  Configuration = new CartesianMapper<Point>()
                    .X(point => point.X)
                    .Y(point => point.Y),
                  Title = "DeltaRms",
                  Values = deltaRmsCollection,
                  PointGeometry = null
                }
            };
            DeltaACollection = new SeriesCollection()
            {
                new LineSeries
                {
                  Configuration = new CartesianMapper<Point>()
                    .X(point => point.X)
                    .Y(point => point.Y),
                  Title = "DeltaA",
                  Values = deltaACollection,
                  PointGeometry = null
                }
            };


            /*PhaseDeltaCollection = new SeriesCollection();
            foreach (var phase in phases)
            {
                var values = new ChartValues<double> { };
                for (int i = 0; i < _n; i++)
                {
                    values.Add(HarmonicSignal.GetHarmonicSignal(i, 9, _n, phase, 4));
                }
                PhaseDeltaCollection.Add(new LineSeries()
                {
                    Title = $"Phase {phase}",
                    Values = values,
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                });
            }

            // B

            var frequencies = new List<float>() { 4, 8, 2, 1, 9 };

            FrequencyDeltaCollection = new SeriesCollection();
            foreach (var frequency in frequencies)
            {
                var values = new ChartValues<double> { };
                for (int i = 0; i < _n; i++)
                {
                    values.Add(HarmonicSignal.GetHarmonicSignal(i, 7, _n, MathF.PI / 6, frequency));
                }
                FrequencyDeltaCollection.Add(new LineSeries()
                {
                    Title = $"Frequency {frequency}",
                    Values = values,
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                });
            }

            // C
            var amplitudes = new List<float>() { 4, 5, 3, 1, 7 };

            AmplitudeDeltaCollection = new SeriesCollection();
            foreach (var amplitude in amplitudes)
            {
                var values = new ChartValues<double> { };
                for (int i = 0; i < _n; i++)
                {
                    values.Add(HarmonicSignal.GetHarmonicSignal(i, amplitude, _n, MathF.PI / 6, 7));
                }
                AmplitudeDeltaCollection.Add(new LineSeries()
                {
                    Title = $"Amplitude {amplitude}",
                    Values = values,
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                });
            }


            // D
            var polyAmplitudes = new List<float>() { 7, 7, 7, 7, 7 };
            var polyFrequencies = new List<float>() { 1, 2, 3, 4, 5 };
            var polyPhases = new List<float>() { MathF.PI, MathF.PI / 4, 0, 3 * MathF.PI / 4, MathF.PI / 2 };

            PolyHarmonicSignalCollection = new SeriesCollection();

            var polyValues = new ChartValues<double> { };
            for (int i = 0; i < _n * 4; i++)
            {
                polyValues.Add(HarmonicSignal.GetPolyHarmonicSignal(i, polyAmplitudes, _n, polyFrequencies, polyPhases));
            }
            PolyHarmonicSignalCollection.Add(new LineSeries()
            {
                Title = $"Poly Harmonic Signal",
                Values = polyValues,
                PointGeometry = null,
                Fill = Brushes.Transparent
            });

            // E
            var polyDeltaAmplitudes = new List<float>() { 7 };
            var polyDeltaFrequencies = new List<float>() { 1 };
            var polyDeltaPhases = new List<float>() { MathF.PI };

            for (int i = 0; i < 60; i++)
            {
                polyDeltaAmplitudes.Add(polyDeltaAmplitudes.Last() + 0.005f);
                polyDeltaFrequencies.Add(polyDeltaFrequencies.Last() + 0.005f);
                polyDeltaPhases.Add(polyDeltaPhases.Last() + 0.005f);
            }

            PolyHarmonicSignalDeltaCollection = new SeriesCollection();

            var polyDeltaValues = new ChartValues<double> { };
            for (int i = 0; i < _n * 4; i++)
            {
                polyDeltaValues.Add(HarmonicSignal.GetPolyHarmonicSignal(i, polyDeltaAmplitudes, _n, polyDeltaFrequencies, polyDeltaPhases));
            }
            PolyHarmonicSignalDeltaCollection.Add(new LineSeries()
            {
                Title = $"Poly Harmonic Signal",
                Values = polyDeltaValues,
                PointGeometry = null,
                Fill = Brushes.Transparent
            });*/
        }
    }
}
