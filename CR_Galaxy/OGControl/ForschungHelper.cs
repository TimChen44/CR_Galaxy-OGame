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
    /// 研究模块
    /// </summary>
    class ForschungHelper
    {
        OGControl _This;

        //停止研究
        string ForsCalcel = "index.php?page=buildings&session={0}&unbau={1}&mode=Forschung{2}";


//        研究
//index.php?page=buildings&session=9458ca80224d&mode=Forschung&bau=111
//取消
//index.php?page=buildings&session=9458ca80224d&unbau=111&mode=Forschung&cp=33622696

        public ForschungHelper(OGControl This)
        {
            _This = This;
        }

        public void ClearForschungBtn(TabControl TabBuild)
        {
            foreach (TabPage TP in TabBuild.TabPages)
            {
                if (TP == TabBuild.TabPages[3]) continue;
                TP.Controls.Clear();
            }
            _This.ForschungLevelList.Controls.Clear();
        }

        //和建筑那个模块相似
        public void ForschungRending(DataTable BuildDT, HtmlElement ContentHE)
        {
            _This.ForschungNow.Visible = false;

            //用来记录建筑物数量
            _This.ForschungLevelList.Tag = 0;

            //真正生成控件
            Calc calc = new Calc();
            for (int i = ContentHE.Children.Count - 1; i >= 1; i--)
            {//遍历所有可建造的东西
                if (ContentHE.Children[i].Children.Count == 0) continue;//有些建筑物没有，所以遍历不到

                HtmlElement ForschungName = ContentHE.Children[i].Children[1].Children[0];//得到建造物名字
                HtmlElement ForschungInfo = ContentHE.Children[i].Children[1];//得到建造物信息,用于获得等级.
                HtmlElement ForschungInfoUp = ContentHE.Children[i].Children[2];//得到建造物的状态、是否可建造
                DataRow[] BuildDRs = BuildDT.Select("Text = '" + ForschungName.InnerText + "'");
                if (BuildDRs.Length == 1)//检索到这个研究信息
                {
                    ObjectInfo ORes = calc.CalcForschungRes(BuildDRs[0], Info.GetLevelValue(ForschungInfo.InnerText), ForschungInfo.InnerText);
                    ORes.Text = ForschungName.InnerText;
                    SetForschungUrl(ForschungInfoUp, ORes, BuildDRs[0]);
                    CreatForschungBtn(_This.TabForschung, BuildDRs[0], ForschungInfoUp, ORes);//创建按钮
                }
            }
        }


            //分析建造链接
        private void SetForschungUrl(HtmlElement ForschungInfoUp, ObjectInfo ORes,DataRow DR)
        {
            if (ForschungInfoUp.InnerHtml == null) return;
            if (ForschungInfoUp.InnerHtml.Trim().Length == 0)
            {//没有内容说明不用升级
                ORes.Url = "";
                ORes.State = EBuildState.Disabled;
            }
            else if (ForschungInfoUp.InnerHtml.Trim().IndexOf("<A href") == 0)
            {//如果可以升级

                string TmpUrl = ForschungInfoUp.InnerHtml.Substring(ForschungInfoUp.InnerHtml.IndexOf("index.php"));
                TmpUrl = TmpUrl.Substring(0, TmpUrl.IndexOf("\">"));

                ORes.Url = TmpUrl.Replace("amp;", "");//不知道是什么，必须剔除
                ORes.State = EBuildState.Enabled;
            }
            else if (ForschungInfoUp.InnerHtml.Trim().IndexOf("<DIV") == 0)
            {//正在升级中的研究
                string TmpStr = ForschungInfoUp.InnerHtml.Replace(" ","");
                ORes.Url = string.Format(ForsCalcel, _This.Session, DR["gid"].ToString(), _This.PlanetID );
                ORes.State = EBuildState.Uping;
                Regex Rx2 = new Regex("ss=[0-9]*;");
                Match Mc2 = Rx2.Match(TmpStr);
                string Pc2 = Info.GetEndNum(Mc2.Value);

                //获得剩余时间
                long T = (long)(Convert.ToDouble(Pc2) * 10000000);
                DateTime DateEnd = DateTime.Now.AddTicks(T);

                //设置倒计时时间控件以及显示的Label
                _This.ForschungNowCountDownEX.Max = ORes.Period.Ticks;
                _This.ForschungNowCountDownEX.Tag = DateEnd;
                _This.BuildNowTimer.Tag = _This.ForschungNowCountDownEX;
                _This.BuildNowTimer.Start();

                //显示当前研究情况
                _This.ForschungNowText.Text = ORes.Text;
                _This.ForschungNowTime.Text = DateEnd.ToString("M-d HH:mm:ss");
                _This.ForschungNowCancel.Tag = ORes.Url;
                _This.ForschungNowCancel.Click += new EventHandler(ForschungName_Click);
                _This.ForschungNow.Visible = true;
            }
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="TabBuild"></param>
        /// <param name="BuildDR"></param>
        /// <param name="BuildInfoUp"></param>
        /// <param name="ORes"></param>
        public void CreatForschungBtn(TabControl TabForschung, DataRow BuildDR, HtmlElement BuildInfoUp, ObjectInfo ORes)
        {
            //设置提示文字，用来人性化提示
            StringBuilder ToopStr = new StringBuilder();

            Label Period = new Label();//建造时间
            Period.Text = ORes.Period.ToLongTimeString();
            Period.AutoSize = false;
            Period.Height = 14;
            Period.Dock = DockStyle.Top;
            Period.TextAlign = ContentAlignment.MiddleRight;
            Period.Parent = TabForschung.TabPages[BuildDR["Category"].ToString()];

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
            Deuterium.Parent = TabForschung.TabPages[BuildDR["Category"].ToString()];

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
            Kristall.Parent = TabForschung.TabPages[BuildDR["Category"].ToString()];

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
            Metall.Parent = TabForschung.TabPages[BuildDR["Category"].ToString()];

            Button BuildName = new Button();//建造名字
            BuildName.Text = BuildDR["SimpleText"].ToString() + ORes.Level.ToString();
            BuildName.AutoSize = false;
            BuildName.Height = 20;
            BuildName.Dock = DockStyle.Top;
            BuildName.TextAlign = ContentAlignment.MiddleCenter;

            BuildName.Parent = TabForschung.TabPages[BuildDR["Category"].ToString()];
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
                BuildName.Click += new EventHandler(ForschungName_Click);
            }
            else if (ORes.State == EBuildState.Uping)
            {//正在升级中
                BuildName.ForeColor = Color.FromArgb(190, 0, 0);
                BuildName.Click += new EventHandler(ForschungName_Click);
            }

            _This.toolTip1.SetToolTip(Period, ToopStr.ToString());
            _This.toolTip1.SetToolTip(Deuterium, ToopStr.ToString());
            _This.toolTip1.SetToolTip(Kristall, ToopStr.ToString());
            _This.toolTip1.SetToolTip(Metall, ToopStr.ToString());
            _This.toolTip1.SetToolTip(BuildName, ToopStr.ToString());


            //创建研究列表

            Button BuildName2 = new Button();//建造名字
            BuildName2.Text = BuildDR["SimpleText"].ToString() + ORes.Level.ToString();
            BuildName2.AutoSize = false;
            BuildName2.Height = 20;
            BuildName2.Dock = DockStyle.Top;
            BuildName2.TextAlign = ContentAlignment.MiddleCenter;

            BuildName2.Parent = _This.ForschungLevelList;
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
                BuildName2.Click += new EventHandler(ForschungName_Click);
            }
            else if (ORes.State == EBuildState.Uping)
            {//正在升级中
                BuildName2.ForeColor = Color.FromArgb(190, 0, 0);
                BuildName2.Click += new EventHandler(ForschungName_Click);
            }

            _This.ForschungLevelList.Tag = Convert.ToDouble(_This.ForschungLevelList.Tag) + ORes.Level;
            _This.ForschungLevelList.Text = "研究数量 " + _This.ForschungLevelList.Tag.ToString();

        }

        void ForschungName_Click(object sender, EventArgs e)
        {
            Button Btn = (Button)sender;
            _This._WebWork = CR_Galaxy.OGControl.OGControl.EWebWork.Forschung; //建造建筑
            _This._WebState = CR_Galaxy.OGControl.OGControl.EWebState.Loading;//载入中，不允许界面做修改
            _This.Enabled = false;
            _This._WB.Stop();
            _This._WB.Navigate(_This.WebsiteEx + Btn.Tag.ToString());
        }



    }
}
