﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OpenTK;
using System.Windows.Forms;
using Utilities;
using System.Diagnostics;

namespace NUClass
{
    public partial class Form1 : Form
    {

        AHRS ahrs = new AHRS();
        Quaternion gdaQuat;
        Quaternion XPrQuat;
        Quaternion SWgdaQuat;
        Quaternion SWXPrQuat, SWXPrQuat2;
        Quaternion targetQuat;
        float samplePeriod = 0.05f;
        Vector3 Acceleration, Magnetometer, refAcceleration = new Vector3(0, 0, -1), refMagnetometer = new Vector3(5000,0, -10000), Gyroscope = new Vector3(0, 0, 0);
        public Form2 Form2a, Form2b, Form2c, Form2d;
        Thread oThread;
        NU parent;
        List<Rectangle> OriginalPosition = new List<Rectangle>() { }; // Stores LocationX, LocationY, SizeX, SizeY
        Size OriginalFormSize;
        public Form1(NU _parent)
        {
            parent = _parent;
            InitializeComponent();
            var controls = this.Controls.Cast<Control>();
            foreach (Control control in controls)
            {
                OriginalPosition.Add(new Rectangle(control.Location.X, control.Location.Y, control.Size.Width, control.Size.Height));
            }
            OriginalFormSize = new Size(ClientRectangle.Size.Width, ClientRectangle.Size.Height);
        }
        public void OpenForms()
        {
            Form2a = initForm(HostPanel1);
            Form2b = initForm(HostPanel2);
            Form2c = initForm(HostPanel3);
            Form2d = initForm(HostPanel4);

            OnResize(new EventArgs());
            oThread = new Thread(() => refreshHP2(this));
            oThread.Start();

        }

        Form2 initForm(Panel hostpanel)
        {
            Form2 form = new Form2()
            {
                TopLevel = false,
                Visible = true,
                FormBorderStyle = FormBorderStyle.None
            };

            form.Init(true);
            while (hostpanel.Controls.Count > 0) hostpanel.Controls[0].Dispose();
            hostpanel.Controls.Add(form);
            return form;
        }

        protected override void OnResize(EventArgs e)
        {
            if (Form2a != null && Form2b != null)
            {
                var controls = this.Controls.Cast<Control>();
                int counter = 0;
                foreach (Control control in controls)
                {
                    control.Location = new System.Drawing.Point((OriginalPosition[counter].X * ClientRectangle.Width) / OriginalFormSize.Width,
                                                               (OriginalPosition[counter].Y * ClientRectangle.Height) / OriginalFormSize.Height);
                    control.Size = new System.Drawing.Size((OriginalPosition[counter].Width * ClientRectangle.Width) / OriginalFormSize.Width,
                        (OriginalPosition[counter].Height * ClientRectangle.Height) / OriginalFormSize.Height);
                    counter++;

                }
                ResizeFont(Controls, ((float)ClientRectangle.Height) / (float)(OriginalFormSize.Height));
                Form2a.resize(HostPanel1.Size.Height, HostPanel1.Size.Width);
                Form2b.resize(HostPanel2.Size.Height, HostPanel2.Size.Width);
                Form2c.resize(HostPanel3.Size.Height, HostPanel3.Size.Width);
                Form2d.resize(HostPanel4.Size.Height, HostPanel4.Size.Width);
                /*
                int positionX = HostPanel1.Location.X;
                int positionY = HostPanel1.Location.Y;
                this.HostPanel1.Location = new System.Drawing.Point(positionX, positionY);
                this.HostPanel1.Size = new System.Drawing.Size((this.Width * 485) / 750, ((this.ClientRectangle.Height) - (3* positionY)) / 2);
                this.HostPanel2.Location = new System.Drawing.Point(positionX, (((this.ClientRectangle.Height) - (3 * positionY)) / 2) + positionY*2);

                this.HostPanel2.Size = new System.Drawing.Size((this.Width * 485) / 750, ((((this.ClientRectangle.Height) + 8) - (3 * positionY)) / 2));
                Form2a.resize(((this.ClientRectangle.Height) - (3 * positionY)) / 2,(this.Width * 485) / 750);
                Form2b.resize(((this.ClientRectangle.Height) - (3 * positionY)) / 2, (this.Width * 485) / 750);
                */
            }
        }
        private void ResizeFont(Control.ControlCollection coll, float scaleFactor)
        {
            foreach (Control c in coll)
            {
                if (c.HasChildren)
                {
                    ResizeFont(c.Controls, scaleFactor);
                }
                else
                {
                    //if (c.GetType().ToString() == "System.Windows.Form.Label")
                    if (true)
                    {
                        // scale font
                        c.Font = new Font(c.Font.FontFamily.Name, 8 * scaleFactor);
                    }
                }
            }
        }

