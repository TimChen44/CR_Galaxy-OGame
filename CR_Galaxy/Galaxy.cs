using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CR_Soft.ClassLibrary.Log;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using System.Reflection;

namespace CR_Galaxy
{
    public partial class Galaxy : Form
    {
        //  public bool _ScanningState;
        public Scanning _Scanning;
        private ScanningRankings _ScaRankings;
        //public string _EndUrl;
        private IOData _IOData;
        private Log Log;

        public Navigate _Navigate;
        private ServerInfo _SerInfo;//服务器信息

        private bool ControlLoad = false;//是否第一次加载，是就初始化控件

        public WebBrowser MsgWB = new WebBrowser();//群体信件发送，临时
        public int UserIndex=0;
        public int MsgCount = 0;
        //发送消息
        //public string MsgStr = "http://uni8.ogame.cn.com/game/index.php?page=writemessages&session={0}&gesendet=1&messageziel={1}&to={2}&betreff={3}&text={4}";
        public bool MsgCancel = false;

        public string _Session;

        //总体控制模块
        public OGControl.OGControlManage _OGControlManage = new CR_Galaxy.OGControl.OGControlManage();
        //舰队控制模块，用来统筹控制舰队
       public OGControl.FlottenCommand _FlottenCmd = new CR_Galaxy.OGControl.FlottenCommand();

        public Galaxy()
        {
            InitializeComponent();
            MsgWB.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(MsgWB_DocumentCompleted);
        }

        //短信浏览器处理
        void MsgWB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //MsgCount++;
            //label9.Text = "已经发送" + MsgCount.ToString();
            //UserIndex++;
            //if (UserIndex >= userList.Items.Count) UserIndex = 0;
            //Random ro=new Random(unchecked((int)DateTime.Now.Ticks));

            //TxtLog.Text += "\r\n" + string.Format(MsgStr, new string[] { _Session, userList.Items[UserIndex].SubItems[1].Text, userList.Items[UserIndex].Text, "无标题", msgList.Lines[ro.Next(msgList.Lines.Length)] });
            //if (MsgCancel==false)
            //    MsgWB.Navigate(string.Format(MsgStr, new string[] { _Session, userList.Items[UserIndex].SubItems[1].Text, userList.Items[UserIndex].Text, "无标题", msgList.Lines[ro.Next(msgList.Lines.Length)] }));
        
        
        }

        private void Galaxy_Load(object sender, EventArgs e)
        {


            Log = new Log(TxtLog);
            _SerInfo = new ServerInfo();
            _Navigate = new Navigate();
            _IOData = new IOData(Path.GetDirectoryName(Application.ExecutablePath) + "\\Data\\Date.mdb", Log);
            _Scanning = new Scanning(GS_Web, _IOData, Log, _SerInfo);
            _ScaRankings = new ScanningRankings(_Navigate, _SerInfo, GS_Web, _IOData, Log);

            GridMain.AutoGenerateColumns = false;//不自动填充列
            GridPlanet.AutoGenerateColumns = false;
            GridNeighbors.AutoGenerateColumns = false;
            GridUserEes.AutoGenerateColumns = false;
            GridUserFlt.AutoGenerateColumns = false;
            GridUserPts.AutoGenerateColumns = false;
            GridUnionEes.AutoGenerateColumns = false;
            GridUnionFlt.AutoGenerateColumns = false;
            GridUnionPts.AutoGenerateColumns = false;

            //设置参数导入
            _IOData.SetRelationsRead(SetFriendList, SetEnemyList, SetUnionList, SetEnemyUnionList);
            DataTable DT = _IOData.SetMy();

            SetMyUnion.Text = DT.Rows[0]["My"].ToString();

            ShowMyUnion.Checked = (bool)DT.Rows[0]["MyUnion"];
            ShowFriend.Checked = (bool)DT.Rows[0]["Friend"];
            ShowEnemy.Checked = (bool)DT.Rows[0]["Enemy"];
            ShowUnion.Checked = (bool)DT.Rows[0]["Union"];
            ShowEnemyUnion.Checked = (bool)DT.Rows[0]["EnemyUnion"];
            ShowiL.Checked = (bool)DT.Rows[0]["i"];
            ShowV.Checked = (bool)DT.Rows[0]["u"];
            ShowG.Checked = (bool)DT.Rows[0]["g"];

            //设置按钮的背景色
            SetMyUnionColor.BackColor = Color.FromArgb((int)DT.Rows[0]["MyUnionColor"]);

            SetFriendColor.BackColor = Color.FromArgb((int)DT.Rows[0]["FriendColor"]);
            SetEnemyColor.BackColor = Color.FromArgb((int)DT.Rows[0]["EnemyColor"]);
            SetUnionColor.BackColor = Color.FromArgb((int)DT.Rows[0]["UnionColor"]);
            SetEnemyUnionColor.BackColor = Color.FromArgb((int)DT.Rows[0]["EnemyUnionColor"]);
            SetLiColor.BackColor = Color.FromArgb((int)DT.Rows[0]["iColor"]);
            SetuColor.BackColor = Color.FromArgb((int)DT.Rows[0]["uColor"]);
            SetgColor.BackColor = Color.FromArgb((int)DT.Rows[0]["gColor"]);
            SetGridBack.BackColor = Color.FromArgb((int)DT.Rows[0]["GridBack"]);
            SetGridFore.BackColor = Color.FromArgb((int)DT.Rows[0]["SetGridFore"]);
            SetCustom1Color.BackColor = Color.FromArgb((int)DT.Rows[0]["Custom1Color"]);
            SetCustom2Color.BackColor = Color.FromArgb((int)DT.Rows[0]["Custom2Color"]);

            //设置背景色和前景色
            GridMain.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            GridPlanet.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            GridNeighbors.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            GridMain.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            GridPlanet.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            GridNeighbors.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;

            GridUserEes.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            GridUserFlt.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            GridUserPts.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            GridUnionEes.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            GridUnionFlt.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            GridUnionPts.RowTemplate.DefaultCellStyle.BackColor = SetGridBack.BackColor;

            GridUserEes.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            GridUserFlt.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            GridUserPts.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            GridUnionEes.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            GridUnionFlt.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            GridUnionPts.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            //结束
        }

