using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LearnLibs.Controls
{
    public partial class frmPress : FormDialog
    {
        private LearnLibs.Models.Press _press = null;
        public override object Object
        {
            get
            {
                if (_press != null) {
                    _press.Code = this.Code;
                    _press.FullName = this.FullName;
                    _press.ShortName = this.ShortName;
                }
                return _press;
            }
            set
            {
                if (value.GetType() == typeof(LearnLibs.Models.Press)) {
                    _press = (LearnLibs.Models.Press)value;
                    Code = _press.Code;
                    FullName = _press.FullName;
                    ShortName = _press.ShortName;
                }
            }
        }
        public string Code {
            get { return ltbCode.TB.Text; }
            set { ltbCode.TB.Text = value; }
        }
        public string FullName {
            get { return ltbFullName.TB.Text; }
            set { ltbFullName.TB.Text = value; }
        }
        public string ShortName {
            get { return ltbShortName.TB.Text; }
            set { ltbShortName.TB.Text = value; }
        }
        public frmPress()
        {
            InitializeComponent();
        }

        private void frmPress_Load(object sender, EventArgs e)
        {
            this.ltbCode.TB.Focus();
        }
    }
}
