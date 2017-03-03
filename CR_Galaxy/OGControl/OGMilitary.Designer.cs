namespace CR_Galaxy.OGControl
{
    partial class OGMilitary
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FleetFlyList = new System.Windows.Forms.DataGridView();
            this.FleetFlyListTimer = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.FSspeed = new System.Windows.Forms.ComboBox();
            this.FSplanet = new System.Windows.Forms.NumericUpDown();
            this.FSsystem = new System.Windows.Forms.NumericUpDown();
            this.FSgalaxy = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.AutoFSTimer = new System.Windows.Forms.Timer(this.components);
            this.FSList = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FleetFlyList)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FSplanet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FSsystem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FSgalaxy)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FleetFlyList);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1000, 314);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "舰队列表";
            // 
            // FleetFlyList
            // 
            this.FleetFlyList.AllowUserToAddRows = false;
            this.FleetFlyList.AllowUserToDeleteRows = false;
            this.FleetFlyList.AllowUserToResizeRows = false;
            this.FleetFlyList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.FleetFlyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FleetFlyList.Location = new System.Drawing.Point(3, 17);
            this.FleetFlyList.Name = "FleetFlyList";
            this.FleetFlyList.ReadOnly = true;
            this.FleetFlyList.RowHeadersVisible = false;
            this.FleetFlyList.RowTemplate.Height = 23;
            this.FleetFlyList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.FleetFlyList.Size = new System.Drawing.Size(994, 294);
            this.FleetFlyList.TabIndex = 0;
            // 
            // FleetFlyListTimer
            // 
            this.FleetFlyListTimer.Interval = 30000;
            this.FleetFlyListTimer.Tick += new System.EventHandler(this.FleetFlyListTimer_Tick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1000, 579);
            this.splitContainer1.SplitterDistance = 314;
            this.splitContainer1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1000, 261);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(992, 236);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "发送舰队";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 26F);
            this.label5.Location = new System.Drawing.Point(348, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 35);
            this.label5.TabIndex = 0;
            this.label5.Text = "开发中";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.FSList);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.FSspeed);
            this.tabPage2.Controls.Add(this.FSplanet);
            this.tabPage2.Controls.Add(this.FSsystem);
            this.tabPage2.Controls.Add(this.FSgalaxy);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(992, 236);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "舰队情况";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "自动FS";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(526, 130);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "出发";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(494, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "速  度";
            this.label4.Visible = false;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // FSspeed
            // 
            this.FSspeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FSspeed.FormattingEnabled = true;
            this.FSspeed.Items.AddRange(new object[] {
            "10",
            "9",
            "8",
            "7",
            "6",
            "5",
            "4",
            "3",
            "2",
            "1"});
            this.FSspeed.Location = new System.Drawing.Point(541, 104);
            this.FSspeed.Name = "FSspeed";
            this.FSspeed.Size = new System.Drawing.Size(60, 20);
            this.FSspeed.TabIndex = 7;
            this.FSspeed.Visible = false;
            this.FSspeed.SelectedIndexChanged += new System.EventHandler(this.FSspeed_SelectedIndexChanged);
            // 
            // FSplanet
            // 
            this.FSplanet.Location = new System.Drawing.Point(541, 77);
            this.FSplanet.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.FSplanet.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FSplanet.Name = "FSplanet";
            this.FSplanet.Size = new System.Drawing.Size(60, 21);
            this.FSplanet.TabIndex = 6;
            this.FSplanet.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.FSplanet.Visible = false;
            this.FSplanet.ValueChanged += new System.EventHandler(this.FSplanet_ValueChanged);
            // 
            // FSsystem
            // 
            this.FSsystem.Location = new System.Drawing.Point(541, 50);
            this.FSsystem.Maximum = new decimal(new int[] {
            499,
            0,
            0,
            0});
            this.FSsystem.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FSsystem.Name = "FSsystem";
            this.FSsystem.Size = new System.Drawing.Size(60, 21);
            this.FSsystem.TabIndex = 5;
            this.FSsystem.Value = new decimal(new int[] {
            276,
            0,
            0,
            0});
            this.FSsystem.Visible = false;
            this.FSsystem.ValueChanged += new System.EventHandler(this.FSsystem_ValueChanged);
            // 
            // FSgalaxy
            // 
            this.FSgalaxy.Location = new System.Drawing.Point(541, 23);
            this.FSgalaxy.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.FSgalaxy.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FSgalaxy.Name = "FSgalaxy";
            this.FSgalaxy.Size = new System.Drawing.Size(60, 21);
            this.FSgalaxy.TabIndex = 4;
            this.FSgalaxy.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.FSgalaxy.Visible = false;
            this.FSgalaxy.ValueChanged += new System.EventHandler(this.FSgalaxy_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(494, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "星  球";
            this.label3.Visible = false;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(494, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "太阳系";
            this.label2.Visible = false;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(494, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "银河系";
            this.label1.Visible = false;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // AutoFSTimer
            // 
            this.AutoFSTimer.Interval = 5000;
            this.AutoFSTimer.Tick += new System.EventHandler(this.AutoFSTimer_Tick);
            // 
            // FSList
            // 
            this.FSList.FormattingEnabled = true;
            this.FSList.ItemHeight = 12;
            this.FSList.Location = new System.Drawing.Point(87, 14);
            this.FSList.Name = "FSList";
            this.FSList.Size = new System.Drawing.Size(401, 208);
            this.FSList.TabIndex = 11;
            // 
            // OGMilitary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "OGMilitary";
            this.Size = new System.Drawing.Size(1000, 579);
            this.Load += new System.EventHandler(this.OGMilitary_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FleetFlyList)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FSplanet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FSsystem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FSgalaxy)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.DataGridView FleetFlyList;
        private System.Windows.Forms.Timer FleetFlyListTimer;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.NumericUpDown FSplanet;
        private System.Windows.Forms.NumericUpDown FSsystem;
        private System.Windows.Forms.NumericUpDown FSgalaxy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox FSspeed;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Timer AutoFSTimer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox FSList;
    }
}
