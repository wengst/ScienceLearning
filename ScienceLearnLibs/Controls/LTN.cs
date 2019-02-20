using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LearnLibs.Controls
{
    public partial class LTN : LearnLibs.Controls.LabelControl
    {
        [Category("行为"),Description("最大值"),DefaultValue(100), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public decimal Maxinum
        {
            get { return numericUpDown1.Maximum; }
            set { numericUpDown1.Maximum = value; }
        }
        [Category("行为"),Description("最小值"),DefaultValue(0), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public decimal Mininum
        {
            get { return numericUpDown1.Minimum; }
            set { numericUpDown1.Minimum = value; }
        }
        [Category("行为"),Description("值"),DefaultValue(0), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public decimal Value
        {
            get { return numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        [Category("行为"),Description("步长"),DefaultValue(1), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public decimal Increment
        {
            get { return numericUpDown1.Increment; }
            set { numericUpDown1.Increment = value; }
        }

        [Category("行为"),Description("小数位数"),DefaultValue(0), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public int DecimalPlaces
        {
            get { return numericUpDown1.DecimalPlaces; }
            set { numericUpDown1.DecimalPlaces = value; }
        }
        public LTN()
        {
            InitializeComponent();
        }
    }
}
