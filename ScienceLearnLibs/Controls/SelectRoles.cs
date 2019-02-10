using System;
using System.ComponentModel;

namespace LearnLibs.Controls
{
    public partial class SelectRoles : LearnLibs.Controls.LabelControl
    {
        [Description("获取和设置选中的角色"),Category("行为"),DefaultValue(Enums.UserRole.学生)]
        public Enums.UserRole SelectedRole
        {
            get
            {
                if (comboBox1.Items != null && comboBox1.SelectedItem != null)
                {
                    return (Enums.UserRole)Enum.Parse(typeof(Enums.UserRole), comboBox1.SelectedItem.ToString());
                }
                return Enums.UserRole.学生;
            }
            set
            {
                if (comboBox1.Items != null)
                {
                    for (int i = 0; i < comboBox1.Items.Count; i++)
                    {
                        if (comboBox1.Items[i].ToString() == value.ToString())
                        {
                            comboBox1.SelectedIndex = i;
                            break;
                        }
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
