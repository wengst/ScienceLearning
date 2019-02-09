using System;
using System.ComponentModel;

namespace LearnLibs.Controls
{
    public partial class LTB : LearnLibs.Controls.LabelControl
    {
        [Description("密码掩码"), Category("外观"), DefaultValue("")]
        public Char PasswordChar
        {
            get { return this.TB.PasswordChar; }
            set { this.TB.PasswordChar = value; }
        }
        [Browsable(true), Description("获取和设置是否只读"), Category("外观"), DefaultValue(false)]
        public bool ReadOnly
        {
            get { return TB.ReadOnly; }
            set { TB.ReadOnly = value; }
        }

        [Browsable(true), Description("多行文本还是单行文本"), Category("外观"), DefaultValue(false)]
        public bool MulitLine
        {
            get { return TB.Multiline; }
            set
            {
                TB.Multiline = value;
                this.AutoSize = !value;
                this.LB.TextAlign = System.Drawing.ContentAlignment.TopRight;
            }
        }

        [Browsable(true),Description("标签文本对齐方式"),Category("外观"),DefaultValue(System.Drawing.ContentAlignment.MiddleRight)]
        public System.Drawing.ContentAlignment LabelAlign {
            get {
                return LB.TextAlign;
            }
            set {
                LB.TextAlign = value;
            }
        }

        /// <summary>
        /// 设置或获取用户在文本框输入或粘贴的最大字符数
        /// </summary>
        [Browsable(true), Description("设置或获取用户在文本框输入或粘贴的最大字符数"),Category("外观"),DefaultValue(32767)]
        public int MaxLenght {
            get {
                return TB.MaxLength;
            }
            set {
                TB.MaxLength = value;
            }
        }

        public LTB()
        {
            InitializeComponent();
        }
    }
}
