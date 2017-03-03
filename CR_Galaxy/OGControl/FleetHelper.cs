using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Text.RegularExpressions;

namespace CR_Galaxy.OGControl
{

    public  enum EFleetType
    {
        /// <summary>
        /// 运输
        /// </summary>
        FlightOwntransport,
        /// <summary>
        /// 运输回城
        /// </summary>
        ReturnOwntransport,
        /// <summary>
        /// 受到攻击
        /// </summary>
        FlightAttack,
        /// <summary>
        /// 收到攻击
        /// </summary>
        Attack,
        /// <summary>
        /// 收到联合攻击
        /// </summary>
        Federation,
        /// <summary>
        /// 间谍
        /// </summary>
        FlightEspionage,
        /// <summary>
        /// 其他
        /// </summary>
        Other
    }

    class FleetHelper
    {
        OGMilitary _This;

        Timer SurplusTimeCalc = new Timer();

        public FleetHelper(OGMilitary This)
        {
            _This = This;
            SurplusTimeCalc.Interval = 1000;
            SurplusTimeCalc.Tick += new EventHandler(SurplusTimeCalc_Tick);
        }

        void SurplusTimeCalc_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < _This._DTFleetFly.Rows.Count; i++)
            {
                DataRow DR = _This._DTFleetFly.Rows[i];
                if (DR[InfoFleet.FFLtColumn.ArrivalTime] != null)
                {
                    if (DR[InfoFleet.FFLtColumn.ArrivalTime].GetType() != typeof(DBNull))
                    {
                        TimeSpan Ts = Convert.ToDateTime(DR[InfoFleet.FFLtColumn.ArrivalTime]).Subtract(DateTime.Now);
                        DR[InfoFleet.FFLtColumn.SurplusTime] = Ts;
                    }
                }
            }
        }

        public void FleetRending(HtmlElement HtmlE)
        {
            DataTable NewDTFleetFly =_This._DTFleetFly.Clone();

            HtmlElement ContentHE = HtmlE.Children[0].Children[4].Children[0];//得到所舰队动作列表
            
            //得到新的舰队列表
            for (int i = 0; i < ContentHE.Children.Count; i++)
            {//循环所有舰队动作列表
                HtmlElement FleetInfo = ContentHE.Children[i];
                if (FleetInfo == null) continue;
                if (FleetInfo.InnerHtml.IndexOf("id=bxx") == -1) continue;
                if (FleetInfo.Children[1].Children.Count > 1)
                {
                    //此处处理联合攻击
                }
                else
                {
                    //FleetInfo FInfo;//舰队动作信息
                    DataRow FleetDR = NewDTFleetFly.NewRow();
                    EFleetType _FleetType = GetFleetType(FleetInfo.Children[1]);

                    //填写统一的内容
                    GetFleetDRInfo1(FleetInfo, FleetDR);
                    //更具不同情况填写不同信息
                    switch (_FleetType)
                    {
                        case EFleetType.FlightOwntransport://运输
                            GetFlightOwntransport(FleetInfo, FleetDR);
                            break;
                        case EFleetType.ReturnOwntransport://运输返回
                            GetReturnOwntransport(FleetInfo, FleetDR);
                            break;
                        case EFleetType.FlightAttack://被攻击
                           GetFlightAttack(FleetInfo, FleetDR);
                           break;
                        case EFleetType.Attack://被攻击
                          GetAttack (FleetInfo, FleetDR);
                          break;
                        case EFleetType.Federation://联合攻击
                          GetFederation (FleetInfo, FleetDR);
                          break;
                        case EFleetType.FlightEspionage://被探测
                          GetFlightEspionage(FleetInfo, FleetDR);
                          break;
 
                    }

                    NewDTFleetFly.Rows.Add(FleetDR);
                }
            }


            //航线存在就更新，不存在就删除航线
            //遍历当前飞行列表
            for (int i = _This._DTFleetFly.Rows.Count-1; i >=0 ; i--)
            {
                bool NotExist = true;
                //遍历找到对应的航线
                for (int Ni = 0; Ni < NewDTFleetFly.Rows.Count; Ni++)
                {
                    if (_This._DTFleetFly.Rows[i][InfoFleet.FFLtColumn.TaskID].ToString() == NewDTFleetFly.Rows[Ni][InfoFleet.FFLtColumn.TaskID].ToString())
                    {//如果能找到行就更新行信息
                        //更新到达时间
                        _This._DTFleetFly.Rows[i][InfoFleet.FFLtColumn.ArrivalTime] = NewDTFleetFly.Rows[Ni][InfoFleet.FFLtColumn.ArrivalTime];
                        //删除行
                        NewDTFleetFly.Rows.RemoveAt(Ni);
                        NotExist = false;
                        break;
                    }
                }
                //如果不存在航线了，就删除该航线
                if (NotExist == true) _This._DTFleetFly.Rows.RemoveAt(i);
            }

            //吧新航线加入
            for (int Ni = 0; Ni < NewDTFleetFly.Rows.Count; Ni++)
            {
                DataRow DR = _This._DTFleetFly.NewRow();
                DR.ItemArray = NewDTFleetFly.Rows[Ni].ItemArray;
                _This._DTFleetFly.Rows.Add(DR);
            }

            SurplusTimeCalc.Enabled = true;
        }
        #region 不同类别的解析

        /// <summary>
        /// 获得运输内容
        /// </summary>
        /// <param name="HtmlE"></param>
        /// <param name="FleetDR"></param>
        private void GetFlightOwntransport(HtmlElement FleetInfo, DataRow FleetDR)
        {
            FleetDR[InfoFleet.FFLtColumn.TaskContent] = "运输";
            FleetDR[InfoFleet.FFLtColumn.Content ] = EFleetType.FlightOwntransport;

        }

        /// <summary>
        /// 获得运输返回
        /// </summary>
        /// <param name="HtmlE"></param>
        /// <param name="FleetDR"></param>
        private void GetReturnOwntransport(HtmlElement FleetInfo, DataRow FleetDR)
        {
            FleetDR[InfoFleet.FFLtColumn.TaskContent] = "运输回程";
            FleetDR[InfoFleet.FFLtColumn.Content] = EFleetType.ReturnOwntransport;
        }

        /// <summary>
        /// 获得攻击
        /// </summary>
        /// <param name="HtmlE"></param>
        /// <param name="FleetDR"></param>
        private void GetFlightAttack(HtmlElement FleetInfo, DataRow FleetDR)
        {
            FleetDR[InfoFleet.FFLtColumn.TaskContent] = "受到攻击";

            if (((TimeSpan)FleetDR[InfoFleet.FFLtColumn.SurplusTime]).Minutes < 21)
            {//如果初次出现时间小于21分钟，那么我就认为这次攻击时间谍行为
                FleetDR[InfoFleet.FFLtColumn.Content] = EFleetType.FlightEspionage ;
            }
            else
            {
                FleetDR[InfoFleet.FFLtColumn.Content] = EFleetType.FlightAttack;
            }

            GetFleetListPlayerName(FleetInfo.Children[1].InnerHtml, FleetDR);

        }

        /// <summary>
        /// 获得攻击
        /// </summary>
        /// <param name="HtmlE"></param>
        /// <param name="FleetDR"></param>
        private void GetAttack(HtmlElement FleetInfo, DataRow FleetDR)
        {
            FleetDR[InfoFleet.FFLtColumn.TaskContent] = "受到攻击";
            FleetDR[InfoFleet.FFLtColumn.Content] = EFleetType.Attack;
            GetFleetListPlayerName(FleetInfo.Children[1].InnerHtml, FleetDR);
        }

        /// <summary>
        /// 获得联合攻击
        /// </summary>
        /// <param name="HtmlE"></param>
        /// <param name="FleetDR"></param>
        private void GetFederation(HtmlElement FleetInfo, DataRow FleetDR)
        {
            FleetDR[InfoFleet.FFLtColumn.TaskContent] = "受到联合";
            FleetDR[InfoFleet.FFLtColumn.Content] = EFleetType.Federation;
            GetFleetListPlayerName(FleetInfo.Children[1].InnerHtml, FleetDR);
        }

        /// <summary>
        /// 获得探测
        /// </summary>
        /// <param name="HtmlE"></param>
        /// <param name="FleetDR"></param>
        private void GetFlightEspionage(HtmlElement FleetInfo, DataRow FleetDR)
        {
            FleetDR[InfoFleet.FFLtColumn.TaskContent] = "受到探测";
            if (((TimeSpan)FleetDR[InfoFleet.FFLtColumn.SurplusTime]).Minutes < 21)
            {//如果初次出现时间小于21分钟，那么我就认为这次探测行为是间谍行为
                FleetDR[InfoFleet.FFLtColumn.Content] = EFleetType.FlightEspionage;
            }
            else
            {
                FleetDR[InfoFleet.FFLtColumn.Content] = EFleetType.FlightAttack;
            }
            GetFleetListPlayerName(FleetInfo.Children[1].InnerHtml, FleetDR);
        }


       //----------------------------------------------------------------------------------

        /// <summary>
        /// 一些相同的星系获得我就不单独裂开了
        /// </summary>
        /// <param name="HtmlE"></param>
        /// <param name="FleetDR"></param>
        private void GetFleetDRInfo1(HtmlElement FleetInfo, DataRow FleetDR)
        {
            //完成时间
           GetFleetListArrivalTime(FleetInfo.Children[0].InnerHtml, FleetDR);
            //任务ID
            FleetDR[InfoFleet.FFLtColumn.TaskID] = GetFleetListTaskID(FleetInfo.Children[0].InnerHtml);
            //出发到达星球坐标
            GetFleetListPlace(FleetInfo.Children[1].InnerHtml, FleetDR);
            //出发到达星球名字
            GetFleetListPlanet(FleetInfo.Children[1].InnerHtml, FleetDR);
            //舰队种类和资源
            GetFleetListFleetCategory(FleetInfo.Children[1].InnerHtml, FleetDR);
            ////任务内容
            //FleetDR[InfoFleet.FFLtColumn.TaskContent] = GetFleetListTaskContent(FleetInfo.Children[1]);

            FleetDR[InfoFleet.FFLtColumn.FS] = false;
            FleetDR[InfoFleet.FFLtColumn.FSWaring ] = false;
            FleetDR[InfoFleet.FFLtColumn.CreateTime ] = DateTime.Now ;
        }

        #endregion
        
        /// <summary>
        /// 获得飞行类型，他属于什么类型
        /// </summary>
        /// <returns></returns>
        private EFleetType GetFleetType(HtmlElement HtmlE)
        {
            Match Mc = Regex.Match(HtmlE.InnerHtml, "(?<=class=[\"|']).+?(?=[\"|']>)");
            switch (Mc.Value.Trim().ToLower())
            {
                case "flight owntransport"://运输
                    return EFleetType.FlightOwntransport;
                case "return owntransport"://运输返回
                    return EFleetType.ReturnOwntransport;
                case "flight attack"://被攻击
                    return EFleetType.FlightAttack;
                case "attack"://被攻击
                    return EFleetType.Attack;
                case "federation"://联合攻击
                    return EFleetType.Federation;
                case "flight espionage"://被探测
                    return EFleetType.FlightEspionage;
                default:
                    return EFleetType.Other;//
            }
        }

        #region 一些正则表达式

        /// <summary>
        /// 获得剩余时间
        /// </summary>
        /// <param name="HtmlE"></param>
        private void  GetFleetListArrivalTime(string HtmlE, DataRow FleetDR)
        {
            Match Mc = Regex.Match(HtmlE, "(?<=title=)[0-9]*");
            if (Mc.Value == "") return;
            FleetDR[InfoFleet.FFLtColumn.ArrivalTime] = DateTime.Now.AddTicks((long)(Convert.ToDouble(Mc.Value) * 10000000));
            FleetDR[InfoFleet.FFLtColumn.SurplusTime] =new TimeSpan( (long)(Convert.ToDouble(Mc.Value) * 10000000));
        }

        /// <summary>
        /// 获得任务ID
        /// </summary>
        /// <param name="HtmlE"></param>
        private string GetFleetListTaskID(string HtmlE)
        {
            Match Mc = Regex.Match(HtmlE, "(?<=star=\")[0-9]*");
            return Mc.Value;
        }

        /// <summary>
        /// 获得起始和结束坐标
        /// </summary>
        /// <param name="HtmlE"></param>
        /// <param name="FleetDR"></param>
        private void GetFleetListPlace(string HtmlE, DataRow FleetDR)
        {
            MatchCollection Mc = Regex.Matches(HtmlE, "(?<=\\[)[0-9:]*(?=\\])");
            if (Mc.Count < 2) return;
            FleetDR[InfoFleet.FFLtColumn.StartPlace] = Mc[0].Value;
            FleetDR[InfoFleet.FFLtColumn.ReachPlace] = Mc[1].Value;

        }

        /// <summary>
        /// 获得起始和结束星球名字
        /// </summary>
        /// <param name="HtmlE"></param>
        /// <param name="FleetDR"></param>
        private void GetFleetListPlanet(string HtmlE, DataRow FleetDR)
        {
            MatchCollection Mc = Regex.Matches(HtmlE, "(?<=星球).+?(?=\\<A)");
            if (Mc.Count < 2) return;
            FleetDR[InfoFleet.FFLtColumn.StartPlanet] = Mc[0].Value.Trim();
            FleetDR[InfoFleet.FFLtColumn.ReachPlanet] = Mc[1].Value.Trim();

        }

                /// <summary>
        /// 获得飞船总类及资源
        /// </summary>
        /// <param name="HtmlE"></param>
        /// <param name="FleetDR"></param>
        private void GetFleetListFleetCategory(string HtmlE, DataRow FleetDR)
        {
            MatchCollection Mc = Regex.Matches(HtmlE, "(?<=<b>).+?(?=</b>)");
            if (Mc.Count < 2) FleetDR[InfoFleet.FFLtColumn.FleetCategory] = Mc[0].Value.Replace("<br>", "");
            else
            {
                FleetDR[InfoFleet.FFLtColumn.FleetCategory] = Mc[0].Value.Replace("<br>", "");
                FleetDR[InfoFleet.FFLtColumn.Res] = Mc[1].Value.Replace("<br />", "");
            }
        }


                /// <summary>
        /// 获得玩家姓名和PID
        /// </summary>
        /// <param name="HtmlE"></param>
        /// <param name="FleetDR"></param>
        private void GetFleetListPlayerName(string HtmlE, DataRow FleetDR)
        {
            Match Mc = Regex.Match(HtmlE, "(?<=来自于).+?(?=<A)");
            FleetDR[InfoFleet.FFLtColumn.PlayerName] = Mc.Value.Trim();
            Match Mc2 = Regex.Match(HtmlE, "(?<=showMessageMenu\\().+?(?=\\))");
            FleetDR[InfoFleet.FFLtColumn.PID ] = Mc2.Value.Trim();
        }

        #endregion

    }
}