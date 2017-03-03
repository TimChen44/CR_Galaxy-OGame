using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CR_Galaxy
{
    public partial class Charts : UserControl
    {
        private DataTable _Data;
        private int _MinF;//第一个坐标轴最小值
        private int _MaxF;//第一个坐标做最大值
        private int _MinS;//第二个坐标轴最小值
        private int _MaxS;//第二个坐标做最大值
    
        public Charts()
        {
            InitializeComponent();
        }

        public DataTable Data
        {
            set
            {
                _Data = value;
            }
        }


        private void GetMinMax()
        {



        }

        private void Main_Click(object sender, EventArgs e)
        {

        }
    }
}