        private void refreshHP2(Form1 form1)
        {
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(20);
                    try
                    {
                        Invoke(new Action(() => form1.HostPanel1.Refresh()));
                        Invoke(new Action(() => form1.HostPanel2.Refresh()));
                    }
                    catch (Exception)
                    {
                        oThread.Abort();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parent.quatSix = parent.quat;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parent.cali.caliCountDown = 100;
            parent.quat = new Quaternion(0, 0, 0, 1);
            parent.quatSix = new Quaternion(0, 0, 0, 1);
            parent.quatNine = new Quaternion(0, 0, 0, 1);
            parent.MAquatNine = new Quaternion(0, 0, 0, 1);
            parent.SWquatNine = new Quaternion(0, 0, 0, 1);
            parent.cali.calibrated = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            gdaQuat = ahrs.Update9DoF(Gyroscope, Acceleration, Magnetometer, gdaQuat, samplePeriod);
            SWgdaQuat = ahrs.SW_Update9DoF(Gyroscope, Acceleration, Magnetometer, SWgdaQuat, samplePeriod);
            XPrQuat = ahrs.SebX_Update9DoF(Gyroscope, Acceleration, Magnetometer, XPrQuat, samplePeriod);
            SWXPrQuat = ahrs.SWX_Update9DoF(Gyroscope, Acceleration, Magnetometer, SWXPrQuat, samplePeriod);
            

            Form2a.setAllValues((new Quaternion(1, 0, 0, 0) * gdaQuat), Acceleration, Magnetometer);
            Form2b.setAllValues(SWgdaQuat, Acceleration, Magnetometer);
            Form2c.setAllValues((new Quaternion(1, 0, 0, 0) * XPrQuat), Acceleration, Magnetometer);
            Form2d.setAllValues(SWXPrQuat, Acceleration, Magnetometer);
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            gdaQuat = new Quaternion(-1, 0, 0, 0).Inverted();
            XPrQuat = new Quaternion(-1, 0, 0, 0).Inverted();
            SWgdaQuat = new Quaternion(0, 0, 0, 1);
            SWXPrQuat = new Quaternion(0, 0, 0, 1);
            if (timer1.Enabled) { timer1.Enabled = false; } else { timer1.Enabled = true; }
            Random rnd = new Random();
            targetQuat = Quaternion.Normalize(new Quaternion(rnd.Next(-1000, 1000), rnd.Next(-1000, 1000), rnd.Next(-1000, 1000), rnd.Next(-1000, 1000)));
            //targetQuat = new Quaternion(new Vector3(179 * MathExtensions.Deg2Radf, -83 * MathExtensions.Deg2Radf, 153 * MathExtensions.Deg2Radf));
            Acceleration = targetQuat.Inverted() * refAcceleration;
            Magnetometer = targetQuat.Inverted() * refMagnetometer;
            /*Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000000; i++)
            {
                ahrs.SWX_Update9DoF(Gyroscope, Acceleration, Magnetometer, gdaQuat, samplePeriod);
            }
            sw.Stop();
            long SWXProd = sw.ElapsedMilliseconds;

            sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000000; i++)
            {
                ahrs.SebX_Update9DoF(Gyroscope, Acceleration, Magnetometer, XPrQuat, samplePeriod);
            }
            sw.Stop();
            long XProd = sw.ElapsedMilliseconds;
            sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000000; i++)
            {
                ahrs.MA_Update9DoF(Gyroscope, Acceleration, Magnetometer, gdaQuat, samplePeriod);
            }
            sw.Stop();
            long Orig = sw.ElapsedMilliseconds;
            sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 1000000; i++)
            {
                ahrs.SW_Update9DoF(Gyroscope, Acceleration, Magnetometer, gdaQuat, samplePeriod);
            }
            sw.Stop();
            long SWGDA = sw.ElapsedMilliseconds;

            
            int a = 0;
            long[] ar = new long[] { SWGDA, Orig,XProd,SWXProd};*/
        }
    }
}
