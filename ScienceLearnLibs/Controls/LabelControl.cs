using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LearnLibs.Controls
{
    public partial class LabelControl : UserControl
    {
        [Description("标签文本"), Category("外观"), Browsable(true), DefaultValue("label1")]
        public string Label
        {
            get { return this.LB.Text; }
            set { this.LB.Text = value; }
        }

        /// <summary>
        /// 标题宽度
        /// </summary>
        [Description("标题宽度"), Category("外观"), Browsable(true)]
        public int LabelWidth
        {
            get { return splitContainer1.SplitterDistance; }
            set { splitContainer1.SplitterDistance = value; }
        }
        public LabelControl()
        {
            InitializeComponent();
        }
    }
}
