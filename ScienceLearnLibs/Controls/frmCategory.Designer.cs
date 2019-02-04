namespace LearnLibs.Controls
{
    partial class frmCategory
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
            this.ltb_pressName = new LearnLibs.Controls.LTB();
            this.ltb_tb_fullname = new LearnLibs.Controls.LTB();
            this.ltb_category_parentName = new LearnLibs.Controls.LTB();
            this.lcb_category_index = new LearnLibs.Controls.LCB();
            this.ltb_category_text = new LearnLibs.Controls.LTB();
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
            this.splitContainer1.Panel1.Controls.Add(this.ltb_category_text);
            this.splitContainer1.Panel1.Controls.Add(this.lcb_category_index);
            this.splitContainer1.Panel1.Controls.Add(this.ltb_category_parentName);
            this.splitContainer1.Panel1.Controls.Add(this.ltb_tb_fullname);
            this.splitContainer1.Panel1.Controls.Add(this.ltb_pressName);
            this.splitContainer1.Size = new System.Drawing.Size(245, 170);
            this.splitContainer1.SplitterDistance = 136;
            // 
            // ltb_pressName
            // 
            this.ltb_pressName.AutoSize = true;
            this.ltb_pressName.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_pressName.Label = "出版社";
            this.ltb_pressName.LabelWidth = 80;
            this.ltb_pressName.Location = new System.Drawing.Point(0, 0);
            this.ltb_pressName.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_pressName.Name = "ltb_pressName";
            this.ltb_pressName.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_pressName.PasswordChar = '\0';
            this.ltb_pressName.ReadOnly = true;
            this.ltb_pressName.Size = new System.Drawing.Size(245, 26);
            this.ltb_pressName.TabIndex = 0;
            // 
            // ltb_tb_fullname
            // 
            this.ltb_tb_fullname.AutoSize = true;
            this.ltb_tb_fullname.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_tb_fullname.Label = "教科书";
            this.ltb_tb_fullname.LabelWidth = 80;
            this.ltb_tb_fullname.Location = new System.Drawing.Point(0, 26);
            this.ltb_tb_fullname.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_tb_fullname.Name = "ltb_tb_fullname";
            this.ltb_tb_fullname.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_tb_fullname.PasswordChar = '\0';
            this.ltb_tb_fullname.ReadOnly = true;
            this.ltb_tb_fullname.Size = new System.Drawing.Size(245, 26);
            this.ltb_tb_fullname.TabIndex = 0;
            // 
            // ltb_category_parentName
            // 
            this.ltb_category_parentName.AutoSize = true;
            this.ltb_category_parentName.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_category_parentName.Label = "上级单元";
            this.ltb_category_parentName.LabelWidth = 80;
            this.ltb_category_parentName.Location = new System.Drawing.Point(0, 52);
            this.ltb_category_parentName.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_category_parentName.Name = "ltb_category_parentName";
            this.ltb_category_parentName.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_category_parentName.PasswordChar = '\0';
            this.ltb_category_parentName.ReadOnly = true;
            this.ltb_category_parentName.Size = new System.Drawing.Size(245, 26);
            this.ltb_category_parentName.TabIndex = 0;
            // 
            // lcb_category_index
            // 
            this.lcb_category_index.AutoSize = true;
            this.lcb_category_index.Dock = System.Windows.Forms.DockStyle.Top;
            this.lcb_category_index.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lcb_category_index.Label = "序号";
            this.lcb_category_index.LabelWidth = 80;
            this.lcb_category_index.Location = new System.Drawing.Point(0, 78);
            this.lcb_category_index.MinimumSize = new System.Drawing.Size(200, 26);
            this.lcb_category_index.Name = "lcb_category_index";
            this.lcb_category_index.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lcb_category_index.Size = new System.Drawing.Size(245, 26);
            this.lcb_category_index.TabIndex = 1;
            // 
            // ltb_category_text
            // 
            this.ltb_category_text.AutoSize = true;
            this.ltb_category_text.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_category_text.Label = "本单元名";
            this.ltb_category_text.LabelWidth = 80;
            this.ltb_category_text.Location = new System.Drawing.Point(0, 104);
            this.ltb_category_text.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_category_text.Name = "ltb_category_text";
            this.ltb_category_text.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_category_text.PasswordChar = '\0';
            this.ltb_category_text.Size = new System.Drawing.Size(245, 26);
            this.ltb_category_text.TabIndex = 2;
            // 
            // frmCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(245, 170);
            this.Name = "frmCategory";
            this.Text = "教材单元";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public LearnLibs.Controls.LTB ltb_pressName;
        public LearnLibs.Controls.LTB ltb_category_parentName;
        public LearnLibs.Controls.LTB ltb_tb_fullname;
        public LearnLibs.Controls.LTB ltb_category_text;
        public LearnLibs.Controls.LCB lcb_category_index;

    }
}
