using System;
using System.ComponentModel;

namespace LearnLibs.Controls
{
    public partial class SelectRoles : LearnLibs.Controls.LabelControl
    {
        [Description("获取和设置选中的角色")]
        public Enums.UserRole SelectedRole {
            get {
                return (Enums.UserRole)Enum.Parse(typeof(Enums.UserRole), comboBox1.SelectedItem.ToString());
            }
            set {
                for (int i = 0; i < comboBox1.Items.Count; i++) {
                    if (comboBox1.Items[i].ToString() == value.ToString()) {
                        comboBox1.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
        public SelectRoles()
        {
            InitializeComponent();
        }
    }
}
