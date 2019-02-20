using LearnLibs.Models;
using System;
using System.Windows.Forms;

namespace LearnLibs.Controls
{
    public partial class frmTeachingEditor : LearnLibs.Controls.FormDialog
    {
        Teaching tching = null;
        School sch = null;
        public override object Object
        {
            get
            {
                if (tching == null) tching = new Teaching();
                if (sch != null)
                {
                    tching.SchoolId = sch.Id;
                }
                tching.Alias = ltbAlias.Text;
                return tching;
            }
            set
            {
                if (value != null && value.GetType() == typeof(Teaching))
                {
                    tching = (Teaching)value;
                    if (tching.Id != Guid.Empty)
                    {
                        ltbSchool.ButtonVisible = false;
                        sch = ModelDbSet.GetObject<School>(tching.SchoolId);
                        if (sch != null)
                        {
                            ltbSchool.Text = sch.FullName;
                        }
                    }
                    ltbAlias.Text = tching.Alias;
                }
                else
                {
                    ltbSchool.ButtonVisible = true;
                }
            }
        }

        void selectSchool(object sender, EventArgs e)
        {
            sch = ModelDbSet.ShowDialog<frmSchoolSelector, School>(this, sch);
            if (sch != null)
            {
                ltbSchool.Text = sch.FullName;
            }
        }

        public frmTeachingEditor()
        {
            InitializeComponent();
        }

        private void frmTeachingEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                Teaching teaching = (Teaching)Object;
                if (teaching.SchoolId == Guid.Empty)
                {
                    MessageBox.Show("教学信息不含工作单位", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cancel = true;
                }
            }
        }
    }
}
