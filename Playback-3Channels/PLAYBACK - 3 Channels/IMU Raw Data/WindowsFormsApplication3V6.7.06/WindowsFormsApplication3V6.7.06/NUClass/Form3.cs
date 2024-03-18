using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NUClass
{
    
    public partial class Form3 : Form
    {
        string name;
        NU parent;
        public Form3(string _name,NU _parent)
        {
            InitializeComponent();
            name = _name;
            parent = _parent;
        }

        
        /// <summary>
        /// Connect via Bluetooth button
        /// Communicates to form 1 that communication will be via Bluetooth
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Application.OpenForms["Form1"] != null)
            {
                parent.ChannelSelect(0);
                this.Close();
            }
        }

        /// <summary>
        /// Connect via USB button
        /// Communicates to form 1 that communication will be via USB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Application.OpenForms["Form1"] != null)
            {
                parent.ChannelSelect(1);
                this.Close();
            }
        }
    }
}
