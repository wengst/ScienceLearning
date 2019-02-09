namespace LearnLibs.Controls
{
    partial class frmSchool
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
            this.ltb_province = new LearnLibs.Controls.LTB();
            this.ltb_city = new LearnLibs.Controls.LTB();
            this.ltb_district = new LearnLibs.Controls.LTB();
            this.lcb_schooltype = new LearnLibs.Controls.LCB();
            this.ltb_fullname = new LearnLibs.Controls.LTB();
            this.ltb_shortname = new LearnLibs.Controls.LTB();
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
            this.splitContainer1.Panel1.Controls.Add(this.ltb_shortname);
            this.splitContainer1.Panel1.Controls.Add(this.ltb_fullname);
            this.splitContainer1.Panel1.Controls.Add(this.lcb_schooltype);
            this.splitContainer1.Panel1.Controls.Add(this.ltb_district);
            this.splitContainer1.Panel1.Controls.Add(this.ltb_city);
            this.splitContainer1.Panel1.Controls.Add(this.ltb_province);
            this.splitContainer1.Size = new System.Drawing.Size(382, 187);
            this.splitContainer1.SplitterDistance = 159;
            // 
            // ltb_province
            // 
            this.ltb_province.AutoSize = true;
            this.ltb_province.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_province.Label = "所在省";
            this.ltb_province.LabelWidth = 98;
            this.ltb_province.Location = new System.Drawing.Point(0, 0);
            this.ltb_province.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_province.Name = "ltb_province";
            this.ltb_province.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_province.PasswordChar = '\0';
            this.ltb_province.ReadOnly = true;
            this.ltb_province.Size = new System.Drawing.Size(382, 26);
            this.ltb_province.TabIndex = 0;
            // 
            // ltb_city
            // 
            this.ltb_city.AutoSize = true;
            this.ltb_city.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_city.Label = "所在市";
            this.ltb_city.LabelWidth = 98;
            this.ltb_city.Location = new System.Drawing.Point(0, 26);
            this.ltb_city.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_city.Name = "ltb_city";
            this.ltb_city.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_city.PasswordChar = '\0';
            this.ltb_city.ReadOnly = true;
            this.ltb_city.Size = new System.Drawing.Size(382, 26);
            this.ltb_city.TabIndex = 1;
            // 
            // ltb_district
            // 
            this.ltb_district.AutoSize = true;
            this.ltb_district.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_district.Label = "所在区县";
            this.ltb_district.LabelWidth = 98;
            this.ltb_district.Location = new System.Drawing.Point(0, 52);
            this.ltb_district.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_district.Name = "ltb_district";
            this.ltb_district.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_district.PasswordChar = '\0';
            this.ltb_district.ReadOnly = true;
            this.ltb_district.Size = new System.Drawing.Size(382, 26);
            this.ltb_district.TabIndex = 2;
            // 
            // lcb_schooltype
            // 
            this.lcb_schooltype.AutoSize = true;
            this.lcb_schooltype.Dock = System.Windows.Forms.DockStyle.Top;
            this.lcb_schooltype.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lcb_schooltype.Label = "类别";
            this.lcb_schooltype.LabelWidth = 98;
            this.lcb_schooltype.Location = new System.Drawing.Point(0, 78);
            this.lcb_schooltype.MinimumSize = new System.Drawing.Size(200, 26);
            this.lcb_schooltype.Name = "lcb_schooltype";
            this.lcb_schooltype.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lcb_schooltype.Size = new System.Drawing.Size(382, 26);
            this.lcb_schooltype.TabIndex = 3;
            // 
            // ltb_fullname
            // 
            this.ltb_fullname.AutoSize = true;
            this.ltb_fullname.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_fullname.Label = "全称";
            this.ltb_fullname.LabelWidth = 98;
            this.ltb_fullname.Location = new System.Drawing.Point(0, 104);
            this.ltb_fullname.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_fullname.Name = "ltb_fullname";
            this.ltb_fullname.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_fullname.PasswordChar = '\0';
            this.ltb_fullname.Size = new System.Drawing.Size(382, 26);
            this.ltb_fullname.TabIndex = 4;
            // 
            // ltb_shortname
            // 
            this.ltb_shortname.AutoSize = true;
            this.ltb_shortname.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_shortname.Label = "简称";
            this.ltb_shortname.LabelWidth = 98;
            this.ltb_shortname.Location = new System.Drawing.Point(0, 130);
            this.ltb_shortname.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_shortname.Name = "ltb_shortname";
            this.ltb_shortname.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_shortname.PasswordChar = '\0';
            this.ltb_shortname.Size = new System.Drawing.Size(382, 26);
            this.ltb_shortname.TabIndex = 5;
            // 
            // frmSchool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(382, 187);
            this.Name = "frmSchool";
            this.Text = "学校";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LTB ltb_province;
        private LTB ltb_district;
        private LTB ltb_city;
        private LTB ltb_shortname;
        private LTB ltb_fullname;
        private LCB lcb_schooltype;
    }
}
