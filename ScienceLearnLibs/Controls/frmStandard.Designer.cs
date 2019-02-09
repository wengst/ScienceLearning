namespace LearnLibs.Controls
{
    partial class frmStandard
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
            this.ltb_parent_name = new LearnLibs.Controls.LTB();
            this.ltb_standard_code = new LearnLibs.Controls.LTB();
            this.ltb_standard_text = new LearnLibs.Controls.LTB();
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
            this.splitContainer1.Panel1.Controls.Add(this.ltb_standard_text);
            this.splitContainer1.Panel1.Controls.Add(this.ltb_standard_code);
            this.splitContainer1.Panel1.Controls.Add(this.ltb_parent_name);
            this.splitContainer1.Size = new System.Drawing.Size(379, 195);
            this.splitContainer1.SplitterDistance = 167;
            // 
            // ltb_parent_name
            // 
            this.ltb_parent_name.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_parent_name.Label = "父课标";
            this.ltb_parent_name.LabelAlign = System.Drawing.ContentAlignment.TopRight;
            this.ltb_parent_name.LabelWidth = 98;
            this.ltb_parent_name.Location = new System.Drawing.Point(0, 0);
            this.ltb_parent_name.MaxLenght = 64;
            this.ltb_parent_name.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_parent_name.MulitLine = true;
            this.ltb_parent_name.Name = "ltb_parent_name";
            this.ltb_parent_name.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_parent_name.PasswordChar = '\0';
            this.ltb_parent_name.ReadOnly = true;
            this.ltb_parent_name.Size = new System.Drawing.Size(379, 52);
            this.ltb_parent_name.TabIndex = 0;
            // 
            // ltb_standard_code
            // 
            this.ltb_standard_code.AutoSize = true;
            this.ltb_standard_code.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_standard_code.Label = "课标代码";
            this.ltb_standard_code.LabelWidth = 98;
            this.ltb_standard_code.Location = new System.Drawing.Point(0, 52);
            this.ltb_standard_code.MaxLenght = 4;
            this.ltb_standard_code.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_standard_code.Name = "ltb_standard_code";
            this.ltb_standard_code.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_standard_code.PasswordChar = '\0';
            this.ltb_standard_code.Size = new System.Drawing.Size(379, 26);
            this.ltb_standard_code.TabIndex = 1;
            // 
            // ltb_standard_text
            // 
            this.ltb_standard_text.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_standard_text.Label = "课标内容";
            this.ltb_standard_text.LabelAlign = System.Drawing.ContentAlignment.TopRight;
            this.ltb_standard_text.LabelWidth = 98;
            this.ltb_standard_text.Location = new System.Drawing.Point(0, 78);
            this.ltb_standard_text.MaxLenght = 32767;
            this.ltb_standard_text.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_standard_text.MulitLine = true;
            this.ltb_standard_text.Name = "ltb_standard_text";
            this.ltb_standard_text.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_standard_text.PasswordChar = '\0';
            this.ltb_standard_text.Size = new System.Drawing.Size(379, 89);
            this.ltb_standard_text.TabIndex = 2;
            // 
            // frmStandard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(379, 195);
            this.Name = "frmStandard";
            this.Text = "课标";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LTB ltb_parent_name;
        private LTB ltb_standard_text;
        private LTB ltb_standard_code;
    }
}
