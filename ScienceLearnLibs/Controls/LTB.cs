using System;
using System.ComponentModel;

namespace LearnLibs.Controls
{
    public partial class LTB : LearnLibs.Controls.LabelControl
    {
        [Description("密码掩码"),Category("外观"),DefaultValue("")]
        public Char PasswordChar {
            get { return this.TB.PasswordChar; }
            set { this.TB.PasswordChar = value; }
        }
        [Browsable(true),Description("获取和设置是否只读"),Category("外观"),DefaultValue(false)]
        public bool ReadOnly {
            get { return TB.ReadOnly; }
            set { TB.ReadOnly = value; }
        }
        public LTB()
        {
            InitializeComponent();
        }
    }
}
