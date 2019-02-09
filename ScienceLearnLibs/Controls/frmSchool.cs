using LearnLibs.Enums;
using LearnLibs.Models;
using System;

namespace LearnLibs.Controls
{
    public partial class frmSchool : LearnLibs.Controls.FormDialog
    {
        private School school = null;
        public override object Object
        {
            get
            {
                if (school == null) school = new School();
                if (lcb_schooltype.CMB.SelectedIndex != -1) {
                    string stname = lcb_schooltype.CMB.Items[lcb_schooltype.CMB.SelectedIndex].ToString();
                    SchoolType st;
                    if (Enum.TryParse<SchoolType>(stname, out st))
                    {
                        school.SchoolType = st;
                    }
                    else {
                        school.SchoolType = SchoolType.全日制学校;
                    }
                }
                school.FullName = ltb_fullname.TB.Text;
                school.ShortName = ltb_shortname.TB.Text;
                return school;
            }
            set
            {
                if (value != null) {
                    school = (School)value;
                    for (int i = 0; i < lcb_schooltype.CMB.Items.Count; i++) {
                        if (lcb_schooltype.CMB.Items[i] == school.SchoolType.ToString("G")) {
                            lcb_schooltype.CMB.SelectedIndex = i;
                        }
                    }
                    ltb_fullname.TB.Text = school.FullName;
                    ltb_shortname.TB.Text = school.ShortName;
                    ltb_province.TB.Text = ModelDbSet.GetFieldString<Area>(BaseModel.FN.Name, new WhereArgs() { new WhereArg(BaseModel.FN.Id, school.ProvinceId) });
                    ltb_city.TB.Text = ModelDbSet.GetFieldString<Area>(BaseModel.FN.Name, new WhereArgs() { new WhereArg(BaseModel.FN.Id, school.CityId) });
                    ltb_district.TB.Text = ModelDbSet.GetFieldString<Area>(BaseModel.FN.Name, new WhereArgs() { new WhereArg(BaseModel.FN.Id, school.DistrictId) });
                }
            }
        }
        public frmSchool()
        {
            InitializeComponent();
        }
    }
}
