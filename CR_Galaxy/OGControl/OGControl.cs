using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

namespace CR_Galaxy.OGControl
{
    public partial class OGControl : UserControl
    {
        public WebBrowser _WB;
        CR_Soft.Windows.Web.ClassWB _ClassWB;
        CR_Soft.Data.DataHelper _DataHelper = new CR_Soft.Data.DataHelper(CR_Soft.Data.EDataType.Oledb);
        string mdb = Path.GetDirectoryName(Application.ExecutablePath) + "\\Data\\Date.mdb";
        DataTable _BuildDT;

        Galaxy _Galaxy;

        #region 属性

        private string _WebsiteEx="";
        /// <summary>
        /// 游戏根目录
        /// </summary>
        public string WebsiteEx
        {
            get { return _WebsiteEx; }
            set { _WebsiteEx = value; }
        }


        private string _Session;
        /// <summary>
        /// 
        /// </summary>
        public string Session
        {
            get { return _Session; }
            set { _Session = value; }
        }

        private string _Planet;
        /// <summary>
        /// 星球
        /// </summary>
        public string Planet
        {
            get { return _Planet; }
            set
            {
                _Planet = value;
                PlanetName.Text = _Planet;
            }
        }

        private string _PlanetID;
        /// <summary>
        /// 星球编号
        /// </summary>
        public string PlanetID
        {
            get { return _PlanetID; }
            set { _PlanetID =  value; }
            //set { _PlanetID = "&planet=" + value; }
        }

        private string _Url;
        /// <summary>
        /// 起始用URL
        /// </summary>
        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
        }

        private string _GalaxyLocation;
        /// <summary>
        /// 银河
        /// </summary>
        public string GalaxyLocation
        {
            get { return _GalaxyLocation; }
            set { _GalaxyLocation = value; }
        }

        private string _SystemLocation;
        /// <summary>
        /// 太阳系
        /// </summary>
        public string SystemLocation
        {
            get { return _SystemLocation; }
            set { _SystemLocation = value; }
        }

        private string _PositionLocation;
        /// <summary>
        /// 星球
        /// </summary>
        public string PositionLocation
        {
            get { return _PositionLocation; }
            set { _PositionLocation = value; }
        }

        private string _Location;
        /// <summary>
        /// 地址
        /// </summary>
        public string Location
        {
            get { return _Location; }
            set { _Location = value; }
        }

        //建造
        private string _Urlb_building ;
        //资源
        private string _UrlResources;
        //研究
        private string _UrlForschung;
        //造船厂
        private string _UrlFlotte ;
        //防御
        private string _UrlVerteidigung;

        public EWebState _WebState;
        public EWebWork _WebWork;

        //当前状态
        public enum EWebState
        {
            Loading, //载入中
            Complete, //完成
            Free//空闲
        }

        //当前操作
        public enum EWebWork
        {
            b_building,  //建造
            Resources, //资源
            Forschung,//研究
            Flotte, //造船厂
            Verteidigung//防御
        }

        #endregion

        #region FS用属性

        public string  _FSGalaxy;

        public string _FSSystem;

        public string _FSPosition;

        public string _FSSheep;

        public string _FSOrder;

        #endregion

        #region 资源属性

        //当前资源
       public  CNowRes _NowRes;
        //所有资源信息
        CRes _Res;
        //舰队飞行处理类
       public  CR_Galaxy.OGControl.FlottenCommand _FlottenCmd;

       public OGControlManage _OGCOntrolManage;

        #endregion

        #region 错误处理用属性

        //加载计时，如果超时那么就从新刷新
        private int ErrorSpan = 0;
        private int ErrorSpanCount =60;
        private bool ErrorRetLoad = false;

        #endregion

