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
        public SeriesCollection PhaseDeltaRTSCollection { get; set; }
        public SeriesCollection PhaseDeltaACollection { get; set; }

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


            var phaseDeltaRmsCollection = new ChartValues<Point>();
            var phaseDeltaACollection = new ChartValues<Point>();
            for (float M = K; M < 2 * N; M += Step)
            {
                var range = HarmonicSignal.GetSignalRangePhase(N, (int)M, Phase);
                var rms = HarmonicSignal.RootMeanSquare(range);
                var rmsd = HarmonicSignal.RootMeanSquareDeviation(range, rms);
                var dft = HarmonicSignal.DiscreteFourierTransform(range);
                var aRange = HarmonicSignal.DiscreteFourierTransformAmplitude(dft);
                var deltaRms = MathF.Abs(0.707f - rms);
                var deltaA = 1f - aRange.Sum();
                phaseDeltaRmsCollection.Add(new Point(M, deltaRms));
                phaseDeltaACollection.Add(new Point(M, deltaA));
            }
            PhaseDeltaRTSCollection = new SeriesCollection()
            {
                new LineSeries
                {
                  Configuration = new CartesianMapper<Point>()
                    .X(point => point.X)
                    .Y(point => point.Y),
                  Title = "DeltaRms",
                  Values = phaseDeltaRmsCollection,
                  PointGeometry = null
                }
            };
            PhaseDeltaACollection = new SeriesCollection()
            {
                new LineSeries
                {
                  Configuration = new CartesianMapper<Point>()
                    .X(point => point.X)
                    .Y(point => point.Y),
                  Title = "DeltaA",
                  Values = phaseDeltaACollection,
                  PointGeometry = null
                }
            };
        }
    }
}
