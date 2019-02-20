namespace LearnLibs.Controls
{
    partial class frmQuestionEditor
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
            this.lcbAnswerMode = new LearnLibs.Controls.LCB();
            this.ltbOptionChars = new LearnLibs.Controls.LTB();
            this.ltbKeyChars = new LearnLibs.Controls.LTB();
            this.ltnDifficult = new LearnLibs.Controls.LTN();
            this.ltnScore = new LearnLibs.Controls.LTN();
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
            this.splitContainer1.Panel1.Controls.Add(this.ltnScore);
            this.splitContainer1.Panel1.Controls.Add(this.ltnDifficult);
            this.splitContainer1.Panel1.Controls.Add(this.ltbKeyChars);
            this.splitContainer1.Panel1.Controls.Add(this.ltbOptionChars);
            this.splitContainer1.Panel1.Controls.Add(this.lcbAnswerMode);
            this.splitContainer1.Size = new System.Drawing.Size(357, 160);
            this.splitContainer1.SplitterDistance = 132;
            // 
            // lcbAnswerMode
            // 
            this.lcbAnswerMode.AutoSize = true;
            this.lcbAnswerMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.lcbAnswerMode.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lcbAnswerMode.Label = "题型";
            this.lcbAnswerMode.LabelWidth = 80;
            this.lcbAnswerMode.Location = new System.Drawing.Point(0, 0);
            this.lcbAnswerMode.MinimumSize = new System.Drawing.Size(200, 26);
            this.lcbAnswerMode.Name = "lcbAnswerMode";
            this.lcbAnswerMode.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lcbAnswerMode.Size = new System.Drawing.Size(357, 26);
            this.lcbAnswerMode.TabIndex = 0;
            // 
            // ltbOptionChars
            // 
            this.ltbOptionChars.AutoSize = true;
            this.ltbOptionChars.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltbOptionChars.Label = "选项字符";
            this.ltbOptionChars.LabelWidth = 80;
            this.ltbOptionChars.Location = new System.Drawing.Point(0, 26);
            this.ltbOptionChars.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltbOptionChars.Name = "ltbOptionChars";
            this.ltbOptionChars.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltbOptionChars.PasswordChar = '\0';
            this.ltbOptionChars.PlaceHolder = "";
            this.ltbOptionChars.Size = new System.Drawing.Size(357, 26);
            this.ltbOptionChars.TabIndex = 1;
            // 
            // ltbKeyChars
            // 
            this.ltbKeyChars.AutoSize = true;
            this.ltbKeyChars.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltbKeyChars.Label = "答案";
            this.ltbKeyChars.LabelWidth = 80;
            this.ltbKeyChars.Location = new System.Drawing.Point(0, 52);
            this.ltbKeyChars.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltbKeyChars.Name = "ltbKeyChars";
            this.ltbKeyChars.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltbKeyChars.PasswordChar = '\0';
            this.ltbKeyChars.PlaceHolder = "";
            this.ltbKeyChars.Size = new System.Drawing.Size(357, 26);
            this.ltbKeyChars.TabIndex = 2;
            // 
            // ltnDifficult
            // 
            this.ltnDifficult.AutoSize = true;
            this.ltnDifficult.DecimalPlaces = 2;
            this.ltnDifficult.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltnDifficult.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ltnDifficult.Label = "难度系数";
            this.ltnDifficult.LabelWidth = 80;
            this.ltnDifficult.Location = new System.Drawing.Point(0, 78);
            this.ltnDifficult.Maxinum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.ltnDifficult.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltnDifficult.Mininum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ltnDifficult.Name = "ltnDifficult";
            this.ltnDifficult.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltnDifficult.Size = new System.Drawing.Size(357, 26);
            this.ltnDifficult.TabIndex = 3;
            this.ltnDifficult.Value = new decimal(new int[] {
            7,
            0,
            0,
            65536});
            // 
            // ltnScore
            // 
            this.ltnScore.AutoSize = true;
            this.ltnScore.DecimalPlaces = 1;
            this.ltnScore.Dock = System.Windows.Forms.DockStyle.Top;
            this.ltnScore.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ltnScore.Label = "分值";
            this.ltnScore.LabelWidth = 80;
            this.ltnScore.Location = new System.Drawing.Point(0, 104);
            this.ltnScore.Maxinum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ltnScore.MinimumSize = new System.Drawing.Size(200, 26);
            this.ltnScore.Mininum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ltnScore.Name = "ltnScore";
            this.ltnScore.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ltnScore.Size = new System.Drawing.Size(357, 26);
            this.ltnScore.TabIndex = 4;
            this.ltnScore.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // frmQuestionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(357, 160);
            this.Name = "frmQuestionEditor";
            this.Text = "习题小问编辑器";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LCB lcbAnswerMode;
        private LTN ltnDifficult;
        private LTB ltbKeyChars;
        private LTB ltbOptionChars;
        private LTN ltnScore;
    }
}
