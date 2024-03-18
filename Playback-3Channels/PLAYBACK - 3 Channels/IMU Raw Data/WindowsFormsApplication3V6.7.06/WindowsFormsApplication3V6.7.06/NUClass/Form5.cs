using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using OpenTK;

namespace NUClass
{
    public partial class Form5 : Form
    {
        public Form2 Form2;
        public GraphForm Form4b;
        public GraphForm Form4;
        Thread oThread;
        int showing = 0;
        public static NU NUClass;
        public static Form5 _Form5;
        int testNum = 5;
        int IMUno;
        int[,] confusion;
        string testText;
        public bool test = false;
        public int CurrentGestures = 0;
        public int R2G = 255;
        public int R2G2 = 255;
        public int currentGesture = -1;
        public List<int> testNumbers;
        List<string> Names;
        int[,] breakdown;
        System.Timers.Timer UsefulTimer;
        bool record = false;
        public bool Computer_control = false;
        NU parent;
        public int GestureStoreCount = 0;

        public Form5(int _IMUno, NU _parent)
        {
            parent = _parent;
            IMUno = _IMUno;
            InitializeComponent();
            richTextBox1.AppendText(IMUno.ToString() + "\n");
            _Form5 = this;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form5_Closing);
            setModeVis(false);
        }

        public void add2conf(int predicted)
        {
            if (record)
            {
                if ((currentGesture > -1) && (currentGesture < (testNum * Names.Count)))
                {
                    confusion[testNumbers[currentGesture], predicted] = confusion[testNumbers[currentGesture], predicted] + 1;
                    breakdown[currentGesture, 0] = currentGesture;
                    breakdown[currentGesture, 1] = testNumbers[currentGesture];
                    breakdown[currentGesture, predicted + 2] = breakdown[currentGesture, predicted + 2] + 1;
                }
                if (testNumbers[currentGesture] == predicted)
                {
                    appendText("\nGesture identified correctly");
                }
                else
                {
                    appendText("\nGesture identified incorrectly");
                }
                record = false;
            }

        }
        public void beginTest(List<string> _Names)
        {
            Names = _Names;
            
            if (richTextBox1.Text.Contains("Test Completed"))
            {
                int index = richTextBox1.Text.LastIndexOf("Test Completed");

                richTextBox1.Text = richTextBox1.Text.Substring(0, index - 2);
            }
            testNumbers = new List<int>() { };
            confusion = new int[(Names.Count), (Names.Count)];
            for (int i = 0; i < Names.Count(); i++)
            {
                for (int j = 0; j < testNum; j++)
                {
                    testNumbers.Add(i);
                }
            }

            breakdown = new int[testNumbers.Count, 2 + Names.Count];
            if (checkBox1.Checked)
            {
                testNumbers.Shuffle();
            }
            testText = richTextBox1.Text;
            timer2.Enabled = true;
            test = true;


        }
        public void setRecMode(bool vis)
        {

        }
        public void setModeVis(bool vis)
        {
            button1.Visible = vis;
            button1.IsAccessible = vis;
            button2.Visible = vis;
            button2.IsAccessible = vis;
            button3.Visible = vis;
            button3.IsAccessible = vis;
            button4.Visible = vis;
            button4.IsAccessible = vis;
            button5.Visible = vis;
            button5.IsAccessible = vis;
            textBox1.Visible = vis;
            textBox1.IsAccessible = vis;
            checkBox1.Visible = vis;
            checkBox1.IsAccessible = vis;
            btnSaveGestureTemplate.Visible = vis;
            btnSaveGestureTemplate.IsAccessible = vis;
            tbUserName.Visible = vis;
            tbUserName.IsAccessible = vis;
            btnReadPreviousData.Visible = vis;
            btnReadPreviousData.IsAccessible = vis;
            Use_IR.Visible = vis;
            Use_IR.IsAccessible = vis;
            Sky_control.Visible = vis;
            Sky_control.IsAccessible = vis;
            ComputerContrl.Visible = vis;
            ComputerContrl.IsAccessible = vis;
            GestureTrigger.Visible = vis;
            GestureTrigger.IsAccessible = vis;


            label1.Visible = vis;
            label1.IsAccessible = vis;
            label2.Visible = vis;
            label2.IsAccessible = vis;
            pictureBox2.Visible = vis;
            pictureBox2.IsAccessible = vis;
            pictureBox1.Visible = vis;
            pictureBox1.IsAccessible = vis;
            textBox2.Visible = vis;
            textBox2.IsAccessible = vis;
            textBox3.Visible = vis;
            textBox3.IsAccessible = vis;
            SaveClassifierData.Visible = vis;
            SaveClassifierData.IsAccessible = vis;
            button7.Visible = vis;
            button7.IsAccessible = vis;
            button6.Visible = vis;
            button6.IsAccessible = vis;



        }
        public void appendText(string text)
        {
            try
            {
                Invoke(new Action(() => richTextBox1.AppendText(text)));
            }
            catch (Exception)
            {

            }
        }

