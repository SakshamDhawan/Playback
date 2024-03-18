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
    public partial class Form_Time : Form
    {
        public Form_Time()
        {
            InitializeComponent();
        }

        public int[] GetOnOffTime()
        {
            int[] OnOffTime;

            OnOffTime = new int[] { int.Parse(textBox1.Text), int.Parse(textBox2.Text) };
            return OnOffTime;
        }
    }
}
