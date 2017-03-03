using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace CR_Galaxy.OGControl
{
    public  class FlottenInfo
    {
        /// <summary>
        /// 出发星球
        /// </summary>
        string thisgalaxy;
        public string Thisgalaxy
        {
            get { return "&thisgalaxy=" + thisgalaxy; }
            set { thisgalaxy = value; }
        }

        string thissystem;
        public string Thissystem
        {
            get { return "&thissystem=" + thissystem; }
            set { thissystem = value; }
        }

        string thisplanet;
        public string Thisplanet
        {
            get { return "&thisplanet=" + thisplanet; }
            set { thisplanet = value; }
        }
        /// <summary>
        /// 出发星球类型
        /// </summary>
        string thisplanettype;
        public string Thisplanettype
        {
            get { return "&thisplanettype=" + thisplanettype; }
            set { thisplanettype = value; }
        }
        /// <summary>
        /// 默认
        /// </summary>
        string speedfactor = "1";
        public string Speedfactor
        {
            get { return "&speedfactor=" + speedfactor; }
            set { speedfactor = value; }
        }
        /// <summary>
        /// 本星球资源
        /// </summary>
        string thisresource1;
        public string Thisresource1
        {
            get { return thisresource1; }
            set { thisresource1 = value; }
        }

        string thisresource2;
        public string Thisresource2
        {
            get { return  thisresource2; }
            set { thisresource2 = value; }
        }

        string thisresource3;
        public string Thisresource3
        {
            get { return  thisresource3; }
            set { thisresource3 = value; }
        }
        /// <summary>
        /// 目标星球
        /// </summary>
        string galaxy;
        public string Galaxy
        {
            get { return "&galaxy=" + galaxy; }
            set { galaxy = value; }
        }

        string system;
        public string System
        {
            get { return "&system=" + system; }
            set { system = value; }
        }

        string planet;
        public string Planet
        {
            get { return "&planet=" + planet; }
            set { planet = value; }
        }
        /// <summary>
        /// 目标星球类型
        /// </summary>
        string planettype;
        public string Planettype
        {
            get { return "&planettype=" + planettype; }
            set { planettype = value; }
        }
        /// <summary>
        /// 速度
        /// </summary>
        string speed;
        public string Speed
        {
            get { return "&speed=" + speed; }
            set { speed = value; }
        }
        /// <summary>
        /// 任务类型
        /// </summary>
        string order;
        public string Order
        {
            get { return "&order=" + order; }
            set { order = value; }
        }

        /// <summary>
        /// 目标资源
        /// </summary>
        string resource1;
        public string Resource1
        {
            get { return resource1; }
            set { resource1 = value; }
        }

        string resource2;
        public string Resource2
        {
            get { return  resource2; }
            set { resource2 = value; }
        }

        string resource3;
        public string Resource3
        {
            get { return  resource3; }
            set { resource3 = value; }
        }


        public List<ShipInfo> Ship=new List<ShipInfo>();


        public string _WebsiteEx;

        public string _Session;

        public string _PlanetID;

        /// <summary>
        /// 当前舰队操作进度
        /// </summary>
        public EFlottenState _FlottenState = EFlottenState.Free ;
        /// <summary>
        /// 默认舰队操作进度，如果是FS那么就在舰队数量选择界面，如果是普通指挥就直接到路径选择
        /// </summary>
        public EFlottenState _DefaultFlottenState = EFlottenState.Flotten2;

        public FlottenInfo(string WebsiteEx, string Session, string PlanetID)
        {
            _WebsiteEx = WebsiteEx;
            _Session = Session;
            _PlanetID = PlanetID;
        }

        //舰队选择页面
        public string Getflotten1Url()
        {
            string Url = _WebsiteEx + "/index.php?page=flotten1&session=" + _Session + "&cp=" + _PlanetID + "&mode=Flotte";

            return Url;
        }

        /// <summary>
        /// 星球选择页面
        /// </summary>
        public string Getflotten2Url()
        {
            string Url = _WebsiteEx + "/index.php?page=flotten2&session=" + _Session + "&cp=" + _PlanetID;
            for (int i = 0; i < Ship.Count; i++)
            {
                Url += "&maxship" + Ship[i].shipID + "=" + Ship[i].maxship + "&ship" + Ship[i].shipID + "=" + Ship[i].ship + "&consumptio=" + Ship[i].shipID + "=" + Ship[i].consumption + "&speed" + Ship[i].shipID + "=" + Ship[i].speed + "&capacity" + Ship[i].shipID + "=" + Ship[i].capacity;
            }
            return Url;


            // http://uni8.ogame.cn.com/game/index.php?page=flotten2&session=874d3848135f&maxship202=6&ship202=2&consumption202=10&speed202=8000&capacity202=5000&consumption210=1&speed210=160000000&capacity210=5&ship210=0
        }

        /// <summary>
        /// 资源选择页面
        /// </summary>
        public string  Getflotten3Url()
        {
            string Url = _WebsiteEx + "/index.php?page=flotten3&session=" + _Session + "&cp=" + _PlanetID;
            Url += Thisgalaxy + Thissystem + Thisplanet + Thisplanettype + Speedfactor + "&thisresource1=" + Thisresource1 + "&thisresource2=" + Thisresource2 + "&thisresource3=" + Thisresource3 + Galaxy + System + Planet + Planettype;

            for (int i = 0; i < Ship.Count; i++)
            {
                Url += "&ship" + Ship[i].shipID + "=" + Ship[i].ship + "&consumption" + Ship[i].shipID + "=" + Ship[i].consumption + "&speed" + Ship[i].shipID + "=" + Ship[i].speed + "&capacity" + Ship[i].shipID + "=" + Ship[i].capacity;
            }
            Url += Speed;
            return Url;

            //http://uni8.ogame.cn.com/game/index.php?page=flotten3&session=874d3848135f&thisgalaxy=4&thissystem=276&thisplanet=4&thisplanettype=1&speedfactor=1&thisresource1=2000&thisresource2=2000&thisresource3=2000&galaxy=4&system=276&planet=6&planettype=1&ship202=2&consumption202=10&speed202=8000&capacity202=5000&speed=10

        }

        /// <summary>
        /// 发送舰队
        /// </summary>
        public string GetflottenversandUrl()
        {
            string Url = _WebsiteEx + "/index.php?page=flottenversand&session=" + _Session + "&cp=" + _PlanetID;
            Url += Thisgalaxy + Thissystem + Thisplanet + Thisplanettype + Speedfactor + "&thisresource1=" + Thisresource1 + "&thisresource2=" + Thisresource2 + "&thisresource3=" + Thisresource3 + Galaxy + System + Planet + Planettype;

            for (int i = 0; i < Ship.Count; i++)
            {
                Url += "&ship" + Ship[i].shipID + "=" + Ship[i].ship + "&consumption" + Ship[i].shipID + "=" + Ship[i].consumption + "&speed" + Ship[i].shipID + "=" + Ship[i].speed + "&capacity" + Ship[i].shipID + "=" + Ship[i].capacity;
            }
            Url += Speed + Order + "&resource1=" + Resource1 + "&resource2=" + Resource2 + "&resource3=" + Resource3;
            return Url;

            //http://uni8.ogame.cn.com/game/index.php?page=flottenversand&session=874d3848135f&thisgalaxy=4&thissystem=276&thisplanet=4&thisplanettype=1&speedfactor=1&thisresource1=2000&thisresource2=2000&thisresource3=2000&galaxy=4&system=276&planet=6&planettype=1&ship202=2&consumption202=10&speed202=8000&capacity202=5000&speed=10&order=3&resource1=200&resource2=300&resource3=400

        }
    }

    public class ShipInfo
    {
        /// <summary>
        /// 飞船ID
        /// </summary>
        public string shipID;
        /// <summary>
        /// 最大飞船数量
        /// </summary>
        public string maxship;
        /// <summary>
        /// 飞船
        /// </summary>
        public string ship;
        /// <summary>
        /// 燃油消耗
        /// </summary>
        public string consumption;
        /// <summary>
        /// 飞行速度
        /// </summary>
        public string speed;
        /// <summary>
        /// 货柜容量
        /// </summary>
        public string capacity;


        //List<string> shipList = new List<string>();

        //List<string> consumptionList = new List<string>();

        //List<string> speedList = new List<string>();
 
        //List<string> capacityList = new List<string>();
    }

    public enum EFlottenState
    {
        /// <summary>
        /// 自由状态，任务没有开始
        /// </summary>
        Free,
        /// <summary>
        /// 舰队选择界面
        /// </summary>
        Flotten1,
        /// <summary>
        /// 舰队飞行方向
        /// </summary>
        Flotten2,
        /// <summary>
        /// 资源选择
        /// </summary>
        Flotten3,
        /// <summary>
        /// 舰队指令完成
        /// </summary>
        FlottenSend

    }

    public class FlottenCommand
    {
        List<FlottenInfo> _FlottenList = new List<FlottenInfo>();

       public WebBrowser _Web = new WebBrowser();

        //EFlottenState _FlottenState = new EFlottenState();

        public bool _Run = false;

        public FlottenCommand()
        {
            _Web.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(_Web_DocumentCompleted);
        }

        void _Web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //MessageBox.Show(e.Url.ToString());
            HtmlElement Content = _Web.Document.GetElementById("content");
            if (Content == null)
            {
                _FlottenList[0]._FlottenState = _FlottenList[0]._DefaultFlottenState;//如果出错就重置舰队发生任务
                string ddd = _Web.Document.Body.InnerHtml ;
                RunFlotten();
                return; 
            }

            //用来实现舰队操作
            if (_FlottenList[0]._FlottenState == EFlottenState.Flotten1)
            {
                if (e.Url.ToString().IndexOf("page=flotten1") ==-1)
                {
                    _FlottenList.RemoveAt(0);
                    RunFlotten();
                    return;
                }
                //得到了所有舰队的，现在转入星球目标操作位置
                Flotten1Rending(Content);
                _FlottenList[0]._FlottenState = EFlottenState.Flotten2;
                _Web.Navigate(_FlottenList[0].Getflotten2Url());
                return;
            }
            else if (_FlottenList[0]._FlottenState == EFlottenState.Flotten2)//载入目标选择
            {
                if (e.Url.ToString().IndexOf("page=flotten2") == -1)
                {
                    _FlottenList.RemoveAt(0);
                    RunFlotten();
                    return;
                }

                if (_FlottenList[0]._DefaultFlottenState == EFlottenState.Flotten1)//如果是自动FS，那么需要得到本星球资源，用于选择最大资源功能
                {
                    Flotten2RendingFS(Content);
                    _FlottenList[0]._FlottenState = EFlottenState.Flotten3;
                    _Web.Navigate(_FlottenList[0].Getflotten3Url());
                    return;
                }
            }
            else if (_FlottenList[0]._FlottenState == EFlottenState.Flotten3)//载入资源选择
            {
                if (e.Url.ToString().IndexOf("page=flotten3") == -1)
                {
                    _FlottenList.RemoveAt(0);
                    RunFlotten();
                    return;
                }

                if (_FlottenList[0]._DefaultFlottenState == EFlottenState.Flotten1)//如果是自动FS，那么需要得到本星球资源，用于选择最大资源功能
                {
                    _FlottenList[0].Resource1 = _FlottenList[0].Thisresource1;
                    _FlottenList[0].Resource2 = _FlottenList[0].Thisresource2;
                    _FlottenList[0].Resource3 = _FlottenList[0].Thisresource3;
                   // Flotten2RendingFS(Content);
                    _FlottenList[0]._FlottenState = EFlottenState.FlottenSend;
                    _Web.Navigate(_FlottenList[0].GetflottenversandUrl());
                    return;
                }
            }
            else if (_FlottenList[0]._FlottenState == EFlottenState.FlottenSend)//舰队出发完毕
            {
                if (e.Url.ToString().IndexOf("page=flottenversand") > 0)
                {
                    FileStream Fs = new FileStream("D:\\ccc.htm", FileMode.CreateNew);
                    byte[] C = new byte[(int)_Web.DocumentStream.Length];
                    _Web.DocumentStream.Read(C, 0, (int  )_Web.DocumentStream.Length);
                    Fs.Write(C, 0, (int)_Web.DocumentStream.Length);
                    Fs.Close();
                    //_Web.DocumentStream = "";
                    _FlottenList.RemoveAt(0);
                    RunFlotten();
                    return;
                }

                //完成URL
                //http://uni8.ogame.cn.com/game/index.php?page=flottenversand&session=a6ef9aa19fe2
            }
        }

        /// <summary>
        /// FS专用，选择所有舰队
        /// </summary>
        /// <param name="HtmlE"></param>
        public void Flotten1Rending(HtmlElement HtmlE)
        {
            //舰队列表
            HtmlElement HtmlContent =HtmlE.Children[0].Children[2].Children[1].Children[0].Children[0];

            //直接从可用的开始，前两行内容不要
            for (int i = 2; i < HtmlContent.Children.Count; i++)
            {
                HtmlElement HE=HtmlContent.Children[i];

                if ((HE.InnerHtml.IndexOf("太阳能卫星") > 0) || (HE.InnerHtml.IndexOf("没有船舰") > 0)) break ;

                ShipInfo ShipI = new ShipInfo();
                ShipI.ship = HE.Children[1].InnerText; 
                ShipI.maxship =GetRegexMatch(HE.Children[1].InnerHtml ,"(?<=value=)[0-9]*");
                ShipI.shipID =GetRegexMatch(HE.Children[1].InnerHtml ,"(?<=maxship)[0-9]*(?=>)");
                //2<INPUT type=hidden value=2 name=maxship202> 
                string Sh = HE.Children[3].InnerHtml.Replace(" ", "").ToLower();
                ShipI.consumption = GetRegexMatch(Sh, "(?<=value=)[0-9]*(?=name=consumption)");
                ShipI.speed = GetRegexMatch(Sh, "(?<=value=)[0-9]*(?=name=speed)");
                ShipI.capacity = GetRegexMatch(Sh, "(?<=value=)[0-9]*(?=name=capacity)");
                //<INPUTtype=hiddenvalue=10name=consumption202> 
                //<INPUTtype=hiddenvalue=8000name=speed202></TH> 
                //<INPUTtype=hiddenvalue=5000name=capacity202></TH> 
                _FlottenList[0].Ship.Add(ShipI);
            }
        }

        /// <summary>
        /// FS专用，设置资源
        /// </summary>
        /// <param name="HtmlE"></param>
        public void Flotten2RendingFS(HtmlElement HtmlE)
        {
            //资源及星球信息
            HtmlElement HtmlContent = HtmlE.Children[0].Children[4].Children[0].Children[0].Children[0];
            string Sh = HtmlContent.InnerHtml.Replace(" ", "").ToLower();
            _FlottenList[0].Thisresource1 = GetRegexMatch(Sh, "(?<=value=)[0-9]*(?=name=thisresource1)");
            _FlottenList[0].Thisresource2 = GetRegexMatch(Sh, "(?<=value=)[0-9]*(?=name=thisresource2)");
            _FlottenList[0].Thisresource3 = GetRegexMatch(Sh, "(?<=value=)[0-9]*(?=name=thisresource3)");
        }

        /// <summary>
        /// 添加舰队任务
        /// </summary>
        /// <param name="FI"></param>
        public void AddFlotten(FlottenInfo FI)
        {
            //加入重复任务检测
            _FlottenList.Add(FI);
            RunFlotten();
        }

       /// <summary>
       /// 执行舰队任务
       /// </summary>
        public void RunFlotten()
        {
            if (_FlottenList.Count > 0)
            {
                if ( _FlottenList[0]._FlottenState !=EFlottenState.Free ) return ;

                _Run = true;
                _FlottenList[0]._FlottenState = _FlottenList[0]._DefaultFlottenState;
                if (_FlottenList[0]._DefaultFlottenState == EFlottenState.Flotten1)
                {
                    _Web.Navigate(_FlottenList[0].Getflotten1Url());
                }
                else
                {
                    _Web.Navigate(_FlottenList[0].Getflotten2Url());
                }
            }
            else
            {
                _Run = false;
            }
        }

        public void Clear()
        {
            _FlottenList.Clear();
        }

        //------------------------------
        //获得舰队航线数量
        //HE.Children[0].Children[2].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0].Children[0]
        //舰队列表
        //HE.Children[0].Children[2].Children[1].Children[0].Children[0]


        #region 正则表达式

        public string GetRegexMatch(string input, string pattern)
        {
            Match Mc = Regex.Match(input, pattern);
            if (Mc.Value==null) return "";
            return Mc.Value;
        }

        #endregion

    }
}
