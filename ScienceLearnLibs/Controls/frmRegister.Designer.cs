namespace LearnLibs.Controls
{
    partial class frmRegister
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
            this.ltbUserName = new LearnLibs.Controls.LTB();
            this.ltbPassword = new LearnLibs.Controls.LTB();
            this.ltbRePassword = new LearnLibs.Controls.LTB();
            this.selectRoles1 = new LearnLibs.Controls.SelectRoles();
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
            this.splitContainer1.Panel1.Controls.Add(this.selectRoles1);
            this.splitContainer1.Panel1.Controls.Add(this.ltbRePassword);
            this.splitContainer1.Panel1.Controls.Add(this.ltbPassword);
            this.splitContainer1.Panel1.Controls.Add(this.ltbUserName);
            this.splitContainer1.Size = new System.Drawing.Size(250, 136);
            this.splitContainer1.SplitterDistance = 108;
            // 
            // ltbUserName
            // 
            this.ltbUserName.AutoSize = true;
            this.ltbUserName.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltbUserName.Label = "用户名";
            this.ltbUserName.LabelWidth = 98;
            this.ltbUserName.Location = new System.Drawing.Point(0, 0);
            this.ltbUserName.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltbUserName.Name = "ltbUserName";
            this.ltbUserName.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltbUserName.PasswordChar = '\0';
            this.ltbUserName.Size = new System.Drawing.Size(250, 26);
            this.ltbUserName.TabIndex = 0;
            // 
            // ltbPassword
            // 
            this.ltbPassword.AutoSize = true;
            this.ltbPassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltbPassword.Label = "密码";
            this.ltbPassword.LabelWidth = 98;
            this.ltbPassword.Location = new System.Drawing.Point(0, 26);
            this.ltbPassword.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltbPassword.Name = "ltbPassword";
            this.ltbPassword.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltbPassword.PasswordChar = '*';
            this.ltbPassword.Size = new System.Drawing.Size(250, 26);
            this.ltbPassword.TabIndex = 1;
            // 
            // ltbRePassword
            // 
            this.ltbRePassword.AutoSize = true;
            this.ltbRePassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltbRePassword.Label = "重复密码";
            this.ltbRePassword.LabelWidth = 98;
            this.ltbRePassword.Location = new System.Drawing.Point(0, 52);
            this.ltbRePassword.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltbRePassword.Name = "ltbRePassword";
            this.ltbRePassword.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltbRePassword.PasswordChar = '*';
            this.ltbRePassword.Size = new System.Drawing.Size(250, 26);
            this.ltbRePassword.TabIndex = 2;
            // 
            // selectRoles1
            // 
            this.selectRoles1.AutoSize = true;
            this.selectRoles1.Dock = System.Windows.Forms.DockStyle.Top;
            this.selectRoles1.Label = "角色";
            this.selectRoles1.LabelWidth = 98;
            this.selectRoles1.Location = new System.Drawing.Point(0, 78);
            this.selectRoles1.MinimumSize = new System.Drawing.Size(200, 26);
            this.selectRoles1.Name = "selectRoles1";
            this.selectRoles1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.selectRoles1.Size = new System.Drawing.Size(250, 26);
            this.selectRoles1.TabIndex = 3;
            // 
            // frmRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(250, 136);
            this.Name = "frmRegister";
            this.Text = "更新账户信息";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LTB ltbRePassword;
        private LTB ltbPassword;
        private LTB ltbUserName;
        private SelectRoles selectRoles1;
    }
}
