using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        public SeriesCollection HarmonicSignalCollection { get; set; }
        public SeriesCollection HarmonicSignalNoiseCollection { get; set; }
        public SeriesCollection CrossCorrelationFunctionCollection { get; set; }
        public SeriesCollection CrossCorrelationNoiseFunctionCollection { get; set; }

        private const int _n = 524;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            GenerateData();
        }

        public void GenerateData()
        {
            int size = 2048;
            int noiseSize = 50;
            var harmonicSignalParams = new HarmonicSignal.HarmonicSignalParams()
            {
                B1 = 1,
                B2 = 0.1f,
                N = 1024
            };

            var harmonicSignal = HarmonicSignal.GenerateHarmonicSignal(harmonicSignalParams, size);
            var harmonicSignalNoise = HarmonicSignal.GenerateHarmonicSignalWithNoise(harmonicSignalParams, size);
            var correlationFunction = HarmonicSignal.CrossCorrelation(harmonicSignal, harmonicSignalNoise, size);
            var correlationFunctionFFT = HarmonicSignal.CrossCorrelationFFT(harmonicSignal, harmonicSignalNoise, size);

            var noiseA = HarmonicSignal.Noise(harmonicSignalParams, noiseSize);
            var noiseB = HarmonicSignal.Noise(harmonicSignalParams, noiseSize);
            var noiseCorrelationFunction = HarmonicSignal.CrossCorrelation(noiseA, noiseA, noiseSize);
            var noiseCorrelationFunctionFFT = HarmonicSignal.CrossCorrelationFFT(noiseA, noiseA, noiseSize);

            /*var medianFiltered = HarmonicSignal.MedianFilter(harmonicSignal, 5);
            var medianAverageFiltered = HarmonicSignal.MedianAverage(harmonicSignal, 9);
            var parabolaDegree4Filtered = HarmonicSignal.ParabolaDegree4(harmonicSignal);

            var harmonicSignalSpecter = HarmonicSignal.FourierTransform(size, harmonicSignal.Select(x => new Complex(x, 0)).ToList());
            var medianFilteredSpecter = HarmonicSignal.FourierTransform(size, medianFiltered.Select(x => new Complex(x, 0)).ToList());
            var medianAverageFilteredSpecter = HarmonicSignal.FourierTransform(size, medianAverageFiltered.Select(x => new Complex(x, 0)).ToList());
            var parabolaDegree4FilteredSpecter = HarmonicSignal.FourierTransform(size, parabolaDegree4Filtered.Select(x => new Complex(x, 0)).ToList());*/

            HarmonicSignalCollection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Values = new ChartValues<double>(harmonicSignalNoise),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                },
                new LineSeries()
                {
                    Values = new ChartValues<double>(harmonicSignal),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                },
            };

            CrossCorrelationFunctionCollection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Values = new ChartValues<double>(correlationFunction),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                },
                new LineSeries()
                {
                    Values = new ChartValues<double>(correlationFunctionFFT),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                },
            };

            HarmonicSignalNoiseCollection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Values = new ChartValues<double>(noiseA),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                },
                new LineSeries()
                {
                    Values = new ChartValues<double>(noiseA),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                },
            };

            CrossCorrelationNoiseFunctionCollection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Values = new ChartValues<double>(noiseCorrelationFunction),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                },
                new LineSeries()
                {
                    Values = new ChartValues<double>(noiseCorrelationFunctionFFT),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                }
            };
        }
    }
}
