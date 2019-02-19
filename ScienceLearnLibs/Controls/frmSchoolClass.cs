using LearnLibs.Models;
using System;
using System.Windows.Forms;

namespace LearnLibs.Controls
{
    public partial class frmSchoolClass : LearnLibs.Controls.FormDialog
    {
        private School mySchool = null;
        private SchoolClass myClass = null;

        private void selectSchool(object sender, EventArgs e)
        {
            if (myClass != null)
            {
                mySchool = ModelDbSet.GetObject<School>(myClass.SchoolId);
            }
            mySchool = ModelDbSet.ShowDialog<frmSchoolSelector, School>(this, mySchool);
        }

        private void bindPeriods()
        {
            lcbPeriod.CMB.Items.Clear();
            int sy = DateTime.Now.Year;
            for (int i = sy - 1; i < sy + 3; i++)
            {
                lcbPeriod.CMB.Items.Add(sy);
            }
            lcbPeriod.CMB.SelectedIndex = 0;
        }

        private void bindIndexs()
        {
            lcbClassIndex.CMB.Items.Clear();
            for (int i = 1; i < 51; i++)
            {
                lcbClassIndex.CMB.Items.Add(i);
            }
            lcbClassIndex.CMB.SelectedIndex = 0;
        }

        private void bindGrades(int l)
        {
            string[] grades = Enum.GetNames(typeof(LearnLibs.Enums.SchoolGrades));
            switch (l)
            {
                case 0:
                    lcbStartGrade.CMB.Items.Clear();
                    lcbStartGrade.CMB.Items.AddRange(grades);
                    if (lcbStartGrade.CMB.Items.Count > 0)
                    {
                        lcbStartGrade.CMB.SelectedIndex = 0;
                    }
                    break;
                default:
                    lcbPeriodGrade.CMB.Items.Clear();
                    lcbPeriodGrade.CMB.Items.AddRange(grades);
                    if (lcbPeriodGrade.CMB.Items.Count > 0)
                    {
                        lcbPeriodGrade.CMB.SelectedIndex = 0;
                    }
                    break;
            }
        }

        private LearnLibs.Enums.SchoolGrades getGrade(int l)
        {
            LearnLibs.Enums.SchoolGrades g = Enums.SchoolGrades.未设置;
            ComboBox cmb = null;
            if (l == 0)
            {
                g = Enums.SchoolGrades.七年级;
                cmb = lcbStartGrade.CMB;
            }
            else
            {
                g = Enums.SchoolGrades.九年级;
                cmb = lcbPeriodGrade.CMB;
            }
            if (cmb.SelectedIndex >= 0)
            {
                if (Enum.TryParse<LearnLibs.Enums.SchoolGrades>(cmb.SelectedItem.ToString(), out g))
                {
                    return g;
                }
            }
            return g;
        }

        private void setGrade(int l, LearnLibs.Enums.SchoolGrades g)
        {
            ComboBox cmb = l == 0 ? lcbStartGrade.CMB : lcbPeriodGrade.CMB;
            string gstr = g.ToString("G");
            for (int i = 0; i < cmb.Items.Count; i++)
            {
                if (cmb.Items[i].ToString() == gstr)
                {
                    cmb.SelectedIndex = i;
                    break;
                }
            }
        }

        private int period
        {
            get
            {
                if (lcbPeriod.CMB.SelectedIndex >= 0)
                {
                    return (int)lcbPeriod.CMB.SelectedItem;
                }
                return DateTime.Now.Year + 2;
            }
            set
            {
                for (int i = 0; i < lcbPeriod.CMB.Items.Count; i++)
                {
                    if (lcbPeriod.CMB.Items[i].ToString() == value.ToString())
                    {
                        lcbPeriod.CMB.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private int IndexNo
        {
            get
            {
                if (lcbClassIndex.CMB.SelectedIndex >= 0)
                {
                    return (int)lcbClassIndex.CMB.SelectedItem;
                }
                return 1;
            }
            set
            {
                for (int i = 0; i < lcbClassIndex.CMB.Items.Count; i++)
                {
                    if (lcbClassIndex.CMB.Items[i].ToString() == value.ToString())
                    {
                        lcbClassIndex.CMB.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void gradeSelectedChanged0(object sender, EventArgs e)
        {
            int index = lcbStartGrade.CMB.SelectedIndex;
            if (lcbPeriodGrade.CMB.SelectedIndex < index && lcbPeriodGrade.CMB.Items.Count > index)
            {
                lcbPeriodGrade.CMB.SelectedIndex = index;
            }
        }

        private void gradeSelectedChanged1(object sender, EventArgs e)
        {
            int index = lcbPeriodGrade.CMB.SelectedIndex;
            if (index < lcbStartGrade.CMB.SelectedIndex && index > lcbStartGrade.CMB.Items.Count)
            {
                lcbStartGrade.CMB.SelectedIndex = index;
            }
        }

        public override object Object
        {
            get
            {
                if (myClass == null) myClass = new SchoolClass();
                if (mySchool != null)
                {
                    myClass.SchoolId = mySchool.Id;
                    myClass.SchoolType = mySchool.SchoolType;
                }
                myClass.Index = IndexNo;
                myClass.Alias = ltbClassAlias.TB.Text;
                myClass.Graduation = getGrade(1);
                myClass.Admission = getGrade(0);
                myClass.Period = period;
                return myClass;
            }
            set
            {
                if (value != null && value.GetType() == typeof(SchoolClass))
                {
                    value = (SchoolClass)value;
                    mySchool = ModelDbSet.GetObject<School>(myClass.SchoolId);
                    if (mySchool != null)
                    {
                        LTBmyschool.TB.Text = mySchool.FullName;
                    }
                    period = myClass.Period;
                    setGrade(0, myClass.Admission);
                    setGrade(1, myClass.Graduation);
                    ltbClassAlias.TB.Text = myClass.Alias;
                    IndexNo = myClass.Index;
                }
            }
        }

        public frmSchoolClass()
        {
            InitializeComponent();
            bindIndexs();
            bindPeriods();
            bindGrades(0);
            bindGrades(1);
            LTBmyschool.btnSelect.Click += new EventHandler(selectSchool);
            lcbStartGrade.CMB.SelectedIndexChanged += new EventHandler(gradeSelectedChanged0);
            lcbPeriodGrade.CMB.SelectedIndexChanged += new EventHandler(gradeSelectedChanged1);
        }
    }
}
