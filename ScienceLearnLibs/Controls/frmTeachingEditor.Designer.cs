namespace LearnLibs.Controls
{
    partial class frmTeachingEditor
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ltbSchool = new LearnLibs.Controls.LabTextButton();
            this.ltbAlias = new LearnLibs.Controls.LTB();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ltbAlias);
            this.splitContainer1.Panel1.Controls.Add(this.ltbSchool);
            this.splitContainer1.Size = new System.Drawing.Size(324, 84);
            this.splitContainer1.SplitterDistance = 56;
            // 
            // ltbSchool
            // 
            this.ltbSchool.AutoSize = true;
            this.ltbSchool.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltbSchool.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ltbSchool.Label = "学校";
            this.ltbSchool.LabelWidth = 40;
            this.ltbSchool.Location = new System.Drawing.Point(0, 0);
            this.ltbSchool.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltbSchool.Name = "ltbSchool";
            this.ltbSchool.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltbSchool.Size = new System.Drawing.Size(324, 26);
            this.ltbSchool.TabIndex = 0;
            this.ltbSchool.ButtonClick += new System.EventHandler(this.selectSchool);
            // 
            // ltbAlias
            // 
            this.ltbAlias.AutoSize = true;
            this.ltbAlias.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltbAlias.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ltbAlias.Label = "别名";
            this.ltbAlias.LabelWidth = 40;
            this.ltbAlias.Location = new System.Drawing.Point(0, 26);
            this.ltbAlias.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltbAlias.Name = "ltbAlias";
            this.ltbAlias.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltbAlias.PasswordChar = '\0';
            this.ltbAlias.PlaceHolder = "";
            this.ltbAlias.Size = new System.Drawing.Size(324, 26);
            this.ltbAlias.TabIndex = 1;
            // 
            // frmTeachingEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(324, 84);
            this.Name = "frmTeachingEditor";
            this.Text = "工作单位信息";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTeachingEditor_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LabTextButton ltbSchool;
        private LTB ltbAlias;
    }
}
