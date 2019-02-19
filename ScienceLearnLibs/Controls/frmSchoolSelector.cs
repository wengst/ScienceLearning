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
    public partial class frmSchoolSelector : LearnLibs.Controls.FormDialog
    {
        School school = null;
        public override object Object
        {
            get
            {
                return ModelDbSet.GetSelectedModel<School>(listBox1);
            }
            set
            {
                if (value != null && value.GetType() == typeof(School))
                {
                    school = (School)value;
                    if (listBox1.Items.Count > 0)
                    {
                        ModelDbSet.SetSelectedModel<School>(listBox1, school);
                    }
                }
            }
        }

        void bindProvinces(object sender, EventArgs e)
        {
            WhereArg arg = new WhereArg(BaseModel.FN.ParentId, Guid.Empty);
            ModelDbSet.BindComboBoxSimple<Area>(lcbProvinces.CMB, arg);
            if (lcbProvinces.CMB.Items.Count > 0)
            {
                bindCities(null, null);
            }
        }

        void bindCities(object sender, EventArgs e)
        {
            Area pa = ModelDbSet.GetSelectedModel<Area>(lcbProvinces.CMB);
            if (pa != null)
            {
                WhereArg arg = new WhereArg(BaseModel.FN.ParentId, pa.Id);
                ModelDbSet.BindComboBoxSimple<Area>(lcbCities.CMB, arg);
            }
            if (lcbCities.CMB.Items.Count > 0)
            {

            }
        }

        void bindDistricts(object sender, EventArgs e)
        {
            Area ca = ModelDbSet.GetSelectedModel<Area>(lcbCities.CMB);
            if (ca != null)
            {
                WhereArg arg = new WhereArg(BaseModel.FN.ParentId, ca.Id);
                ModelDbSet.BindComboBoxSimple<Area>(lcbDistricts.CMB, arg);
            }
            if (lcbDistricts.CMB.Items.Count > 0)
            {
                bindSchools(null, null);
            }
        }

        void bindSchools(object sender, EventArgs e)
        {
            Area pa = ModelDbSet.GetSelectedModel<Area>(lcbProvinces.CMB);
            Area ca = ModelDbSet.GetSelectedModel<Area>(lcbCities.CMB);
            Area cd = ModelDbSet.GetSelectedModel<Area>(lcbDistricts.CMB);
            if (cd != null)
            {
                WhereArgs args = new WhereArgs();
                args.Add(new WhereArg(BaseModel.FN.ProvinceId, pa.Id));
                args.Add(new WhereArg(BaseModel.FN.CityId, ca.Id));
                args.Add(new WhereArg(BaseModel.FN.DistrictId, cd.Id));
                ModelDbSet.BindListBox<School>(listBox1, args);
            }
            if (school != null && listBox1.Items.Count > 0)
            {
                ModelDbSet.SetSelectedModel<School>(listBox1, school);
            }
        }

        public frmSchoolSelector()
        {
            InitializeComponent();
            bindProvinces(null, null);
            lcbProvinces.CMB.SelectedIndexChanged += new EventHandler(bindCities);
            lcbCities.CMB.SelectedIndexChanged += new EventHandler(bindDistricts);
            lcbDistricts.CMB.SelectedIndexChanged += new EventHandler(bindSchools);
        }

        private void frmSchoolSelector_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                DataRowView drv = (DataRowView)listBox1.SelectedItem;
                school = ModelDbSet.ToObj<School>(drv.Row);
            }
            else
            {
                school = null;
            }
        }

        private void frmSchoolSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                School school = ModelDbSet.GetSelectedModel<School>(listBox1);
                if (school == null)
                {
                    MessageBox.Show("请选择一所学校", "信息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cancel = true;
                }
            }
        }
    }
}
