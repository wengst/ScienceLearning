namespace LearnLibs.Controls
{
    partial class frmArea
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
            this.ltb_area_parentName = new LearnLibs.Controls.LTB();
            this.ltb_area_code = new LearnLibs.Controls.LTB();
            this.ltb_area_name = new LearnLibs.Controls.LTB();
            this.lcb_area_presses = new LearnLibs.Controls.LCB();
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
            this.splitContainer1.Panel1.Controls.Add(this.lcb_area_presses);
            this.splitContainer1.Panel1.Controls.Add(this.ltb_area_name);
            this.splitContainer1.Panel1.Controls.Add(this.ltb_area_code);
            this.splitContainer1.Panel1.Controls.Add(this.ltb_area_parentName);
            this.splitContainer1.Size = new System.Drawing.Size(238, 147);
            this.splitContainer1.SplitterDistance = 113;
            // 
            // ltb_area_parentName
            // 
            this.ltb_area_parentName.AutoSize = true;
            this.ltb_area_parentName.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_area_parentName.Label = "上级地区";
            this.ltb_area_parentName.LabelWidth = 80;
            this.ltb_area_parentName.Location = new System.Drawing.Point(0, 0);
            this.ltb_area_parentName.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_area_parentName.Name = "ltb_area_parentName";
            this.ltb_area_parentName.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_area_parentName.PasswordChar = '\0';
            this.ltb_area_parentName.ReadOnly = true;
            this.ltb_area_parentName.Size = new System.Drawing.Size(238, 26);
            this.ltb_area_parentName.TabIndex = 0;
            // 
            // ltb_area_code
            // 
            this.ltb_area_code.AutoSize = true;
            this.ltb_area_code.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_area_code.Label = "地区编号";
            this.ltb_area_code.LabelWidth = 80;
            this.ltb_area_code.Location = new System.Drawing.Point(0, 26);
            this.ltb_area_code.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_area_code.Name = "ltb_area_code";
            this.ltb_area_code.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_area_code.PasswordChar = '\0';
            this.ltb_area_code.Size = new System.Drawing.Size(238, 26);
            this.ltb_area_code.TabIndex = 0;
            // 
            // ltb_area_name
            // 
            this.ltb_area_name.AutoSize = true;
            this.ltb_area_name.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_area_name.Label = "地区名";
            this.ltb_area_name.LabelWidth = 80;
            this.ltb_area_name.Location = new System.Drawing.Point(0, 52);
            this.ltb_area_name.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_area_name.Name = "ltb_area_name";
            this.ltb_area_name.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_area_name.PasswordChar = '\0';
            this.ltb_area_name.Size = new System.Drawing.Size(238, 26);
            this.ltb_area_name.TabIndex = 0;
            // 
            // lcb_area_presses
            // 
            this.lcb_area_presses.AutoSize = true;
            this.lcb_area_presses.Dock = System.Windows.Forms.DockStyle.Top;
            this.lcb_area_presses.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lcb_area_presses.Label = "出版社";
            this.lcb_area_presses.LabelWidth = 80;
            this.lcb_area_presses.Location = new System.Drawing.Point(0, 78);
            this.lcb_area_presses.MinimumSize = new System.Drawing.Size(200, 26);
            this.lcb_area_presses.Name = "lcb_area_presses";
            this.lcb_area_presses.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lcb_area_presses.Size = new System.Drawing.Size(238, 26);
            this.lcb_area_presses.TabIndex = 1;
            // 
            // frmArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(238, 147);
            this.Name = "frmArea";
            this.Text = "地区";
            this.Load += new System.EventHandler(this.frmArea_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public LearnLibs.Controls.LTB ltb_area_parentName;
        public LearnLibs.Controls.LCB lcb_area_presses;
        public LearnLibs.Controls.LTB ltb_area_name;
        public LearnLibs.Controls.LTB ltb_area_code;

    }
}
