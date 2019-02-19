namespace LearnLibs.Controls
{
    partial class frmSchoolClass
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
            this.LTBmyschool = new LearnLibs.Controls.LabTextButton();
            this.lcbClassIndex = new LearnLibs.Controls.LCB();
            this.ltbClassAlias = new LearnLibs.Controls.LTB();
            this.lcbPeriod = new LearnLibs.Controls.LCB();
            this.lcbStartGrade = new LearnLibs.Controls.LCB();
            this.lcbPeriodGrade = new LearnLibs.Controls.LCB();
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
            this.splitContainer1.Panel1.Controls.Add(this.lcbPeriodGrade);
            this.splitContainer1.Panel1.Controls.Add(this.lcbStartGrade);
            this.splitContainer1.Panel1.Controls.Add(this.lcbPeriod);
            this.splitContainer1.Panel1.Controls.Add(this.ltbClassAlias);
            this.splitContainer1.Panel1.Controls.Add(this.lcbClassIndex);
            this.splitContainer1.Panel1.Controls.Add(this.LTBmyschool);
            this.splitContainer1.Size = new System.Drawing.Size(338, 188);
            this.splitContainer1.SplitterDistance = 160;
            // 
            // LTBmyschool
            // 
            this.LTBmyschool.AutoSize = true;
            this.LTBmyschool.Dock = System.Windows.Forms.DockStyle.Top;
            this.LTBmyschool.Label = "学校";
            this.LTBmyschool.LabelWidth = 75;
            this.LTBmyschool.Location = new System.Drawing.Point(0, 0);
            this.LTBmyschool.MinimumSize = new System.Drawing.Size(200, 26);
            this.LTBmyschool.Name = "LTBmyschool";
            this.LTBmyschool.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.LTBmyschool.Size = new System.Drawing.Size(338, 26);
            this.LTBmyschool.TabIndex = 0;
            // 
            // lcbClassIndex
            // 
            this.lcbClassIndex.AutoSize = true;
            this.lcbClassIndex.Dock = System.Windows.Forms.DockStyle.Top;
            this.lcbClassIndex.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lcbClassIndex.Label = "序号";
            this.lcbClassIndex.LabelWidth = 75;
            this.lcbClassIndex.Location = new System.Drawing.Point(0, 26);
            this.lcbClassIndex.MinimumSize = new System.Drawing.Size(200, 26);
            this.lcbClassIndex.Name = "lcbClassIndex";
            this.lcbClassIndex.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lcbClassIndex.Size = new System.Drawing.Size(338, 26);
            this.lcbClassIndex.TabIndex = 1;
            // 
            // ltbClassAlias
            // 
            this.ltbClassAlias.AutoSize = true;
            this.ltbClassAlias.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltbClassAlias.Label = "名称";
            this.ltbClassAlias.LabelWidth = 75;
            this.ltbClassAlias.Location = new System.Drawing.Point(0, 52);
            this.ltbClassAlias.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltbClassAlias.Name = "ltbClassAlias";
            this.ltbClassAlias.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltbClassAlias.PasswordChar = '\0';
            this.ltbClassAlias.Size = new System.Drawing.Size(338, 26);
            this.ltbClassAlias.TabIndex = 2;
            // 
            // lcbPeriod
            // 
            this.lcbPeriod.AutoSize = true;
            this.lcbPeriod.Dock = System.Windows.Forms.DockStyle.Top;
            this.lcbPeriod.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lcbPeriod.Label = "届";
            this.lcbPeriod.LabelWidth = 75;
            this.lcbPeriod.Location = new System.Drawing.Point(0, 78);
            this.lcbPeriod.MinimumSize = new System.Drawing.Size(200, 26);
            this.lcbPeriod.Name = "lcbPeriod";
            this.lcbPeriod.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lcbPeriod.Size = new System.Drawing.Size(338, 26);
            this.lcbPeriod.TabIndex = 3;
            // 
            // lcbStartGrade
            // 
            this.lcbStartGrade.AutoSize = true;
            this.lcbStartGrade.Dock = System.Windows.Forms.DockStyle.Top;
            this.lcbStartGrade.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lcbStartGrade.Label = "入学年级";
            this.lcbStartGrade.LabelWidth = 75;
            this.lcbStartGrade.Location = new System.Drawing.Point(0, 104);
            this.lcbStartGrade.MinimumSize = new System.Drawing.Size(200, 26);
            this.lcbStartGrade.Name = "lcbStartGrade";
            this.lcbStartGrade.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lcbStartGrade.Size = new System.Drawing.Size(338, 26);
            this.lcbStartGrade.TabIndex = 4;
            // 
            // lcbPeriodGrade
            // 
            this.lcbPeriodGrade.AutoSize = true;
            this.lcbPeriodGrade.Dock = System.Windows.Forms.DockStyle.Top;
            this.lcbPeriodGrade.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lcbPeriodGrade.Label = "毕业年级";
            this.lcbPeriodGrade.LabelWidth = 75;
            this.lcbPeriodGrade.Location = new System.Drawing.Point(0, 130);
            this.lcbPeriodGrade.MinimumSize = new System.Drawing.Size(200, 26);
            this.lcbPeriodGrade.Name = "lcbPeriodGrade";
            this.lcbPeriodGrade.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lcbPeriodGrade.Size = new System.Drawing.Size(338, 26);
            this.lcbPeriodGrade.TabIndex = 5;
            // 
            // frmSchoolClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(338, 188);
            this.Name = "frmSchoolClass";
            this.Text = "我的班级";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LabTextButton LTBmyschool;
        private LCB lcbPeriod;
        private LTB ltbClassAlias;
        private LCB lcbClassIndex;
        private LCB lcbPeriodGrade;
        private LCB lcbStartGrade;
    }
}
