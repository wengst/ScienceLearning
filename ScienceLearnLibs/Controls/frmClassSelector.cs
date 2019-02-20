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
        Guid fixSchId = Guid.Empty;

        void selectSchool(object sender, EventArgs e)
        {
            sch = ModelDbSet.ShowDialog<frmSchoolSelector, School>(this, sch);
            if (sch != null)
            {
                labSchools.Text = sch.FullName;
                WhereArgs args = new WhereArgs();
                args.Add(new WhereArg(BaseModel.FN.SchoolId, sch.Id));
                int Period = ModelDbSet.GetSelectedInt(lcbPeriods.CMB);
                args.Add(new WhereArg(BaseModel.FN.Period, Period));
                ModelDbSet.BindComboBox<SchoolClass>(lcbClasses.CMB, args);
            }
        }

        void selectPeriod(object sender, EventArgs e)
        {
            if (sch != null)
            {
                int Period = ModelDbSet.GetSelectedInt(lcbPeriods.CMB);
                WhereArgs args = new WhereArgs();
                args.Add(new WhereArg(BaseModel.FN.SchoolId, sch.Id));
                args.Add(new WhereArg(BaseModel.FN.Period, Period));
                ModelDbSet.BindComboBox<SchoolClass>(lcbClasses.CMB, args, cls);
            }
        }

        void bindClasses(Guid schId)
        {
            sch = ModelDbSet.GetObject<School>(schId);
            int Period = ModelDbSet.GetSelectedInt(lcbPeriods.CMB);
            if (sch != null)
            {
                labSchools.Text = sch.FullName;
                WhereArgs args = new WhereArgs();
                args.Add(new WhereArg(BaseModel.FN.SchoolId, schId));
                args.Add(new WhereArg(BaseModel.FN.Period, Period));
                ModelDbSet.BindComboBox<SchoolClass>(lcbClasses.CMB, args);
            }
        }

        public override object Object
        {
            get
            {
                cls = ModelDbSet.GetSelectedModel<SchoolClass>(lcbClasses.CMB);
                return cls;
            }
            set
            {
                if (value != null && value.GetType() == typeof(SchoolClass))
                {
                    cls = (SchoolClass)value;
                    if (cls.SchoolId != Guid.Empty)
                    {
                        labSchools.ButtonVisible = false;
                        bindClasses(cls.SchoolId);
                    }
                    else
                    {
                        labSchools.ButtonVisible = true;
                    }
                }
            }
        }

        /// <summary>
        /// 当不希望用户选择学校时，请提供一个学校ID。
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Always)]
        public Guid FixedSchoolId
        {
            get { return fixSchId; }
            set
            {
                if (value != Guid.Empty)
                {
                    fixSchId = value;
                    this.labSchools.ButtonVisible = false;
                    bindClasses(fixSchId);
                }
            }
        }

        public frmClassSelector()
        {
            InitializeComponent();
            ModelDbSet.BindComboBoxByInts(lcbPeriods.CMB, DateTime.Now.Year - 3, DateTime.Now.Year + 3, DateTime.Now.Year);
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
