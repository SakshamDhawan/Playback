namespace NUClass
{
    partial class Form1
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
            this.HostPanel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.HostPanel3 = new System.Windows.Forms.Panel();
            this.HostPanel4 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // HostPanel1
            // 
            this.HostPanel1.Location = new System.Drawing.Point(11, 10);
            this.HostPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.HostPanel1.Name = "HostPanel1";
            this.HostPanel1.Size = new System.Drawing.Size(431, 235);
            this.HostPanel1.TabIndex = 1;
            // 
            // HostPanel2
            // 
            this.HostPanel2.Location = new System.Drawing.Point(11, 254);
            this.HostPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.HostPanel2.Name = "HostPanel2";
            this.HostPanel2.Size = new System.Drawing.Size(431, 235);
            this.HostPanel2.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(907, 11);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 47);
            this.button1.TabIndex = 0;
            this.button1.Text = "Tare";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1007, 11);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 47);
            this.button2.TabIndex = 3;
            this.button2.Text = "Cali";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(907, 82);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(89, 44);
            this.button3.TabIndex = 4;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // HostPanel3
            // 
            this.HostPanel3.Location = new System.Drawing.Point(453, 11);
            this.HostPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.HostPanel3.Name = "HostPanel3";
            this.HostPanel3.Size = new System.Drawing.Size(431, 235);
            this.HostPanel3.TabIndex = 2;
            // 
            // HostPanel4
            // 
            this.HostPanel4.Location = new System.Drawing.Point(453, 254);
            this.HostPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.HostPanel4.Name = "HostPanel4";
            this.HostPanel4.Size = new System.Drawing.Size(431, 235);
            this.HostPanel4.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1108, 518);
            this.Controls.Add(this.HostPanel4);
            this.Controls.Add(this.HostPanel3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.HostPanel2);
            this.Controls.Add(this.HostPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "t";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel HostPanel1;
        private System.Windows.Forms.Panel HostPanel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel HostPanel3;
        private System.Windows.Forms.Panel HostPanel4;
    }
}