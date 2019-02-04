namespace LearnLibs.Controls
{
    partial class frmPress
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
            this.ltbCode = new LearnLibs.Controls.LTB();
            this.ltbFullName = new LearnLibs.Controls.LTB();
            this.ltbShortName = new LearnLibs.Controls.LTB();
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
            this.splitContainer1.Panel1.Controls.Add(this.ltbShortName);
            this.splitContainer1.Panel1.Controls.Add(this.ltbFullName);
            this.splitContainer1.Panel1.Controls.Add(this.ltbCode);
            this.splitContainer1.Size = new System.Drawing.Size(253, 116);
            this.splitContainer1.SplitterDistance = 82;
            // 
            // ltbCode
            // 
            this.ltbCode.AutoSize = true;
            this.ltbCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltbCode.ImeMode = System.Windows.Forms.ImeMode.Close;
            this.ltbCode.Label = "编号";
            this.ltbCode.LabelWidth = 60;
            this.ltbCode.Location = new System.Drawing.Point(0, 0);
            this.ltbCode.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltbCode.Name = "ltbCode";
            this.ltbCode.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltbCode.PasswordChar = '\0';
            this.ltbCode.Size = new System.Drawing.Size(253, 26);
            this.ltbCode.TabIndex = 0;
            // 
            // ltbFullName
            // 
            this.ltbFullName.AutoSize = true;
            this.ltbFullName.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltbFullName.ImeMode = System.Windows.Forms.ImeMode.On;
            this.ltbFullName.Label = "全名";
            this.ltbFullName.LabelWidth = 60;
            this.ltbFullName.Location = new System.Drawing.Point(0, 26);
            this.ltbFullName.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltbFullName.Name = "ltbFullName";
            this.ltbFullName.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltbFullName.PasswordChar = '\0';
            this.ltbFullName.Size = new System.Drawing.Size(253, 26);
            this.ltbFullName.TabIndex = 1;
            // 
            // ltbShortName
            // 
            this.ltbShortName.AutoSize = true;
            this.ltbShortName.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltbShortName.ImeMode = System.Windows.Forms.ImeMode.On;
            this.ltbShortName.Label = "简称";
            this.ltbShortName.LabelWidth = 60;
            this.ltbShortName.Location = new System.Drawing.Point(0, 52);
            this.ltbShortName.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltbShortName.Name = "ltbShortName";
            this.ltbShortName.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltbShortName.PasswordChar = '\0';
            this.ltbShortName.Size = new System.Drawing.Size(253, 26);
            this.ltbShortName.TabIndex = 2;
            // 
            // frmPress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(253, 116);
            this.Name = "frmPress";
            this.Text = "出版社";
            this.Load += new System.EventHandler(this.frmPress_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public LearnLibs.Controls.LTB ltbCode;
        public LearnLibs.Controls.LTB ltbShortName;
        public LearnLibs.Controls.LTB ltbFullName;

    }
}
