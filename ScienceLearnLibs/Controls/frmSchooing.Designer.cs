namespace LearnLibs.Controls
{
    partial class frmSchooing
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
            this.lcbBj = new LearnLibs.Controls.LCB();
            this.lcbXjh = new LearnLibs.Controls.LCB();
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
            this.splitContainer1.Panel1.Controls.Add(this.lcbXjh);
            this.splitContainer1.Panel1.Controls.Add(this.lcbBj);
            this.splitContainer1.Panel1.Controls.Add(this.labSchools);
            this.splitContainer1.Size = new System.Drawing.Size(295, 136);
            this.splitContainer1.SplitterDistance = 108;
            // 
            // labSchools
            // 
            this.labSchools.AutoSize = true;
            this.labSchools.Dock = System.Windows.Forms.DockStyle.Top;
            this.labSchools.Label = "学校";
            this.labSchools.LabelWidth = 42;
            this.labSchools.Location = new System.Drawing.Point(0, 0);
            this.labSchools.MinimumSize = new System.Drawing.Size(200, 26);
            this.labSchools.Name = "labSchools";
            this.labSchools.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.labSchools.Size = new System.Drawing.Size(295, 26);
            this.labSchools.TabIndex = 0;
            // 
            // lcbBj
            // 
            this.lcbBj.AutoSize = true;
            this.lcbBj.Dock = System.Windows.Forms.DockStyle.Top;
            this.lcbBj.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lcbBj.Label = "班级";
            this.lcbBj.LabelWidth = 42;
            this.lcbBj.Location = new System.Drawing.Point(0, 26);
            this.lcbBj.MinimumSize = new System.Drawing.Size(200, 26);
            this.lcbBj.Name = "lcbBj";
            this.lcbBj.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lcbBj.Size = new System.Drawing.Size(295, 26);
            this.lcbBj.TabIndex = 1;
            // 
            // lcbXjh
            // 
            this.lcbXjh.AutoSize = true;
            this.lcbXjh.Dock = System.Windows.Forms.DockStyle.Top;
            this.lcbXjh.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lcbXjh.Label = "学号";
            this.lcbXjh.LabelWidth = 42;
            this.lcbXjh.Location = new System.Drawing.Point(0, 52);
            this.lcbXjh.MinimumSize = new System.Drawing.Size(200, 26);
            this.lcbXjh.Name = "lcbXjh";
            this.lcbXjh.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lcbXjh.Size = new System.Drawing.Size(295, 26);
            this.lcbXjh.TabIndex = 2;
            // 
            // ltbAlias
            // 
            this.ltbAlias.AutoSize = true;
            this.ltbAlias.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltbAlias.ForeColor = System.Drawing.Color.Gray;
            this.ltbAlias.Label = "昵称";
            this.ltbAlias.LabelWidth = 42;
            this.ltbAlias.Location = new System.Drawing.Point(0, 78);
            this.ltbAlias.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltbAlias.Name = "ltbAlias";
            this.ltbAlias.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltbAlias.PasswordChar = '\0';
            this.ltbAlias.PlaceHolder = "定义一个你在班级里的个性化名称";
            this.ltbAlias.Size = new System.Drawing.Size(295, 26);
            this.ltbAlias.TabIndex = 3;
            // 
            // frmSchooing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(295, 136);
            this.Name = "frmSchooing";
            this.Text = "就学信息";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSchooing_FormClosing);
            this.Load += new System.EventHandler(this.frmSchooing_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LabTextButton labSchools;
        private LCB lcbBj;
        private LCB lcbXjh;
        private LTB ltbAlias;
    }
}
