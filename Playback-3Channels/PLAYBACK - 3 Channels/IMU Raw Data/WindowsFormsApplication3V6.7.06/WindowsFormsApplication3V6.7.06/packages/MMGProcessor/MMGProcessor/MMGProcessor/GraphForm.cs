using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace MMGProcessor
{
    public partial class GraphForm : Form
    {
        public OxyPlot.WindowsForms.PlotView Plot;
        public LineSeries s1;
        public LineSeries s2;
        public LineSeries s3;
        public LineSeries s4;
        public LineSeries s5;
        public LineSeries s6;
        public LineSeries s7;
        public LineSeries s8;
        public List<LineSeries> Lines;
        public LineSeries plotdata;
        int count = 0;
        int fps = 2;

        public void resize(int h, int w)
        {
            this.Height = h;
            this.Width = w;
        }
        public void Graph(int series, bool docked)
        {
            InitializeComponent();
            Plot = new OxyPlot.WindowsForms.PlotView();
            Plot.Model = new PlotModel();
            Plot.Dock = DockStyle.Fill;
            this.Controls.Add(Plot);
            if (docked)
            {
                this.Height = 182;
                this.Width = 323;
            }
            Plot.Model.PlotType = PlotType.XY;
            Plot.Model.Background = OxyColor.FromRgb(255, 255, 255);
            Plot.Model.TextColor = OxyColor.FromRgb(255, 255, 255);

            // Create Line series


            // add Series and Axis to plot model

            LinearAxis x = new LinearAxis { Position = AxisPosition.Bottom, AbsoluteMinimum = 0.0, AbsoluteMaximum = 100.0 };
            LinearAxis y = new LinearAxis { Position = AxisPosition.Left, AbsoluteMinimum = 0.0, AbsoluteMaximum = 5.0 };
            y.Zoom(0, 5);
            x.Zoom(0, 100);

            s1 = new LineSeries { Title = "LineSeries", StrokeThickness = 1 };
            for (int i = 0; i <= 99; i++)
            {
                s1.Points.Add(new DataPoint(i, 0));
            }
            Plot.Model.Series.Add(s1);
            if (series > 1)
            {
                s2 = new LineSeries { Title = "LineSeries2", StrokeThickness = 1 };
                for (int i = 0; i <= 99; i++)
                {
                    s2.Points.Add(new DataPoint(i, 0));
                }
                Plot.Model.Series.Add(s2);
            }
            if (series > 2)
            {
                s3 = new LineSeries { Title = "LineSeries3", StrokeThickness = 1 };
                for (int i = 0; i <= 99; i++)
                {
                    s3.Points.Add(new DataPoint(i, 0));
                }
                Plot.Model.Series.Add(s3);
            }
            if (series > 3)
            {
                s4 = new LineSeries { Title = "LineSeries4", StrokeThickness = 1 };
                for (int i = 0; i <= 99; i++)
                {
                    s4.Points.Add(new DataPoint(i, 0));
                }
                Plot.Model.Series.Add(s4);
            }
            if (series > 4)
            {
                s5 = new LineSeries { Title = "LineSeries5", StrokeThickness = 1 };
                for (int i = 0; i <= 99; i++)
                {
                    s5.Points.Add(new DataPoint(i, 0));
                }
                Plot.Model.Series.Add(s5);
            }
            if (series > 5)
            {
                s6 = new LineSeries { Title = "LineSeries6", StrokeThickness = 1 };
                for (int i = 0; i <= 99; i++)
                {
                    s6.Points.Add(new DataPoint(i, 0));
                }
                Plot.Model.Series.Add(s6);
            }
            if (series > 6)
            {
                s7 = new LineSeries { Title = "LineSeries7", StrokeThickness = 1 };
                for (int i = 0; i <= 99; i++)
                {
                    s7.Points.Add(new DataPoint(i, 0));
                }
                Plot.Model.Series.Add(s7);
            }
            if (series > 7)
            {
                s8 = new LineSeries { Title = "LineSeries8", StrokeThickness = 1 };
                for (int i = 0; i <= 99; i++)
                {
                    s8.Points.Add(new DataPoint(i, 0));
                }
                Plot.Model.Series.Add(s8);
            }

            Lines = new List<LineSeries> { s1, s2, s3, s4, s5, s6, s7, s8 };
            Plot.Model.Axes.Add(x);
            Plot.Model.Axes.Add(y);

        }

        public void addValue(float value1, float value2, float value3, float value4, float value5, float value6, float value7, float value8)
        {

            s1.Points.Add(new DataPoint(99, value1));
            s2.Points.Add(new DataPoint(99, value2 + 0.5));
            s3.Points.Add(new DataPoint(99, value3 + 1));
            s4.Points.Add(new DataPoint(99, value4 + 1.5));
            s5.Points.Add(new DataPoint(99, value5 + 2));
            s6.Points.Add(new DataPoint(99, value6 + 2.5));
            s7.Points.Add(new DataPoint(99, value7 + 3));
            s8.Points.Add(new DataPoint(99, value8 + 3.5));
            for (int i = 0; i <= 99; i++)
            {
                s1.Points[i] = new DataPoint(s1.Points[i].X - 1, s1.Points[i].Y);
                s2.Points[i] = new DataPoint(s2.Points[i].X - 1, s2.Points[i].Y);
                s3.Points[i] = new DataPoint(s3.Points[i].X - 1, s3.Points[i].Y);
                s4.Points[i] = new DataPoint(s4.Points[i].X - 1, s4.Points[i].Y);
                s5.Points[i] = new DataPoint(s5.Points[i].X - 1, s5.Points[i].Y);
                s6.Points[i] = new DataPoint(s6.Points[i].X - 1, s6.Points[i].Y);
                s7.Points[i] = new DataPoint(s7.Points[i].X - 1, s7.Points[i].Y);
                s8.Points[i] = new DataPoint(s8.Points[i].X - 1, s8.Points[i].Y);
            }

            s1.Points.RemoveAt(0);
            s2.Points.RemoveAt(0);
            s3.Points.RemoveAt(0);
            s4.Points.RemoveAt(0);
            s5.Points.RemoveAt(0);
            s6.Points.RemoveAt(0);
            s7.Points.RemoveAt(0);
            s8.Points.RemoveAt(0);
            Plot.InvalidatePlot(true);
        }
        public void addValue(float value1)
        {

            s1.Points.Add(new DataPoint(99, value1));
            for (int i = 0; i <= 99; i++)
            {
                s1.Points[i] = new DataPoint(s1.Points[i].X - 1, s1.Points[i].Y);
            }

            s1.Points.RemoveAt(0);
            Plot.InvalidatePlot(true);

            Application.DoEvents();


        }
        public void addValue(float value1, float value2, float value3)
        {

            s1.Points.Add(new DataPoint(99, value1));
            s2.Points.Add(new DataPoint(99, value2));
            s3.Points.Add(new DataPoint(99, value3));
            for (int i = 0; i <= 99; i++)
            {
                s1.Points[i] = new DataPoint(s1.Points[i].X - 1, s1.Points[i].Y);
                s2.Points[i] = new DataPoint(s2.Points[i].X - 1, s2.Points[i].Y);
                s3.Points[i] = new DataPoint(s3.Points[i].X - 1, s3.Points[i].Y);
            }

            s1.Points.RemoveAt(0);
            s2.Points.RemoveAt(0);
            s3.Points.RemoveAt(0);
            Plot.InvalidatePlot(true);

            Application.DoEvents();

        }
        public void addfullarray(List<float> values)
        {
            int count = 0;
            s1.Points.RemoveRange(0, s1.Points.Count());
            s2.Points.RemoveRange(0, s2.Points.Count());

            foreach (float value in values)
            {
                count++;
                s1.Points.Add(new DataPoint(count, value));
            }

            s1.Points.RemoveAt(0);
            Plot.InvalidatePlot(true);

            Application.DoEvents();

        }
        public void addfullarray(double[] values)
        {


            foreach (float value in values)
            {
                s1.Points.Add(new DataPoint(99 + values.Length, value / 25));
            }

            for (int i = 0; i <= (99 + values.Length); i++)
            {
                s1.Points[i] = new DataPoint(s1.Points[i].X - values.Length + 1, s1.Points[i].Y);

            }

            s1.Points.RemoveRange(0, values.Length);


            Plot.InvalidatePlot(true);

            Application.DoEvents();

        }
        public void addfullarray(double[] values, int line)
        {
            Lines[line].Points.Clear();
            int skip_value = values.Length / 100;
            for(int i = 0; i<values.Length; i = i + skip_value)
            {
                Lines[line].Points.Add(new DataPoint(i, (values[i] / 50)+2.5));
            }
            

            Plot.InvalidatePlot(true);

            Application.DoEvents();

        }

    }
}
