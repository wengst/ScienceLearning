using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LearnLibs.Models;
using LearnLibs;

namespace LearnLibs.Controls
{
    public partial class frmStandard : LearnLibs.Controls.FormDialog
    {
        Standard standard = null;
        public override object Object
        {
            get
            {
                if (standard == null) { standard = new Standard(); }
                standard.Code = ltb_standard_code.TB.Text;
                standard.Text = ltb_standard_text.TB.Text;
                return standard;
            }
            set
            {
                if (value != null && value.GetType() == typeof(Standard))
                {
                    standard = (Standard)value;
                    ltb_standard_text.TB.Text = standard.Text;
                    ltb_standard_code.TB.Text = standard.Code;
                    if (standard.ParentId != Guid.Empty)
                    {
                        WhereArg arg = new WhereArg(BaseModel.FN.Id, standard.ParentId);
                        ltb_parent_name.TB.Text = ModelDbSet.GetFieldString<Standard>(BaseModel.FN.Text, new WhereArgs() { arg });
                    }
                }
            }
        }
        public frmStandard()
        {
            InitializeComponent();
        }
    }
}
