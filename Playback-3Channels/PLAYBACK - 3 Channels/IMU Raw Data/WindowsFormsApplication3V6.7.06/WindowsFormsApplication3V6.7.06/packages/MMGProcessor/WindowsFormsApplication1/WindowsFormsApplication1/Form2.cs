using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace MMGProcessor
{
    public partial class GraphForm : Form
    {
        public OxyPlot.WindowsForms.PlotView Plot;
        public LineSeries s1;
        public LineSeries s2;
        public LineSeries plotdata;
        int count = 0;
        int fps = 2;


        public void Graph()
        {
            InitializeComponent();

            Plot = new OxyPlot.WindowsForms.PlotView();
            Plot.Model = new PlotModel();
            Plot.Dock = DockStyle.Fill;
            this.Controls.Add(Plot);

            Plot.Model.PlotType = PlotType.XY;
            Plot.Model.Background = OxyColor.FromRgb(255, 255, 255);
            Plot.Model.TextColor = OxyColor.FromRgb(255, 255, 255);

            // Create Line series


            // add Series and Axis to plot model

            LinearAxis x = new LinearAxis { Position = AxisPosition.Bottom, AbsoluteMinimum = 0.0, AbsoluteMaximum = 512.0 };
            LinearAxis y = new LinearAxis { Position = AxisPosition.Left, AbsoluteMinimum = 0.0, AbsoluteMaximum = 160 };
            y.Zoom(0, 250);
            x.Zoom(0, 500);
            s1 = new LineSeries { Title = "LineSeries", StrokeThickness = 1 };
            s2 = new LineSeries { Title = "LineSeries2", StrokeThickness = 1 };
            for (int i = 0; i <= 511; i++)
            {
                s1.Points.Add(new DataPoint(i, 0));
                s2.Points.Add(new DataPoint(i, 0));
            }
            Plot.Model.Series.Add(s1);
            Plot.Model.Series.Add(s2);
            Plot.Model.Axes.Add(x);
            Plot.Model.Axes.Add(y);

        }


        public void addValues(double[] values)
        {
            int count = 511;
            foreach (double value in values){

                s1.Points.Add(new DataPoint(count, (value)));

                count++;
                }

                    for (int i = 0; i <= 511; i++)
                    {
                        s1.Points[i] = new DataPoint(s1.Points[i].X - 40, s1.Points[i].Y);

                    }
            s1.Points.RemoveRange(0,40);
            Plot.InvalidatePlot(true);


        }

        public void addValue(float value)
        {
            s1.Points.Add(new DataPoint(511, (value)));
            for (int i = 0; i <= 511; i++)
            {
                s1.Points[i] = new DataPoint(s1.Points[i].X - 1, s1.Points[i].Y);

            }
            s1.Points.RemoveAt(0);
            Plot.InvalidatePlot(true);
        }

        public void addValue2(float value)
        {
            s2.Points.Add(new DataPoint(511, (value)));
            for (int i = 0; i <= 511; i++)
            {
                s2.Points[i] = new DataPoint(s2.Points[i].X - 1, s2.Points[i].Y);

            }
            s2.Points.RemoveAt(0);
            Plot.InvalidatePlot(true);
        }
        public void addfullarray(List<float> values)
        {
            int count = 0;
            s2.Points.RemoveRange(0, s2.Points.Count());

            foreach (float value in values)
            {
                count++;
                s1.Points.Add(new DataPoint(count, value));
            }

            s1.Points.RemoveAt(0);
            Plot.InvalidatePlot(true);


        }


    }
}

