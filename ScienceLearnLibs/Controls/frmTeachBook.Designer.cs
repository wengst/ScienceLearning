namespace LearnLibs.Controls
{
    partial class frmTeachBook
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
            this.ltb_tb_shortName = new LearnLibs.Controls.LTB();
            this.lcb_tb_grade = new LearnLibs.Controls.LCB();
            this.lcb_tb_semester = new LearnLibs.Controls.LCB();
            this.ldt_tb_implementdate = new LearnLibs.Controls.LDT();
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
            this.splitContainer1.Panel1.Controls.Add(this.ldt_tb_implementdate);
            this.splitContainer1.Panel1.Controls.Add(this.lcb_tb_semester);
            this.splitContainer1.Panel1.Controls.Add(this.lcb_tb_grade);
            this.splitContainer1.Panel1.Controls.Add(this.ltb_tb_shortName);
            this.splitContainer1.Panel1.Controls.Add(this.ltb_tb_fullname);
            this.splitContainer1.Panel1.Controls.Add(this.ltb_pressName);
            this.splitContainer1.Size = new System.Drawing.Size(258, 197);
            this.splitContainer1.SplitterDistance = 163;
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
            this.ltb_pressName.Size = new System.Drawing.Size(258, 26);
            this.ltb_pressName.TabIndex = 0;
            // 
            // ltb_tb_fullname
            // 
            this.ltb_tb_fullname.AutoSize = true;
            this.ltb_tb_fullname.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_tb_fullname.Label = "教材全名";
            this.ltb_tb_fullname.LabelWidth = 80;
            this.ltb_tb_fullname.Location = new System.Drawing.Point(0, 26);
            this.ltb_tb_fullname.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_tb_fullname.Name = "ltb_tb_fullname";
            this.ltb_tb_fullname.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_tb_fullname.PasswordChar = '\0';
            this.ltb_tb_fullname.Size = new System.Drawing.Size(258, 26);
            this.ltb_tb_fullname.TabIndex = 1;
            // 
            // ltb_tb_shortName
            // 
            this.ltb_tb_shortName.AutoSize = true;
            this.ltb_tb_shortName.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_tb_shortName.Label = "简称";
            this.ltb_tb_shortName.LabelWidth = 80;
            this.ltb_tb_shortName.Location = new System.Drawing.Point(0, 52);
            this.ltb_tb_shortName.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_tb_shortName.Name = "ltb_tb_shortName";
            this.ltb_tb_shortName.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_tb_shortName.PasswordChar = '\0';
            this.ltb_tb_shortName.Size = new System.Drawing.Size(258, 26);
            this.ltb_tb_shortName.TabIndex = 2;
            // 
            // lcb_tb_grade
            // 
            this.lcb_tb_grade.AutoSize = true;
            this.lcb_tb_grade.Dock = System.Windows.Forms.DockStyle.Top;
            this.lcb_tb_grade.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lcb_tb_grade.Label = "适用年级";
            this.lcb_tb_grade.LabelWidth = 80;
            this.lcb_tb_grade.Location = new System.Drawing.Point(0, 78);
            this.lcb_tb_grade.MinimumSize = new System.Drawing.Size(200, 26);
            this.lcb_tb_grade.Name = "lcb_tb_grade";
            this.lcb_tb_grade.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lcb_tb_grade.Size = new System.Drawing.Size(258, 26);
            this.lcb_tb_grade.TabIndex = 3;
            // 
            // lcb_tb_semester
            // 
            this.lcb_tb_semester.AutoSize = true;
            this.lcb_tb_semester.Dock = System.Windows.Forms.DockStyle.Top;
            this.lcb_tb_semester.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lcb_tb_semester.Label = "适用学期";
            this.lcb_tb_semester.LabelWidth = 80;
            this.lcb_tb_semester.Location = new System.Drawing.Point(0, 104);
            this.lcb_tb_semester.MinimumSize = new System.Drawing.Size(200, 26);
            this.lcb_tb_semester.Name = "lcb_tb_semester";
            this.lcb_tb_semester.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lcb_tb_semester.Size = new System.Drawing.Size(258, 26);
            this.lcb_tb_semester.TabIndex = 4;
            // 
            // ldt_tb_implementdate
            // 
            this.ldt_tb_implementdate.AutoSize = true;
            this.ldt_tb_implementdate.Dock = System.Windows.Forms.DockStyle.Top;
            this.ldt_tb_implementdate.Label = "实施日期";
            this.ldt_tb_implementdate.LabelWidth = 80;
            this.ldt_tb_implementdate.Location = new System.Drawing.Point(0, 130);
            this.ldt_tb_implementdate.MinimumSize = new System.Drawing.Size(200, 26);
            this.ldt_tb_implementdate.Name = "ldt_tb_implementdate";
            this.ldt_tb_implementdate.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ldt_tb_implementdate.Size = new System.Drawing.Size(258, 26);
            this.ldt_tb_implementdate.TabIndex = 5;
            // 
            // frmTeachBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(258, 197);
            this.Name = "frmTeachBook";
            this.Text = "教科书";
            this.Load += new System.EventHandler(this.frmTeachBook_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public LearnLibs.Controls.LTB ltb_pressName;
        public LearnLibs.Controls.LTB ltb_tb_fullname;
        public LearnLibs.Controls.LTB ltb_tb_shortName;
        public LearnLibs.Controls.LCB lcb_tb_grade;
        public LearnLibs.Controls.LCB lcb_tb_semester;
        public LearnLibs.Controls.LDT ldt_tb_implementdate;

    }
}