        public OGControl(FlottenCommand FlottenCmd, OGControlManage OGCOntrolManage, Galaxy Galaxy)
        {
            InitializeComponent();
            _WB = new WebBrowser();
            _WB.ScriptErrorsSuppressed = true;
            _WB.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(_WB_DocumentCompleted);
            _WB.Navigating += new WebBrowserNavigatingEventHandler(_WB_Navigating);
            _WB.Navigated += new WebBrowserNavigatedEventHandler(_WB_Navigated);
            _WB.Navigate("about:blank");

            _ClassWB = new CR_Soft.Windows.Web.ClassWB(_WB);

            _FlottenCmd = FlottenCmd;
            _OGCOntrolManage = OGCOntrolManage;
            _Galaxy = Galaxy;
        }


        public void StartLoad()
        {
            _DataHelper.Connection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=[Path];Persist Security Info=True", mdb);
            _BuildDT = _DataHelper.TableSql(CR_Soft.Data.CreatSQLEX.ShowAll("Build"));
            _DataHelper.disConnection();

            //建造
            _Urlb_building = _WebsiteEx + "/index.php?page=b_building&session={0}{1}";
            //资源
            _UrlResources = _WebsiteEx + "/index.php?page=resources&session={0}{1}";
            //研究
            _UrlForschung = _WebsiteEx + "/index.php?page=buildings&session={0}&mode=Forschung{1}";
            //造船厂
            _UrlFlotte = _WebsiteEx + "/index.php?page=buildings&session={0}&mode=Flotte{1}";
            //防御
            _UrlVerteidigung = _WebsiteEx + "/index.php?page=buildings&session={0}&mode=Verteidigung{1}";

            resources_Enter(null, null);
        }

