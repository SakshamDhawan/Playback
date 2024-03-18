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
    public partial class Threshold : Form
    {
        NU parent;
        public Threshold(NU _parent)
        {
            parent = _parent;
            InitializeComponent();
        }
        public async void GyroThreshold(float _value)
        {
            try
            {
                if (_value >= 1) { _value = 1; }
                await Task.Run(() => trackBar1.Value = (int)(_value * 1000f));
            }
            catch (Exception) { }
        }
        public void SetGyroThreshold(float _value)
        {
            if (_value >= 1) { _value = 1; }
            trackBar3.Value = (int)(_value * 1000f);
        }
        public async void MMGThreshold(float _value)
        {
            try
            {
                if (_value >= 1) { _value = 1; }
                await Task.Run(() => trackBar2.Value = (int)(_value * 1000f));
            }
            catch (Exception) { }
        }
        public void SetMMGThreshold(float _value)
        {
            if (_value >= 1) { _value = 1; }
            trackBar4.Value = (int)(_value * 1000f);
        }
    }
}
