using LearnLibs.Models;
using System;
using System.Windows.Forms;

namespace LearnLibs.Controls
{
    public partial class frmSchooing : LearnLibs.Controls.FormDialog
    {
        Schooling sching = null;
        School sch = null;
        SchoolClass cls = null;

        void selectSchools(object sender, EventArgs e)
        {
            if (cls != null)
            {
                sch = ModelDbSet.GetObject<School>(cls.SchoolId);
            }
            sch = ModelDbSet.ShowDialog<frmSchoolSelector, School>(this, sch);
            if (sch != null)
            {
                WhereArg arg = new WhereArg(BaseModel.FN.SchoolId, sch.Id);
                ModelDbSet.BindComboBoxSimple<SchoolClass>(lcbBj.CMB, arg);
            }
        }

        SchoolClass selectedClass
        {
            get
            {
                return ModelDbSet.GetSelectedModel<SchoolClass>(lcbBj.CMB);
            }
            set
            {
                ModelDbSet.SetSelectedModel<SchoolClass>(lcbBj.CMB, sching == null ? Guid.Empty : sching.SchoolClassId);
            }
        }

        public override object Object
        {
            get
            {
                if (sching == null) sching = new Schooling();
                if (sch != null)
                {
                    sching.ProvinceId = sch.ProvinceId;
                    sching.CityId = sch.CityId;
                    sching.DistrictId = sch.DistrictId;
                    sching.SchoolId = sch.Id;
                }
                cls = ModelDbSet.GetSelectedModel<SchoolClass>(lcbBj.CMB);
                if (cls != null)
                {
                    sching.SchoolClassId = cls.Id;
                }
                sching.Index = ModelDbSet.GetSelectedInt(lcbXjh.CMB, 1);
                sching.Alias = string.IsNullOrWhiteSpace(ltbAlias.Text) ? "Std" + sching.Index.ToString().PadLeft(2, '0') : ltbAlias.Text;
                return sching;
            }
            set
            {
                if (value != null)
                {
                    sching = (Schooling)value;
                    sch = ModelDbSet.GetObject<School>(sching.SchoolId);
                    if (sch != null)
                    {
                        labSchools.TB.Text = sch.FullName;
                        WhereArg arg = new WhereArg(BaseModel.FN.SchoolId, sch.Id);
                        ModelDbSet.BindComboBoxSimple<SchoolClass>(lcbBj.CMB, arg);
                    }
                    if (lcbBj.CMB.Items.Count > 0)
                    {
                        ModelDbSet.SetSelectedModel<SchoolClass>(lcbBj.CMB, sching.SchoolClassId);
                    }
                    if (lcbXjh.CMB.Items.Count > 0)
                    {
                        ModelDbSet.SetSelectedInt(lcbXjh.CMB, sching.Index);
                    }
                    ltbAlias.Text = sching.Alias;
                }
            }
        }

        public frmSchooing()
        {
            InitializeComponent();
            ModelDbSet.BindComboBoxByInts(lcbXjh.CMB, 1, 99);
        }

        private void frmSchooing_Load(object sender, EventArgs e)
        {

        }

        private void frmSchooing_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                Schooling s = (Schooling)Object;
                if (s.ProvinceId == Guid.Empty || s.SchoolClassId == Guid.Empty)
                {
                    MessageBox.Show("请选择就学班级", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cancel = true;
                }
            }
        }
    }
}
