using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace NUClass
{
    public partial class Form6 : Form
    {
        public static Form6 _Form6;
        int IMUno;
        NU parent;

        private int MMG_RA, MMG_TA, MMG_ES;
        private double MMG_RA_sum, MMG_TA_sum, MMG_ES_sum;
        private int MMG_RA_MVC = 1, MMG_TA_MVC = 1, MMG_ES_MVC = 1;
        private double MMG_RA_MVC_sum = 0, MMG_TA_MVC_sum = 0, MMG_ES_MVC_sum = 0;
        private double MMG_RA_MVC_k = 0, MMG_TA_MVC_k = 0, MMG_ES_MVC_k = 0;
        private float Exer_time = 0, contract_time = 0;
        private int contract_no = 0;
        public int active_rest = 0;
        private bool start;
        private bool Calibration_state = true;

        private int Set_REPS = 2;
        private int Ontime = 5;
        private int Offtime = 5;

        private string MMG_units = "mV";

        List<double> MMG_Data_Trials = new List<double>() { };
        public Form6()
        {
            InitializeComponent();
        }

        public Form6(int _IMUno, NU _parent)
        {
            parent = _parent;
            IMUno = _IMUno;
            InitializeComponent();
            _Form6 = this;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form6_Closing);
            //setModeVis(false);
            UpdateTimer.Interval = 10;
            UpdateTimer.Enabled = true;
            UpdateTimer.Start();
        }

        private void Form6_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NU.NUClass.softStopNU();
            NU.NUClass.saveData = false;
            NU.NUClass.Kat = false;
            UpdateTimer.Stop();
            UpdateTimer.Enabled = false;
            
        }

        public void Update_MMGValue(int RA, int TA, int ES)
        {
            //
            if (RA > 0)
            { 
                if (Math.Abs(RA - MMG_RA) > 3 || RA < 10)
                {
                    MMG_RA = RA;
                }
            }

            if (TA > 0)
            {
                if (Math.Abs(TA - MMG_TA) > 3 || TA < 10)
                {
                    MMG_TA = TA;
                }
            }
            if (ES > 0)
            {
                if (Math.Abs(ES - MMG_ES) > 3 || ES < 10)
                {
                    MMG_ES = ES;
                }
            }


            if (MMG_RA > 100)
            {
                MMG_RA = 100;
            }


            if (MMG_TA > 100)
            {
                MMG_TA = 100;
            }

            if (MMG_ES>100)
            {
                MMG_ES = 100;
            }
        }

        private void circularProgressBar2_Click(object sender, EventArgs e)
        {

        }

        private void circularProgressBar1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            contract_no = 0;
            label4.Text = contract_no.ToString();
            label4.Update();

            pictureBox4.Visible = false;
            pictureBox3.Visible = true;
            start = true;
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            pictureBox9.Visible = false;
            pictureBox8.Visible = true;
            pictureBox7.Visible = false;
            pictureBox6.Visible = false;
            pictureBox5.Visible = false;
            pictureBox1.Visible = false;

            Set_REPS = 2;
            label10.Text = "/" + Set_REPS.ToString();
            label10.Update();

            contract_no = 0;
            label4.Text = contract_no.ToString();
            label4.Update();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            pictureBox9.Visible = false;
            pictureBox8.Visible = false;
            pictureBox7.Visible = true;
            pictureBox6.Visible = false;
            pictureBox5.Visible = false;
            pictureBox1.Visible = false;

            Set_REPS = 2;
            label10.Text = "/" + Set_REPS.ToString();
            label10.Update();

            contract_no = 0;
            label4.Text = contract_no.ToString();
            label4.Update();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            pictureBox9.Visible = false;
            pictureBox8.Visible = false;
            pictureBox7.Visible = false;
            pictureBox6.Visible = true;
            pictureBox5.Visible = false;
            pictureBox1.Visible = false;

            Set_REPS = 10;
            label10.Text = "/"+ Set_REPS.ToString();
            label10.Update();
            Calibration_state = false;

            MMG_units = "%";

            contract_no = 0;
            label4.Text = contract_no.ToString();
            label4.Update();

        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            pictureBox9.Visible = false;
            pictureBox8.Visible = false;
            pictureBox7.Visible = false;
            pictureBox6.Visible = false;
            pictureBox5.Visible = true;
            pictureBox1.Visible = false;

            contract_no = 0;
            label4.Text = contract_no.ToString();
            label4.Update();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            pictureBox9.Visible = false;
            pictureBox8.Visible = false;
            pictureBox7.Visible = false;
            pictureBox6.Visible = false;
            pictureBox5.Visible = false;
            pictureBox1.Visible = true;

            contract_no = 0;
            label4.Text = contract_no.ToString();
            label4.Update();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox9.Visible = true;
            pictureBox8.Visible = false;
            pictureBox7.Visible = false;
            pictureBox6.Visible = false;
            pictureBox5.Visible = false;
            pictureBox1.Visible = false;

            contract_no = 0;
            label4.Text = contract_no.ToString();
            label4.Update();


            Set_REPS = 2;
            label10.Text = "/" + Set_REPS.ToString();
            label10.Update();

            MMG_units = "mV";
            MMG_RA_MVC = 1;
            MMG_TA_MVC = 1;
            MMG_ES_MVC = 1;

            Calibration_state = true;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }


        private void circularProgressBar5_Click(object sender, EventArgs e)
        {
            var formTime = new Form_Time();
            formTime.ShowDialog(this);
            int[] OnOfftime = formTime.GetOnOffTime();
            Ontime = OnOfftime[0];
            Offtime = OnOfftime[1];
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var formReport = new Form_Report();
            for (int i = 0; i < MMG_Data_Trials.Count()/3; i++)
            {
                formReport.MMG_1_Data_Trials.Add(MMG_Data_Trials[i*3]);
                formReport.MMG_2_Data_Trials.Add(MMG_Data_Trials[i*3+1]);
                formReport.MMG_3_Data_Trials.Add(MMG_Data_Trials[i*3+2]);
            }
            formReport.DataUpdate();
            formReport.ShowDialog(this);
        }

        private void label10_Click(object sender, EventArgs e)
        {
            var formREPS = new Form_REPS();
            formREPS.ShowDialog(this);
            Set_REPS = formREPS.GetReps();
            label10.Text = "/" + Set_REPS.ToString();
            label10.Update();
        }

        private void RecordButton1_Click(object sender, EventArgs e)
        {

              
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pictureBox3.Visible = false;
            pictureBox4.Visible = true;
            start = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            int time_exer;

            if (Calibration_state == true)
            {
                circularProgressBar3.Value = MMG_RA;
                circularProgressBar1.Value = MMG_TA;
                circularProgressBar2.Value = MMG_ES;
            }  
            else
            {
                MMG_RA_MVC = (int)(MMG_RA_MVC_sum / MMG_RA_MVC_k);
                MMG_TA_MVC = (int)(MMG_TA_MVC_sum / MMG_TA_MVC_k);
                MMG_ES_MVC = (int)(MMG_ES_MVC_sum / MMG_ES_MVC_k);

                double MMG_data_RA = (double)MMG_RA;
                double MMG_max_RA = (double)MMG_RA_MVC;
                int MMGvalue = (int)(MMG_data_RA / MMG_max_RA * 100);
                if (MMGvalue > 100)
                {
                    MMGvalue = 100;
                }
                circularProgressBar3.Value = MMGvalue;

                double MMG_data_TA = (double)MMG_TA;
                double MMG_max_TA = (double)MMG_TA_MVC;
                MMGvalue = (int)(MMG_data_TA / MMG_max_TA * 100);
                if (MMGvalue > 100)
                {
                    MMGvalue = 100;
                }
                circularProgressBar1.Value = MMGvalue;

                double MMG_data_ES = (double)MMG_ES;
                double MMG_max_ES = (double)MMG_ES_MVC;
                MMGvalue = (int)(MMG_data_ES / MMG_max_ES * 100);
                if (MMGvalue > 100)
                {
                    MMGvalue = 100;
                }
                circularProgressBar2.Value = MMGvalue;
            }

            circularProgressBar3.Text = circularProgressBar3.Value.ToString() + MMG_units;
            circularProgressBar3.Update();

            circularProgressBar1.Text = circularProgressBar1.Value.ToString() + MMG_units;
            circularProgressBar1.Update();

            circularProgressBar2.Text = circularProgressBar2.Value.ToString() + MMG_units;
            circularProgressBar2.Update();

            if (start == true)
            {
                if (Exer_time == Ontime*100 && active_rest == 0)
                {
                    active_rest = 1;
                    Exer_time = Offtime * 100-1;
                    circularProgressBar5.ProgressColor = System.Drawing.Color.Silver;
                    circularProgressBar5.Update();
                    label6.Visible = true;
                    label6.Update();
                    label7.Visible = false;
                    label7.Update();

                    string filename = "Stop_C.wav";
                    string path = Path.Combine(Environment.CurrentDirectory, @"Sound\", filename); 

                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(path);
                    player.Play();
                }
                else if (Exer_time == 0)
                {
                    active_rest = 0;
                    circularProgressBar5.ProgressColor = System.Drawing.Color.Lime;
                    circularProgressBar5.Update();
                    label6.Visible = false;
                    label6.Update();
                    label7.Visible = true;
                    label7.Update();

                    string filename = "Start_C.wav";
                    string path = Path.Combine(Environment.CurrentDirectory, @"Sound\", filename);

                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(path);

                    //System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"C:\Users\rs4318\Desktop\IMU V2\Start_C.wav");
                    player.Play();
                }

                if (active_rest == 1) // Off period
                {
                    Exer_time--;
                    
                    time_exer = Offtime - (int)(Exer_time / 100) - 1;

                    if (contract_time > 10)
                    {
                        contract_no++;
                        label4.Text = contract_no.ToString();
                        label4.Update();

                        if (contract_no >= Set_REPS)
                        {
                            pictureBox3.Visible = false;
                            pictureBox4.Visible = true;
                            start = false;
                            return;
                        }

                    }
                    if (Calibration_state == false && Exer_time == Offtime * 100 - 2)
                    {
                        MMG_Data_Trials.Add(MMG_RA_sum / (Ontime * 100));
                        MMG_Data_Trials.Add(MMG_TA_sum / (Ontime * 100));
                        MMG_Data_Trials.Add(MMG_ES_sum / (Ontime * 100));
                        MMG_RA_sum = 0;
                        MMG_TA_sum = 0;
                        MMG_ES_sum = 0;

                    }

                    contract_time = 0;


                    circularProgressBar5.Value = 100 - (int)(Exer_time / (Offtime * 2)) * 2;
                    circularProgressBar5.Text = time_exer.ToString() + "s";
                    circularProgressBar5.Update();

                }    
                else   // On period    
                {
                    Exer_time++;
                    time_exer = Ontime - (int)(Exer_time / 100);

                    if (MMG_RA>1 || MMG_TA > 1 || MMG_ES > 1)
                    {
                        contract_time++;
                    }

                    if (Calibration_state == true)
                    {
                        if (pictureBox9.Visible == true)
                        {
                            MMG_RA_MVC_sum += MMG_RA;
                            MMG_RA_MVC_k++;
                        }
                        if (pictureBox8.Visible == true)
                        {
                            MMG_TA_MVC_sum += MMG_TA;
                            MMG_TA_MVC_k++;
                        }

                        if (pictureBox7.Visible == true)
                        {
                            MMG_ES_MVC_sum += MMG_ES;
                            MMG_ES_MVC_k++;
                        }

                    }
                    else 
                    {
                        MMG_RA_MVC = (int)(MMG_RA_MVC_sum / MMG_RA_MVC_k);
                        MMG_TA_MVC = (int)(MMG_TA_MVC_sum / MMG_TA_MVC_k);
                        MMG_ES_MVC = (int)(MMG_ES_MVC_sum / MMG_ES_MVC_k);

                        double MMG_data_RA = (double)MMG_RA;
                        double MMG_max_RA = (double)MMG_RA_MVC;
                        MMG_RA_sum += (int)(MMG_data_RA / MMG_max_RA * 100);

                        double MMG_data_TA = (double)MMG_TA;
                        double MMG_max_TA = (double)MMG_TA_MVC;
                        MMG_TA_sum += (int)(MMG_data_TA / MMG_max_TA * 100);

                        double MMG_data_ES = (double)MMG_ES;
                        double MMG_max_ES = (double)MMG_ES_MVC;
                        MMG_ES_sum += (int)(MMG_data_ES / MMG_max_ES * 100);
                    }

                    circularProgressBar5.Value = 100 - (int)(Exer_time / (Ontime*2)) * 2;
                    circularProgressBar5.Text = time_exer.ToString() + "s";
                    circularProgressBar5.Update();

                }

            }
            else 
            {
                Exer_time = 0;
                time_exer = Ontime;
                circularProgressBar5.Value = 100;
                circularProgressBar5.Text = time_exer.ToString() + "s";
                circularProgressBar5.Update();

                //contract_no = 0;
                //label4.Text = contract_no.ToString();
               // label4.Update();

                circularProgressBar5.ProgressColor = System.Drawing.Color.Lime;
                circularProgressBar5.Update();
                label6.Visible = false;
                label6.Update();
                label7.Visible = true;
                label7.Update();

            }
        }
    }
}
