namespace LearnLibs.Controls
{
    partial class LCB
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
            this.CMB = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.CMB);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.splitContainer1.Size = new System.Drawing.Size(321, 23);
            // 
            // CMB
            // 
            this.CMB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CMB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB.FormattingEnabled = true;
            this.CMB.Location = new System.Drawing.Point(0, 0);
            this.CMB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CMB.Name = "CMB";
            this.CMB.Size = new System.Drawing.Size(217, 23);
            this.CMB.TabIndex = 0;
            // 
            // LabelComboBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label = "Items";
            this.Name = "LabelComboBox";
            this.Size = new System.Drawing.Size(321, 27);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ComboBox CMB;
    }
}
