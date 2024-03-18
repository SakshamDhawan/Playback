using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Math;

namespace MMGProcessor
{

    public partial class Form1 : Form
    {
        GraphForm Form4;
        MMGProcessorClass MMGProcessor = new MMGProcessorClass();
        List<int> list = new List<int>();
        int original_count = 0;

        public Form1()
        {
            List<int> raw = new List<int>();
            InitializeComponent();
            MMGProcessor.initialise(400, 4, 9, 1f, 40, 1);
            string whole_file = System.IO.File.ReadAllText("C: \\Users\\SW7814\\Google Drive\\Code\\Imperial College London\\Visual Studio 2015\\Projects\\WindowsFormsApplication2\\DataFiles\\test3.csv");
            //string whole_file = System.IO.File.ReadAllText("C:\\Users\\Sam\\Google Drive\\Code\\Imperial College London\\Visual Studio 2015\\Projects\\WindowsFormsApplication2\\DataFiles\\test.csv");
            richTextBox1.AppendText(whole_file);

            Form4 = new GraphForm();
            Form4.Graph();
            Form4.Show();
            String hold = null;
            int j;
            foreach (char c in whole_file)
            {
                if((c!= '\r')&&(c!='\n')&&(c!=44))
                {
                    hold = hold + c.ToString();
                }
                else
                {
                    if ((c == '\r')| (c == 44))
                    {
                        Int32.TryParse(hold, out j);
                        raw.Add(j);
                    }
                    hold = null;
                }
            }

            for(int count = 22; count<=raw.Count();count=count+35)
            {
                list.Add(((raw[count]*256)+raw[count+1]));
            }

                int wait = 0;
            original_count = list.Count();
            Form4.Show();
            timer1.Enabled = true;
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        { timer1.Enabled = false;
            while (list.Count >= 1)
            {
                if (MMGProcessor.AddData(list.GetRange(0, 1)))
                {
                    richTextBox1.AppendText((((original_count - list.Count)*41/ original_count).ToString())+" ");
                }
                else
                {
                }
                list.RemoveRange(0, 1);
            }
            
            {
                timer1.Enabled = false;
                MMGProcessor.outputData();
            }
        }
    }
}
