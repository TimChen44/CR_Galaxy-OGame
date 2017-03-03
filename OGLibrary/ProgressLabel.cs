using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace OGLibrary
{
    public partial class ProgressLabel : UserControl
    {
        #region 属性

        private string _LabelText;

        public string LabelText
        {
            get { return _LabelText; }
            set
            {
                _LabelText = value;
                ProLabel.Text = value; }
        }


        public ContentAlignment TextAlign
        {
            get { return ProLabel.TextAlign; }
            set
            { ProLabel.TextAlign = value; }
        }


        public Image Image
        {
            get { return ProLabel.Image; }
            set { ProLabel.Image = value; }
        }


        public ContentAlignment ImageAlign
        {
            get { return ProLabel.ImageAlign; }
            set { ProLabel.ImageAlign = value; }
        }

        private long  _Min = 0;

        public long Min
        {
            get { return _Min; }
            set { _Min = value; }
        }

        private long _Max = 100;

        public long Max
        {
            get { return _Max; }
            set { _Max = value; }
        }

        private long _Value = 0;

        public long Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// 最左侧的颜色
        /// </summary>
        private Color _Color1 = Color.FromArgb(230, 50, 20);

        public Color Color1
        {
            get { return _Color1; }
            set { _Color1 = value; }
        }

        /// <summary>
        /// 定义色彩左侧留空量
        /// </summary>
        private int _ColorT1 = 0;

        public int ColorT1
        {
            get { return _ColorT1; }
            set { _ColorT1 = value; }
        }

        /// <summary>
        /// 最右侧的颜色
        /// </summary>
        private Color _Color2 = Color.FromArgb(20, 230, 20);

        public Color Color2
        {
            get { return _Color2; }
            set
            {
                _Color2 = value;
            }
        }

        /// <summary>
        /// 定义色彩右侧留空量
        /// </summary>
        private int _ColorT2 = 0;

        public int ColorT2
        {
            get { return _ColorT2; }
            set { _ColorT2 = value; }
        }

        /// <summary>
        /// 是否显示百分比
        /// </summary>
        private bool _Percentage = false;

        public bool Percentage
        {
            get { return _Percentage; }
            set { _Percentage = value; }
        }

        #endregion

        #region 全局变量



        #endregion

        public ProgressLabel()
        {
            InitializeComponent();
        }

        public void ParPaint()
        {

        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {


        }

        private void ProLabel_Click(object sender, EventArgs e)
        {

        }

        private void ProgressLabel_Paint(object sender, PaintEventArgs e)
        {
            Point P1 = new Point(0 + _ColorT1, 0);
            Point P2 = new Point(this.Width - _ColorT2, 0);
            System.Drawing.Drawing2D.LinearGradientBrush _lgbBrush;
            _lgbBrush = new System.Drawing.Drawing2D.LinearGradientBrush(P1, P2, _Color1, _Color2);

            e.Graphics.FillRectangle(_lgbBrush, 0 + _ColorT1, 0, ((float)(this.Width - _ColorT1 - _ColorT2) * (float)_Value / (float)_Max), this.Height);

            if (_Percentage == true)
            {
                ProLabel.Text = (((float)_Value / (float)_Max).ToString("(0.0%)") + _LabelText);
            }

        }
    }
}
