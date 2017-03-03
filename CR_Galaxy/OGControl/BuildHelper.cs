using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;


namespace CR_Galaxy.OGControl
{
    class BuildHelper
    {
        OGControl _This;

        public BuildHelper(OGControl This)
        {
            _This = This;
        }

        public void ClearBuildBtn(TabControl TabBuild)
        {
            foreach (TabPage TP in TabBuild.TabPages)
            {
                if (TP == TabBuild.TabPages[3]) continue;
                TP.Controls.Clear();
            }
            _This.BuildLevelList.Controls.Clear();
        }

        //建造
        string BuildAdd = "index.php?page=b_building&session={0}&modus=add&techid={2}&cp={1}";
        //取消
        string BuildRemove = "index.php?page=b_building&session={0}&listid=1&modus=remove&cp={1}";



        public void BuildRending(DataTable BuildDT, HtmlElement ContentHE)
        {
            //预读机器人信息，纳米机器人信息
            double RotLevel = 0;
            double NanoRotLevel = 0;
            for (int i = 0; i < ContentHE.Children.Count; i++)
            {
                if (ContentHE.Children[i].Children.Count == 0) continue;
                HtmlElement BuildName = ContentHE.Children[i].Children[1].Children[0];//得到建造物名字
                if (BuildName.InnerText.Trim() == Info.Rot)
                {
                    RotLevel = Info.GetLevelValue(ContentHE.Children[i].Children[1].InnerText);
                }
                else if (BuildName.InnerText.Trim() == Info.NanoRot)
                {
                    NanoRotLevel = Info.GetLevelValue(ContentHE.Children[i].Children[1].InnerText);
                }
            }

            _This.BuildNow.Visible = false;

            //用来记录建筑物数量
            _This.BuildLevelList.Tag = 0;

            //真正生成控件
            Calc calc = new Calc();
            for (int i = ContentHE.Children.Count - 1; i >= 0; i--)
            {//遍历所有可建造的东西
                if (ContentHE.Children[i].Children.Count == 0) continue;//有些建筑物没有，所以遍历不到

                HtmlElement BuildName = ContentHE.Children[i].Children[1].Children[0];//得到建造物名字
                HtmlElement BuildInfo = ContentHE.Children[i].Children[1];//得到建造物信息,用于获得等级.
                HtmlElement BuildInfoUp = ContentHE.Children[i].Children[2];//得到建造物的状态、是否可建造
                DataRow[] BuildDRs = BuildDT.Select("Text = '" + BuildName.InnerText + "'");
                if (BuildDRs.Length == 1)//检索到这个建筑的信息
                {
                    ObjectInfo ORes = calc.CalcBuildRes(BuildDRs[0], Info.GetLevelValue(BuildInfo.InnerText), RotLevel, NanoRotLevel);
                    ORes.Text = BuildName.InnerText;
                    SetBuildUrl(BuildDRs[0], BuildInfoUp, ORes, _This);
                    CreatBuildBtn(_This.TabBuild, BuildDRs[0], BuildInfoUp, ORes);//创建按钮
                }
            }


        }

        //分析建造链接
        private void SetBuildUrl(DataRow BuildDR, HtmlElement BuildInfoUp, ObjectInfo ORes, OGControl This)
        {
            if (BuildInfoUp.InnerHtml == null) return;
            if (BuildInfoUp.InnerHtml.Trim().Length == 0)
            {//没有内容说明不用升级
                ORes.Url = "";
                ORes.State = EBuildState.Disabled;
            }
            else if (BuildInfoUp.InnerHtml.Trim().IndexOf("<A href") == 0)
            {//如果可以升级
                //string TmpUrl = BuildInfoUp.InnerHtml.Substring(BuildInfoUp.InnerHtml.IndexOf("index.php"));
                //TmpUrl = TmpUrl.Substring(0, TmpUrl.IndexOf("\">"));
                //ORes.Url = TmpUrl.Replace("amp;", "");//不知道是什么，必须剔除

                ORes.Url = string.Format(BuildAdd, _This.Session, _This.PlanetID, BuildDR["gid"].ToString());
                ORes.State = EBuildState.Enabled;
            }
            else if (BuildInfoUp.InnerHtml.Trim().IndexOf("<DIV") == 0)
            {//正在升级中的建筑
                string TmpStr = BuildInfoUp.InnerHtml.Substring(BuildInfoUp.InnerHtml.IndexOf("index.php?"));
                //ORes.Url = TmpStr.Substring(0, TmpStr.IndexOf("\">")).Replace("amp;", "");
                ORes.Url = string.Format(BuildRemove, _This.Session, _This.PlanetID);
                ORes.State = EBuildState.Uping;

                //当前建造情况
                TmpStr = TmpStr.Substring(TmpStr.IndexOf("pp='") + "pp='".Length);
                TmpStr = TmpStr.Substring(0, TmpStr.IndexOf("'"));
                //获得剩余时间

                //显示当前建造情况
                long T = (long)(Convert.ToDouble(TmpStr) * 10000000);
                DateTime DateEnd = DateTime.Now.AddTicks(T);

                //设置倒计时时间控件以及显示的Label
                _This.BuildNowCountdownEX.Max = ORes.Period.Ticks;
                _This.BuildNowCountdownEX.Tag = DateEnd;
                _This.BuildNowTimer.Tag = _This.BuildNowCountdownEX;
                _This.BuildNowTimer.Start();

                //显示当前研究情况
                _This.BuildNowText.Text = ORes.Text;
                _This.BuildNowTime.Text = DateEnd.ToString("M-d HH:mm:ss");
                _This.BuildNowCancel.Tag = ORes.Url;
                _This.BuildNowCancel.Click += new EventHandler(BuildName_Click);
                _This.BuildNow.Visible = true;
            }

        }