        private void Galaxy_FormClosing(object sender, FormClosingEventArgs e)
        {
            _IOData.SetWrite(SetFriendList, SetEnemyList, SetUnionList, SetEnemyUnionList, new string[] { 
            SetMyUnion.Text,
            ShowMyUnion.Checked.ToString(),
            ShowFriend.Checked.ToString(),
            ShowEnemy.Checked.ToString(),
            ShowUnion.Checked.ToString(),
            ShowEnemyUnion.Checked.ToString(),
            ShowiL.Checked.ToString(),
            ShowV.Checked.ToString(),
            ShowG.Checked.ToString(),
            SetMyUnionColor.BackColor.ToArgb().ToString(),
            SetFriendColor.BackColor.ToArgb().ToString(),
            SetEnemyColor.BackColor.ToArgb().ToString(),
            SetUnionColor.BackColor.ToArgb().ToString(),
            SetEnemyUnionColor.BackColor.ToArgb().ToString(),
            SetLiColor.BackColor.ToArgb().ToString(),
            SetuColor.BackColor.ToArgb().ToString(),
            SetgColor.BackColor.ToArgb().ToString(),
            SetGridBack.BackColor.ToArgb().ToString(),
            SetGridFore.BackColor.ToArgb().ToString(),
            SetCustom1Color.BackColor.ToArgb().ToString(),
            SetCustom2Color.BackColor.ToArgb().ToString()
            });
        }

        private void GS_WebGo_Click(object sender, EventArgs e)
        {
            GS_WebAddress_SelectedIndexChanged(sender, null);
            // GS_Web.Navigate(GS_WebAddress.Text);
        }

        private void GS_WebRetreat_Click(object sender, EventArgs e)
        {
            GS_Web.GoBack();
        }

        private void GS_WebForward_Click(object sender, EventArgs e)
        {
            GS_Web.GoForward();
        }

        private void GS_WebStop_Click(object sender, EventArgs e)
        {
            GS_Web.Stop();
        }

        private void GS_WebRefresh_Click(object sender, EventArgs e)
        {
            GS_Web.Refresh();
        }

