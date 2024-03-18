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
    public partial class Form_REPS : Form
    {
        public Form_REPS()
        {
            InitializeComponent();
        }

        public int GetReps()
        {
            int SetReps = int.Parse(textBox1.Text);
            return SetReps;
        }
    }
}
