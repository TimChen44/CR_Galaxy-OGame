using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections;

namespace CR_Galaxy.OGControl
{
    public partial class OGMilitary : UserControl
    {
        #region 属性

        private string _WebsiteEx;
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

        /// <summary>
        /// 所有星球PlanetID
        /// </summary>
        //public Hashtable _PlanetInfo=new Hashtable();


        #endregion

        #region 全局参数

        /// <summary>
        /// 目前在天上的航线
        /// </summary>
        public DataTable _DTFleetFly = new DataTable();

        WebBrowser _WB = new WebBrowser();

        //一些需要的URL地址
        string _FleetUrl;

        public string FleetUrl
        {
            get { return _WebsiteEx + "/index.php?page=overview&session={0}"; }
            set { _FleetUrl = value; }
        }

        //当前状态
        public enum EWebState
        {
            Loading, //载入中
            Complete, //完成
            Free//空闲
        }

        public EWebState _WebState;

        //当前操作
        public enum EWebWork
        {
            Fleet  //舰队列表
        }

        public EWebWork _WebWork;

        //全权处理飞行列表类
        FleetHelper _FleetH;

        /// <summary>
        /// 舰队综合管理类
        /// </summary>
        public CR_Galaxy.OGControl.FlottenCommand _FlottenCmd;

        /// <summary>
        /// 总体综合管理
        /// </summary>
        public OGControlManage _OGCOntrolManage;

        #endregion

        #region 全局函数

