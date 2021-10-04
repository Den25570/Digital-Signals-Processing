using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace DigitalSignals.Plots
{
    public partial class LineChart : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public static DependencyProperty SeriesCollectionProperty = DependencyProperty.Register("SeriesCollection", typeof(SeriesCollection), typeof(LineChart));
        public static DependencyProperty LabelsProperty = DependencyProperty.Register("Labels", typeof(string[]), typeof(LineChart));

        public LineChart()
        {
            InitializeComponent();

            YFormatter = value => value.ToString();
            DataContext = this;
        }
    }
}