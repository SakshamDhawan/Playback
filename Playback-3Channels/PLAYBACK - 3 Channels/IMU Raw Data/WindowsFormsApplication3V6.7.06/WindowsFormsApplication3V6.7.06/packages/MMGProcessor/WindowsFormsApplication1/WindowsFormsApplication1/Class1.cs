using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMGProcessor
{
    public class DigitalFilter
    {
        private double[] b = new double[2];
        private double[] a = new double[2];
        private double[] x;
        private double zi;
        private double[] y = new double[40];
        private double[] zf = new double[1];

        public DigitalFilter(double[] b, double[] a, double[] x, double zi)
        {
            this.b = b;
            this.a = a;
            this.x = x;
            this.zi = zi;
        }
        private double[] getY()
        {
            calc();
            return y;
        }
        private double[] getZf()
        {
            calc();
            return zf;
        }
        private void calc()
        {
            for (int i = 0; i < y.Length; i++)
            {
                if (i == 0)
                {
                    y[i] = b[0] * x[i] + zi;
                }
                else
                {
                    y[i] = b[0] * x[i] + b[1] * x[i - 1] - a[1] * y[i - 1];
                    if (i == x.Length - 1)
                    {
                zf[0] = y[i];
            }
        }
    }
}
public double[] zeroFilter()
{
    int len = x.Length;     // Length of input
            int nb = b.Length;
    int na = a.Length;
    int nfilt = Math.Max(na, nb);
    int nfact = 3 * (nfilt - 1);   // Length of edge transients
    // Computing the initial value
    Double Data = 1 + a[1];
    double zi;
    zi = (b[1] - a[1] * b[0]) / Data;
            // Number of both Tim
            double[] yTemp = new double[y.Length + 2 * nfact];
    for (int i = 0; i <= nfact; i++)
    {
        yTemp[i] = 2 * x[0] - x[nfact - i];
    }
    for (int i = nfact; i < y.Length + nfact; i++)
    {
        yTemp[i] = x[i - nfact];
    }
    for (int i = y.Length + nfact; i < yTemp.Length; i++)
    {
        yTemp[i] = 2 * x[x.Length - 1] - x[yTemp.Length - 2 - i + y.Length - nfact];
    }
            // Forward filtering
            this.zi = zi * yTemp[0];
    yTemp = zeroCalc(yTemp);
            // In reverse order
            yTemp = this.reverse(yTemp);
            // Reverse filtering
            this.zi = zi * yTemp[0];
    yTemp = zeroCalc(yTemp);
            // In reverse order
            yTemp = this.reverse(yTemp);
    for (int i = 0; i < y.Length; i++)
    {
        y[i] = yTemp[i + nfact];
    }
    return y;
}
private double[] zeroCalc(double[] xx)
{
    double[] yy = new double[xx.Length];
    for (int i = 0; i < yy.Length; i++)
    {
        if (i == 0)
        {
            yy[i] = b[0] * xx[i] + zi;
        }
        else
        {
            yy[i] = b[0] * xx[i] + b[1] * xx[i - 1] - a[1] * yy[i - 1];
        }
    }
    return yy;
}
private double[] reverse(double[] data)
{
    double tmp;
    for (int i = 0; i < data.Length / 2; i++)
    {
        tmp = data[data.Length - i - 1];
        data[data.Length - i - 1] = data[i];
        data[i] = tmp;
    }
    return data;
}
    }
}