        /// <summary>
        /// 创建DT结构，用来显示舰队情况
        /// </summary>
        private void CreatDT()
        {
            /*
             * 剩余时间
            0、到达时间
            1、任务内容
            2、任务ID
            3、精确任务内容（再次定义的任务内容，比如遇到间谍流攻击任务内容从定义为探测）
            4、出发地点
            5、出发星球
            6、玩家姓名
            7、玩家PID
            8、目标地点
            9、到达星球
            10、舰队总类
            11、字体颜色
            12、背景颜色
            13、联合分组

            自己如果是自己的船就没有玩家信息，别人的船都有
             */
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.SurplusTime, typeof(System.TimeSpan));
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.ArrivalTime, typeof(System.DateTime));
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.TaskContent);
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.TaskID);
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.Content);
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.StartPlace);
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.StartPlanet);
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.PlayerName);
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.PID);
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.ReachPlace);
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.ReachPlanet);
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.FleetCategory);
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.Res);
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.FontColor);
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.BackgroundColor);
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.Group);
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.FS, typeof(System.Boolean));
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.FSWaring, typeof(System.Boolean));
            _DTFleetFly.Columns.Add(InfoFleet.FFLtColumn.CreateTime, typeof(System.DateTime));

            FleetFlyList.DataSource = _DTFleetFly;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.SurplusTime].Width = 100;
            //FleetFlyList.Columns[InfoFleet.FFLtColumn.SurplusTime].DefaultCellStyle.Format = "M-d HH:mm";
            FleetFlyList.Columns[InfoFleet.FFLtColumn.SurplusTime].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.ArrivalTime].Width = 100;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.ArrivalTime].DefaultCellStyle.Format = "M-d HH:mm:ss";
            FleetFlyList.Columns[InfoFleet.FFLtColumn.TaskContent].Width = 100;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.TaskID].Width = 100;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.TaskID].Visible = false;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.Content].Width = 100;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.Content].Visible = false;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.StartPlace].Width = 100;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.StartPlanet].Width = 100;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.PlayerName].Width = 100;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.PID].Width = 100;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.PID].Visible = false;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.ReachPlace].Width = 100;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.ReachPlanet].Width = 100;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.FleetCategory].Width = 100;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.Res].Width = 100;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.FontColor].Width = 100;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.FontColor].Visible = false;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.BackgroundColor].Width = 100;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.BackgroundColor].Visible = false;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.Group].Width = 100;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.Group].Visible = false;

            FleetFlyList.Columns[InfoFleet.FFLtColumn.FS].Width = 100;
            //FleetFlyList.Columns[InfoFleet.FFLtColumn.FS].Visible = false;
            FleetFlyList.Columns[InfoFleet.FFLtColumn.FSWaring].Width = 100;
            // FleetFlyList.Columns[InfoFleet.FFLtColumn.FSWaring].Visible = false;

            FleetFlyList.Columns[InfoFleet.FFLtColumn.CreateTime].Width = 100;
        }

        #endregion

        public OGMilitary()
        {
            InitializeComponent();
            _WB.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(_Web_DocumentCompleted);
            _WB.ScriptErrorsSuppressed = true;

            InfoFleet.CN();
            //创建表
            CreatDT();
            //创建飞行列表处理类
            _FleetH = new FleetHelper(this);
            //启动飞行列表显示

            //启动舰队列表
            _WebWork = EWebWork.Fleet;
            _WebState = EWebState.Free;
            FleetFlyList.DataSource = _DTFleetFly;
        }

        public void FleetFlyListStart()
        {
            FleetFlyListTimer.Enabled = true;
        }

        void _Web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                HtmlElement Content = _WB.Document.GetElementById("content");
                if (Content == null)
                {
                    return;
                }
                if (_WebWork == EWebWork.Fleet)
                {
                    _FleetH.FleetRending(Content);
                }

            }
            catch
            {
                //防止出做，呵呵
            }
            _WebState = EWebState.Free;
        }



        private void OGMilitary_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void FleetFlyListTimer_Tick(object sender, EventArgs e)
        {
            if (_WebState == EWebState.Free)
            {
                _WebWork = EWebWork.Fleet;
                _WebState = EWebState.Loading;
                _WB.Navigate(string.Format(FleetUrl, _OGCOntrolManage._Session));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FlottenInfo FlottenI = new FlottenInfo(_WebsiteEx, _OGCOntrolManage._Session , ((OGControl)_OGCOntrolManage._OGControl["4:276:4"]).PlanetID);
            FlottenI._DefaultFlottenState = EFlottenState.Flotten1;
            FlottenI.Thisgalaxy = "4";
            FlottenI.Thissystem = "276";
            FlottenI.Thisplanet = "4";

            FlottenI.Thisplanettype = "1";
            FlottenI.Planettype = "1";
            FlottenI.Order = "3";
            FlottenI.Galaxy = FSgalaxy.Value.ToString();
            FlottenI.System = FSsystem.Value.ToString();
            FlottenI.Planet = FSplanet.Value.ToString();
            FlottenI.Speed = FSspeed.Text;
            _FlottenCmd.AddFlotten(FlottenI);

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void FSspeed_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FSplanet_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FSsystem_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FSgalaxy_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void AutoFSTimer_Tick(object sender, EventArgs e)
        {
            //加入重复任务检测就OK了
            for (int i = 0; i < _DTFleetFly.Rows.Count; i++)
            {

                if (((_DTFleetFly.Rows[i][InfoFleet.FFLtColumn.Content].ToString()) == "Attack") ||
                    ((_DTFleetFly.Rows[i][InfoFleet.FFLtColumn.Content].ToString()) == "FlightAttack") ||
                    ((_DTFleetFly.Rows[i][InfoFleet.FFLtColumn.Content].ToString()) == "Federation") ||
                    ((_DTFleetFly.Rows[i][InfoFleet.FFLtColumn.Content].ToString()) == "FlightEspionage"))
                {
                    if ((DateTime.Now.Subtract((DateTime)_DTFleetFly.Rows[i][InfoFleet.FFLtColumn.CreateTime]).Ticks > new TimeSpan(0, 2, 0).Ticks)
                   && (((bool)_DTFleetFly.Rows[i][InfoFleet.FFLtColumn.FSWaring]) == false))
                    {
                        _DTFleetFly.Rows[i][InfoFleet.FFLtColumn.FSWaring] = true;
                        string MsgStr = "http://uni8.cn.ogame.org/game/index.php?page=writemessages&session={0}&gesendet=1&messageziel={1}&to={2}&betreff={3}&text={4}";
                        WebBrowser MsgWB = new WebBrowser();
                        string cc = string.Format(MsgStr, new string[] { _OGCOntrolManage._Session, _DTFleetFly.Rows[i][InfoFleet.FFLtColumn.PID].ToString(), _DTFleetFly.Rows[i][InfoFleet.FFLtColumn.PlayerName].ToString(), "无标题", "我在线，你就不用来了!!" });
                        MsgWB.Navigate(string.Format(MsgStr, new string[] { _OGCOntrolManage._Session, _DTFleetFly.Rows[i][InfoFleet.FFLtColumn.PID].ToString(), _DTFleetFly.Rows[i][InfoFleet.FFLtColumn.PlayerName].ToString(), "无标题", "我在线，你就不用来了!!" }));
                    }

                    if ((((TimeSpan)_DTFleetFly.Rows[i][InfoFleet.FFLtColumn.SurplusTime]).Ticks < new TimeSpan(0, 5, 0).Ticks)
                                       && (((bool)_DTFleetFly.Rows[i][InfoFleet.FFLtColumn.FS]) == false))
                    {
                        OGControl OGCl = ((OGControl)_OGCOntrolManage._OGControl[_DTFleetFly.Rows[i][InfoFleet.FFLtColumn.ReachPlace]]);

                        FlottenInfo FlottenI = new FlottenInfo(_WebsiteEx, _OGCOntrolManage._Session, OGCl.PlanetID);
                        FlottenI._DefaultFlottenState = EFlottenState.Flotten1;
                        FlottenI.Thisgalaxy = OGCl.GalaxyLocation;
                        FlottenI.Thissystem = OGCl.SystemLocation;
                        FlottenI.Thisplanet = OGCl.PositionLocation;

                        FlottenI.Thisplanettype = "1";
                        FlottenI.Planettype = "1";
                        FlottenI.Order = OGCl._FSOrder;
                        FlottenI.Galaxy = OGCl._FSGalaxy;
                        FlottenI.System = OGCl._FSSystem;
                        FlottenI.Planet = OGCl._FSPosition;
                        FlottenI.Speed = OGCl._FSSheep;
                        _FlottenCmd.AddFlotten(FlottenI);

                        FSList.Items.Add(DateTime.Now.ToString() + "  " + OGCl.Planet + "(" + OGCl.Location + ")收到攻击，全舰队逃跑至" + OGCl._FSGalaxy.ToString() + OGCl._FSSystem.ToString() + OGCl._FSPosition.ToString());
                        _DTFleetFly.Rows[i][InfoFleet.FFLtColumn.FS] = true;
                    }
                }
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            AutoFSTimer.Start();
            button1.Text = "已经启动";
            _OGCOntrolManage._AutoFS = true;
        }
    }
}