        /// <summary>
        /// 页面载入完成需要执行的东西
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _WB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.ToString() == "about:blank") return;

            try
            {
                //得到资源信息
                ResRead resRead = new ResRead();
                _NowRes = resRead.GetNowRes(_WB.Document);
                metall.LabelText  = _NowRes.Metall.ToString("#,##0");
                kristall.LabelText  = _NowRes.Kristall.ToString("#,##0");
                deuterium.LabelText  = _NowRes.Deuterium.ToString("#,##0");

                energie.Text = _NowRes.Energie;
                if (_NowRes.Energie.Substring(0, 1) == "-") energie.ForeColor = Color.Red;
                else energie.ForeColor = Color.Black ;

                if (_WebWork == EWebWork.Resources)
                {
                    //完成资源载入，那么就不要站着位置了，呵呵
                    #region 资源面板

                    _Res = resRead.GetRes(_WB.Document);
                    //金属
                    labMetall.Text = _Res.Metall.ToString("#,##0") + "/时";
                    labMetDay.Text = _Res.MetDay.ToString("#,##0") + "/天";
                    labMetWeek.Text = _Res.MetWeek.ToString("#,##0") + "/周";
                    labMetEnergie.Text = _Res.MetEnergie;
                    cmbMetProduction.Text = _Res.MetProduction;
                    labMetLevel.Text = _Res.MetLevel;
                    labMetMemory.Text = "存储器 " + (_Res.MetMemory.ToString("#,##0") + "K").Replace(",000K", "K");
                    //晶体
                    if (_Res.KriLevel != "0")
                    {
                        labKristall.Text = _Res.Kristall.ToString("#,##0") + "/时";
                        labKriDay.Text = _Res.KriDay.ToString("#,##0") + "/天";
                        labKriWeek.Text = _Res.KriWeek.ToString("#,##0") + "/周";
                        labKriEnergie.Text = _Res.KriEnergie;
                        cmbKriProduction.Text = _Res.KriProduction;
                        labKriLevel.Text = _Res.KriLevel;
                        labKriMemory.Text = "存储器 " + (_Res.KriMemory.ToString("#,##0") + "K").Replace(",000K", "K");
                    }
                    //重氢
                    if (_Res.DeuLevel != "0")
                    {
                        labDeuterium.Text = _Res.Deuterium.ToString("#,##0") + "/时";
                        labDeuDay.Text = _Res.DeuDay.ToString("#,##0") + "/天";
                        labDeuWeek.Text = _Res.DeuWeek.ToString("#,##0") + "/周";
                        labDeuEnergie.Text = _Res.DeuEnergie;
                        cmbDeuProduction.Text = _Res.DeuProduction;
                        labDeuLevel.Text = _Res.DeuLevel;
                        labDeuMemory.Text = "存储器 " + (_Res.DeuMemory.ToString("#,##0") + "K").Replace(",000K", "K");
                    }
                    //核电站
                    if (_Res.AtomicLevel != "0")
                    {
                        panAtomic.Visible = true;
                        labAtomicLevel.Text = "核电站 " + _Res.AtomicLevel;
                        labAtomicLost.Text = _Res.AtomicLost.ToString("#,##0") + "/时";
                        labAtomicMake.Text = _Res.AtomicMake.ToString("#,##0") + "/时";
                        cmbAtomicProduction.Text = _Res.AtomicProduction;
                    }
                    //太卫
                    if (_Res.SatelliteLevel != "0")
                    {
                        panSatellite.Visible = true;
                        labSatelliteEnergie.Text = _Res.Satellite.ToString("#,##0") + "/时";
                        labSatelliteLevel.Text = "太卫 " + _Res.SatelliteLevel;
                        cmbSatelliteProduction.Text = _Res.SatelliteProduction;
                    }

                    //电站
                    labEnergie.Text = _Res.Energie;
                    cmbEneProduction.Text = _Res.EneProduction;
                    labEneLevel.Text = "发电站 " + _Res.EneLevel;

                    ResCalc.Text = "计算";

                    //_Galaxy.TxtLog.Text += "\r\n <Res>" + e.Url.ToString() ;
                    if (_Res.Metall != 0)
                    {
                        _OGCOntrolManage._ResRefNow = false;
                        _OGCOntrolManage._PlanetID = "";
                        //_Galaxy.TxtLog.Text += "\r\n" + this._Planet + " --释放资源加载权利";
                    }


                    #endregion
                    if (_Res != null)
                    {//更新百分比
                        metall.Max =Convert.ToInt64( _Res.MetMemory);
                        kristall.Max = Convert.ToInt64(_Res.KriMemory);
                        deuterium.Max = Convert.ToInt64(_Res.DeuMemory);

                        if (_Res.MetMemory != 0) metall.Value = Convert.ToInt64(_NowRes.Metall / _Res.MetMemory * 100);
                        if (_Res.KriMemory != 0) kristall.Value = Convert.ToInt64(_NowRes.Kristall / _Res.KriMemory * 100);
                        if (_Res.DeuMemory != 0) deuterium.Value = Convert.ToInt64(_NowRes.Deuterium / _Res.DeuMemory * 100);
                    }


                }
                else if (_WebWork == EWebWork.b_building)
                {
                    #region 建筑分类
                    /*
                    基础建设

                    金属矿 
                    晶体矿 
                    重氢分离器 
                    太阳能发电站 
                    核电站 

                    辅助建筑

                    机器人工厂 
                    纳米机器人工厂 
                    金属仓库    
                    晶体仓库    
                    重氢槽 

                    战斗&研究

                    造船厂
                    研究实验室
                    联盟太空站
                    导弹发射井
                    地形改造器 
                     */
                    #endregion
                    #region 建筑面板

                    HtmlElement Content = _WB.Document.GetElementById("content");
                    if (Content == null)
                    {
                        ErrorSpan = ErrorSpanCount;
                        return;
                    }

                    HtmlElement ContentHE = Content.Children[0].Children[1].Children[0].Children[0].Children[0].Children[0].Children[0];
                    if (ContentHE == null) return;

                    BuildHelper BuildH = new BuildHelper(this);//用来创建控件的一个类
                    BuildH.ClearBuildBtn(TabBuild);
                    BuildH.BuildRending(_BuildDT, ContentHE);//解析文件以及创建按钮

                    #endregion
                }

                else if (_WebWork == EWebWork.Forschung)
                {
                    #region 研究分类
                    /*
                    空间探测技术
                    计算机技术
                    武器技术
                    防御盾系统
                    装甲技术

                    能量技术
                    超空间技术
                    燃烧引擎
                    脉冲引擎
                    超空间引擎

                    激光技术
                    中子技术
                    等离子技术
                    跨星系科研网络
                    远征科技

                    引力技术
                     */
                    #endregion
                    #region 建筑面板

                    HtmlElement ContentHE = _WB.Document.GetElementById("content").Children[0].Children[1].Children[0].Children[0].Children[0].Children[0].Children[0];
                    if (ContentHE == null) return;

                    ForschungHelper ForschungH = new ForschungHelper(this);//用来处理研究界面的类
                    ForschungH.ClearForschungBtn(TabForschung);
                    ForschungH.ForschungRending(_BuildDT, ContentHE);//解析文件以及创建按钮

                    #endregion
                }
                else if (_WebWork == EWebWork.Flotte)
                {
                    #region 舰队
                    /*
                        小型运输舰
                        大型运输舰
                        轻型战斗机
                        重型战斗机

                        巡洋舰
                        战列舰
                        轰炸机
                        毁灭者
                        战斗巡洋舰
                        

                        太阳能卫星
                        探测器
                        殖民船
                        回收船
                        死星
                     */
                    #endregion
                    #region 舰队面板

                    HtmlElement Content = _WB.Document.GetElementById("content");
                    if (Content == null)
                    {
                        ErrorSpan = ErrorSpanCount;
                        return;
                    }

                    HtmlElement ContentHE = Content.Children[0].Children[1].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0];
                    if (ContentHE == null) return;

                    if (ContentHE.Children.Count == 1)
                    {
                        //没有造船厂
                        _WebState = EWebState.Free;
                        this.Enabled = true;
                        return;
                    }

                    FlotteHelper FlotteH = new FlotteHelper(this);//用来处理造船厂界面的类
                    FlotteH.ClearBuildBtn(TabFlotte);
                    FlotteH.FlotteRending(_BuildDT, ContentHE);//解析文件以及创建按钮

                    //解析建造列表
                    HtmlElement BuildList = Content.Children[0].Children[5];
                    FlotteH.FlotteBuildList(BuildList);

                    #endregion

                }
                else if (_WebWork == EWebWork.Verteidigung)
                {
                    #region 防御
                    /*
                        火箭发射装置
                        轻型激光炮
                        重型激光炮
                        高斯炮
                        中子炮
                     * 
                        等离子武器
                        小型防护罩
                        大型防护罩
                        拦截导弹
                        星际导弹
                     */
                    #endregion

                    #region 防御面板

                    HtmlElement Content = _WB.Document.GetElementById("content");
                    if (Content == null)
                    {
                        ErrorSpan = ErrorSpanCount;
                        return;
                    }

                    HtmlElement ContentHE = Content.Children[0].Children[1].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0];
                    if (ContentHE == null) return;

                    if (ContentHE.Children.Count == 1)
                    {
                        //没有造船厂
                        _WebState = EWebState.Free;
                        this.Enabled = true;
                        return;
                    }

                    VerteidigungHelper VerteidigungH = new VerteidigungHelper(this);//用来处理防御界面的类
                    VerteidigungH.ClearBuildBtn(TabVerteidigung);
                    VerteidigungH.VerteidigungRending(_BuildDT, ContentHE);//解析文件以及创建按钮

                    //解析建造列表
                    HtmlElement BuildList = Content.Children[0].Children[5];
                    VerteidigungH.VerteidigungBuildList(BuildList);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            _WebState = EWebState.Free;
            this.Enabled = true;
           // _OGCOntrolManage._LinkCount--;
        }


        /// <summary>
        /// 根据情况进行延迟加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _WB_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (this._PlanetID != _OGCOntrolManage._PlanetID)
            {//如果当前操作星球不是自己，那么就看看有没有轮到你,如果是你那么就免检
                if (e.Url.ToString().IndexOf("page=resources") > 0)
                {//如果是刷新资源，那么就要看看有没有其他星球正在获取资源
                    if (_OGCOntrolManage.GetNavigateAllow(ENavigateOther.Res))
                    {
                        DelayRefTeimer.Enabled = true;
                        DelayRefTeimer.Tag = e.Url;
                        ErrorSpan = 0;//应为是延迟刷新，说一不需要计算页面演示来引发错误
                        e.Cancel = true;
                        return;
                    }
                    _OGCOntrolManage._ResRefNow = true;
                    _OGCOntrolManage._PlanetID = this._PlanetID;
                   // _Galaxy.TxtLog.Text += "\r\n" + this._Planet + " " + this._Session + " ++获得资源加载权利  " + (_OGCOntrolManage._LinkCount+1).ToString() ;
                }
                else
                {//其他刷新检测
                    if (_OGCOntrolManage.GetNavigateAllow(ENavigateOther.Other))
                    {
                        DelayRefTeimer.Enabled = true;
                        DelayRefTeimer.Tag = e.Url;
                        e.Cancel = true;
                        return;
                    }

                }
            }
            DelayRefTeimer.Enabled = false;
            //_OGCOntrolManage._LinkCount++;            
        }

        /// <summary>
        /// 延迟加载某些页面，防止同步加载出现错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelayRefTeimer_Tick(object sender, EventArgs e)
        {
            _WB.Navigate((Uri)DelayRefTeimer.Tag);
        }

        void _WB_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
           
        }

        private void OGControl_Resize(object sender, EventArgs e)
        {
            this.Width = 110;
        }

        /// <summary>
        /// 资源面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resources_Enter(object sender, EventArgs e)
        {
            if ((_WB.Url != null) && (ErrorRetLoad == false))
            {
                if (_WB.Url.ToString().IndexOf("resources") > 0) return;
            }
            ErrorRetLoad = false;
            ResCalc.Text = "数据更新中...";
            _WB.Stop();
            _WB.Navigate(string.Format(_UrlResources, _Session, _PlanetID));
            _WebWork = EWebWork.Resources; //载入资源
            _WebState = EWebState.Loading;//载入中，不允许界面做修改
            this.Enabled = false;
        }

        /// <summary>
        /// 建造面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void b_building_Enter(object sender, EventArgs e)
        {
            if ((_WB.Url != null) && (ErrorRetLoad == false))
            {
                if (_WB.Url.ToString().IndexOf("b_building") > 0) return;
            }
            ErrorRetLoad = false;
            _WB.Stop();
            _WB.Navigate(string.Format(_Urlb_building, _Session, _PlanetID));
            _WebWork = EWebWork.b_building; //载入资源
            _WebState = EWebState.Loading;//载入中，不允许界面做修改
            this.Enabled = false;
        }

        /// <summary>
        /// 研究面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Forschung_Enter(object sender, EventArgs e)
        {
            if ((_WB.Url != null) && (ErrorRetLoad == false))
            {
                if (_WB.Url.ToString().IndexOf("mode=Forschung") > 0) return;
            }
            ErrorRetLoad = false;
            _WB.Stop();
            _WB.Navigate(string.Format(_UrlForschung, _Session, _PlanetID));
            _WebWork = EWebWork.Forschung; //载入资源
            _WebState = EWebState.Loading;//载入中，不允许界面做修改
            this.Enabled = false;

        }

        /// <summary>
        /// 造船厂面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Flotte_Enter(object sender, EventArgs e)
        {
            if ((_WB.Url != null) && (ErrorRetLoad == false))
            {
                if (_WB.Url.ToString().IndexOf("mode=Flotte") > 0) return;
            }
            ErrorRetLoad = false;
            _WB.Stop();
            _WB.Navigate(string.Format(_UrlFlotte , _Session, _PlanetID));
            _WebWork = EWebWork.Flotte; //载入资源
            _WebState = EWebState.Loading;//载入中，不允许界面做修改
            this.Enabled = false;
        }

        /// <summary>
        /// 防御面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Verteidigung_Enter(object sender, EventArgs e)
        {
            if ((_WB.Url != null) && (ErrorRetLoad == false))
            {
                if (_WB.Url.ToString().IndexOf("mode=Verteidigung") > 0) return;
            }
            ErrorRetLoad = false;
            _WB.Stop();
            _WB.Navigate(string.Format(_UrlVerteidigung, _Session, _PlanetID));
            _WebWork = EWebWork.Verteidigung; //载入资源
            _WebState = EWebState.Loading;//载入中，不允许界面做修改
            this.Enabled = false;
        }

        /// <summary>
        /// 资源计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResCalc_Click(object sender, EventArgs e)
        {
            //计算资源按钮点击
            ResCalc.Text = "计算中...";

            string ResUrl = _UrlResources + "&last1={2}&last2={3}&last4={4}";
            string last1 = ((10 - cmbMetProduction.SelectedIndex) * 10).ToString();
            string last2 = ((10 - cmbKriProduction.SelectedIndex) * 10).ToString();
            string last3 = ((10 - cmbEneProduction.SelectedIndex) * 10).ToString();
            if (_Res.DeuLevel != "0")//如果存在重氢分离器，那么就允许计算产量
            {
                ResUrl += "&last3=" + ((10 - cmbDeuProduction.SelectedIndex) * 10).ToString();
            }
            if (_Res.AtomicLevel != "0")//如果存在核电站，那么就允许计算产量
            {
                ResUrl += "&last12=" + ((10 - cmbAtomicProduction.SelectedIndex) * 10).ToString();
            }
            if (_Res.SatelliteLevel != "0")//如果存在太卫，那么就允许计算产量
            {
                ResUrl += "&last212=" + ((10 - cmbSatelliteProduction.SelectedIndex) * 10).ToString();
            }


            _WebWork = EWebWork.Resources; //载入资源
            _WebState = EWebState.Loading;//载入中，不允许界面做修改
            _WB.Navigate(string.Format(ResUrl, new object[] { _Session, _PlanetID, last1, last2, last3 }));
            this.Enabled = false;
        }

        /// <summary>
        /// 载入超时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ErrorTime_Tick(object sender, EventArgs e)
        {
            if ((ErrorSpan > ErrorSpanCount) && (this.Enabled == false) && (_WebState != EWebState.Free))
            {
                ErrorRetLoad = true;
                ErrorTime.Enabled = false;
                //MessageBox.Show("网络超时，重新加载，并保存错误信息！");
                //File.WriteAllText(Application.StartupPath + "\\Error" + DateTime.Now.Ticks + ".txt", _WB.DocumentText);
                RefOgControl(tabControl1.SelectedTab.Name);
                ErrorSpan = 0;
                ErrorTime.Enabled = true;
            }
            ErrorSpan++;
        }

        private void RefOgControl(string PageName)
        {
            switch (PageName)
            {
                case "Pageresources":
                    resources_Enter(null, null);
                    break;
                case "b_building":
                    b_building_Enter(null, null);
                    break;
                case "Forschung":
                    Forschung_Enter(null, null);
                    break;
                case "Flotte":
                    Flotte_Enter(null, null);
                    break;
            }
        }

        /// <summary>
        /// 超时检测启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OGControl_EnabledChanged(object sender, EventArgs e)
        {
            //如果窗体灰色，那么超时检测开启
            ErrorTime.Enabled = !this.Enabled;
            if (this.Enabled == true) ErrorSpan = 0;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 显示选择的项生产所需要的时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlotteBuildList_Click(object sender, EventArgs e)
        {
            long BuildTime = (long)(Convert.ToDouble(FlotteBuildList.SelectedItems[0].SubItems[1].Text) * Convert.ToDouble(FlotteBuildList.SelectedItems[0].SubItems[2].Text) * 10000000);
            TimeSpan Ts = new TimeSpan(BuildTime);

            FlotteBuildTime.Text = Ts.Days + "天" + Ts.Hours + "时" + Ts.Minutes + "分" + Ts.Seconds + "秒";
        }

        /// <summary>
        /// 显示选择的项生产所需要的时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VerteidigungBuildList_Click(object sender, EventArgs e)
        {
            long VerteidigungTime = (long)(Convert.ToDouble(VerteidigungBuildList.SelectedItems[0].SubItems[1].Text) * Convert.ToDouble(VerteidigungBuildList.SelectedItems[0].SubItems[2].Text) * 10000000); 
            TimeSpan Ts = new TimeSpan(VerteidigungTime);

            VerteidigungBuildTime.Text = Ts.Days + "天" + Ts.Hours + "时" + Ts.Minutes + "分" + Ts.Seconds + "秒";
        }


        private void labMetDay_Click(object sender, EventArgs e)
        {
      
        }

        /// <summary>
        /// 即时显示剩余建造时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuildNowTimer_Tick(object sender, EventArgs e)
        {
            if (BuildNowTimer.Tag != null)
            {
                OGLibrary.ProgressLabel NowBuildLable = (OGLibrary.ProgressLabel)BuildNowTimer.Tag;//要显示的LBALE放在时间空间的TAG中
                if (NowBuildLable.Parent.Visible == true)
                {//如果显示建造情况，那么说明有必要更新他
                    DateTime EndTime = (DateTime)NowBuildLable.Tag;
                    if (EndTime > DateTime.Now)
                    {
                        TimeSpan TimeCountDown = EndTime.Subtract(DateTime.Now);
                        NowBuildLable.LabelText = "";
                        if (TimeCountDown.Days > 0) NowBuildLable.LabelText  = TimeCountDown.Days.ToString() + "天";
                        NowBuildLable.LabelText += TimeCountDown.Hours.ToString() + ":" + TimeCountDown.Minutes.ToString() + ":" + TimeCountDown.Seconds.ToString();

                        NowBuildLable.Value = NowBuildLable.Max - TimeCountDown.Ticks;
                    }
                    else if (NowBuildLable.LabelText == "完成/刷新")
                    {
                        if (_WB.StatusText == "完成")
                        {
                            NowBuildLable.LabelText = "完成";
                            ErrorRetLoad = true;
                            RefOgControl(tabControl1.SelectedTab.Name);
                            NowBuildLable.Parent.Visible = false;
                        }
                    }
                    else
                    {
                      
                        NowBuildLable.LabelText = "完成/刷新";
                    }

                }
                else
                {
                    NowBuildLable.LabelText = "待命";
                }

            }

            //资源自动增长
            if ((_NowRes != null) && (_Res != null))
            {
                TimeSpan Ts = DateTime.Now.Subtract(_NowRes.UpDate);
                decimal TsD = Convert.ToDecimal(Ts.Ticks / 10000000);
                //_NowRes.Metall += (_Res.Metall * TsD /3600);
                //_NowRes.Kristall += (_Res.Kristall * TsD / 3600);
                //_NowRes.Deuterium += (_Res.Deuterium * TsD / 3600);
                metall.LabelText  = (_NowRes.Metall + _Res.Metall * TsD / 3600).ToString("#,##0");
                kristall.LabelText = (_NowRes.Kristall + _Res.Kristall * TsD / 3600).ToString("#,##0");
                deuterium.LabelText = (_NowRes.Deuterium + _Res.Deuterium * TsD / 3600).ToString("#,##0");

                if (_Res.MetMemory != 0) metall.Value = Convert.ToInt64(_NowRes.Metall );
                if (_Res.KriMemory != 0) kristall.Value = Convert.ToInt64(_NowRes.Kristall );
                if (_Res.DeuMemory != 0) deuterium.Value = Convert.ToInt64(_NowRes.Deuterium );
            }


        }
        private void PlanetName_Click(object sender, EventArgs e)
        {
            ErrorRetLoad = true;
            RefOgControl(tabControl1.SelectedTab.Name);
         
        }

        private void progressLabel1_Load(object sender, EventArgs e)
        {

        }



    }
}