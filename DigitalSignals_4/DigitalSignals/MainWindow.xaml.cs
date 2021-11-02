using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public string timeToCrossCorr { get; set; }

        public string timeToCrossCorrFFT { get; set; }

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
            var correlationFunctionFFT = HarmonicSignal.CrossCorrelationFFT(harmonicSignal, harmonicSignalNoise, size, harmonicSignalParams.N);

            var noiseA = HarmonicSignal.Noise(harmonicSignalParams, noiseSize);
            var noiseCorrelationFunction = HarmonicSignal.CrossCorrelation(noiseA, noiseA, noiseSize);
            var noiseCorrelationFunctionFFT = HarmonicSignal.AutoCrossCorrelationFFT(noiseA, noiseSize, noiseSize);

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                var tmp = HarmonicSignal.CrossCorrelation(noiseA, noiseA, noiseSize);
            }
            watch.Stop();
            timeToCrossCorr = watch.ElapsedMilliseconds.ToString() + "ms";

            watch.Restart();
            for (int i = 0; i < 100; i++)
            {
                var tmp = HarmonicSignal.AutoCrossCorrelationFFT(noiseA, noiseSize, noiseSize);
            }
            watch.Stop();
            timeToCrossCorrFFT = watch.ElapsedMilliseconds.ToString() + "ms";


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
                    Values = new ChartValues<double>(correlationFunction),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                }
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
                    Values = new ChartValues<double>(noiseCorrelationFunction),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                }
            };
        }
    }
}
