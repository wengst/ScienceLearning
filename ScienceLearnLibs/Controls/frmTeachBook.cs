using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LearnLibs.Controls
{
    public partial class frmTeachBook : FormDialog
    {
        LearnLibs.Models.TeachBook tb;
        public override object Object
        {
            get
            {
                if (tb != null) {
                    tb.FullName = FullName;
                    tb.ShortName = ShortName;
                    tb.SchoolGrade = Grade;
                    tb.Semester = Semester;
                    tb.ImplementDate = ImplementDate;
                }
                return tb;
            }
            set
            {
                if (value.GetType() == typeof(LearnLibs.Models.TeachBook)) {
                    tb = (LearnLibs.Models.TeachBook)value;
                    FullName = tb.FullName;
                    ShortName = tb.ShortName;
                    Grade = tb.SchoolGrade;
                    Semester = tb.Semester;
                    ImplementDate = tb.ImplementDate;
                    if (tb.PressId != Guid.Empty) { 
                        WhereArgs args = new WhereArgs();
                        args.Add(new WhereArg(BaseModel.FN.Id,tb.PressId));
                        object objShortName = ModelDbSet.GetFieldValue<Models.Press>(BaseModel.FN.FullName,args);
                        if (objShortName != null) {
                            ltb_pressName.TB.Text = objShortName.ToString();
                        }
                    }
                }
            }
        }
        public string PressName
        {
            set { ltb_pressName.TB.Text = value; }
        }
        public string FullName
        {
            get { return ltb_tb_fullname.TB.Text; }
            set { ltb_tb_fullname.TB.Text = value; }
        }
        public string ShortName
        {
            get { return ltb_tb_shortName.TB.Text; }
            set { ltb_tb_shortName.TB.Text = value; }
        }
        public LearnLibs.Enums.SchoolGrades Grade
        {
            get
            {
                LearnLibs.Enums.SchoolGrades g;
                if (Enum.TryParse<LearnLibs.Enums.SchoolGrades>(this.lcb_tb_grade.CMB.SelectedItem != null ? lcb_tb_grade.CMB.SelectedItem.ToString() : "未设置", true, out g))
                {
                    return g;
                }
                return LearnLibs.Enums.SchoolGrades.未设置;
            }
            set
            {
                if (lcb_tb_grade.CMB.Items.Count > 0)
                {
                    for (int i = 0; i < lcb_tb_grade.CMB.Items.Count; i++)
                    {
                        if (lcb_tb_grade.CMB.Items[i].ToString() == value.ToString())
                        {
                            lcb_tb_grade.CMB.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }
        public LearnLibs.Enums.Semesters Semester
        {
            get
            {
                LearnLibs.Enums.Semesters semester;
                if (Enum.TryParse<LearnLibs.Enums.Semesters>(lcb_tb_semester.CMB.SelectedItem == null ? "未设置" : lcb_tb_semester.CMB.SelectedItem.ToString(), true, out semester))
                {
                    return semester;
                }
                return LearnLibs.Enums.Semesters.未设置;
            }
            set
            {
                if (lcb_tb_semester.CMB.Items.Count > 0)
                {
                    for (int i = 0; i < lcb_tb_semester.CMB.Items.Count; i++)
                    {
                        if (lcb_tb_semester.CMB.Items[i].ToString() == value.ToString())
                        {
                            lcb_tb_semester.CMB.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }
        public DateTime ImplementDate {
            get { return ldt_tb_implementdate.DT.Value; }
            set { ldt_tb_implementdate.DT.Value = value; }
        }
        public frmTeachBook()
        {
            InitializeComponent();
            lcb_tb_grade.CMB.Items.AddRange(Enum.GetNames(typeof(LearnLibs.Enums.SchoolGrades)));
            lcb_tb_grade.CMB.SelectedIndex = 0;
            lcb_tb_semester.CMB.Items.AddRange(Enum.GetNames(typeof(LearnLibs.Enums.Semesters)));
            lcb_tb_semester.CMB.SelectedIndex = 0;
            ldt_tb_implementdate.DT.Value = DateTime.Now;
        }

        private void frmTeachBook_Load(object sender, EventArgs e)
        {

        }
    }
}
