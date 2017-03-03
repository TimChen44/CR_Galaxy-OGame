namespace CR_Galaxy
{
    partial class Charts
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
            this.Top = new System.Windows.Forms.Panel();
            this.Bottom = new System.Windows.Forms.Panel();
            this.Left = new System.Windows.Forms.Panel();
            this.Right = new System.Windows.Forms.Panel();
            this.Main = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Main)).BeginInit();
            this.SuspendLayout();
            // 
            // Top
            // 
            this.Top.Dock = System.Windows.Forms.DockStyle.Top;
            this.Top.Location = new System.Drawing.Point(0, 0);
            this.Top.Name = "Top";
            this.Top.Size = new System.Drawing.Size(700, 25);
            this.Top.TabIndex = 0;
            // 
            // Bottom
            // 
            this.Bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Bottom.Location = new System.Drawing.Point(0, 290);
            this.Bottom.Name = "Bottom";
            this.Bottom.Size = new System.Drawing.Size(700, 10);
            this.Bottom.TabIndex = 1;
            // 
            // Left
            // 
            this.Left.Dock = System.Windows.Forms.DockStyle.Left;
            this.Left.Location = new System.Drawing.Point(0, 25);
            this.Left.Name = "Left";
            this.Left.Size = new System.Drawing.Size(50, 265);
            this.Left.TabIndex = 2;
            // 
            // Right
            // 
            this.Right.Dock = System.Windows.Forms.DockStyle.Right;
            this.Right.Location = new System.Drawing.Point(650, 25);
            this.Right.Name = "Right";
            this.Right.Size = new System.Drawing.Size(50, 265);
            this.Right.TabIndex = 3;
            // 
            // Main
            // 
            this.Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Main.Location = new System.Drawing.Point(50, 25);
            this.Main.Name = "Main";
            this.Main.Size = new System.Drawing.Size(600, 265);
            this.Main.TabIndex = 4;
            this.Main.TabStop = false;
            this.Main.Click += new System.EventHandler(this.Main_Click);
            // 
            // Charts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Main);
            this.Controls.Add(this.Right);
            this.Controls.Add(this.Left);
            this.Controls.Add(this.Bottom);
            this.Controls.Add(this.Top);
            this.Name = "Charts";
            this.Size = new System.Drawing.Size(700, 300);
            ((System.ComponentModel.ISupportInitialize)(this.Main)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Top;
        private System.Windows.Forms.Panel Bottom;
        private System.Windows.Forms.Panel Left;
        private System.Windows.Forms.Panel Right;
        private System.Windows.Forms.PictureBox Main;
    }
}
