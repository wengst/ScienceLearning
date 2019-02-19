using System;
using System.ComponentModel;

namespace LearnLibs.Controls
{
    public partial class LTB : LearnLibs.Controls.LabelControl
    {
        System.Drawing.Color defaultForeColor;
        System.Drawing.Color holderColor = System.Drawing.Color.Gray;
        string placeHolder = string.Empty;

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

        [Browsable(true), Description("标签文本对齐方式"), Category("外观"), DefaultValue(System.Drawing.ContentAlignment.MiddleRight)]
        public System.Drawing.ContentAlignment LabelAlign
        {
            get
            {
                return LB.TextAlign;
            }
            set
            {
                LB.TextAlign = value;
            }
        }

        [Description("占位符"), Category("行为"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public string PlaceHolder
        {
            get { return placeHolder; }
            set
            {
                placeHolder = value;
                if (!string.IsNullOrWhiteSpace(placeHolder) && this.DesignMode)
                {
                    TB.Text = placeHolder;
                    TB.ForeColor = holderColor;
                }
                else
                {
                    TB.Text = string.Empty;
                    TB.ForeColor = defaultForeColor;
                }
            }
        }

        [Description("文本框内容"), Category("行为"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(PlaceHolder) && TB.Text == PlaceHolder)
                {
                    return string.Empty;
                }
                else
                {
                    return TB.Text;
                }
            }
            set
            {
                TB.Text = value;
            }
        }

        /// <summary>
        /// 设置或获取用户在文本框输入或粘贴的最大字符数
        /// </summary>
        [Browsable(true), Description("设置或获取用户在文本框输入或粘贴的最大字符数"), Category("外观"), DefaultValue(32767)]
        public int MaxLenght
        {
            get
            {
                return TB.MaxLength;
            }
            set
            {
                TB.MaxLength = value;
            }
        }

        public LTB()
        {
            InitializeComponent();
            defaultForeColor = this.TB.ForeColor;
        }

        private void TB_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PlaceHolder) && TB.Text == PlaceHolder)
            {
                this.ForeColor = holderColor;
            }
            else
            {
                this.ForeColor = defaultForeColor;
            }
        }

        private void LTB_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TB.Text) && !string.IsNullOrWhiteSpace(PlaceHolder))
            {
                TB.Text = PlaceHolder;
            }
        }

        private void TB_Enter(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PlaceHolder) && TB.Text == PlaceHolder)
            {
                TB.Text = string.Empty;
            }
        }

        private void TB_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(PlaceHolder) && string.IsNullOrWhiteSpace(TB.Text))
            {
                TB.Text = PlaceHolder;
                TB.ForeColor = holderColor;
            }
            else {
                TB.ForeColor = defaultForeColor;
            }
        }
    }
}
