using System;

namespace LearnLibs.Controls
{
    public partial class frmCategory : FormDialog
    {
        LearnLibs.Models.Category category = null;
        public override object Object
        {
            get
            {
                if (category != null) {
                    category.Text = CategoryName;
                    category.Index = IndexNo;
                }
                return category;
            }
            set
            {
                if (value.GetType() == typeof(LearnLibs.Models.Category)) {
                    
                    category = (LearnLibs.Models.Category)value;
                    ltb_pressName.TB.Text = ModelDbSet.GetFieldString<Models.Press>(BaseModel.FN.FullName, BaseModel.FN.Id, category.PressId);
                    ltb_tb_fullname.TB.Text = ModelDbSet.GetFieldString<Models.TeachBook>(BaseModel.FN.ShortName, BaseModel.FN.Id, category.TeachBookId);
                    string pName = category.ParentId == Guid.Empty ? "无" : ModelDbSet.GetFieldString<Models.Category>(BaseModel.FN.Name, BaseModel.FN.Id, category.ParentId);
                    ltb_category_parentName.TB.Text = pName;
                    IndexNo = category.Index;
                    CategoryName = category.Text;
                }
            }
        }
        public string PressName {
            get { return ltb_pressName.TB.Text; }
            set { ltb_pressName.TB.Text = value; }
        }
        public string TeachBookFullName {
            get { return ltb_tb_fullname.TB.Text; }
            set { ltb_tb_fullname.TB.Text = value; }
        }
        public string ParentCategoryName {
            get { return ltb_category_parentName.TB.Text; }
            set { ltb_category_parentName.TB.Text = value; }
        }
        public int IndexNo {
            get {
                if (lcb_category_index.CMB.SelectedIndex == -1)
                {
                    return 1;
                }
                else {
                    return lcb_category_index.CMB.SelectedIndex + 1;
                }
            }
            set {
                for (int i = 0; i < lcb_category_index.CMB.Items.Count; i++) {
                    if (lcb_category_index.CMB.Items[i].ToString() == value.ToString()) {
                        lcb_category_index.CMB.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
        public string CategoryName {
            get { return ltb_category_text.TB.Text; }
            set { ltb_category_text.TB.Text = value; }
        }
        public frmCategory()
        {
            InitializeComponent();
            for (int i = 0; i < 20; i++) {
                lcb_category_index.CMB.Items.Add((i + 1).ToString());
            }
            lcb_category_index.CMB.SelectedIndex = 0;
        }
    }
}
