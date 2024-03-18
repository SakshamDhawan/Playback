using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NUClass
{
    public partial class Form8 : Form
    {
        int Firmware_version;
        bool Fake;
        NU parent;
        ~Form8()
        {
            this.Dispose();

        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {

            parent.form8 = null;
        }
        public void ResetPage2()
        {
            for (int i = 0; i <= 2; i++){comboBox2.Items.RemoveAt(0);}
            for (int i = 0; i <= 15; i++){comboBox3.Items.RemoveAt(0); }
            for (int i = 0; i <= 4; i++) { comboBox4.Items.RemoveAt(0); }
            for (int i = 0; i <= 9; i++) { comboBox5.Items.RemoveAt(0); }
            for (int i = 0; i <= 3; i++) { comboBox6.Items.RemoveAt(0); }
            for (int i = 0; i <= 5; i++) { comboBox7.Items.RemoveAt(0); }

            comboBox2.Items.Add("G_SCALE_245DPS");
            comboBox2.Items.Add("G_SCALE_500DPS");
            comboBox2.Items.Add("Not Used");
            comboBox2.Items.Add("G_SCALE_2000DPS");


            comboBox3.Items.Add("Off");
            comboBox3.Items.Add("G_ODR_14.9");
            comboBox3.Items.Add("G_ODR_59.5");
            comboBox3.Items.Add("G_ODR_119");
            comboBox3.Items.Add("G_ODR_238");
            comboBox3.Items.Add("G_ODR_476");
            comboBox3.Items.Add("G_ODR_952");


            comboBox4.Items.Add("A_SCALE_2G");
            comboBox4.Items.Add("A_SCALE_16G");
            comboBox4.Items.Add("A_SCALE_4G");
            comboBox4.Items.Add("A_SCALE_8G");

            comboBox5.Items.Add("A_POWER_DOWN");
            comboBox5.Items.Add("A_ODR_10");
            comboBox5.Items.Add("A_ODR_50");
            comboBox5.Items.Add("A_ODR_119");
            comboBox5.Items.Add("A_ODR_238");
            comboBox5.Items.Add("A_ODR_476");
            comboBox5.Items.Add("A_952");

            comboBox6.Items.Add("M_SCALE_4GS");
            comboBox6.Items.Add("M_SCALE_8GS");
            comboBox6.Items.Add("M_SCALE_12GS");
            comboBox6.Items.Add("M_SCALE_16GS");

;
            comboBox7.Items.Add("M_ODR_0.625");
            comboBox7.Items.Add("M_ODR_1.25");
            comboBox7.Items.Add("M_ODR_2.5");
            comboBox7.Items.Add("M_ODR_5");
            comboBox7.Items.Add("M_ODR_10");
            comboBox7.Items.Add("M_ODR_20");
            comboBox7.Items.Add("M_ODR_40");
            comboBox7.Items.Add("M_ODR_80");
        }
        public Form8(bool _Fake, NU _parent)
        {
            Fake = _Fake;
            parent = _parent;
            InitializeComponent();
            Firmware_version = 5;
            comboBox2.Items.Add("G_SCALE_245DPS");
            comboBox2.Items.Add("G_SCALE_500DPS");
            comboBox2.Items.Add("G_SCALE_2000DPS");


            comboBox3.Items.Add("G_ODR_95_BW_125");
            comboBox3.Items.Add("G_ODR_95_BW_25");
            comboBox3.Items.Add("Not Used");
            comboBox3.Items.Add("Not Used");
            comboBox3.Items.Add("G_ODR_190_BW_125");
            comboBox3.Items.Add("G_ODR_190_BW_25");
            comboBox3.Items.Add("G_ODR_190_BW_50");
            comboBox3.Items.Add("G_ODR_190_BW_70");
            comboBox3.Items.Add("G_ODR_380_BW_20");
            comboBox3.Items.Add("G_ODR_380_BW_25");
            comboBox3.Items.Add("G_ODR_380_BW_50");
            comboBox3.Items.Add("G_ODR_380_BW_100");
            comboBox3.Items.Add("G_ODR_760_BW_30");
            comboBox3.Items.Add("G_ODR_760_BW_35");
            comboBox3.Items.Add("G_ODR_760_BW_50");
            comboBox3.Items.Add("G_ODR_760_BW_100");


            comboBox4.Items.Add("A_SCALE_2G");
            comboBox4.Items.Add("A_SCALE_4G");
            comboBox4.Items.Add("A_SCALE_6G");
            comboBox4.Items.Add("A_SCALE_8G");
            comboBox4.Items.Add("A_SCALE_16G");


            comboBox5.Items.Add("A_POWER_DOWN");
            comboBox5.Items.Add("A_ODR_3125");
            comboBox5.Items.Add("A_ODR_625");
            comboBox5.Items.Add("A_ODR_125");
            comboBox5.Items.Add("A_ODR_25");
            comboBox5.Items.Add("A_ODR_50");
            comboBox5.Items.Add("A_ODR_100");
            comboBox5.Items.Add("A_ODR_200");
            comboBox5.Items.Add("A_ODR_400");
            comboBox5.Items.Add("A_ODR_1600");


            comboBox6.Items.Add("M_SCALE_2GS");
            comboBox6.Items.Add("M_SCALE_4GS");
            comboBox6.Items.Add("M_SCALE_8GS");
            comboBox6.Items.Add("M_SCALE_12GS");


            comboBox7.Items.Add("M_ODR_3125");
            comboBox7.Items.Add("M_ODR_625");
            comboBox7.Items.Add("M_ODR_125");
            comboBox7.Items.Add("M_ODR_25");
            comboBox7.Items.Add("M_ODR_50");
            comboBox7.Items.Add("M_ODR_100");
        }
        public void pickFreq(int Freq, int current, int freq)
        {
            Invoke(new Action(() => comboBox1.Items.Clear()));
            int[] divisor = new int[] {1,2,5,10,20,50,100, 200, 500, 1000, 2000, 5000, 10000, };
            int counter = 0, hold = 10000, selectedIndex = 0;
            while (hold >= 2)
            {
                hold = (Freq / divisor[counter]);
                if(divisor[counter] == freq+1)
                {
                    selectedIndex = counter;

                }
                counter++;
                Invoke(new Action(() => comboBox1.Items.Add(hold.ToString() + "Hz")));
            }

            Invoke(new Action(() => comboBox1.SelectedIndex = selectedIndex));

        }
        void SetPage2(int gscale, int grate, int ascale, int arate, int mscale, int mrate)
        {
            Invoke(new Action(() => comboBox2.SelectedIndex = gscale));
            Invoke(new Action(() => comboBox3.SelectedIndex = grate));
            Invoke(new Action(() => comboBox4.SelectedIndex = ascale));
            Invoke(new Action(() => comboBox5.SelectedIndex = arate));
            Invoke(new Action(() => comboBox6.SelectedIndex = mscale));
            Invoke(new Action(() => comboBox7.SelectedIndex = mrate));
        }
        int intTo16int(int int32)
        {
            if (int32 >= 32768)
            {
                return (int32 - 65536);
            }
            else
            {
                return int32;
            }
        }
        void SetPage3(bool set, int MagX, int MagY, int MagZ, int AccX, int AccY, int AccZ, int GyroX, int GyroY, int GyroZ)
        {
            if (set)
            {
                Invoke(new Action(() => textBox2.Text = intTo16int(MagX).ToString()));
                Invoke(new Action(() => textBox3.Text = intTo16int(MagY).ToString()));
                Invoke(new Action(() => textBox4.Text = intTo16int(MagZ).ToString()));
                Invoke(new Action(() => textBox1.Text = intTo16int(AccX).ToString()));
                Invoke(new Action(() => textBox6.Text = intTo16int(AccY).ToString()));
                Invoke(new Action(() => textBox5.Text = intTo16int(AccZ).ToString()));
                Invoke(new Action(() => textBox7.Text = intTo16int(GyroX).ToString()));
                Invoke(new Action(() => textBox8.Text = intTo16int(GyroY).ToString()));
                Invoke(new Action(() => textBox9.Text = intTo16int(GyroZ).ToString()));
            }else{
                Invoke(new Action(() => textBox2.Text = "NA"));
                Invoke(new Action(() => textBox3.Text = "NA"));
                Invoke(new Action(() => textBox4.Text = "NA"));
                Invoke(new Action(() => textBox1.Text = "NA"));
                Invoke(new Action(() => textBox6.Text = "NA"));
                Invoke(new Action(() => textBox5.Text = "NA"));
                Invoke(new Action(() => textBox7.Text = "NA"));
                Invoke(new Action(() => textBox8.Text = "NA"));
                Invoke(new Action(() => textBox9.Text = "NA"));
            }
        }
        public void setdata(int PERIOD, int FREQ, int ADC, int PS, int gscale, int grate, int ascale, int arate, int mscale, int mrate, int firmware)
        {
            Firmware_version = firmware;
            try { Invoke(new Action(() => richTextBox1.AppendText(getFrequency(PS, PERIOD).ToString()))); }
            catch (Exception) { };
            pickFreq(getFrequency(PS, PERIOD), (getFrequency(PS, PERIOD) * FREQ), FREQ);

            setADC(ADC);
            if (firmware >= 10)
            {
                ResetPage2();
                SetPage2(gscale, grate, ascale, arate, mscale, mrate);
            }
            else
            {
                SetPage2(gscale, grate, ascale, arate, mscale, mrate);
            }
            SetPage3(false,  0,  0,  0,  0,  0, 0, 0,  0,  0);

        }
        public void setFakeData()
        {
            tabControl1.SelectTab(2);
        }
        public void setdata(int PERIOD, int FREQ, int ADC, int PS, int gscale, int grate, int ascale, int arate, int mscale, int mrate, int MagX, int MagY, int MagZ, int AccX, int AccY, int AccZ, int GyroX, int GyroY, int GyroZ, int firmware)
        {
            Firmware_version = firmware;
            try { Invoke(new Action(() => richTextBox1.AppendText(getFrequency(PS, PERIOD).ToString()))); }
            catch (Exception) { };
            pickFreq(getFrequency(PS, PERIOD), (getFrequency(PS, PERIOD) * FREQ), FREQ);

            setADC(ADC);
            if (firmware >= 10)
            {
                ResetPage2();
                SetPage2(gscale, grate, ascale, arate, mscale, mrate);
            }
            else
            {
                SetPage2(gscale, grate, ascale, arate, mscale, mrate);
            }
            SetPage3(true, MagX, MagY, MagZ, AccX, AccY, AccZ, GyroX, GyroY, GyroZ);
        }

        void setADC(int ADC)
        {

            if ((ADC & 128) != 0)
            {
                Invoke(new Action(() => checkBox1.Checked = true));
            }
            else
            {
                Invoke(new Action(() => checkBox1.Checked = false));
            }
            if ((ADC & 64) != 0)
            {
                Invoke(new Action(() => checkBox2.Checked = true));
            }
            else
            {
                Invoke(new Action(() => checkBox2.Checked = false));
            }
            if ((ADC & 32) != 0)
            {
                Invoke(new Action(() => checkBox3.Checked = true));
            }
            else
            {
                Invoke(new Action(() => checkBox3.Checked = false));
            }
            if ((ADC & 16) != 0)
            {
                Invoke(new Action(() => checkBox4.Checked = true));
            }
            else
            {
                Invoke(new Action(() => checkBox4.Checked = false));
            }
            if ((ADC & 8) != 0)
            {
                Invoke(new Action(() => checkBox5.Checked = true));
            }
            else
            {
                Invoke(new Action(() => checkBox5.Checked = false));
            }
            if ((ADC & 4) != 0)
            {
                Invoke(new Action(() => checkBox6.Checked = true));
            }
            else
            {
                Invoke(new Action(() => checkBox6.Checked = false));
            }
            if ((ADC & 2) != 0)
            {
                Invoke(new Action(() => checkBox7.Checked = true));
            }
            else
            {
                Invoke(new Action(() => checkBox7.Checked = false));
            }
            if ((ADC & 1) != 0)
            {
                Invoke(new Action(() => checkBox8.Checked = true));
            }
            else
            {
                Invoke(new Action(() => checkBox8.Checked = false));
            }
        }
        int getFrequency(int ps, int period)
        {
            int frequency;
            int[] prescaler = { 1, 8, 64, 256 };
            int clock = 30000000;
            frequency = (clock) / (period * (prescaler[ps] * 2));

            return frequency;
        }
        int[] setFrequency(int frequency)
        {
            int[] variables = new int[2] { 65536, 0 };
            int ps = 0;

            int[] prescaler = { 1, 8, 64, 256 };
            int clock = 30000000;
            while ((variables[0] >= 65536) && (variables[1] <= 4))
            {
                variables[0] = (clock) / (frequency * prescaler[variables[1]] * 2);
                variables[1]++;
            }

            if (ps == 5)
            {
                return null;
            }
            variables[1]--;
            return variables;

        }
        int toInt(string var)
        {
            int n;
            int.TryParse(var, out n);
            return n;
        }
        private void textBox10_DragDrop(object sender,
System.Windows.Forms.DragEventArgs e)
        {
            int i;
            String s;
            s = e.Data.GetData(DataFormats.Text).ToString();
            var result = Regex.Split(s, "\r\n|\r|\n");
            foreach(string line in result)
            {
                if (line.Contains("MagX"))
                {
                    int n;
                    string sub = line.Substring(line.IndexOf('=')+1, line.Length - (line.IndexOf('=')+1));
                    int.TryParse(sub, out n);
                    textBox2.Text = n.ToString();
                }
                if (line.Contains("MagY"))
                {
                    int n;
                    string sub = line.Substring(line.IndexOf('=') + 1, line.Length - (line.IndexOf('=') + 1));
                    int.TryParse(sub, out n);
                    textBox3.Text = n.ToString();
                }
                if (line.Contains("MagZ"))
                {
                    int n;
                    string sub = line.Substring(line.IndexOf('=') + 1, line.Length - (line.IndexOf('=') + 1));
                    int.TryParse(sub, out n);
                    textBox4.Text = n.ToString();
                }
                if (line.Contains("AccelX"))
                {
                    int n;
                    string sub = line.Substring(line.IndexOf('=') + 1, line.Length - (line.IndexOf('=') + 1));
                    int.TryParse(sub, out n);
                    textBox1.Text = n.ToString();
                }
                if (line.Contains("AccelY"))
                {
                    int n;
                    string sub = line.Substring(line.IndexOf('=') + 1, line.Length - (line.IndexOf('=') + 1));
                    int.TryParse(sub, out n);
                    textBox6.Text = n.ToString();
                }
                if (line.Contains("AccelZ"))
                {
                    int n;
                    string sub = line.Substring(line.IndexOf('=') + 1, line.Length - (line.IndexOf('=') + 1));
                    int.TryParse(sub, out n);
                    textBox5.Text = n.ToString();
                }
            }
       }

        private void textBox10_DragEnter(object sender,
        System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void button1_Click(object sender, EventArgs e)
        {

            int n;
            int[] divisor = new int[] { 1, 2, 5, 10, 20, 50, 100, 200, 500, 1000 };
            int[] returnVariable = new int[21];
            if (int.TryParse(richTextBox1.Text, out n))
            {
                if (n >= 1 && n <= 1000)
                {
                    int newFreq = n;
                }
                else
                {
                    
                }
            }
            if (setFrequency(n) != null)
            {
                int[] variables = setFrequency(n);
                returnVariable[0] = variables[0];
                returnVariable[1] = divisor[comboBox1.SelectedIndex] - 1;
                returnVariable[2] = GetADC();
                returnVariable[3] = variables[1];
                returnVariable[4] = comboBox2.SelectedIndex;
                returnVariable[5] = comboBox3.SelectedIndex;
                returnVariable[6] = comboBox4.SelectedIndex;
                returnVariable[7] = comboBox5.SelectedIndex;
                returnVariable[8] = comboBox6.SelectedIndex;
                returnVariable[9] = comboBox7.SelectedIndex;
                returnVariable[10] = toInt(textBox2.Text.ToString());
                returnVariable[11] = toInt(textBox3.Text.ToString());
                returnVariable[12] = toInt(textBox4.Text.ToString());
                returnVariable[13] = toInt(textBox1.Text.ToString());
                returnVariable[14] = toInt(textBox6.Text.ToString());
                returnVariable[15] = toInt(textBox5.Text.ToString());
                returnVariable[16] = toInt(textBox7.Text.ToString());
                returnVariable[17] = toInt(textBox8.Text.ToString());
                returnVariable[18] = toInt(textBox9.Text.ToString());
                returnVariable[19] = Firmware_version;

            }
            parent.setAllReg(returnVariable);
            this.Close();
        }
        int GetADC()
        {
            int ADC = 0;
            if (checkBox1.Checked)
            {
                ADC += 128;
            }
            if (checkBox2.Checked)
            {
                ADC += 64;
            }
            if (checkBox3.Checked)
            {
                ADC += 32;
            }
            if (checkBox4.Checked)
            {
                ADC += 16;
            }
            if (checkBox5.Checked)
            {
                ADC += 8;
            }
            if (checkBox6.Checked)
            {
                ADC += 4;
            }
            if (checkBox7.Checked)
            {
                ADC += 2;
            }
            if (checkBox8.Checked)
            {
                ADC += 1;
            }
            return ADC;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        { int n;
            if (int.TryParse(richTextBox1.Text, out n))
            {
                if (n >= 1 && n <= 1000)
                {
                    int newFreq = n;

                    pickFreq(n, n,0);
                }
            }
        
        }
        
    }
}
