namespace LearnLibs.Controls
{
    partial class frmTeachingDetailEditor
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
            this.labTextButton1 = new LearnLibs.Controls.LabTextButton();
            this.ltb1 = new LearnLibs.Controls.LTB();
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
            this.splitContainer1.Panel1.Controls.Add(this.ltb1);
            this.splitContainer1.Panel1.Controls.Add(this.labTextButton1);
            this.splitContainer1.Size = new System.Drawing.Size(265, 86);
            this.splitContainer1.SplitterDistance = 58;
            // 
            // labTextButton1
            // 
            this.labTextButton1.AutoSize = true;
            this.labTextButton1.Dock = System.Windows.Forms.DockStyle.Top;
            this.labTextButton1.Label = "班级";
            this.labTextButton1.LabelWidth = 45;
            this.labTextButton1.Location = new System.Drawing.Point(0, 0);
            this.labTextButton1.MinimumSize = new System.Drawing.Size(200, 26);
            this.labTextButton1.Name = "labTextButton1";
            this.labTextButton1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.labTextButton1.Size = new System.Drawing.Size(265, 26);
            this.labTextButton1.TabIndex = 0;
            // 
            // ltb1
            // 
            this.ltb1.AutoSize = true;
            this.ltb1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb1.ForeColor = System.Drawing.Color.Black;
            this.ltb1.Label = "别名";
            this.ltb1.LabelWidth = 45;
            this.ltb1.Location = new System.Drawing.Point(0, 26);
            this.ltb1.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb1.Name = "ltb1";
            this.ltb1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb1.PasswordChar = '\0';
            this.ltb1.PlaceHolder = "班里你的昵称是什么？";
            this.ltb1.Size = new System.Drawing.Size(265, 26);
            this.ltb1.TabIndex = 1;
            // 
            // frmTeachingDetailEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(265, 86);
            this.Name = "frmTeachingDetailEditor";
            this.Text = "教学明细信息";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LTB ltb1;
        private LabTextButton labTextButton1;


    }
}
