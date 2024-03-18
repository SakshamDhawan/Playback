namespace NUClass
{
    partial class Form5
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.HostPanel1 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.Sky_control = new System.Windows.Forms.CheckBox();
            this.Use_IR = new System.Windows.Forms.CheckBox();
            this.btnReadPreviousData = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.ComputerContrl = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSaveGestureTemplate = new System.Windows.Forms.Button();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.HostPanel2 = new System.Windows.Forms.Panel();
            this.HostPanel3 = new System.Windows.Forms.Panel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.GestureTrigger = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SaveClassifierData = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button7 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer4 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // HostPanel1
            // 
            this.HostPanel1.Location = new System.Drawing.Point(16, 15);
            this.HostPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.HostPanel1.Name = "HostPanel1";
            this.HostPanel1.Size = new System.Drawing.Size(431, 235);
            this.HostPanel1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 3000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Sky_control
            // 
            this.Sky_control.AutoSize = true;
            this.Sky_control.Location = new System.Drawing.Point(841, 95);
            this.Sky_control.Margin = new System.Windows.Forms.Padding(4);
            this.Sky_control.Name = "Sky_control";
            this.Sky_control.Size = new System.Drawing.Size(53, 21);
            this.Sky_control.TabIndex = 15;
            this.Sky_control.Text = "Sky";
            this.Sky_control.UseVisualStyleBackColor = true;
            this.Sky_control.CheckedChanged += new System.EventHandler(this.Sky_control_CheckedChanged);
            // 
            // Use_IR
            // 
            this.Use_IR.AutoSize = true;
            this.Use_IR.Location = new System.Drawing.Point(841, 122);
            this.Use_IR.Margin = new System.Windows.Forms.Padding(4);
            this.Use_IR.Name = "Use_IR";
            this.Use_IR.Size = new System.Drawing.Size(43, 21);
            this.Use_IR.TabIndex = 14;
            this.Use_IR.Text = "IR";
            this.Use_IR.UseVisualStyleBackColor = true;
            this.Use_IR.CheckedChanged += new System.EventHandler(this.Use_IR_CheckedChanged);
            // 
            // btnReadPreviousData
            // 
            this.btnReadPreviousData.Location = new System.Drawing.Point(683, 150);
            this.btnReadPreviousData.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadPreviousData.Name = "btnReadPreviousData";
            this.btnReadPreviousData.Size = new System.Drawing.Size(133, 28);
            this.btnReadPreviousData.TabIndex = 12;
            this.btnReadPreviousData.Text = "Read Last Saved";
            this.btnReadPreviousData.UseVisualStyleBackColor = true;
            this.btnReadPreviousData.Click += new System.EventHandler(this.btnReadPreviousData_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(716, 183);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 3;
            this.button1.Text = "Add Gesture";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ComputerContrl
            // 
            this.ComputerContrl.Location = new System.Drawing.Point(824, 150);
            this.ComputerContrl.Margin = new System.Windows.Forms.Padding(4);
            this.ComputerContrl.Name = "ComputerContrl";
            this.ComputerContrl.Size = new System.Drawing.Size(67, 63);
            this.ComputerContrl.TabIndex = 13;
            this.ComputerContrl.Text = "Comp Control";
            this.ComputerContrl.UseVisualStyleBackColor = true;
            this.ComputerContrl.Click += new System.EventHandler(this.ComputerContrl_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(791, 217);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 28);
            this.button3.TabIndex = 6;
            this.button3.Text = "Free Run";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(683, 217);
            this.button5.Margin = new System.Windows.Forms.Padding(4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(100, 28);
            this.button5.TabIndex = 8;
            this.button5.Text = "Test";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(575, 217);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 28);
            this.button2.TabIndex = 4;
            this.button2.Text = "Next";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(575, 186);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(132, 22);
            this.textBox1.TabIndex = 5;
            // 
            // btnSaveGestureTemplate
            // 
            this.btnSaveGestureTemplate.Location = new System.Drawing.Point(575, 150);
            this.btnSaveGestureTemplate.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveGestureTemplate.Name = "btnSaveGestureTemplate";
            this.btnSaveGestureTemplate.Size = new System.Drawing.Size(100, 28);
            this.btnSaveGestureTemplate.TabIndex = 10;
            this.btnSaveGestureTemplate.Text = "Save";
            this.btnSaveGestureTemplate.UseVisualStyleBackColor = true;
            this.btnSaveGestureTemplate.Click += new System.EventHandler(this.btnSaveGestureTemplate_Click);
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(467, 150);
            this.tbUserName.Margin = new System.Windows.Forms.Padding(4);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(99, 22);
            this.tbUserName.TabIndex = 11;
            this.tbUserName.Text = "Default";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(467, 191);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(89, 21);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "Scramble";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(467, 217);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 28);
            this.button4.TabIndex = 7;
            this.button4.Text = "Record";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // HostPanel2
            // 
            this.HostPanel2.Location = new System.Drawing.Point(16, 257);
            this.HostPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.HostPanel2.Name = "HostPanel2";
            this.HostPanel2.Size = new System.Drawing.Size(964, 224);
            this.HostPanel2.TabIndex = 0;
            // 
            // HostPanel3
            // 
            this.HostPanel3.Location = new System.Drawing.Point(546, 15);
            this.HostPanel3.Margin = new System.Windows.Forms.Padding(4);
            this.HostPanel3.Name = "HostPanel3";
            this.HostPanel3.Size = new System.Drawing.Size(59, 63);
            this.HostPanel3.TabIndex = 1;
            this.HostPanel3.Visible = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(467, 15);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(431, 235);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "NU: ";
            this.richTextBox1.Visible = false;
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(831, 15);
            this.button6.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(67, 23);
            this.button6.TabIndex = 16;
            this.button6.Text = "Tare";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // GestureTrigger
            // 
            this.GestureTrigger.Location = new System.Drawing.Point(739, 11);
            this.GestureTrigger.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GestureTrigger.Name = "GestureTrigger";
            this.GestureTrigger.Size = new System.Drawing.Size(75, 23);
            this.GestureTrigger.TabIndex = 17;
            this.GestureTrigger.Text = "G1";
            this.GestureTrigger.UseVisualStyleBackColor = true;
            this.GestureTrigger.Click += new System.EventHandler(this.GestureTrigger_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(463, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 17);
            this.label1.TabIndex = 18;
            this.label1.Text = "Saved Gestures:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(576, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 17);
            this.label2.TabIndex = 19;
            this.label2.Text = "0";
            // 
            // SaveClassifierData
            // 
            this.SaveClassifierData.Location = new System.Drawing.Point(671, 117);
            this.SaveClassifierData.Margin = new System.Windows.Forms.Padding(4);
            this.SaveClassifierData.Name = "SaveClassifierData";
            this.SaveClassifierData.Size = new System.Drawing.Size(145, 28);
            this.SaveClassifierData.TabIndex = 20;
            this.SaveClassifierData.Text = "Save Classifier Data";
            this.SaveClassifierData.UseVisualStyleBackColor = true;
            this.SaveClassifierData.Click += new System.EventHandler(this.SaveClassifierData_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(671, 90);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(69, 22);
            this.textBox2.TabIndex = 21;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(747, 90);
            this.textBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(69, 22);
            this.textBox3.TabIndex = 22;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(899, 148);
            this.button7.Margin = new System.Windows.Forms.Padding(4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(100, 46);
            this.button7.TabIndex = 23;
            this.button7.Text = "Process Mode 0";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(603, 95);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(61, 54);
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // timer4
            // 
            this.timer4.Enabled = true;
            this.timer4.Tick += new System.EventHandler(this.timer4_Tick);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(671, 30);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(61, 54);
            this.pictureBox2.TabIndex = 25;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            this.pictureBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox2_Paint);
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 496);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.SaveClassifierData);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GestureTrigger);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.Sky_control);
            this.Controls.Add(this.Use_IR);
            this.Controls.Add(this.ComputerContrl);
            this.Controls.Add(this.btnReadPreviousData);
            this.Controls.Add(this.tbUserName);
            this.Controls.Add(this.btnSaveGestureTemplate);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.HostPanel3);
            this.Controls.Add(this.HostPanel1);
            this.Controls.Add(this.HostPanel2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form5";
            this.Text = "Form5";
            this.Load += new System.EventHandler(this.Form5_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel HostPanel1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        public System.Windows.Forms.Timer timer3;
        public System.Windows.Forms.CheckBox Sky_control;
        public System.Windows.Forms.CheckBox Use_IR;
        private System.Windows.Forms.Button btnReadPreviousData;
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.Button ComputerContrl;
        public System.Windows.Forms.Button button3;
        public System.Windows.Forms.Button button5;
        public System.Windows.Forms.Button button2;
        public System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnSaveGestureTemplate;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.CheckBox checkBox1;
        public System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel HostPanel2;
        private System.Windows.Forms.Panel HostPanel3;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button GestureTrigger;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SaveClassifierData;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        public System.Windows.Forms.Button button7;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer4;
        public System.Windows.Forms.PictureBox pictureBox2;
    }
}