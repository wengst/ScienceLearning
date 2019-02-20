using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LearnLibs.Controls
{
    public partial class LabTextButton : LearnLibs.Controls.LabelControl
    {
        [Description("按钮可见性"), Category("外观"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always), DefaultValue(true)]
        public bool ButtonVisible
        {
            get { return btnSelect.Visible; }
            set { btnSelect.Visible = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text
        {
            get
            {
                return TB.Text;
            }
            set
            {
                TB.Text = value;
            }
        }

        [Description("按钮单击事件"), Category("鼠标"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public event EventHandler ButtonClick
        {
            add
            {
                btnSelect.Click += value;
            }
            remove
            {
                btnSelect.Click -= value;
            }
        }

        public LabTextButton()
        {
            InitializeComponent();
        }
    }
}
