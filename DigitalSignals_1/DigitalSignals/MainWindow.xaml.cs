using LiveCharts;
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
        public SeriesCollection PhaseDeltaCollection { get; set; }
        public SeriesCollection FrequencyDeltaCollection { get; set; }
        public SeriesCollection AmplitudeDeltaCollection { get; set; }
        public SeriesCollection PolyHarmonicSignalCollection { get; set; }
        public SeriesCollection PolyHarmonicSignalDeltaCollection { get; set; }

        private const int _n = 524;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            GenerateData();
        }

        public void GenerateData()
        {
            // A
            var phases = new List<float>() {
                MathF.PI / 3,
                3 * MathF.PI / 4,
                2 * MathF.PI,
                MathF.PI,
                MathF.PI / 6 };

            PhaseDeltaCollection = new SeriesCollection();
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
            });
        }
    }
}
