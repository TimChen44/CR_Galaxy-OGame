using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CR_Galaxy
{
    public partial class Login : Form
    {
        private DialogResult _Result=DialogResult.No;

        public Login()
        {
            InitializeComponent();
        }

        public DialogResult Result
        {
            get
            {
                return _Result;
            }
        }

        public string Server
        {
            get
            {
                return LoginServer.Text;
            }
        }

        public string U
        {
            get
            {
                return LoginU.Text;
            }
        }

        public string UserName
        {
            get
            {
                return LoginName.Text;
            }
        }

        public string UserPass
        {
            get
            {
                return LoginPass.Text;
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            try
            {


           string[] SP= File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\Data\\Pass.txt");

                if (SP[0] == "T")
                {
                    SavePass.Checked = true;
                    LoginServer.Text = SP[1];
                    LoginU.Text = SP[2];
                    LoginName.Text = SP[3];
                    LoginPass.Text = SP[4];
                }
            }
            catch (Exception)
            {

              
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _Result = DialogResult.No;
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void  button1_Click(object sender, EventArgs e)
        {
             //;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if ((LoginServer.Text == "") || (LoginU.Text == "") || (LoginName.Text == "") || (LoginPass.Text == ""))
            {
                MessageBox.Show("请先填写完整数据");
                return;
            }
            if (SavePass.Checked == true)
            {
                File.WriteAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\Data\\Pass.txt", new string[] { "T", LoginServer.Text, LoginU.Text, LoginName.Text, LoginPass.Text });
            }
            else
            {
                File.WriteAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "\\Data\\Pass.txt", new string[] { "F", "", "", "", "" });
            };

            _Result = DialogResult.OK;
            Close();
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void SavePass_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SavePass_Click(object sender, EventArgs e)
        {
            if (SavePass.Checked == true)
            {
                MessageBox.Show("使用保存密码功能时密码将会用明码方式保存在安装目录\\Data文件夹中。");
            }
        }
    }
}