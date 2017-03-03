using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;

namespace CR_Galaxy.OGControl
{
    /// <summary>
    /// 防御类
    /// </summary>
    class VerteidigungHelper
    {

 OGControl _This;

 public VerteidigungHelper(OGControl This)
        {
            _This = This;
        }

        public void ClearBuildBtn(TabControl TabVerteidigung)
        {
            foreach (TabPage TP in TabVerteidigung.TabPages)
            {
                if (TP == TabVerteidigung.TabPages[3]) continue;
                TP.Controls.Clear();
            }
        }

        string VerteidigungUrl = "index.php?page=buildings&session={0}&mode=Verteidigung&cp={1}";
        //http://uni8.ogame.cn.com/game/index.php?page=buildings&session=e246a3aaf54c&mode=Verteidigung&fmenge[202]=1&cp=33626998

        //页面分析主入口
        public void VerteidigungRending(DataTable BuildDT, HtmlElement ContentHE)
        {
            _This.ForschungNow.Visible = false;

            //真正生成控件
            Calc calc = new Calc();
            List<NumericUpDown> LNud = new List<NumericUpDown>();
            CreatVerteidigungBuildBtn(_This.TabVerteidigung, LNud);
            for (int i = ContentHE.Children.Count - 1; i >= 1; i--)
            {//遍历所有可建造的东西
                if (ContentHE.Children[i].Children.Count < 3) continue;//有些战舰没有，所以遍历不到
                if (ContentHE.Children[i].Children[1].Children.Count == 0) continue;//有些战舰没有，所以遍历不到

                HtmlElement VerteidigungName = ContentHE.Children[i].Children[1].Children[0];//得到战舰名字
                HtmlElement VerteidigungInfo = ContentHE.Children[i].Children[1];//得到战舰信息
                HtmlElement VerteidigungInfoUp = ContentHE.Children[i].Children[2];//得到战舰的状态、是否可建造
                DataRow[] BuildDRs = BuildDT.Select("Text = '" + VerteidigungName.InnerText + "'");
                if (BuildDRs.Length == 1)//检索到这个研究信息
                {
                    ObjectInfo ORes = new ObjectInfo();
                    ORes.Period = Info.GetBDateTime(VerteidigungInfo.InnerText);
                    ORes.Metall =Convert.ToDouble  ( BuildDRs[0]["JS"].ToString());
                    ORes.Kristall = Convert.ToDouble(BuildDRs[0]["JT"].ToString());
                    ORes.Deuterium = Convert.ToDouble(BuildDRs[0]["HH"].ToString());
                    ORes.Text = VerteidigungName.InnerText;
                    ORes.Level = Info.GetObjectNum(VerteidigungInfo.InnerText);

                    if (VerteidigungInfoUp.InnerHtml  == null)
                        ORes.Url = "";
                    else
                        ORes.Url = "fmenge";
                    CreatVerteidigungBtn(_This.TabVerteidigung, BuildDRs[0], VerteidigungInfoUp, ORes, LNud);//创建按钮
                }
            }
            
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="TabBuild"></param>
        /// <param name="BuildDR"></param>
        /// <param name="BuildInfoUp"></param>
        /// <param name="ORes"></param>
        public void CreatVerteidigungBtn(TabControl TabVerteidigung, DataRow BuildDR, HtmlElement BuildInfoUp, ObjectInfo ORes, List<NumericUpDown> LNud)
        {
            //设置提示文字，用来人性化提示
            StringBuilder ToopStr = new StringBuilder();

            Label Period = new Label();//建造时间
            Period.Text = ORes.Period.ToLongTimeString();
            Period.AutoSize = false;
            Period.Height = 14;
            Period.Dock = DockStyle.Top;
            Period.TextAlign = ContentAlignment.MiddleRight;
            Period.Parent = TabVerteidigung.TabPages[BuildDR["Category"].ToString()];

            Label Deuterium = new Label();//重氢
            if (ORes.Deuterium > (double)_This._NowRes.Deuterium)
            {
                ToopStr.Insert(0, "氢：" + ORes.Deuterium.ToString("#,##0") + "(-" + (ORes.Deuterium - (double)_This._NowRes.Deuterium).ToString("#,##0") + ")");
                Deuterium.ForeColor = Color.FromKnownColor(KnownColor.Red);
            }
            else
            {
                ToopStr.Insert(0, "氢：" + ORes.Deuterium.ToString("#,##0"));
            }
            Deuterium.Text = ORes.Deuterium.ToString("#,##0") + "氢";
            Deuterium.AutoSize = false;
            Deuterium.Height = 14;
            Deuterium.Dock = DockStyle.Top;
            Deuterium.TextAlign = ContentAlignment.MiddleRight;
            Deuterium.Parent = TabVerteidigung.TabPages[BuildDR["Category"].ToString()];

            Label Kristall = new Label();//晶体
            if (ORes.Kristall > (double)_This._NowRes.Kristall)
            {
                // Kristall.Text = "(-" + (ORes.Kristall - (double)_This._NowRes.Kristall).ToString("#,##0") + ")" + ORes.Kristall.ToString("#,##0") + "晶";
                ToopStr.Insert(0, "晶：" + ORes.Kristall.ToString("#,##0") + "(-" + (ORes.Kristall - (double)_This._NowRes.Kristall).ToString("#,##0") + ")" + "\r\n");
                Kristall.ForeColor = Color.FromKnownColor(KnownColor.Red);
            }
            else
            {
                ToopStr.Insert(0, "晶：" + ORes.Kristall.ToString("#,##0") + "\r\n");
            }
            Kristall.Text = ORes.Kristall.ToString("#,##0") + "晶";
            Kristall.AutoSize = false;
            Kristall.Height = 14;
            Kristall.Dock = DockStyle.Top;
            Kristall.TextAlign = ContentAlignment.MiddleRight;
            Kristall.Parent = TabVerteidigung.TabPages[BuildDR["Category"].ToString()];

            Label Metall = new Label();//金属
            if (ORes.Metall > (double)_This._NowRes.Metall)
            {
                // Metall.Text = "(-" + (ORes.Metall - (double)_This._NowRes.Metall).ToString("#,##0") + ")" + ORes.Metall.ToString("#,##0") + "金";
                ToopStr.Insert(0, "金：" + ORes.Metall.ToString("#,##0") + "(-" + (ORes.Metall - (double)_This._NowRes.Metall).ToString("#,##0") + ")" + "\r\n");
                Metall.ForeColor = Color.FromKnownColor(KnownColor.Red);
            }
            else
            {
                 
                ToopStr.Insert(0, "金：" + ORes.Metall.ToString("#,##0") + "\r\n");
            }
            Metall.Text = ORes.Metall.ToString("#,##0") + "金";
            Metall.AutoSize = false;
            Metall.Height = 14;
            Metall.Dock = DockStyle.Top;
            Metall.TextAlign = ContentAlignment.MiddleRight;
            Metall.Parent = TabVerteidigung.TabPages[BuildDR["Category"].ToString()];

            NumericUpDown VerteidigungBuild = new NumericUpDown();//建造数量
            VerteidigungBuild.Height = 20;
            VerteidigungBuild.Dock = DockStyle.Top;
            VerteidigungBuild.TextAlign = HorizontalAlignment.Right;

            VerteidigungBuild.Parent = TabVerteidigung.TabPages[BuildDR["Category"].ToString()];
            VerteidigungBuild.Tag = "&fmenge[" + BuildDR["gid"].ToString() + "]=";
            if (ORes.Url.Length == 0)
            {//无法建造
                VerteidigungBuild.Enabled = false;
            }
            else
            {
                VerteidigungBuild.Enabled = true;
                LNud.Add(VerteidigungBuild);
            }

            Label VerteidigungName = new Label();//防御名字
            VerteidigungName.Text = BuildDR["SimpleText"].ToString() + "(" + ORes.Level.ToString() +")";
            VerteidigungName.AutoSize = false;
            VerteidigungName.Height = 14;
            VerteidigungName.Dock = DockStyle.Top;
            VerteidigungName.TextAlign = ContentAlignment.MiddleCenter ;
            VerteidigungName.Parent = TabVerteidigung.TabPages[BuildDR["Category"].ToString()];

            _This.toolTip1.SetToolTip(VerteidigungBuild, ToopStr.ToString());
            _This.toolTip1.SetToolTip(Period, ToopStr.ToString());
            _This.toolTip1.SetToolTip(Deuterium, ToopStr.ToString());
            _This.toolTip1.SetToolTip(Kristall, ToopStr.ToString());
            _This.toolTip1.SetToolTip(Metall, ToopStr.ToString());
            _This.toolTip1.SetToolTip(VerteidigungName, ToopStr.ToString());
        }

        public void CreatVerteidigungBuildBtn(TabControl TabVerteidigung, List<NumericUpDown> LNud)
        {
            Button BuildBtn1 = new Button();
            BuildBtn1.Text = "建造";
            BuildBtn1.Dock = DockStyle.Bottom ;
            BuildBtn1.Dock = DockStyle.Top;
            BuildBtn1.Tag = LNud;
            BuildBtn1.Click += new EventHandler(VerteidigungBuild_Click);
            BuildBtn1.Parent = TabVerteidigung.TabPages[0];

            Button BuildBtn2 = new Button();
            BuildBtn2.Text = "建造";
            BuildBtn2.Dock = DockStyle.Top;
            BuildBtn2.Tag = LNud;
            BuildBtn2.Click += new EventHandler(VerteidigungBuild_Click);
            BuildBtn2.Parent = TabVerteidigung.TabPages[1];

            Button BuildBtn3= new Button();
            BuildBtn3.Text = "建造";
            BuildBtn3.Dock = DockStyle.Top;
            BuildBtn3.Tag = LNud;
            BuildBtn3.Click += new EventHandler(VerteidigungBuild_Click);
            BuildBtn3.Parent = TabVerteidigung.TabPages[2];

        }

        void VerteidigungBuild_Click(object sender, EventArgs e)
        {
            Button Btn = (Button)sender;
            _This._WebWork = CR_Galaxy.OGControl.OGControl.EWebWork.Verteidigung; //建造建筑
            _This._WebState = CR_Galaxy.OGControl.OGControl.EWebState.Loading;//载入中，不允许界面做修改
            _This.Enabled = false;

            //生成建造URL
            string FUrl = string.Format(VerteidigungUrl, _This.Session, _This.PlanetID);
            List<NumericUpDown> LNud = (List<NumericUpDown>)Btn.Tag;
            for (int i = 0; i < LNud.Count; i++)
            {
                if (LNud[i].Value != 0)
                {
                    FUrl += LNud[i].Tag.ToString() + LNud[i].Value.ToString();
                }
            }
            _This._WB.Stop();
            _This._WB.Navigate(_This.WebsiteEx + FUrl);
        }

        //---------------------------------------------------
        //建造列表

        public void VerteidigungBuildList(HtmlElement BuildList)
        {
            List<string[]> BuildListInfo = Info.GetBuildListInfo(BuildList.InnerHtml);
            if (BuildListInfo == null) return;
            _This.VerteidigungBuildList.Items.Clear();
            for (int i = 0; i < BuildListInfo.Count; i++)
            {
                ListViewItem Lvi = _This.VerteidigungBuildList.Items.Add(BuildListInfo[i][2].ToString() + " " + BuildListInfo[i][1].ToString());
                Lvi.SubItems.Add(BuildListInfo[i][2].ToString());
                Lvi.SubItems.Add(BuildListInfo[i][0].ToString());
                if (i == 0) _This.VerteidigungNowCountDownEX.Max = Convert.ToInt64(BuildListInfo[i][0].ToString()) * 10000000; ;
            }
            Match Mc = Regex.Match(BuildList.InnerHtml, @"(?<=g = ).+?(?=;)");
            long NowBuildTime = (long)((Convert.ToDouble( BuildListInfo[0][0])- Convert.ToDouble(Mc.Value.Trim())) * 10000000);
            DateTime Dt = DateTime.Now.AddTicks(NowBuildTime);
            _This.VerteidigungNowBuild.Text = BuildListInfo[0][1].ToString();
            _This.VerteidigungNowBuildTime.Text = Dt.ToString("M-d HH:mm:ss");

            //设置倒计时时间控件以及显示的Label
            _This.VerteidigungNowCountDownEX.Tag = Dt;
            _This.BuildNowTimer.Tag = _This.VerteidigungNowCountDownEX;
            _This.BuildNowTimer.Start();

            //如果有建造列表，那么就直接转入建造列表
            _This.TabVerteidigung.SelectedIndex = 3;
        }

    }
}
