using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LearnLibs.Controls
{
    public partial class LCB : LearnLibs.Controls.LabelControl
    {
        [Browsable(true), Description("选中项的索引发生改变时发生"), Category("属性已更改"), EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler SelectedIndexChanged
        {
            add { CMB.SelectedIndexChanged += value; }
            remove
            {
                CMB.SelectedIndexChanged -= value;
            }
        }
        public LCB()
        {
            InitializeComponent();
        }
    }
}
