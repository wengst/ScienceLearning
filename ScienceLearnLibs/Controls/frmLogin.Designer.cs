namespace LearnLibs.Controls
{
    partial class frmLogin
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
            this.ltb_username = new LearnLibs.Controls.LTB();
            this.ltb_password = new LearnLibs.Controls.LTB();
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
            this.splitContainer1.Panel1.Controls.Add(this.ltb_password);
            this.splitContainer1.Panel1.Controls.Add(this.ltb_username);
            this.splitContainer1.Size = new System.Drawing.Size(263, 91);
            this.splitContainer1.SplitterDistance = 63;
            // 
            // ltb_username
            // 
            this.ltb_username.AutoSize = true;
            this.ltb_username.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_username.Label = "用户名";
            this.ltb_username.LabelWidth = 98;
            this.ltb_username.Location = new System.Drawing.Point(0, 0);
            this.ltb_username.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_username.Name = "ltb_username";
            this.ltb_username.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_username.PasswordChar = '\0';
            this.ltb_username.Size = new System.Drawing.Size(263, 26);
            this.ltb_username.TabIndex = 0;
            // 
            // ltb_password
            // 
            this.ltb_password.AutoSize = true;
            this.ltb_password.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltb_password.Label = "密码";
            this.ltb_password.LabelWidth = 98;
            this.ltb_password.Location = new System.Drawing.Point(0, 26);
            this.ltb_password.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltb_password.Name = "ltb_password";
            this.ltb_password.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltb_password.PasswordChar = '*';
            this.ltb_password.Size = new System.Drawing.Size(263, 26);
            this.ltb_password.TabIndex = 1;
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(263, 91);
            this.Name = "frmLogin";
            this.Text = "登录";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LTB ltb_username;
        private LTB ltb_password;
    }
}