        public void CreatNowBuildBtn(string Time, ObjectInfo ORes)
        {


        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="TabBuild"></param>
        /// <param name="BuildDR"></param>
        /// <param name="BuildInfoUp"></param>
        /// <param name="ORes"></param>
        public void CreatBuildBtn(TabControl TabBuild, DataRow BuildDR, HtmlElement BuildInfoUp, ObjectInfo ORes)
        {

            //设置提示文字，用来人性化提示
            StringBuilder ToopStr = new StringBuilder();

            Label Period = new Label();//建造时间
            Period.Text = ORes.Period.ToLongTimeString();
            Period.AutoSize = false;
            Period.Height = 14;
            Period.Dock = DockStyle.Top;
            Period.TextAlign = ContentAlignment.MiddleRight;
            Period.Parent = TabBuild.TabPages[BuildDR["Category"].ToString()];

            Label Deuterium = new Label();//重氢
            if (ORes.Deuterium > (double)_This._NowRes.Deuterium)
            {
                //Deuterium.Text = "(-" + (ORes.Deuterium - (double)_This._NowRes.Deuterium).ToString("#,##0") + ")" + ORes.Deuterium.ToString("#,##0") + "氢";
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
            Deuterium.Parent = TabBuild.TabPages[BuildDR["Category"].ToString()];

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
            Kristall.Parent = TabBuild.TabPages[BuildDR["Category"].ToString()];

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
            Metall.Parent = TabBuild.TabPages[BuildDR["Category"].ToString()];

            Button BuildName = new Button();//建造名字
            BuildName.Text = BuildDR["SimpleText"].ToString() + ORes.Level.ToString();
            BuildName.AutoSize = false;
            BuildName.Height = 20;
            BuildName.Dock = DockStyle.Top;
            BuildName.TextAlign = ContentAlignment.MiddleCenter;

            BuildName.Parent = TabBuild.TabPages[BuildDR["Category"].ToString()];
            BuildName.Tag = ORes.Url;
            BuildName.Font = new Font("宋体", 9, FontStyle.Bold);
            if (ORes.State == EBuildState.Disabled)
            {//无法建造
                BuildName.ForeColor = Color.FromArgb(190, 0, 0);
                BuildName.Enabled = false;
            }
            else if (ORes.State == EBuildState.Enabled)
            {//可以建造
                BuildName.ForeColor = Color.FromArgb(0, 130, 0);
                BuildName.Click += new EventHandler(BuildName_Click);
            }
            else if (ORes.State == EBuildState.Uping)
            {//正在升级中
                BuildName.ForeColor = Color.FromArgb(190, 0, 0);
                BuildName.Click += new EventHandler(BuildName_Click);
            }
            ToopStr.Insert(0, "资源列表" + "\r\n");

            _This.toolTip1.SetToolTip(Period, ToopStr.ToString());
            _This.toolTip1.SetToolTip(Deuterium, ToopStr.ToString());
            _This.toolTip1.SetToolTip(Kristall, ToopStr.ToString());
            _This.toolTip1.SetToolTip(Metall, ToopStr.ToString());
            _This.toolTip1.SetToolTip(BuildName, ToopStr.ToString());

            //创建建筑物等级列表
            //if (ORes.Level > 0)
            //{
            //Label BuildList = new Label();
            //BuildList.Text = BuildDR["SimpleText"].ToString() + ORes.Level.ToString();
            //BuildList.Parent = _This.BuildLevelList;
            //BuildList.AutoSize = false;
            //BuildList.Height = 14;
            //BuildList.Dock = DockStyle.Top;


            Button BuildName2 = new Button();//建造名字
            BuildName2.Text = BuildDR["SimpleText"].ToString() + ORes.Level.ToString();
            BuildName2.AutoSize = false;
            BuildName2.Height = 20;
            BuildName2.Dock = DockStyle.Top;
            BuildName2.TextAlign = ContentAlignment.MiddleCenter;

            BuildName2.Parent = _This.BuildLevelList;
            BuildName2.Tag = ORes.Url;
            BuildName2.Font = new Font("宋体", 9, FontStyle.Bold);
            if (ORes.State == EBuildState.Disabled)
            {//无法建造
                BuildName2.ForeColor = Color.FromArgb(190, 0, 0);
                BuildName2.Enabled = false;
            }
            else if (ORes.State == EBuildState.Enabled)
            {//可以建造
                BuildName2.ForeColor = Color.FromArgb(0, 130, 0);
                BuildName2.Click += new EventHandler(BuildName_Click);
            }
            else if (ORes.State == EBuildState.Uping)
            {//正在升级中
                BuildName2.ForeColor = Color.FromArgb(190, 0, 0);
                BuildName2.Click += new EventHandler(BuildName_Click);
            }

            _This.BuildLevelList.Tag = Convert.ToDouble ( _This.BuildLevelList.Tag) + ORes.Level;
            _This.BuildLevelList.Text = "建筑数量 " + _This.BuildLevelList.Tag.ToString();
            //}
        }

        void BuildName_Click(object sender, EventArgs e)
        {
            Button Btn = (Button)sender;
            _This._WebWork = CR_Galaxy.OGControl.OGControl.EWebWork.b_building; //建造建筑
            _This._WebState = CR_Galaxy.OGControl.OGControl.EWebState.Loading;//载入中，不允许界面做修改
            _This.Enabled = false;
            _This._WB.Navigate(_This.WebsiteEx + Btn.Tag.ToString());
        }
    }
}