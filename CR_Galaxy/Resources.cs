using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CR_Galaxy
{
    //定义委托
    public delegate void btnClickEventHander(object sender, EventArgs e);

    public partial class Resources : UserControl
    {
        public event btnClickEventHander btnOkClick;

        private int _Planet;
    
        public Resources()
        {
            InitializeComponent();
        }

        public int Planet
        {
            get
            {
                return _Planet;
            }
            set
            {
                _Planet = value;
                LabPlanet.Text = "星球" + _Planet;
            }
        }

        public int GetMetalAll
        {
            get
            {
                return Convert.ToInt32(MetalAll.Text); 
            }
        }

        public int GetMetalDay
        {
            get
            {
                return Convert.ToInt32(MetalDay.Text); 
            }
        }

        public int GetCrystalAll
        {
            get
            {
                return Convert.ToInt32(CrystalAll.Text); 
            }
        }

        public int GetCrystalDay
        {
            get
            {
                return Convert.ToInt32(CrystalDay.Text); 
            }
        }

        public int GetHHAll
        {
            get
            {
                return Convert.ToInt32(HHAll.Text); 
            }
        }

        public int GetHHDay
        {
            get
            {
                return Convert.ToInt32(HHDay.Text); 
            }
        }

        private void MetalLeave_SelectedIndexChanged(object sender, EventArgs e)
        {
            //每小時產量 = 30 * 等級 * （1.1 ^ 等級）
            Metal.Text =Convert.ToString(Math.Floor(30 * (MetalLeave.SelectedIndex + 1) *  Math.Pow(1.1,(MetalLeave.SelectedIndex + 1))));
            Calculation();
            if (btnOkClick != null)
                btnOkClick(this, e);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CrystalLeave_SelectedIndexChanged(object sender, EventArgs e)
        {
            //每小時產量 = 20 * 等級 * （1.1 ^ 等級）
            Crystal.Text = Convert.ToString(Math.Floor(20 * (CrystalLeave.SelectedIndex + 1) * Math.Pow(1.1, (CrystalLeave.SelectedIndex + 1))));
            Calculation();
            if (btnOkClick != null)
                btnOkClick(this, e);
        }

        private void HHLeave_SelectedIndexChanged(object sender, EventArgs e)
        {
            //每小時產量 = 10 * 等級 * （1.1 ^ 等級） * （-0.002 * 最高溫度 + 1.28） 
            HH.Text = Convert.ToString(Convert.ToInt32(10 * (HHLeave.SelectedIndex + 1) * Math.Pow(1.1, (HHLeave.SelectedIndex + 1)) * (-0.002 * Convert.ToInt32(Temperature.Text) + 1.28)));
            Calculation();
            if (btnOkClick != null)
                btnOkClick(this, e);
        }

        private void Calculation()
        {
            MetalAll.Text = Convert.ToString(Convert.ToInt32(MetalBase.Text) + Convert.ToInt32(Metal.Text));
            MetalDay.Text = Convert.ToString(Convert.ToInt32(MetalAll.Text) * 24);

            CrystalAll.Text = Convert.ToString(Convert.ToInt32(CrystalBase.Text) + Convert.ToInt32(Crystal.Text));
            CrystalDay.Text = Convert.ToString(Convert.ToInt32(CrystalAll.Text) * 24);

            HHAll.Text =  Convert.ToString(Convert.ToInt32(HH.Text) + Convert.ToInt32(NuclearPower.Text));
            HHDay.Text = Convert.ToString(Convert.ToInt32(HHAll.Text) * 24);
        }

        private void NuclearPowerLeave_SelectedIndexChanged(object sender, EventArgs e)
        {
            //重氫消耗 = 10 * 等級 * （1.1 ^ 等級）
            NuclearPower.Text = Convert.ToString(Convert.ToInt32(10 * (NuclearPowerLeave.SelectedIndex + 1) * Math.Pow(1.1, (NuclearPowerLeave.SelectedIndex + 1))) * -1);
            Calculation();
            if (btnOkClick != null)
                btnOkClick(this, e);
        }

        public void SaveFile()
        {
            File.WriteAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\Data\\ResPlanet" + _Planet.ToString() + ".txt",
                new string[] { Temperature.Text, MetalLeave.SelectedIndex.ToString(), CrystalLeave.SelectedIndex.ToString(), HHLeave.SelectedIndex.ToString(), NuclearPowerLeave.SelectedIndex.ToString()});
        }

        public void LoadFile()
        {
            string[] SP = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\Data\\ResPlanet" + _Planet.ToString() + ".txt");
            if (SP.Length >=5)
            {
                Temperature.Text = SP[0];
                MetalLeave.SelectedIndex = Convert.ToInt32(SP[1]);
                CrystalLeave.SelectedIndex = Convert.ToInt32(SP[2]);
                HHLeave.SelectedIndex = Convert.ToInt32(SP[3]);
                NuclearPowerLeave.SelectedIndex = Convert.ToInt32(SP[4]);
            }
        }
    }
}