        public void WriteButton4(string text)
        {
            Invoke(new Action(() => button4.Text = text));
        }

        public void WriteText(string text)
        {
            try
            {
                Invoke(new Action(() => richTextBox1.Text = ""));

                Invoke(new Action(() => richTextBox1.AppendText("NU: " + IMUno.ToString() + "\n")));
                Invoke(new Action(() => richTextBox1.AppendText(text)));
            }
            catch (Exception)
            {

            };
        }


        protected override void OnResize(EventArgs e)
        {
            if (Form2 != null)
            {

                this.HostPanel1.Location = new System.Drawing.Point((this.Width * 12) / 701, (this.Height * 12) / 441);
                this.HostPanel2.Location = new System.Drawing.Point((this.Width * 12) / 701, (this.Height * 120) / 441);
                this.HostPanel3.Location = new System.Drawing.Point((this.Width * 350) / 701, (this.Height * 209) / 441);
                this.richTextBox1.Location = new System.Drawing.Point((this.Width * 350) / 701, (this.Height * 12) / 441);
                this.HostPanel1.Size = new System.Drawing.Size((this.Width * (350-100)) / 701, (this.Height * 190/2) / 441);
                this.HostPanel2.Size = new System.Drawing.Size((this.Width * 350*2) / 701, (this.Height * (190+100)) / 441);
                this.HostPanel3.Size = new System.Drawing.Size((this.Width * 323) / 701, (this.Height * 182) / 441);
                this.richTextBox1.Size = new System.Drawing.Size((this.Width * 323) / 701, (this.Height * 182) / 441);
                Form2.resize((this.Height * 182/2) / 441, (this.Width * (323-100)) / 701);
                Form4.resize((this.Height * (182+100)) / 441, (this.Width * 323*2) / 701);
                Form4b.resize((this.Height * 182) / 441, (this.Width * 323) / 701);
            }
        }
        private void Form5_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NU.NUClass.softStopNU();
            NU.NUClass.saveData = false;
            NU.NUClass.Kat = false;
        }
        public void addButtons(NU _NU)
        {
            NUClass = _NU;
        }
        public void OpenForms()
        {
            Form2 = new Form2()
            {
                TopLevel = false,
                Visible = true,
                FormBorderStyle = FormBorderStyle.None
            };

            Form2.Init(true);
            while (HostPanel1.Controls.Count > 0) HostPanel1.Controls[0].Dispose();
            HostPanel1.Controls.Add(Form2);



            Form4b = new GraphForm()
            {
                TopLevel = false,
                Visible = true,
                FormBorderStyle = FormBorderStyle.None
            };

            Form4b.Graph(8, true);
            while (HostPanel3.Controls.Count > 0) HostPanel3.Controls[0].Dispose();
            HostPanel3.Controls.Add(Form4b);


            Form4 = new GraphForm()
            {
                TopLevel = false,
                Visible = true,
                FormBorderStyle = FormBorderStyle.None
            };

            Form4.Graph(8, true);
            while (HostPanel2.Controls.Count > 0) HostPanel2.Controls[0].Dispose();
            HostPanel2.Controls.Add(Form4);

            oThread = new Thread(() => refreshHP2(this));
            oThread.Start();

        }



        private void refreshHP2(Form5 form5)
        {
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(20);
                    try
                    {
                        Invoke(new Action(() => form5.HostPanel1.Refresh()));
                        Invoke(new Action(() => form5.HostPanel2.Refresh()));
                        Invoke(new Action(() => form5.HostPanel3.Refresh()));
                    }
                    catch (Exception)
                    {
                        oThread.Abort();
                    }
                }
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void WriteVarToFile()
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + "..\\..\\..\\DataFiles\\Raw Data\\MMGDebug-" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-fff", CultureInfo.InvariantCulture) + ".csv");
            for (int i = 0; i < testNumbers.Count; i++)
            {
                for (int j = 0; j < Names.Count() + 2; j++)
                {
                    file.Write(breakdown[i, j] + ",");
                }
                file.Write("\n");
            }
            file.Write("\n");
            file.Write("\n");


            for (int j = 0; j < Names.Count(); j++)
            {

                for (int i = 0; i < Names.Count(); i++)
                {
                    file.Write(confusion[j, i] + ",");
                }
                file.Write("\n");
            }

            file.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                NUClass.MMGButtonAdd(textBox1.Text);
            }
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NUClass.Form5Next();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Computer_control = false;
            NUClass.Form5Training();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "Record")
            {
                NUClass.Form5Record(true, 0);
                button4.Text = "Stop";
                GestureStoreCount = GestureStoreCount + 5;
                label2.Text = GestureStoreCount.ToString();
            }
            else
            {
                for(int i = 0; i <= 10; i++)
                {
                    NUClass.sendData("M0dg");
                }
                button4.Text = "Record";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NUClass.Form5Test();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (currentGesture < testNumbers.Count)
            {
                record = true;
                richTextBox1.Text = testText + "\n" + Names[testNumbers[currentGesture]] + "\n" + (testNumbers.Count - currentGesture);
                timer1.Enabled = false;
                timer2.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
                timer2.Enabled = false;
                test = false;
                richTextBox1.Text = testText + "\nTest Completed";
                NUClass.Form5End();
                currentGesture = -1;
                int error = 0;
                for (int i = 0; i < testNumbers.Count; i++)
                {
                    int GesturesNumbers = 0;
                    for (int j = 0; j < Names.Count(); j++)
                    {
                        GesturesNumbers = GesturesNumbers + breakdown[i, j + 2];
                    }
                    if (GesturesNumbers != 1)
                    {
                        error = error + 1;
                    }
                }
                int Class = 0;
                for (int j = 0; j < Names.Count(); j++)
                {
                    Class = Class + confusion[j, j];
                }

                richTextBox1.AppendText("\nSegmentation Accuracy = " + (((double)testNumbers.Count - (double)error) / (double)testNumbers.Count) * (double)100);
                richTextBox1.AppendText("\nClassification Accuracy = " + ((double)Class / ((double)testNumbers.Count - (double)error)) * (double)100);

                WriteVarToFile();

            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            richTextBox1.Text = testText;
            record = false;
            currentGesture++;
            timer2.Enabled = false;
            timer1.Enabled = true;
        }

        private void btnSaveGestureTemplate_Click(object sender, EventArgs e)
        {
            NUClass.Form5RecordCurrentTemplate(tbUserName.Text, CurrentGestures);
        }

        private void btnReadPreviousData_Click(object sender, EventArgs e)
        {
            NUClass.Form5LoadPreviousTemplate(tbUserName.Text, CurrentGestures);
        }

        private void ComputerContrl_Click(object sender, EventArgs e)
        {
            Computer_control = true;
            NUClass.Form5ComputerControl();
        }

        private void Use_IR_CheckedChanged(object sender, EventArgs e)
        {
            if (Use_IR.Checked == true)
            {
                NUClass.IRGestureUsed('l');
            }
            if (Use_IR.Checked == false)
            {
                NUClass.IRGestureUsed('l');
            }
        }

        private void timer_Elapsed(object sender, EventArgs e)
        {
            NUClass.select();
            UsefulTimer.Stop();
            UsefulTimer.Dispose();
            timer3.Stop();
        }

        public void timerReset()
        {
            if (UsefulTimer != null)
            {
                UsefulTimer.Dispose();
            }
            UsefulTimer = new System.Timers.Timer(5000);
            UsefulTimer.Enabled = true;
            UsefulTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            UsefulTimer.Start();
            timer3.Interval = 5000;
            timer3.Start();
        }

        private void Sky_control_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            parent.cali.caliCountDown = 100;
            parent.quat = new Quaternion(0, 0, 0, 1);
            parent.cali.calibrated = false;
        }

        private void GestureTrigger_Click(object sender, EventArgs e)
        {
            parent.gesture = 1;
        }

        private void SaveClassifierData_Click(object sender, EventArgs e)
        {
            parent.SaveClassifierData(textBox2.Text,textBox3.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            switch (button7.Text)
            {
                case "Process Mode 0":
                    button7.Text = "Process Mode 1";
                    parent.MMGRec.MMGClassifierMode = 1;
                    break;
                case "Process Mode 1":
                    button7.Text = "Process Mode 2";
                    parent.MMGRec.MMGClassifierMode = 2;
                    break;
                case "Process Mode 2":
                    button7.Text = "Process Mode 0";
                    parent.MMGRec.MMGClassifierMode = 0;
                    break;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;


            for (int i = 0; i < 96; i++)
            {

                SolidBrush myPen;
                myPen = new SolidBrush(Color.FromArgb(255, 255 - R2G, (R2G), 0));
                graphics.FillEllipse(myPen, 0, 0, 25, 25);
                myPen.Dispose();
            }
            
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;


            for (int i = 0; i < 96; i++)
            {

                SolidBrush myPen;
                myPen = new SolidBrush(Color.FromArgb(255, 255 - R2G2, (R2G2), 0));
                graphics.FillEllipse(myPen, 0, 0, 25, 25);
                myPen.Dispose();
            }

        }
        private void timer4_Tick(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
            pictureBox1.Update();
            pictureBox2.Invalidate();
            pictureBox2.Update();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
    public static class ListExtensions
    {
        private static Random rng = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
