using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LearnLibs.Models;

namespace LearnLibs.Controls
{
    public partial class frmClassSelector : LearnLibs.Controls.FormDialog
    {
        SchoolClass cls = null;
        School sch = null;

        void selectSchool(object sender, EventArgs e)
        {
            sch = ModelDbSet.ShowDialog<frmSchoolSelector, School>(this, sch);
            if (sch != null)
            {
                WhereArg arg = new WhereArg(BaseModel.FN.SchoolId, sch.Id);
                ModelDbSet.BindComboBoxSimple<SchoolClass>(lcbClasses.CMB, arg);
            }
        }

        public override object Object
        {
            get
            {
                return ModelDbSet.GetSelectedModel<SchoolClass>(lcbClasses.CMB);
            }
            set
            {
                cls = (SchoolClass)value;
            }
        }

        public frmClassSelector()
        {
            InitializeComponent();
            labSchools.btnSelect.Click += new EventHandler(selectSchool);
        }

        private void frmClassSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                SchoolClass sc = (SchoolClass)Object;
                if (sc == null)
                {
                    MessageBox.Show("请选择一个班级", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cancel = true;
                }
            }
        }
    }
}
