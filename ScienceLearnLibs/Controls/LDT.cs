using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LearnLibs.Controls
{
    public partial class LDT : LearnLibs.Controls.LabelControl
    {
        public LDT()
        {
            InitializeComponent();
            this.DT.Value = DateTime.Now;
        }
    }
}
