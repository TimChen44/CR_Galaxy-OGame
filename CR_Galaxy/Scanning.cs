using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CR_Galaxy
{
    public class Scanning
    {
        private GalaxySystem _GalaxySystem;
        private WebBrowser _WB;
        //Planet[] _Planet = new Planet[14];
        private bool _Begin ;//是不是刚刚启动，如果是那么先转到指定太阳系；
        private IOData _IOData;
        private ServerInfo _SerInfo;

        private Navigate _Navigate;
       

        private int _SystemCount ;
        private CR_Soft.ClassLibrary.Log.Log _Log;

        public Scanning(WebBrowser spWB,IOData spIOData,CR_Soft.ClassLibrary.Log.Log spLog,ServerInfo spSerInfo)
        {
            _WB = spWB;
            _GalaxySystem = new GalaxySystem(spWB);
            _IOData = spIOData;
            _Log = spLog;
            _SerInfo = spSerInfo;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="spBeingGalaxy"></param>
        /// <param name="spBeginSystem"></param>
        /// <param name="spEndGalaxy"></param>
        /// <param name="spEndSystem"></param>
        /// <param name="spEndUrl"></param>
        public void BeginScanning(int spBeingGalaxy, int spBeginSystem, int spEndGalaxy, int spEndSystem, Navigate spNav)
        {
            _Navigate = spNav;
            if (_Navigate._Session == null)
            {
                _Navigate.SaveSession(_WB.Document);
            }
            _GalaxySystem.BeginGalaxy = spBeingGalaxy;
            _GalaxySystem.BeginSystem = spBeginSystem;
            _GalaxySystem.EndGalaxy = spEndGalaxy;
            _GalaxySystem.EndSystem = spEndSystem;
           _SystemCount = 0;//初始扫描数量 

           _GalaxySystem.Goto(_SerInfo.WebsiteEx, _Navigate._Session, spBeingGalaxy, spBeginSystem);
            _Begin = true;
        }


        /// <summary>
        /// 主过程协调一切事宜
        /// </summary>
        public bool Main()
        {
            if (_WB.Document.GetElementById("fleetstatusrow") == null)
            {
                //_WB.Navigate(_Navigate.EndUrl);
                _GalaxySystem.Goto(_SerInfo.WebsiteEx, _Navigate._Session);
                _Log.AddWarning(_GalaxySystem.GetSystme() + "页面分析错误，重新载入并分析");
                return true;
            }

            //Planet[] Planet = new Planet[15];//为了每次都有新的数据，所以创建新对象
            //使用新的结构存储数据

            ExPlanet[] ExPlanet = new ExPlanet[15];
            //Rankings[] Rankings =new Rankings [15]

            GetUserInfo(ExPlanet);//数据获得

            _IOData.OutData(ExPlanet);//数据保存

            _SystemCount++;

            _GalaxySystem.SystemInc();//转入下一个星系
            if (_GalaxySystem.Overflow() == 4)
            {
                return false;
            }
            else
                return true;

        }

        public void GetUserInfo(ExPlanet[] spExPlanet)
        {
            try
            {
                HtmlElement dom = _WB.Document.GetElementById("fleetstatusrow");
                HtmlElement tt = dom.Parent.Parent;//到这里获得表格对象

                //  TxtShowWeb.Text = tt.Children[0].Children[2].Children[0].InnerText;
                for (int i = 0; i <= 14; i++)
                {
                    try
                    {

                        spExPlanet[i].Galaxy = Convert.ToInt32(_GalaxySystem.GetGalaxy());//银河
                        spExPlanet[i].System = Convert.ToInt32(_GalaxySystem.GetSystme());//太阳系
                        spExPlanet[i].Location = Convert.ToInt32(tt.Children[0].Children[i + 2].Children[0].InnerText.Trim());//位置
                        spExPlanet[i].PlanetName = tt.Children[0].Children[i + 2].Children[2].InnerText;//玩家名字
                    }
                    catch (Exception ee)
                    {
                        _Log.AddError("<GetUserInfo-星系星系>" + ee.Message);
                    }

                    if (tt.Children[0].Children[i + 2].Children[3].InnerHtml != null)//月球状态
                    {
                        try
                        {
                            string MoonInfo = tt.Children[0].Children[i + 2].Children[3].InnerHtml;
                            if (MoonInfo.IndexOf(_SerInfo.Moon) != -1)
                            {
                                spExPlanet[i].Moon = true;
                                MoonInfo = MoonInfo.Substring(MoonInfo.IndexOf(_SerInfo.MoonSize) + _SerInfo.MoonSize.Length);
                                MoonInfo = MoonInfo.Substring(0, MoonInfo.IndexOf(_SerInfo.MoonSizeEnd));
                                if (MoonInfo.IndexOf(".") > -1)
                                    spExPlanet[i].MoonSize = Convert.ToInt32(MoonInfo.Replace(".", "").Trim());
                                else
                                    spExPlanet[i].MoonSize = Convert.ToInt32(MoonInfo.Trim());
                            }
                        }
                        catch (Exception ee)
                        {
                            _Log.AddError("<GetUserInfo-月球状态>" + ee.Message);
                        }
                    }



                    if (tt.Children[0].Children[i + 2].Children[4].InnerHtml != null)//垃圾情况
                    {
                        try
                        {
                            string Wreckage = tt.Children[0].Children[i + 2].Children[4].InnerHtml;
                            Wreckage = Wreckage.Substring(Wreckage.IndexOf(_SerInfo.Metal) + _SerInfo.Metal.Length);

                            spExPlanet[i].Metal = Convert.ToInt32(Wreckage.Substring(0, Wreckage.IndexOf(_SerInfo.WreckageEnd)).Replace(".", "").Trim());

                            Wreckage = Wreckage.Substring(Wreckage.IndexOf(_SerInfo.Crystal) + _SerInfo.Crystal.Length);
                            spExPlanet[i].Crystal = Convert.ToInt32(Wreckage.Substring(0, Wreckage.IndexOf(_SerInfo.WreckageEnd)).Replace(".", "").Trim());
                        }
                        catch (Exception ee)
                        {
                            _Log.AddError("<GetUserInfo-月球状态>" + ee.Message);
                        }
                    }
                    else
                    {
                        spExPlanet[i].Metal = 0;
                        spExPlanet[i].Crystal = 0;
                    }


                    //用户名

                    try
                    {
                        spExPlanet[i].Username = tt.Children[0].Children[i + 2].Children[5].InnerText;
                        if (tt.Children[0].Children[i + 2].Children[5].Children != null)
                        {
                            foreach (HtmlElement HE in tt.Children[0].Children[i + 2].Children[5].Children)
                            {

                                switch (HE.InnerText)
                                {
                                    case "s": ;
                                        break;
                                    case "i": spExPlanet[i].Inactive = true;
                                        break;
                                    case "I": spExPlanet[i].LongInactive = true;
                                        break;
                                    case "u": spExPlanet[i].Vacation = true;
                                        break;
                                    case "g": spExPlanet[i].Banned = true;
                                        break;
                                    case "n": spExPlanet[i].Username += "(n)";
                                        break;//小绿人
                                    case "v": spExPlanet[i].Vacation = true;//英国服务器的假期
                                        break;
                                    case "b": spExPlanet[i].Banned = true;//英国服务器的被封
                                        break;
                                    default:
                                        spExPlanet[i].Username = HE.InnerText;
                                        break;
                                }
                            }
                        }

                    }
                    catch (Exception ee)
                    {
                        _Log.AddError("<GetUserInfo-用户信息>" + ee.Message);
                    }


                    try
                    {
                        if ((spExPlanet[i].Username != "") && (spExPlanet[i].Username != null))
                        {

                            string PM;
                            PM = tt.Children[0].Children[i + 2].Children[5].InnerHtml;
                            string TmpPM = PM.Substring(PM.IndexOf(_SerInfo.Rankings) + _SerInfo.Rankings.Length, PM.IndexOf(_SerInfo.td) - PM.IndexOf(_SerInfo.Rankings) - _SerInfo.Rankings.Length).Trim();
                            if (TmpPM.Length == 0)
                            {
                                spExPlanet[i].Rankings = 0;
                            }
                            else
                            {
                                spExPlanet[i].Rankings = Convert.ToInt32(TmpPM);
                            }
                        }

                    }
                    catch (Exception ee)
                    {
                        _Log.AddError("<GetUserInfo-用户排名信息>" + ee.Message);
                    }

                    try
                    {

                        spExPlanet[i].Union = tt.Children[0].Children[i + 2].Children[6].InnerText;

                        if ((spExPlanet[i].Union != "") && (spExPlanet[i].Union != null))
                        {
                            string LM;
                            LM = tt.Children[0].Children[i + 2].Children[6].InnerHtml;
                            string TmpPM = LM.Substring(LM.IndexOf(_SerInfo.Rankings) + _SerInfo.Rankings.Length, LM.IndexOf(_SerInfo.Have) - LM.IndexOf(_SerInfo.Rankings) - _SerInfo.Rankings.Length).Trim();
                            if (TmpPM.Length == 0)
                            {
                                spExPlanet[i].UnionRankings = 0;
                            }
                            else
                            {
                                spExPlanet[i].UnionRankings = Convert.ToInt32(TmpPM);
                            }
                            spExPlanet[i].Members = Convert.ToInt32(LM.Substring(LM.IndexOf(_SerInfo.Have) + _SerInfo.Have.Length, LM.IndexOf(_SerInfo.Members) - LM.IndexOf(_SerInfo.Have) - _SerInfo.Have.Length).Trim());
                        }
                    }
                    catch (Exception ee)
                    {
                        _Log.AddError("<GetUserInfo-联盟信息>" + ee.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                _Log.AddError("<GetUserInfo>" + ee.Message);
            }
        }

        public string GetNowSystem()
        {
            return _GalaxySystem.Galaxy.ToString() + ":" + _GalaxySystem.System.ToString() + " - " + _SystemCount.ToString();

        }

        /// <summary>
        /// 获得当前搜索的数量
        /// </summary>
        /// <returns></returns>
        public int GetScanningNumValue()
        {
            return _SystemCount;
        }

        /// <summary>
        /// 获得要扫描星系的总数
        /// </summary>
        /// <returns></returns>
        public int GetScanningCount()
        {
            return _GalaxySystem.GetSystemCount(); ;
        }


        public void Test()
        {
            _Log.AddError("asdfdsafas");
        }


    }
}
