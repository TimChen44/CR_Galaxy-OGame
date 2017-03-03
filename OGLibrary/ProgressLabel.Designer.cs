namespace OGLibrary
{
    partial class ProgressLabel
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ProLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ProLabel
            // 
            this.ProLabel.BackColor = System.Drawing.Color.Transparent;
            this.ProLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProLabel.Location = new System.Drawing.Point(0, 0);
            this.ProLabel.Name = "ProLabel";
            this.ProLabel.Size = new System.Drawing.Size(267, 25);
            this.ProLabel.TabIndex = 1;
            this.ProLabel.Text = "label1";
            this.ProLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ProgressLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ProLabel);
            this.Name = "ProgressLabel";
            this.Size = new System.Drawing.Size(267, 25);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ProgressLabel_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ProLabel;

    }
}
