namespace LearnLibs.Controls
{
    partial class frmSchoolSelector
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
            this.lcbProvinces = new LearnLibs.Controls.LCB();
            this.lcbCities = new LearnLibs.Controls.LCB();
            this.lcbDistricts = new LearnLibs.Controls.LCB();
            this.listBox1 = new System.Windows.Forms.ListBox();
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
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            this.splitContainer1.Panel1.Controls.Add(this.lcbDistricts);
            this.splitContainer1.Panel1.Controls.Add(this.lcbCities);
            this.splitContainer1.Panel1.Controls.Add(this.lcbProvinces);
            this.splitContainer1.Size = new System.Drawing.Size(239, 372);
            this.splitContainer1.SplitterDistance = 344;
            // 
            // lcbProvinces
            // 
            this.lcbProvinces.AutoSize = true;
            this.lcbProvinces.Dock = System.Windows.Forms.DockStyle.Top;
            this.lcbProvinces.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lcbProvinces.Label = "省";
            this.lcbProvinces.LabelWidth = 40;
            this.lcbProvinces.Location = new System.Drawing.Point(0, 0);
            this.lcbProvinces.MinimumSize = new System.Drawing.Size(200, 26);
            this.lcbProvinces.Name = "lcbProvinces";
            this.lcbProvinces.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lcbProvinces.Size = new System.Drawing.Size(239, 26);
            this.lcbProvinces.TabIndex = 0;
            // 
            // lcbCities
            // 
            this.lcbCities.AutoSize = true;
            this.lcbCities.Dock = System.Windows.Forms.DockStyle.Top;
            this.lcbCities.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lcbCities.Label = "市";
            this.lcbCities.LabelWidth = 40;
            this.lcbCities.Location = new System.Drawing.Point(0, 26);
            this.lcbCities.MinimumSize = new System.Drawing.Size(200, 26);
            this.lcbCities.Name = "lcbCities";
            this.lcbCities.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lcbCities.Size = new System.Drawing.Size(239, 26);
            this.lcbCities.TabIndex = 0;
            // 
            // lcbDistricts
            // 
            this.lcbDistricts.AutoSize = true;
            this.lcbDistricts.Dock = System.Windows.Forms.DockStyle.Top;
            this.lcbDistricts.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lcbDistricts.Label = "区县";
            this.lcbDistricts.LabelWidth = 40;
            this.lcbDistricts.Location = new System.Drawing.Point(0, 52);
            this.lcbDistricts.MinimumSize = new System.Drawing.Size(200, 26);
            this.lcbDistricts.Name = "lcbDistricts";
            this.lcbDistricts.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lcbDistricts.Size = new System.Drawing.Size(239, 26);
            this.lcbDistricts.TabIndex = 0;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(0, 78);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(239, 266);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // frmSchoolSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(239, 372);
            this.Name = "frmSchoolSelector";
            this.Text = "学校选择器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSchoolSelector_FormClosing);
            this.Load += new System.EventHandler(this.frmSchoolSelector_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private LCB lcbDistricts;
        private LCB lcbCities;
        private LCB lcbProvinces;
    }
}
