using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;


namespace NUClass
{
    public partial class Form_Report : Form
    {
        public ChartValues<double> MMG_1_Data_Trials = new ChartValues<double> {};
        public ChartValues<double> MMG_2_Data_Trials = new ChartValues<double> {};
        public ChartValues<double> MMG_3_Data_Trials = new ChartValues<double> {};

        public Form_Report()
        {
            InitializeComponent();
            RASeries = new LineSeries
            {
                Values = new ChartValues<double> {0, 12, 12, 17, 6, 20, 21, 26, 25, 18, 25},
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = new System.Windows.Media.SolidColorBrush(Colors.Orange)
            };
            TASeries = new LineSeries
            {
                Values = new ChartValues<double> {0, 22, 8, 23, 25, 6, 30, 26, 24, 16, 19},
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = new System.Windows.Media.SolidColorBrush(Colors.Green)
            };
            ESSeries = new LineSeries
            {
                Values = new ChartValues<double> {0, 16, 23, 10, 26, 8, 21, 30, 22, 32, 19},
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = new System.Windows.Media.SolidColorBrush(Colors.Purple)
            };

            cartesianChart1.Series = new SeriesCollection
            {
                RASeries,
                TASeries,
                ESSeries
            };

            /* cartesianChart1.Series = new SeriesCollection
             {
                 new LineSeries
                 {
                     Values = new ChartValues<ObservableValue>
                     {
                         new ObservableValue(3),
                         new ObservableValue(5),
                         new ObservableValue(2),
                         new ObservableValue(7),
                         new ObservableValue(7),
                         new ObservableValue(4)
                     },
                     PointGeometry = DefaultGeometries.None,
                     StrokeThickness = 4,
                     Fill = Brushes.Transparent
                 },
                 new LineSeries
                 {
                     Values = new ChartValues<ObservableValue>
                     {
                         new ObservableValue(3),
                         new ObservableValue(4),
                         new ObservableValue(6),
                         new ObservableValue(8),
                         new ObservableValue(7),
                         new ObservableValue(5)
                     },
                     PointGeometry = DefaultGeometries.None,
                     StrokeThickness = 4,
                     //Fill = Brushes.Transparent
                 }
             };*/
            cartesianChart1.AxisX.Add(new Axis
            {

            });
            cartesianChart1.AxisY.Add(new Axis
            {
                MinValue = 0,
                MinRange = 0,
                Sections = new SectionsCollection
                {
                  /*  new AxisSection
                    {
                        Value = 8.5,
                        Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(248, 213, 72))
                    },*/
                   /* new AxisSection
                    {
                        Label = "Good",
                        Value = 4,
                        SectionWidth = 4,
                        Fill = new SolidColorBrush
                        {
                            Color = System.Windows.Media.Color.FromRgb(145,255,197),
                            Opacity = .4
                        }
                    },*/
                    new AxisSection
                    {
                       // Label = "Bad",
                        Value = 0,
                        SectionWidth = 15,
                        Fill = new SolidColorBrush
                        {
                            Color = System.Windows.Media.Color.FromRgb(254,132,132),
                            Opacity = .3
                        }
                    }
                }
            });


            var gradientBrush1 = new LinearGradientBrush
            {
                StartPoint = new System.Windows.Point(0, 0),
                EndPoint = new System.Windows.Point(0, 1)
            };
            gradientBrush1.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromRgb(255, 209, 204), 0));
            gradientBrush1.GradientStops.Add(new GradientStop(Colors.Transparent, 0.8));

            var gradientBrush2 = new LinearGradientBrush
            {
                StartPoint = new System.Windows.Point(0, 0),
                EndPoint = new System.Windows.Point(0, 1)
            };
            gradientBrush2.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromRgb(209, 255, 204), 0));
            gradientBrush2.GradientStops.Add(new GradientStop(Colors.Transparent, 0.8));

            var gradientBrush3 = new LinearGradientBrush
            {
                StartPoint = new System.Windows.Point(0, 0),
                EndPoint = new System.Windows.Point(0, 1)
            };
            gradientBrush3.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromRgb(255, 153, 225), 0));
            gradientBrush3.GradientStops.Add(new GradientStop(Colors.Transparent, 0.8));

            cartesianChart2.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = GetData(0.5,10),
                    Fill = gradientBrush1,
                    Stroke = new System.Windows.Media.SolidColorBrush(Colors.Orange),
                    StrokeThickness = 2,
                    PointGeometry = null,

                },
                new LineSeries
                {
                    Values = GetData(0.1,10),
                    Fill = gradientBrush2,
                    Stroke = new System.Windows.Media.SolidColorBrush(Colors.Green),
                    StrokeThickness = 2,
                    PointGeometry = null,
                },
               new LineSeries
                {
                    Values = GetData(0.2,5),
                    Stroke = new System.Windows.Media.SolidColorBrush(Colors.Purple),
                    Fill = gradientBrush3,
                    StrokeThickness = 2,
                    PointGeometry = null,
                }
            };

           /* cartesianChart2.Series.Add(new LineSeries
            {
                Values = GetData(),
                Fill = gradientBrush,
                StrokeThickness = 1,
                PointGeometry = null,
            });*/


            cartesianChart2.Zoom = ZoomingOptions.X;

            cartesianChart2.AxisX.Add(new Axis
            {
                LabelFormatter = val => new System.DateTime((long)val).ToString("dd MMM")
            });

            cartesianChart2.AxisY.Add(new Axis
            {
                //LabelFormatter = val => val.ToString("C")
            }) ;

        }
        public LineSeries RASeries { get; set; }
        public LineSeries TASeries { get; set; }
        public LineSeries ESSeries { get; set; }

        public void DataUpdate()
        {

            RASeries = new LineSeries
            {
                Values = MMG_1_Data_Trials,
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = new System.Windows.Media.SolidColorBrush(Colors.Orange)
            };
            TASeries = new LineSeries
            {
                Values = MMG_2_Data_Trials,
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = new System.Windows.Media.SolidColorBrush(Colors.Green)
            };
            ESSeries = new LineSeries
            {
                Values = MMG_3_Data_Trials,
                Fill = System.Windows.Media.Brushes.Transparent,
                Stroke = new System.Windows.Media.SolidColorBrush(Colors.Purple)
            };

            cartesianChart1.Series = new SeriesCollection
            {
                RASeries,
                TASeries,
                ESSeries
            };

        }
        private ChartValues<DateTimePoint> GetData(double ratio, int num)
        {
            var r = new Random();
            var trend = 35;
            var values = new ChartValues<DateTimePoint>();

            for (var i = 0; i < 35; i++)
            {
                var seed = r.NextDouble();
                //if (seed > .8) trend += seed > .9 ? 50 : -50;
                values.Add(new DateTimePoint(System.DateTime.Now.AddDays(i), trend + i*ratio + r.Next(0, num)));
            }

            return values;
        }

        private void ClearZoom()
        {
            //to clear the current zoom/pan just set the axis limits to double.NaN

            cartesianChart1.AxisX[0].MinValue = double.NaN;
            cartesianChart1.AxisX[0].MaxValue = double.NaN;
            cartesianChart1.AxisY[0].MinValue = double.NaN;
            cartesianChart1.AxisY[0].MaxValue = double.NaN;
        }

        private void ZomingAndPanningExample_Load(object sender, EventArgs e)
        {

        }
    }
}
