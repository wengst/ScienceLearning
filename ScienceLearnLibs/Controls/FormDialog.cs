using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LearnLibs.Controls
{
    public partial class FormDialog : Form
    {
        /// <summary>
        /// 获取或设置对话框的对象
        /// </summary>
        public virtual object Object { get; set; }
        public FormDialog()
        {
            InitializeComponent();
        }
    }
}