        private void GS_Web_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            GS_WebProBar.Maximum = Convert.ToInt32(e.MaximumProgress);
            GS_WebProBar.Value = Convert.ToInt32(e.CurrentProgress);
        }

        private void GalaxyScanning_Click(object sender, EventArgs e)
        {
            if (_Navigate.Work == 0)
            {
                _Navigate.Work = 1;
                _Scanning.BeginScanning(Convert.ToInt32(GS_WebBeginGalaxy.Text), Convert.ToInt32(GS_WebBeginSystem.Text),
                Convert.ToInt32(GS_WebEndGalaxy.Text), Convert.ToInt32(GS_WebEndSystem.Text), _Navigate);

                GS_ScanningProBar.Maximum = _Scanning.GetScanningCount();//设置总扫描数量
                GS_SysValue.Text = "0";
                GS_SysMax.Text = GS_ScanningProBar.Maximum.ToString();//设置总扫描数量

                State.Text = "状态：星图扫描";
                GalaxyScanning.Text = "扫描中...(点击停止)";
                IEgo.Enabled = false;
            }
            else if (_Navigate.Work == 1)
            {
                _Navigate.Work = 0;
                State.Text = "状态：自由操作";
                GalaxyScanning.Text = "开始扫描";
                IEgo.Enabled = true;

                GS_SysValue.Text = "0";
                GS_SysMax.Text = "0";
                GS_ScanningProBar.Value = 0;
            }
            else
            {
                MessageBox.Show("有一个操作正在进行中，请先停止其他操作");

            }
        }

        /// <summary>
        /// 随机延时模块
        /// </summary>
        private void SleepTime()
        {
            Random Rm = new Random();
            int i = Rm.Next(Convert.ToInt32(GS_Time.Tag));
            Log.AddInformation("随机延时 " + i.ToString() + " 秒");
            Application.DoEvents();
            Thread.Sleep(i * 1000);
        }

        private void GS_Web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            _Navigate.State = 1;
            if (_Navigate.Work == 1)
            {
                SleepTime();

                State.Text = "状态：星图扫描";
                try
                {
                    Log.AddInformation("开始扫描" + _Scanning.GetNowSystem());
                    _Navigate.WorkState = _Scanning.Main();

                }
                catch (Exception eee)
                {
                    Log.AddError("<扫描>" + eee.Message);
                    Log.AddInformation("扫描失败" + _Scanning.GetNowSystem());
                }

                if (_Scanning.GetScanningNumValue() <= GS_ScanningProBar.Maximum)//进度条操作
                {
                    GS_ScanningProBar.Value = _Scanning.GetScanningNumValue();
                    GS_SysValue.Text = GS_ScanningProBar.Value.ToString();
                }

                if (_Navigate.WorkState == false)
                {
                    Log.AddInformation("一共扫描了" + _Scanning.GetScanningNumValue().ToString() + "太阳系");
                    GalaxyScanning.Text = "开始扫描";
                    IEgo.Enabled = true;

                    _Navigate.Work = 0;

                    GS_SysValue.Text = "0";
                    GS_SysMax.Text = "0";
                    GS_ScanningProBar.Value = 0;

                    MessageBox.Show("扫描结束");
                }
            }
            else if (_Navigate.Work == 2)//扫描个人排名
            {
                SleepTime();

                State.Text = "状态：玩家扫描";
                try
                {
                    //Thread.Sleep(new Random().Next(500,1000));
                    _Navigate.WorkState = _ScaRankings.MainPlayer();
                    Log.AddInformation("已经扫描" + _ScaRankings.GetPlayerStart() + " - " + _ScaRankings.GetSacCount());
                }
                catch (Exception eee)
                {
                    Log.AddError("<扫描>" + eee.Message);
                    Log.AddInformation("扫描失败" + _ScaRankings.GetPlayerStart());
                }

                GS_SysValue.Text = Convert.ToString((Convert.ToInt32(GS_SysValue.Text) + 1));
                GS_SysMax.Text = Convert.ToString((Convert.ToInt32(GS_SysMax.Text) + 1));

                if (_Navigate.WorkState == false)
                {
                    Log.AddInformation("一共扫描了" + _ScaRankings.GetSacCount() + "组");
                    ScanningPlayer.Text = "扫描个人排名";
                    IEgo.Enabled = true;

                    _Navigate.Work = 0;

                    GS_SysValue.Text = "0";
                    GS_SysMax.Text = "0";
                    GS_ScanningProBar.Value = 0;

                    MessageBox.Show("扫描结束");
                }


            }
            else if (_Navigate.Work == 3)//扫描舰队排名
            {
                SleepTime();

                State.Text = "状态：舰队扫描";

                try
                {
                    //Thread.Sleep(new Random().Next(500,1000));
                    _Navigate.WorkState = _ScaRankings.MainUnion();
                    Log.AddInformation("已经扫描" + _ScaRankings.GetPlayerStart() + " - " + _ScaRankings.GetSacCount());
                }
                catch (Exception eee)
                {
                    Log.AddError("<扫描>" + eee.Message);
                    Log.AddInformation("扫描失败" + _ScaRankings.GetPlayerStart());
                }

                GS_SysValue.Text = _ScaRankings.GetSacCount().ToString();
                GS_SysMax.Text = GS_SysValue.Text;

                if (_Navigate.WorkState == false)
                {
                    Log.AddInformation("一共扫描了" + _ScaRankings.GetSacCount() + "组");
                    ScanningUnion.Text = "扫描舰队排名";
                    IEgo.Enabled = true;

                    _Navigate.Work = 0;

                    GS_SysValue.Text = "0";
                    GS_SysMax.Text = "0";
                    GS_ScanningProBar.Value = 0;

                    MessageBox.Show("扫描结束");
                }


            }
            else if (_Navigate.Work == 10)
            {
                State.Text = "状态：载入中";
                bool TmpSes;
                TmpSes = _Navigate.SaveMainSession(GS_Web.Document);
                if (TmpSes)
                {
                    _Navigate.Work = 0;
                    State.Text = "状态：自由操作";
                }
            }
            else if (_Navigate.Work == 0)
            {
                State.Text = "状态：自由操作";
            }

            _Navigate.State = 0;

        }

        private void GS_Web_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            _Navigate.State = 1;
        }

        private void GS_Web_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            _Navigate.EndUrl = e.Url.OriginalString.ToString();
        }

        private void 扫描全部银河ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GS_WebBeginGalaxy.Text = "1";
            GS_WebBeginSystem.Text = "1";
            GS_WebEndGalaxy.Text = "9";
            GS_WebEndSystem.Text = "499";
            GalaxyScanning_Click(sender, e);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            GS_WebBeginGalaxy.Text = ((ToolStripMenuItem)sender).Text;
            GS_WebBeginSystem.Text = "1";
            GS_WebEndGalaxy.Text = ((ToolStripMenuItem)sender).Text;
            GS_WebEndSystem.Text = "499";
            GalaxyScanning_Click(sender, e);
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Reflection.Assembly cc = Assembly.GetExecutingAssembly();
            about1.MyAssembly = cc;
            about1.ShowDialog();
        }

        private void 技术支持ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://blog.163.com/cr_soft/");

        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.ShowAll();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.FastPlanet(M_TxtFast.Text, M_MHFast.Checked);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.FastUser(M_TxtFast.Text, M_MHFast.Checked);
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.FastUnion(M_TxtFast.Text, M_MHFast.Checked);
        }

        private void M_MHFast_Click(object sender, EventArgs e)
        {
            M_MHFast.Checked = !M_MHFast.Checked;
        }

        private void ToolTxtSelectAll(object sender, MouseEventArgs e)
        {
            ((ToolStripTextBox)sender).SelectAll();
        }

        private void toolStripComboBox5_MouseDown(object sender, MouseEventArgs e)
        {
            M_FUserName.SelectAll();
        }

        private void toolStripComboBox7_MouseDown(object sender, MouseEventArgs e)
        {
            M_FUnion.SelectAll();
        }

        private void toolStripMenuItem24_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.FastGalaxy(Convert.ToInt32(((ToolStripMenuItem)sender).Text.Substring(0, 1)));
        }

        private void 技术支持ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("等程序做完了再写^_^");
        }

        private void yToolStripMenuItem15_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.Findi(Convert.ToInt32(((ToolStripMenuItem)sender).Text.Substring(0, 1)));
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.FindI(Convert.ToInt32(((ToolStripMenuItem)sender).Text.Substring(0, 1)));
        }

        private void GridPlanet_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
               // GridNeighbors.DataSource = _IOData.FindPlanetNeighbors(GridPlanet.SelectedRows[0].Cells["PGalaxy"].Value.ToString(), GridPlanet.SelectedRows[0].Cells["PSystem"].Value.ToString());
                GridNeighbors.DataSource = _IOData.FindPlanetNeighbors(GridPlanet["PGalaxy", e.RowIndex].Value.ToString(), GridPlanet["PSystem", e.RowIndex].Value.ToString());
                GroupNeighbors.Text = "星系: " + GridPlanet["PGalaxySystem", e.RowIndex].Value.ToString();
            }
            catch (Exception ee)
            {
                Log.AddError("<GridPlanet_RowEnter>" + ee.Message);
                GridNeighbors.DataSource = null;
            }
        }

        private void toolStripDropDownButton3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(M_Galaxy.Text) > 1)
            {
                M_Galaxy.Text = (Convert.ToInt32(M_Galaxy.Text) - 1).ToString();
                GridMain.DataSource = _IOData.FindPlanetNeighbors(M_Galaxy.Text, M_System.Text);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(M_Galaxy.Text) < 9)
            {
                M_Galaxy.Text = (Convert.ToInt32(M_Galaxy.Text) + 1).ToString();
                GridMain.DataSource = _IOData.FindPlanetNeighbors(M_Galaxy.Text, M_System.Text);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(M_System.Text) > 1)
            {
                M_System.Text = (Convert.ToInt32(M_System.Text) - 1).ToString();
                GridMain.DataSource = _IOData.FindPlanetNeighbors(M_Galaxy.Text, M_System.Text);
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(M_System.Text) < 499)
            {
                M_System.Text = (Convert.ToInt32(M_System.Text) + 1).ToString();
                GridMain.DataSource = _IOData.FindPlanetNeighbors(M_Galaxy.Text, M_System.Text);
            }
        }

        private void UserRankings(object sender, EventArgs e)
        {
            string Ran = ((ToolStripMenuItem)sender).Text;
            GridMain.DataSource = _IOData.FindUserRankings(Ran.Substring(0, Ran.IndexOf("~")).Trim(), Ran.Substring(Ran.IndexOf("~") + 1).Trim());
        }

        private void UnionRankings_Click(object sender, EventArgs e)
        {
            string Ran = ((ToolStripMenuItem)sender).Text;
            GridMain.DataSource = _IOData.FindUserUnionRankings(Ran.Substring(0, Ran.IndexOf("~")).Trim(), Ran.Substring(Ran.IndexOf("~") + 1).Trim());
        }

        private void M_FFind_Click(object sender, EventArgs e)
        {
            string Galaxy, UserName, Rankings, Union, UnionRankings, i, Li, Vacation, Banned;

            //星系
            if ((M_FSys.Text == "") || (M_FSys.Text == "全部"))
                Galaxy = "1 = 1";
            else
                Galaxy = "[Galaxy] = " + M_FSys.Text.Substring(0, 1).Trim();

            //用户名
            if ((M_FUserName.Text == "") || (M_FUserName.Text == "输入用户名"))
                UserName = "1=1";
            else
                UserName = "[UserName] = '" + M_FUserName.Text + "'";

            //排名
            if ((M_FRankings.Text == "") || (M_FRankings.Text == "全部"))
                Rankings = "1=1";
            else
            {
                if (M_FRankings.SelectedIndex == 1)
                    Rankings = "[Rankings] >= 1 and [Rankings] <= 99";
                else
                    Rankings = "[Rankings] >= " + Convert.ToString(M_FRankings.SelectedIndex * 100 - 100) + " and [Rankings] <= " + Convert.ToString(M_FRankings.SelectedIndex * 100 - 1);
            }

            //联盟
            if ((M_FUnion.Text == "") || (M_FUnion.Text == "输入联盟名"))
                Union = "1=1";
            else
                Union = "[Union] = '" + M_FUnion.Text + "'";

            //联盟排名
            if ((M_FUnionRankings.Text == "") || (M_FUnionRankings.Text == "全部"))
                UnionRankings = "1=1";
            else
            {
                if (M_FUnionRankings.SelectedIndex == 1)
                    UnionRankings = "[UnionRankings] >= 1 and [UnionRankings] <= 99";
                else
                    UnionRankings = "[UnionRankings] >= " + Convert.ToString(M_FUnionRankings.SelectedIndex * 100 - 100) + " and [UnionRankings] <= " + Convert.ToString(M_FUnionRankings.SelectedIndex * 100 - 1);
            }

            //7天不在线
            if (M_Fi.Text == "否")
                i = "[inactive] = false";
            else if (M_Fi.Text == "是")
                i = "[inactive] = true";
            else
                i = "1=1";

            //28天不在线
            if (M_FLi.Text == "否")
                Li = "[longinactive] = false";
            else if (M_FLi.Text == "是")
                Li = "[longinactive] = true";
            else
                Li = "1=1";

            //假期
            if (M_FVacation.Text == "否")
                Vacation = "[vacation] = false";
            else if (M_FVacation.Text == "是")
                Vacation = "[vacation] = true";
            else
                Vacation = "1=1";

            //被封
            if (M_FBanned.Text == "否")
                Banned = "[banned] = false";
            else if (M_FBanned.Text == "是")
                Banned = "[banned] = true";
            else
                Banned = "1=1";

            GridMain.DataSource = _IOData.FindAdvancedFind(Galaxy, UserName, Rankings, Union, UnionRankings, i, Li, Vacation, Banned);

        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + DateTime.Now.ToString("yyyy-M-d") + " ALL.XML";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                _IOData.OutAllData(saveFileDialog1.FileName);
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + DateTime.Now.ToString("yyyy-M-d") + ".XML";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                _IOData.OutNowData(saveFileDialog1.FileName);
        }

        private void SaveGalaxyAllData_Click(object sender, EventArgs e)
        {
            string LGaxlax = ((ToolStripMenuItem)sender).Text;
            saveFileDialog1.FileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + DateTime.Now.ToString("yyyy-M-d") + " " + LGaxlax + " ALL.XML";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                _IOData.OutGalaxyAllData(saveFileDialog1.FileName, LGaxlax.Substring(0, 1));
        }

        private void SaveGalaxyNowData_Click(object sender, EventArgs e)
        {
            string LGaxlax = ((ToolStripMenuItem)sender).Text;
            saveFileDialog1.FileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + DateTime.Now.ToString("yyyy-M-d") + " " + LGaxlax + ".XML";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                _IOData.OutGalaxyNowData(saveFileDialog1.FileName, LGaxlax.Substring(0, 1));
        }

        private void toolStripButton16_Click_1(object sender, EventArgs e)
        {

            //openFileDialog1.FileName = Path.GetDirectoryName(Application.ExecutablePath) + "\\ ";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DataInBox.Left = DataInBox.Parent.Width / 2 - 200;
                DataInBox.Top = DataInBox.Parent.Height / 2 - 20;
                DataInBox.Visible = true;
                tabPage2.Enabled = false;
                _IOData.InData(openFileDialog1.FileName, DataInBar);
                tabPage2.Enabled = true;
                DataInBox.Visible = false;

                Log.AddInformation("数据导入完成");
            }
        }

        private void 本版本属于预览版尚有很多功能未制作这些功能将在近期补全ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("IEXPLORE.EXE", "http://blog.163.com/cr_soft/");
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {
            if (FlyTimeWeb.Url.ToString() == "about:blank")
                FlyTimeWeb.Navigate(Path.GetDirectoryName(Application.ExecutablePath) + "\\Tool\\Time.htm");
        }

        private void ExRowPostPaint(DataGridViewRow dgr)
        {
            int i;

            if (dgr.Cells["Custom1"].Value.ToString().Trim() == "True")
            {
                dgr.DefaultCellStyle.BackColor = SetCustom1Color.BackColor;
                return;
            }
            //else
            //{
            //    dgr.DefaultCellStyle.BackColor = SetGridBack.BackColor;
            //}

            if (dgr.Cells["Union"].Value != null)
            {
                //自己联盟
                if ((ShowMyUnion.Checked) && (SetMyUnion.Text != ""))
                    if (dgr.Cells["Union"].Value.ToString().Trim() == SetMyUnion.Text)
                    {
                        dgr.DefaultCellStyle.BackColor = SetMyUnionColor.BackColor;
                        return;
                    }

                //盟友
                if (ShowUnion.Checked)
                    for (i = 0; i < SetUnionList.Items.Count; i++)
                        if (dgr.Cells["Union"].Value.ToString().Trim() == SetUnionList.Items[i].ToString())
                        {
                            dgr.DefaultCellStyle.BackColor = SetUnionColor.BackColor;
                            return;
                        }

                //敌对联盟
                if (ShowEnemyUnion.Checked)
                    for (i = 0; i < SetEnemyUnionList.Items.Count; i++)
                        if (dgr.Cells["Union"].Value.ToString().Trim() == SetEnemyUnionList.Items[i].ToString())
                        {
                            dgr.DefaultCellStyle.BackColor = SetEnemyUnionColor.BackColor;
                            return;
                        }
            }

            if (dgr.Cells["UserName"].Value != null)
            {
                //朋友
                if (ShowFriend.Checked)
                    for (i = 0; i < SetFriendList.Items.Count; i++)
                        if (dgr.Cells["UserName"].Value.ToString().Trim() == SetFriendList.Items[i].ToString())
                        {
                            dgr.DefaultCellStyle.BackColor = SetFriendColor.BackColor;
                            return;
                        }

                //敌人
                if (ShowEnemy.Checked)
                    for (i = 0; i < SetEnemyList.Items.Count; i++)
                        if (dgr.Cells["UserName"].Value.ToString().Trim() == SetEnemyList.Items[i].ToString())
                        {
                            dgr.DefaultCellStyle.BackColor = SetEnemyColor.BackColor;
                            return;
                        }
            }
        }

        private void GridMain_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
        }

        private void GridMain_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= GridMain.Rows.Count - 1)
                return;
            DataGridViewRow dgr = GridMain.Rows[e.RowIndex];
            //如果没有玩家就不显示东西，最高优先级
            if (dgr.Cells["Username"].Value.ToString().Trim() == "")
            {
                dgr.DefaultCellStyle.ForeColor = dgr.DefaultCellStyle.BackColor;
                //dgr.DefaultCellStyle.SelectionForeColor  =dgr.DefaultCellStyle.SelectionBackColor  ;
                dgr.Cells[0].Style.ForeColor = Color.White;
                //dgr.Cells[0].Style.SelectionForeColor = Color.White;
                return;
            }
            try
            {
                ExRowPostPaint(dgr);//其他颜色，都是用于设置背景的

                //标记字体

                //i/I羊
                //假期
                //被封
                if (dgr.Cells["Custom2"].Value.ToString().Trim() == "True")
                {
                    dgr.DefaultCellStyle.ForeColor = SetCustom2Color.BackColor;
                }
                if (dgr.Cells["banned"].Value.ToString() == "True")
                {
                    if (ShowG.Checked)
                        dgr.DefaultCellStyle.ForeColor = SetgColor.BackColor;
                }
                else if (dgr.Cells["Vacation"].Value.ToString() == "True")
                {
                    if (ShowV.Checked)
                        dgr.DefaultCellStyle.ForeColor = SetuColor.BackColor;
                }
                else if (dgr.Cells["inactive"].Value.ToString() == "True")
                {
                    dgr.DefaultCellStyle.ForeColor = SetLiColor.BackColor;
                }
                else if (dgr.Cells["longinactive"].Value.ToString() == "True")
                {
                    if (ShowiL.Checked)
                        dgr.DefaultCellStyle.ForeColor = SetLiColor.BackColor;
                }
                else if (dgr.Cells["Custom2"].Value.ToString().Trim() == "False")
                {
                    dgr.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
                }
            }
            catch (Exception ee)
            {
                Log.AddError("<GridMain_RowPostPaint>" + ee.Message);
            }
        }

        private void SetFriendAdd_Click(object sender, EventArgs e)
        {
            if (SetFriendTxt.Text != "输入朋友名字")
            {
                SetFriendList.Items.Add(SetFriendTxt.Text);
            }
        }

        private void SetEnemyAdd_Click(object sender, EventArgs e)
        {
            if (SetEnemyTxt.Text != "输入敌人名字")
            {
                SetEnemyList.Items.Add(SetEnemyTxt.Text);
            }
        }

        private void SetUnionAdd_Click(object sender, EventArgs e)
        {
            if (SetUnionTxt.Text != "输入联盟名字")
            {
                SetUnionList.Items.Add(SetUnionTxt.Text);
            }
        }

        private void SetEnemyUnionAdd_Click(object sender, EventArgs e)
        {
            if (SetEnemyUnionTxt.Text != "输入敌对联盟名字")
            {
                SetEnemyUnionList.Items.Add(SetEnemyUnionTxt.Text);
            }
        }

        private void SetFriendDel_Click(object sender, EventArgs e)
        {
            if (SetFriendList.SelectedIndex != -1)
            {
                SetFriendList.Items.RemoveAt(SetFriendList.SelectedIndex);
            }
        }

        private void SetEnemyDel_Click_1(object sender, EventArgs e)
        {
            if (SetEnemyList.SelectedIndex != -1)
            {
                SetEnemyList.Items.RemoveAt(SetEnemyList.SelectedIndex);
            }
        }

        private void SetUnionDel_Click(object sender, EventArgs e)
        {
            if (SetUnionList.SelectedIndex != -1)
            {
                SetUnionList.Items.RemoveAt(SetUnionList.SelectedIndex);
            }
        }

        private void SetEnemyUnionDel_Click(object sender, EventArgs e)
        {
            if (SetEnemyUnionList.SelectedIndex != -1)
            {
                SetEnemyUnionList.Items.RemoveAt(SetEnemyUnionList.SelectedIndex);
            }
        }

        private void ShowMyUnion_Click(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
        }

        //清除表
        private void TabInfoClear()
        {
            GridUserEes.DataSource = null;
            GridUserFlt.DataSource = null;
            GridUserPts.DataSource = null;
            GridUnionEes.DataSource = null;
            GridUnionFlt.DataSource = null;
            GridUnionPts.DataSource = null;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.FindPlanetNeighbors(M_Galaxy.Text, M_System.Text);
        }

        private void TxtSpy_Leave(object sender, EventArgs e)
        {
            if (GridMain["GalaxySystem", GridMain.CurrentCell.RowIndex].Value != null)
                _IOData.UpDataSpy(TxtSpy.Text, GridMain["GalaxySystem", GridMain.CurrentCell.RowIndex].Value.ToString());
        }

        private void SetMyUnionColor_Click(object sender, EventArgs e)
        {
            if (Color1.ShowDialog() == DialogResult.OK)
                ((Button)sender).BackColor = Color1.Color;

            // TxtLog.Text += ((Button)sender).BackColor.ToArgb().ToString() + "\r\n";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Color1.ShowDialog() == DialogResult.OK)
            {
                ((Button)sender).BackColor = Color1.Color;
                GridMain.RowTemplate.DefaultCellStyle.BackColor = Color1.Color;
                GridPlanet.RowTemplate.DefaultCellStyle.BackColor = Color1.Color;
                GridNeighbors.RowTemplate.DefaultCellStyle.BackColor = Color1.Color;
            }
        }

        private void SetGridFore_Click(object sender, EventArgs e)
        {
            if (Color1.ShowDialog() == DialogResult.OK)
            {
                ((Button)sender).BackColor = Color1.Color;
                GridMain.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
                GridPlanet.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
                GridNeighbors.RowTemplate.DefaultCellStyle.ForeColor = SetGridFore.BackColor;
            }
        }

        private void toolStripDropDownButton4_Click(object sender, EventArgs e)
        {

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void yToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.Find4Planet(Convert.ToInt32(((ToolStripMenuItem)sender).Text.Substring(0, 1)), M_4Txt.Text);
        }

        private void GS_WebAddress_Click(object sender, EventArgs e)
        {

        }

        private void GS_WebAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (GS_WebAddress.Text)
            {
                case "www.ogame.com.cn":
                    _SerInfo.CN();
                    ServerInfo.Text = "服务器：" + _SerInfo.Server;
                    GS_Web.Navigate("www.ogame.com.cn");
                    break;
                case "www.ogame.tw":
                    _SerInfo.TW();
                    ServerInfo.Text = "服务器：" + _SerInfo.Server;
                    GS_Web.Navigate("www.ogame.tw");
                    break;
                case "ogame.de":
                    _SerInfo.DE();
                    ServerInfo.Text = "服务器：" + _SerInfo.Server;
                    GS_Web.Navigate("ogame.de");
                    break;
                case "www.ogame.org":
                    _SerInfo.EN();
                    ServerInfo.Text = "服务器：" + _SerInfo.Server;
                    GS_Web.Navigate("www.ogame.org");
                    break;
                default:
                    _SerInfo.CN();
                    ServerInfo.Text = "服务器：未知";
                    GS_Web.Navigate(GS_WebAddress.Text);
                    break;
            }
        }

        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            Login LG = new Login();
            LG.ShowDialog();
            if (LG.Result == DialogResult.OK)
            {
                _SerInfo.AutoServer(LG.Server);
                GS_WebAddress.Text = _SerInfo.Website;
                ServerInfo.Text = "服务器：" + _SerInfo.Server;
                _Navigate.Work = 10;
                GS_Web.Navigate(_SerInfo.LoginUrl(LG.U, LG.UserName, LG.UserPass));

            }
        }

        private void toolStripButton18_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton18_Click_1(object sender, EventArgs e)
        {
            string a = GS_Web.Document.Cookie.ToString();
        }

        private void ScanningPlayer_Click(object sender, EventArgs e)
        {
            if (_Navigate.Work == 0)
            {
                _Navigate.Work = 2;
                State.Text = "状态：玩家扫描";
                ScanningPlayer.Text = "扫描中...(点击停止)";
                IEgo.Enabled = false;
                //GS_ScanningProBar.Maximum = 213;//设置总扫描数量
                GS_SysValue.Text = "0";
                GS_SysMax.Text = "0";//设置总扫描数量

                _ScaRankings.BeginPlayer();

            }
            else if (_Navigate.Work == 2)
            {
                _Navigate.Work = 0;
                State.Text = "状态：自由操作";
                ScanningPlayer.Text = "扫描个人排名";
                IEgo.Enabled = true;

                GS_SysValue.Text = "0";
                GS_SysMax.Text = "0";
                GS_ScanningProBar.Value = 0;
            }
            else
            {
                MessageBox.Show("有一个操作正在进行中，请先停止其他操作");

            }

        }

        private void ScanningUnion_Click(object sender, EventArgs e)
        {
            if (_Navigate.Work == 0)
            {
                _Navigate.Work = 3;
                State.Text = "状态：舰队扫描";
                ScanningUnion.Text = "扫描中...(点击停止)";
                IEgo.Enabled = false;
                //GS_ScanningProBar.Maximum = 213;//设置总扫描数量
                GS_SysValue.Text = "0";
                GS_SysMax.Text = "0";//设置总扫描数量

                _ScaRankings.BeginUnion();

            }
            else if (_Navigate.Work == 3)
            {
                _Navigate.Work = 0;
                State.Text = "状态：自由操作";
                ScanningUnion.Text = "扫描舰队排名";
                IEgo.Enabled = true;

                GS_SysValue.Text = "0";
                GS_SysMax.Text = "0";
                GS_ScanningProBar.Value = 0;
            }
            else
            {
                MessageBox.Show("有一个操作正在进行中，请先停止其他操作");

            }
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage9_Enter(object sender, EventArgs e)
        {
            if (GridUserEes.DataSource == null)
            {
                GridUserEes.DataSource = _IOData.HistoryUserRankings("UserEes", GridMain["Username", GridMain.CurrentCell.RowIndex].Value.ToString());
                GridUserFlt.DataSource = _IOData.HistoryUserRankings("UserFlt", GridMain["Username", GridMain.CurrentCell.RowIndex].Value.ToString());
                GridUserPts.DataSource = _IOData.HistoryUserRankings("UserPts", GridMain["Username", GridMain.CurrentCell.RowIndex].Value.ToString());
            }
        }

        private void tabPage10_Enter(object sender, EventArgs e)
        {
            if (GridUnionEes.DataSource == null)
            {
                GridUnionEes.DataSource = _IOData.HistoryUnionRankings("UnionEes", GridMain["Union", GridMain.CurrentCell.RowIndex].Value.ToString());
                GridUnionFlt.DataSource = _IOData.HistoryUnionRankings("UnionFlt", GridMain["Union", GridMain.CurrentCell.RowIndex].Value.ToString());
                GridUnionPts.DataSource = _IOData.HistoryUnionRankings("UnionPts", GridMain["Union", GridMain.CurrentCell.RowIndex].Value.ToString());
            }
        }

        private void toolStripButton19_Click(object sender, EventArgs e)
        {

        }

        private void yToolStripMenuItem13_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.FindNi(Convert.ToInt32(((ToolStripMenuItem)sender).Text.Substring(0, 1)));
        }

        private void toolStripButton20_Click(object sender, EventArgs e)
        {
            GridMain.DataSource = _IOData.MetalCrystal();
        }

        private void 积分ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
            _ScaRankings.SetScanningObject(GS_SB1.Checked, GS_SB2.Checked, GS_SB3.Checked);
        }

        private void toolStripDropDownButton5_Click(object sender, EventArgs e)
        {

        }

        private void GS_Time0_Click(object sender, EventArgs e)
        {
            GS_Time0.Checked = false;
            GS_Time1.Checked = false;
            GS_Time2.Checked = false;
            GS_Time3.Checked = false;
            GS_Time4.Checked = false;
            GS_Time5.Checked = false;
            GS_Time10.Checked = false;
            GS_Time15.Checked = false;
            ((ToolStripMenuItem)sender).Checked = true;
            GS_Time.Tag = ((ToolStripMenuItem)sender).Text;
        }

        private void tabPage11_Enter(object sender, EventArgs e)
        {
            if (FightingReported.Url.ToString() == "about:blank")
                FightingReported.Navigate(Path.GetDirectoryName(Application.ExecutablePath) + "\\Tool\\Converter.htm");
        }

        private void SetCustom1Color_Click(object sender, EventArgs e)
        {
            if (Color1.ShowDialog() == DialogResult.OK)
                ((Button)sender).BackColor = Color1.Color;
        }

        private void SetCustom2Color_Click(object sender, EventArgs e)
        {
            if (Color1.ShowDialog() == DialogResult.OK)
                ((Button)sender).BackColor = Color1.Color;
        }

        private void toolStripButton11_Click_1(object sender, EventArgs e)
        {
            if (GridMain.SelectedRows != null)
            {
                bool Cus = Convert.ToBoolean(GridMain.SelectedRows[0].Cells["Custom1"].Value.ToString());
                _IOData.Custom1Check(GridMain.SelectedRows[0].Cells["GalaxySystem"].Value.ToString(), Cus);
                GridMain.SelectedRows[0].Cells["Custom1"].Value = !Cus;
            }
        }

        private void toolStripButton14_Click_1(object sender, EventArgs e)
        {
            if (GridMain.SelectedRows != null)
            {
                bool Cus = Convert.ToBoolean(GridMain.SelectedRows[0].Cells["Custom2"].Value.ToString());
                _IOData.Custom2Check(GridMain.SelectedRows[0].Cells["GalaxySystem"].Value.ToString(), Cus);
                GridMain.SelectedRows[0].Cells["Custom2"].Value = !Cus;
            }
        }

        private void tabpage13_Enter(object sender, EventArgs e)
        {

        }

        private void tabpage13_Leave(object sender, EventArgs e)
        {

        }

        private void tabPage12_Enter(object sender, EventArgs e)
        {
            resources1.LoadFile();
            resources2.LoadFile();
            resources3.LoadFile();
            resources4.LoadFile();
            resources5.LoadFile();
            resources6.LoadFile();
            resources7.LoadFile();
            resources8.LoadFile();
            resources9.LoadFile();
            Res();
        }

        private void tabPage12_Leave(object sender, EventArgs e)
        {
            resources1.SaveFile();
            resources2.SaveFile();
            resources3.SaveFile();
            resources4.SaveFile();
            resources5.SaveFile();
            resources6.SaveFile();
            resources7.SaveFile();
            resources8.SaveFile();
            resources9.SaveFile();
        }

        /// <summary>
        /// 计算资源
        /// </summary>
        private void Res()
        {
            MetalAll.Text = Convert.ToString(resources1.GetMetalAll + resources2.GetMetalAll + resources3.GetMetalAll + resources4.GetMetalAll + resources5.GetMetalAll + resources6.GetMetalAll + resources7.GetMetalAll + resources8.GetMetalAll + resources9.GetMetalAll);
            MetalDay.Text = Convert.ToString(resources1.GetMetalDay + resources2.GetMetalDay + resources3.GetMetalDay + resources4.GetMetalDay + resources5.GetMetalDay + resources6.GetMetalDay + resources7.GetMetalDay + resources8.GetMetalDay + resources9.GetMetalDay);

            CrystalAll.Text = Convert.ToString(resources1.GetCrystalAll + resources2.GetCrystalAll + resources3.GetCrystalAll + resources4.GetCrystalAll + resources5.GetCrystalAll + resources6.GetCrystalAll + resources7.GetCrystalAll + resources8.GetCrystalAll + resources9.GetCrystalAll);
            CrystalDay.Text = Convert.ToString(resources1.GetCrystalDay + resources2.GetCrystalDay + resources3.GetCrystalDay + resources4.GetCrystalDay + resources5.GetCrystalDay + resources6.GetCrystalDay + resources7.GetCrystalDay + resources8.GetCrystalDay + resources9.GetCrystalDay);

            HHAll.Text = Convert.ToString(resources1.GetHHAll + resources2.GetHHAll + resources3.GetHHAll + resources4.GetHHAll + resources5.GetHHAll + resources6.GetHHAll + resources7.GetHHAll + resources8.GetHHAll + resources9.GetHHAll);
            HHDay.Text = Convert.ToString(resources1.GetHHDay + resources2.GetHHDay + resources3.GetHHDay + resources4.GetHHDay + resources5.GetHHDay + resources6.GetHHDay + resources7.GetHHDay + resources8.GetHHDay + resources9.GetHHDay);
        }

        private void resources1_btnOkClick(object sender, EventArgs e)
        {
            Res();
        }

        //private Match _PcMc;
        //private Match _PlTextMc;

        private void tabPage15_Enter(object sender, EventArgs e)
        {
            //进入全局控制，初始所有该初始的资源
            if (GS_Web.Document == null) return;
            if (ControlLoad == false)
            {
                HtmlElement HeaderHE = GS_Web.Document.GetElementById("header_top");
                if (HeaderHE == null)
                {
                    MessageBox.Show("请先选择到帝国首页");
                    return;
                }
                OGControl.Info.CN();//初始化信息
                HtmlElement SelectHE = HeaderHE.Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[1].Children[0].Children[0];
               
                Regex Rx = new Regex(@"(<option)[\s\S]+?(</option>)");
                MatchCollection MatchC = Rx.Matches(SelectHE.InnerHtml.ToLower());

                //获得session
                Regex SesRx = new Regex(@"(?<=session=)[\S]+?(?=&)");
                Match SesMc = SesRx.Match(MatchC[0].Value.ToLower());
                _Session = SesMc.Value;

                System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

                for (int i = MatchC.Count - 1; i >= 0; i--)
                {
                    string OptionStr = MatchC[i].Value.ToLower();
                    Match PcMc = Regex.Match(OptionStr, @"(?<=cp=)[\S]+?(?=&)"); //获得星球ID
                    Match PlTextMc = Regex.Match(OptionStr, @"(?<=(<option[\s\S]+?>))[\s\S]+?(?=</option>)");//星球地址
                    string Location = Regex.Match(PlTextMc.Value, "(?<=\\[)([0-9]?):([0-9])*:(([0-9]?[0-9]?))(?=\\])").Value;//地址
                    string GalaxyLocation = Regex.Match(PlTextMc.Value, "[0-9](?=:[0-9]*:)").Value;
                    string SystemLocation = Regex.Match(PlTextMc.Value, "(?<=:)[0-9]+?(?=:)").Value;
                    string PositionLocation = Regex.Match(PlTextMc.Value, "(?<=:[0-9]+?:)[0-9]*").Value;
                    //_PcMc = PcMc;
                    //_PlTextMc = PlTextMc;


                    //System.Threading.ThreadStart start = new System.Threading.ThreadStart(OGadd);
                    //System.Threading.Thread Th = new System.Threading.Thread(start);
                    //Th.ApartmentState = System.Threading.ApartmentState.STA;//这句关键的 
                    //Th.SetApartmentState(ApartmentState.STA);
                    //Th.Start();

                    //传入舰队处理类和综合管理类。
                    OGControl.OGControl OGCol = new CR_Galaxy.OGControl.OGControl(_FlottenCmd, _OGControlManage, this);
                    OGCol.Session = _Session;//身份ID
                    OGCol.PlanetID = "&cp=" + PcMc.Value;//星球ID
                    OGCol.Planet = PlTextMc.Value;
                    OGCol.Location = Location;
                    OGCol.GalaxyLocation = GalaxyLocation;
                    OGCol.SystemLocation = SystemLocation;
                    OGCol.PositionLocation = PositionLocation;
                    //OGCol._FlottenCmd = _FlottenCmd;//写入舰队处理类
                    OGCol.WebsiteEx = _SerInfo.WebsiteEx;
                    PlanetControl.Controls.Add(OGCol);
                    OGCol.Dock = DockStyle.Left;
                    OGCol.Enabled = false;
                    OGCol.Visible = true;
                    Application.DoEvents();
                    OGCol.StartLoad();

                    _OGControlManage._OGControl.Add(Location, OGCol);

                    SetFS(OGCol);
                    //_OGControlManage._OGControl.Add(

                   // MessageBox.Show(OGCol._FSGalaxy + OGCol._FSSystem + OGCol._FSPosition + OGCol._FSSheep + OGCol._FSOrder);
                }

                _OGControlManage._Session = SesMc.Value;
                ogMilitary1.WebsiteEx = _SerInfo.WebsiteEx;
                ogMilitary1._FlottenCmd = _FlottenCmd;
                ogMilitary1._OGCOntrolManage = _OGControlManage;
                ogMilitary1.FleetFlyListStart();
                ControlLoad = true;
            }
        }

        private void SetFS(OGControl.OGControl OG)
        {
            string[] FSstr = File.ReadAllLines(Application.StartupPath + "\\Data\\FS_Info.txt");
            for (int i = 0; i < FSstr.Length; i++)
            {
                if (FSstr[i].IndexOf(OG.Location) >= 0)
                {
                    string[] Sq = Regex.Split(FSstr[i], ",");
                    OG._FSGalaxy = Sq[1];
                    OG._FSSystem  = Sq[2];
                    OG._FSPosition  = Sq[3];
                    OG._FSSheep  = Sq[4];
                    OG._FSOrder  = Sq[5];
                }
            }
        }

        //private void OGadd()
        //{
        //    //传入舰队处理类和综合管理类。
        //    OGControl.OGControl OGCol = new CR_Galaxy.OGControl.OGControl(_FlottenCmd, _OGControlManage, this);
        //    OGCol.Session = _Session;//身份ID
        //    OGCol.PlanetID = "&cp=" + _PcMc.Value;//星球ID
        //    OGCol.Planet = _PlTextMc.Value;
        //    //OGCol._FlottenCmd = _FlottenCmd;//写入舰队处理类
        //    OGCol.WebsiteEx = _SerInfo.WebsiteEx;
        //    PlanetControl.Controls.Add(OGCol);


        //    OGCol.Dock = DockStyle.Left;
        //    OGCol.Enabled = false;
        //    OGCol.Visible = true;
        //    Application.DoEvents();
        //    OGCol.StartLoad();


        //}

        private void tabPage16_Enter(object sender, EventArgs e)
        {
            if (webBrowser1.Url == null)
                webBrowser1.Navigate(@"http://www.crsoft.net.cn");
        }

        private void GridMain_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            {
                try
                {
                    if (this.GridMain["Username", e.RowIndex].Value.ToString() != "")
                    {
                        this.TxtSpy.Text = "";
                        string spGalaxySystem = this.GridMain["GalaxySystem", e.RowIndex].Value.ToString();
                        this.TxtSpy.Text = this._IOData.GetDataSpy(spGalaxySystem);
                        if (this.TxtSpy.Text == "")
                        {
                            this.TxtSpy.Text = spGalaxySystem + " 的间谍报告\r\n";
                        }
                        this.M_Galaxy.Text = spGalaxySystem.Substring(0, 1);
                        spGalaxySystem = spGalaxySystem.Substring(2);
                        this.M_System.Text = spGalaxySystem.Substring(0, spGalaxySystem.IndexOf(":"));
                        this.GridPlanet.DataSource = this._IOData.FindUserOtherPlanet(this.GridMain["Username", e.RowIndex].Value.ToString());
                        this.GroupPlanet.Text = "用户: " + this.GridMain["Username", e.RowIndex].Value.ToString() + "    联盟: " + this.GridMain["Union", e.RowIndex].Value.ToString();
                        if (this.TabInfo.SelectedIndex == 1)
                        {
                            this.GridUserEes.DataSource = this._IOData.HistoryUserRankings("UserEes", this.GridMain["Username", e.RowIndex].Value.ToString());
                            this.GridUserFlt.DataSource = this._IOData.HistoryUserRankings("UserFlt", this.GridMain["Username", e.RowIndex].Value.ToString());
                            this.GridUserPts.DataSource = this._IOData.HistoryUserRankings("UserPts", this.GridMain["Username", e.RowIndex].Value.ToString());
                            this.GridUnionEes.DataSource = null;
                            this.GridUnionFlt.DataSource = null;
                            this.GridUnionPts.DataSource = null;
                        }
                        else if (this.TabInfo.SelectedIndex == 2)
                        {
                            this.GridUnionEes.DataSource = this._IOData.HistoryUnionRankings("UnionEes", this.GridMain["Union", e.RowIndex].Value.ToString());
                            this.GridUnionFlt.DataSource = this._IOData.HistoryUnionRankings("UnionFlt", this.GridMain["Union", e.RowIndex].Value.ToString());
                            this.GridUnionPts.DataSource = this._IOData.HistoryUnionRankings("UnionPts", this.GridMain["Union", e.RowIndex].Value.ToString());
                            this.GridUserEes.DataSource = null;
                            this.GridUserFlt.DataSource = null;
                            this.GridUserPts.DataSource = null;
                        }
                        else
                        {
                            this.TabInfoClear();
                        }
                    }
                    else
                    {
                        this.TxtSpy.Text = "";
                        this.GroupPlanet.Text = "";
                        this.GroupNeighbors.Text = "";
                        this.GridPlanet.DataSource = null;
                        this.GridNeighbors.DataSource = null;
                        this.TabInfoClear();
                    }
                }
                catch (Exception exception)
                {
                    this.Log.AddError("<GridMain_RowEnter_1>" + exception.Message);
                    this.GroupPlanet.Text = "";
                    this.GroupNeighbors.Text = "";
                    this.GridPlanet.DataSource = null;
                    this.GridNeighbors.DataSource = null;
                }
            }

            //try
            //{
            //    if (GridMain["Username", e.RowIndex].Value.ToString().Trim().Length == 0) return;
            //    GridPlanet.DataSource = _IOData.FindUserOtherPlanet(GridMain["Username", e.RowIndex].Value.ToString());
            //    GroupPlanet.Text = "其他星球  玩家:" + GridMain["Username", e.RowIndex].Value.ToString();
            //}
            //catch (Exception ee)
            //{
            //    Log.AddError("<GridMain_RowEnter>" + ee.Message);
            //    GridNeighbors.DataSource = null;
            //}
        }

        private void GridMain_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            this.TxtLog.Text = this.TxtLog.Text + this.GridMain["GalaxySystem", e.RowIndex].Value.ToString() + "\r\n";

        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                msgList.Text = File.ReadAllText(openFileDialog2.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            userList.Items.Clear();
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string[] FileText = File.ReadAllLines(openFileDialog2.FileName);
                for (int i = 0; i < FileText.Length; i++)
                {
                    string[] UT= Regex.Split(FileText[i],",");
                    if (UT.Length != 2) continue;
                    ListViewItem LVI = userList.Items.Add(UT[0]);
                    LVI.SubItems.Add(UT[1]);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(openFileDialog2.FileName, msgList.Text);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                StreamWriter st = File.AppendText(openFileDialog2.FileName);
                for (int i = 0; i <userList.Items.Count ; i++)
                {
                    st.WriteLine(userList.Items[i].Text + "," + userList.Items[i].SubItems[0].Text);
                }
            }
        }




        private void button5_Click(object sender, EventArgs e)
        {
//群发功能不开放
            //if (button5.Text == "开始")
            //{
            //    MsgCancel = false;
            //    Random ro = new Random(unchecked((int)DateTime.Now.Ticks));

            //    MsgWB.Navigate(string.Format(MsgStr, new string[] { _Session, userList.Items[UserIndex].SubItems[1].Text, userList.Items[UserIndex].Text, "无标题", msgList.Lines[ro.Next(msgList.Lines.Length)] }));

            //    button5.Text = "停止";
            //}
            //else if (button5.Text == "停止")
            //{
            //    MsgCancel = true;
            //    button5.Text = "开始";
            //}
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(WebUrl.Text);

        }
        
    }
}