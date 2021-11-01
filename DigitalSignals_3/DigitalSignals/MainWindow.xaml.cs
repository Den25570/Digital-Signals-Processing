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
        public SeriesCollection MedianFilterCollection { get; set; }
        public SeriesCollection ParabolaDegree4Collection { get; set; }
        public SeriesCollection MedianAverageCollection { get; set; }

        public SeriesCollection HarmonicSignalAmplitudesCollection { get; set; }
        public SeriesCollection MedianFilterAmplitudesCollection { get; set; }
        public SeriesCollection ParabolaDegree4AmplitudesCollection { get; set; }
        public SeriesCollection MedianAverageAmplitudesCollection { get; set; }
        public SeriesCollection HarmonicSignalPhasesCollection { get; set; }
        public SeriesCollection MedianFilterPhasesCollection { get; set; }
        public SeriesCollection ParabolaDegree4PhasesCollection { get; set; }
        public SeriesCollection MedianAveragePhasesCollection { get; set; }

        private const int _n = 524;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            GenerateData();
        }

        public void GenerateData()
        {
            var harmonicSignalParams = new HarmonicSignal.HarmonicSignalParams()
            {
                B1 = 1,
                B2 = 0.01f,
                N = 1024
            };

            var harmonicSignal = HarmonicSignal.GenerateHarmonicSignal(harmonicSignalParams, 1024);
            var medianFiltered = HarmonicSignal.MedianFilter(harmonicSignal, 5);
            var medianAverageFiltered = HarmonicSignal.MedianAverage(harmonicSignal, 9);
            var parabolaDegree4Filtered = HarmonicSignal.ParabolaDegree4(harmonicSignal);

            var harmonicSignalSpecter = HarmonicSignal.FourierTransform(1024, harmonicSignal.Select(x => new Complex(x, 0)).ToList());
            var medianFilteredSpecter = HarmonicSignal.FourierTransform(1024, medianFiltered.Select(x => new Complex(x, 0)).ToList());
            var medianAverageFilteredSpecter = HarmonicSignal.FourierTransform(1024, medianAverageFiltered.Select(x => new Complex(x, 0)).ToList());
            var parabolaDegree4FilteredSpecter = HarmonicSignal.FourierTransform(1024, parabolaDegree4Filtered.Select(x => new Complex(x, 0)).ToList());

            HarmonicSignalCollection = new SeriesCollection() 
            {
                new LineSeries()
                {
                    Values = new ChartValues<double>(harmonicSignal),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                },
            };

            MedianFilterCollection = new SeriesCollection() 
            {
                new LineSeries()
                {
                    Values = new ChartValues<double>(harmonicSignal),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                },
                new LineSeries()
                {
                    Values = new ChartValues<double>(medianFiltered),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                }
            };

            MedianAverageCollection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Values = new ChartValues<double>(harmonicSignal),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                },
                new LineSeries()
                {
                    Values = new ChartValues<double>(medianAverageFiltered),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                }
            };

            ParabolaDegree4Collection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Values = new ChartValues<double>(harmonicSignal),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                },
                new LineSeries()
                {
                    Values = new ChartValues<double>(parabolaDegree4Filtered),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                }
            };

            // Spectres

            HarmonicSignalAmplitudesCollection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Values = new ChartValues<double>(harmonicSignalSpecter.Select(x => Complex.Abs(x) / 1024)),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                },
            };

            MedianFilterAmplitudesCollection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Values = new ChartValues<double>(medianFilteredSpecter.Select(x => Complex.Abs(x) / 1024)),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                },
            };

            MedianAverageAmplitudesCollection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Values = new ChartValues<double>(medianAverageFilteredSpecter.Select(x => Complex.Abs(x) / 1024)),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                },
            };

            ParabolaDegree4AmplitudesCollection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Values = new ChartValues<double>(parabolaDegree4FilteredSpecter.Select(x => Complex.Abs(x) / 1024)),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                },
            };

            HarmonicSignalPhasesCollection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Values = new ChartValues<double>(harmonicSignalSpecter.Select(x => (Math.PI / 2 - Math.Atan(x.Imaginary / x.Real)) / 1024)),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                },
            };

            MedianFilterPhasesCollection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Values = new ChartValues<double>(medianFilteredSpecter.Select(x => (Math.PI / 2 - Math.Atan(x.Imaginary / x.Real)) / 1024)),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                }
            };

            MedianAveragePhasesCollection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Values = new ChartValues<double>(medianAverageFilteredSpecter.Select(x => (Math.PI / 2 - Math.Atan(x.Imaginary / x.Real)) / 1024)),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                }
            };

            ParabolaDegree4PhasesCollection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Values = new ChartValues<double>(parabolaDegree4FilteredSpecter.Select(x => (Math.PI / 2 - Math.Atan(x.Imaginary / x.Real)) / 1024)),
                    PointGeometry = null,
                    Fill = Brushes.Transparent
                }
            };
        }
    }
}
