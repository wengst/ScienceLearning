namespace LearnLibs.Controls
{
    partial class frmClassSelector
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
            this.labSchools = new LearnLibs.Controls.LabTextButton();
            this.lcbClasses = new LearnLibs.Controls.LCB();
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
            this.splitContainer1.Panel1.Controls.Add(this.lcbClasses);
            this.splitContainer1.Panel1.Controls.Add(this.labSchools);
            this.splitContainer1.Size = new System.Drawing.Size(318, 86);
            this.splitContainer1.SplitterDistance = 58;
            // 
            // labSchools
            // 
            this.labSchools.AutoSize = true;
            this.labSchools.Dock = System.Windows.Forms.DockStyle.Top;
            this.labSchools.Label = "学校";
            this.labSchools.LabelWidth = 40;
            this.labSchools.Location = new System.Drawing.Point(0, 0);
            this.labSchools.MinimumSize = new System.Drawing.Size(200, 26);
            this.labSchools.Name = "labSchools";
            this.labSchools.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.labSchools.Size = new System.Drawing.Size(318, 26);
            this.labSchools.TabIndex = 0;
            // 
            // lcbClasses
            // 
            this.lcbClasses.AutoSize = true;
            this.lcbClasses.Dock = System.Windows.Forms.DockStyle.Top;
            this.lcbClasses.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lcbClasses.Label = "班级";
            this.lcbClasses.LabelWidth = 40;
            this.lcbClasses.Location = new System.Drawing.Point(0, 26);
            this.lcbClasses.MinimumSize = new System.Drawing.Size(200, 26);
            this.lcbClasses.Name = "lcbClasses";
            this.lcbClasses.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lcbClasses.Size = new System.Drawing.Size(318, 26);
            this.lcbClasses.TabIndex = 1;
            // 
            // frmClassSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(318, 86);
            this.Name = "frmClassSelector";
            this.Text = "班级选择器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmClassSelector_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LabTextButton labSchools;
        private LCB lcbClasses;
    }
}
