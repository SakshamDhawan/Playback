using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge;
using AForge.Math;

namespace MMGProcessor
{
    public class MMGProcessorClass
    {
        //        MMGProcessorClass MMGProcessor = new MMGProcessorClass();
        //        MMGProcessor.initialise(400, 4, 35, 1f, 40, 1);

        public double val = 0;
        List<double> save = new List<double>();
        public List<double> memory = new List<double>();
        private double[] a2 = new double[] { 1f, -0.9404f };
        private double[] b2 = new double[] { 0.9702f, -0.9702f };
        private double[] a = new double[] { 1f, -0.7322f };
        private double[] b = new double[] { 0.1339f, 0.1339f };
        public List<int> MMGdata = new List<int>();
        List<int> FirstFiltered = new List<int>();
        public int circleW;
        public int circleH;
        public int circlePW;
        public int circlePH;
        public int thres;
        public float accurary;
        public int processSpeed;
        public int processProgress;
        public int speed2;
        public bool flag;
        public int current = 0;
        public bool saveReturn = false;
        public double[] data = new double[40];

        public void initialise(int IcircleW, int IcircleH, int Ithres, float Iaccuracy, int IprocessSpeed, int Ispeed2)
        {
            circleW = IcircleW;
            circleH = IcircleH;
            if (circleW % 2 == 1) circlePW = (circleW + 1) / 2; circlePW = (circleW / 2) + 1;
            if (circleH % 2 == 1) circlePH = (circleW + 1) / 2; circlePH = (circleH / 2);
            thres = Ithres;
            accurary = Iaccuracy;
            processSpeed = IprocessSpeed;
            processProgress = 0;
            speed2 = Ispeed2;
            flag = false;
            current = 0;
        }
        public void initialise(int IcircleW, int IcircleH, int Ithres, float Iaccuracy, int IprocessSpeed, int Ispeed2, List<int> Data, List<double> Memory, int Current)
        {
            MMGdata = Data;
            memory = Memory;
            current = Current;
            circleW = IcircleW;
            circleH = IcircleH;
            if (circleW % 2 == 1) circlePW = (circleW + 1) / 2; circlePW = (circleW / 2) + 1;
            if (circleH % 2 == 1) circlePH = (circleW + 1) / 2; circlePH = (circleH / 2);
            thres = Ithres;
            accurary = Iaccuracy;
            processSpeed = IprocessSpeed;
            processProgress = 0;
            speed2 = Ispeed2;
            flag = false;
        }
        public bool AddData(List<int> NewData)
        {
            MMGdata.AddRange(NewData);
            return DataProcessor();
        }
        public bool AddData(int NewData)
        {
            MMGdata.Add(NewData);
            return DataProcessor();
        }



        public bool DataProcessor()
        {
            List<int> ProcessedData = new List<int>();
            if (MMGdata.Count > 40)
            {
                List<int> hold = MMGdata.GetRange(1, 40);

                int index = 0;

                
                foreach (int value in hold)
                {
                    data[index++] = (double)value;
                }

                DigitalFilter filter = new DigitalFilter(b2, a2, data, 0);
                data = filter.zeroFilter();
                DigitalFilter filter2 = new DigitalFilter(b, a, data, 0);
                data = filter2.zeroFilter();

                index = 0;
                Complex[] complexData = new Complex[64];
                foreach (double value in data)
                {
                    complexData[index++] = (Complex)value;
                }

                AForge.Math.FourierTransform.FFT(complexData, FourierTransform.Direction.Forward);

                int size = complexData.Count();

                double[] h = new double[64];

                for (index = 0; index < 64; index++)
                {
                    if (index == 0 || index == 32)
                    {
                        complexData[index] = complexData[index] * 1;

                    }

                    else
                    {
                        if (index > 0 && index < 32)
                        {
                            complexData[index] = complexData[index] * 2;
                        }
                        else
                        {
                            complexData[index] = complexData[index] * 0;
                        }
                    }
                }
                AForge.Math.FourierTransform.FFT(complexData, FourierTransform.Direction.Backward);
                for (index = 0; index < data.Count(); index++)
                {
                    data[index] = Math.Abs(complexData[index].Magnitude);
                }





                if (memory.Count <= circleW)
                {
                    memory.AddRange(data);
                }
                else
                {
                    memory.AddRange(data);
                    val = memory.GetRange(0, circleW).Max() + circleH / 2;
                    bool complete = false;

                    while (!complete)
                    {
                        int count = 0;
                        while (count <= circleW)
                        {
                            double ypos = memory[count];
                            if (((Math.Pow((count - circlePW), 2) / Math.Pow((circleW / 2), 2)) + ((Math.Pow((ypos - val), 2)) / (Math.Pow((circleH / 2), 2)))) < 1)
                            {
                                complete = true;
                            }
                            count += speed2;
                        }
                        val -= accurary;
                    }






                    memory.RemoveRange(0, processSpeed);
                }


                bool trigger = false;
                if (val >= thres)
                {
                    if (current == 0)
                    {
                        trigger = true;
                        current = 1;
                    }
                    else
                    {
                        trigger = false;
                    }
                }
                else
                {
                    if (current == 1)
                    {
                        trigger = false;
                        current = 0;
                    }
                    else
                    {
                        trigger = false;
                    }
                }


                MMGdata.RemoveRange(0, 40);
                if (trigger)
                {
                    saveReturn = true;
                    return true;
                }
                else
                {
                    saveReturn = false;
                    return false;
                }
            }
            saveReturn = false;
            return false;
        }
        public void outputData()
        {
        }
    }


}
