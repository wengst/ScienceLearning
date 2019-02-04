using System;
using System.Data;

namespace LearnLibs.Controls
{
    public partial class frmArea : FormDialog
    {
        private LearnLibs.Models.Area area;
        public override object Object
        {
            get
            {
                if (area != null) {
                    area.Code = this.ltb_area_code.TB.Text;
                    area.Name = this.ltb_area_name.TB.Text;
                    area.PressId = PressId;
                }
                return area;
            }
            set
            {
                if (value.GetType() == typeof(LearnLibs.Models.Area)) {
                    area = (LearnLibs.Models.Area)value;
                    object parentObjName = ModelDbSet.GetFieldValue<Models.Area>(BaseModel.FN.Name, new WhereArgs() { new WhereArg(BaseModel.FN.Id, area.ParentId) });
                    if (parentObjName != null)
                    {
                        this.ltb_area_parentName.TB.Text = parentObjName.ToString();
                    }
                    else {
                        this.ltb_area_parentName.TB.Text = "无";
                    }
                    this.ltb_area_code.TB.Text = area.Code;
                    this.ltb_area_name.TB.Text = area.Name;
                    PressId = area.PressId;
                }
            }
        }

        public Guid PressId {
            get {
                if (lcb_area_presses.CMB.SelectedItem != null)
                {
                    return (Guid)lcb_area_presses.CMB.SelectedValue;
                }
                else {
                    return Guid.Empty;
                }
            }
            set {
                if (lcb_area_presses.CMB.DataSource != null) {
                    DataView dv = null;
                    if (lcb_area_presses.CMB.DataSource.GetType() == typeof(DataView)) {
                        dv = (DataView)lcb_area_presses.CMB.DataSource;
                    }
                    else if (lcb_area_presses.CMB.DataSource.GetType() == typeof(DataTable)) {
                        dv = ((DataTable)lcb_area_presses.CMB.DataSource).DefaultView;
                    }
                    if (dv != null) {
                        for (int i = 0; i < dv.Count; i++) {
                            DataRowView drv = dv[i];
                            Guid Id = LearnLibs.F.GetValue<Guid>(drv.Row, LearnLibs.BaseModel.FN.Id, Guid.Empty);
                            if (Id == value) {
                                lcb_area_presses.CMB.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
        }
        public frmArea()
        {
            InitializeComponent();
            lcb_area_presses.CMB.DataSource = ModelDbSet.GetDataView<Models.Press>(null);
            lcb_area_presses.CMB.ValueMember = BaseModel.FN.Id;
            lcb_area_presses.CMB.DisplayMember = BaseModel.FN.FullName;
            if (lcb_area_presses.CMB.Items.Count > 0) {
                lcb_area_presses.CMB.SelectedIndex = 0;
            }
        }

        private void frmArea_Load(object sender, EventArgs e)
        {
            
        }
    }
}
