using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CR_Galaxy.OGControl
{
    /// <summary>
    /// 资源类
    /// </summary>
    public class CNowRes
    {
        public decimal Metall=0;
        public decimal Kristall=0;
        public decimal Deuterium=0;
        public string Energie="0/0";//能量

        public DateTime UpDate;
    }

    public class CRes
    {
        /// <summary>
        /// 金属
        /// </summary>
        public decimal Metall = 0;
        public decimal MetDay = 0;
        public decimal MetWeek = 0;
        public string MetEnergie = "0";//电力消耗
        public string MetProduction = "0";//产量
        public string MetLevel = "0";//等级
        public decimal MetMemory = 0;//存储器

        /// <summary>
        /// 晶体
        /// </summary>
        public decimal Kristall = 0;
        public decimal KriDay = 0;
        public decimal KriWeek = 0;
        public string KriEnergie = "0";
        public string KriProduction = "0";
        public string KriLevel = "0";
        public decimal KriMemory = 0;
        /// <summary>
        /// 重氢
        /// </summary>
        public decimal Deuterium = 0;
        public decimal DeuDay = 0;
        public decimal DeuWeek = 0;
        public string DeuEnergie = "0";
        public string DeuProduction = "0";
        public string DeuLevel = "0";
        public decimal DeuMemory = 0;
        /// <summary>
        /// 核电站
        /// </summary>
        public decimal AtomicLost = 0;
        public decimal AtomicMake = 0;
        public string AtomicProduction = "0";
        public string AtomicLevel = "0";
        /// <summary>
        /// 太阳能卫星
        /// </summary>
        public decimal Satellite = 0;
        public string SatelliteProduction = "0";
        public string SatelliteLevel = "0";
        /// <summary>
        /// 能量
        /// </summary>
        public string Energie = "";
        public string EneProduction = "0";
        public string EneLevel = "0";
    }


    public class ResRead
    {
        public ResRead()
        { }
        /// <summary>
        /// 获得当前资源数量
        /// </summary>
        /// <returns></returns>
        public CNowRes GetNowRes(HtmlDocument HtmlDoc)
        {
            HtmlElement HtmlEmt = HtmlDoc.GetElementById("resources");
            CNowRes NowRes = new CNowRes();
            if (HtmlEmt == null) return NowRes;
            NowRes.Metall = Convert.ToDecimal(HtmlEmt.Children[0].Children[2].Children[0].InnerText.Replace(".", ""));//金属
            NowRes.Kristall = Convert.ToDecimal(HtmlEmt.Children[0].Children[2].Children[1].InnerText.Replace(".", ""));
            NowRes.Deuterium = Convert.ToDecimal(HtmlEmt.Children[0].Children[2].Children[2].InnerText.Replace(".", ""));
            NowRes.Energie = HtmlEmt.Children[0].Children[2].Children[4].InnerText;
            NowRes.UpDate = DateTime.Now;
            return NowRes;
        }

        /// <summary>
        /// 获得资源
        /// </summary>
        /// <returns></returns>
        public CRes GetRes(HtmlDocument HtmlDoc)
        {
            CRes Res = new CRes();
            HtmlElement HtmlEmtF = HtmlDoc.GetElementById("content");
            if (HtmlEmtF == null) return Res;//如果不存在就返回空
            HtmlElement HtmlEmt = HtmlEmtF.Children[0].Children[0].Children[2].Children[1].Children[0];

            for (int i = 0; i < HtmlEmt.Children.Count; i++)
            {
                if (HtmlEmt.Children[i] == null) continue;
                if (HtmlEmt.Children[i].Children.Count  == 0) continue;
                if (HtmlEmt.Children[i].Children[0].InnerText == null) continue;
                string Caption =Convert.ToString(Info.ResLocation[Info.GetCaption(HtmlEmt.Children[i].Children[0].InnerText)]);
                if (Caption == "Metall")
                {
                    //金属
                    Res.MetEnergie = HtmlEmt.Children[i].Children[5].InnerText.Replace(".", "");//能量
                    Res.MetProduction = GetProduction(HtmlEmt.Children[i].Children[6].InnerHtml);//产量计算
                    Res.MetLevel = GetLevel(HtmlEmt.Children[i].Children[0].InnerText);//等级
                }
                else if (Caption == "Kristall")
                {
                    /// 晶体
                    Res.KriEnergie = HtmlEmt.Children[i].Children[5].InnerText.Replace(".", "");
                    Res.KriProduction = GetProduction(HtmlEmt.Children[i].Children[6].InnerHtml);
                    Res.KriLevel = GetLevel(HtmlEmt.Children[i].Children[0].InnerText);
                }
                else if (Caption == "Deuterium")
                {
                    /// 重氢
                    Res.DeuEnergie = HtmlEmt.Children[i].Children[5].InnerText.Replace(".", "");
                    Res.DeuProduction = GetProduction(HtmlEmt.Children[i].Children[6].InnerHtml);
                    Res.DeuLevel = GetLevel(HtmlEmt.Children[i].Children[0].InnerText);
                }
                else if (Caption == "Energie")
                {
                    Res.Energie += HtmlEmt.Children[i].Children[5].InnerText;//总电量
                    Res.EneProduction = GetProduction(HtmlEmt.Children[i].Children[6].InnerHtml);//电量计算
                    Res.EneLevel = GetLevel(HtmlEmt.Children[i].Children[0].InnerText);//等级
                }
                else if (Caption == "Atomic")
                {
                    Res.AtomicLost = Convert.ToDecimal(HtmlEmt.Children[i].Children[4].InnerText.Replace(".", ""));
                    Res.AtomicMake = Convert.ToDecimal(HtmlEmt.Children[i].Children[5].InnerText.Replace(".", ""));
                    Res.AtomicProduction = GetProduction(HtmlEmt.Children[i].Children[6].InnerHtml);
                    Res.AtomicLevel = GetLevel(HtmlEmt.Children[i].Children[0].InnerText);//等级
                }
                else if (Caption == "Satellite")
                {
                    Res.Satellite  = Convert.ToDecimal(HtmlEmt.Children[i].Children[5].InnerText.Replace(".", ""));
                    Res.SatelliteProduction = GetProduction(HtmlEmt.Children[i].Children[6].InnerHtml);
                    Res.SatelliteLevel = GetLevel(HtmlEmt.Children[i].Children[0].InnerText);//等级
                }
                else if (Caption == "Memory")
                {
                    Res.MetMemory = GetMemory(HtmlEmt.Children[i].Children[1].InnerText.Replace(".", ""));//金属罐头
                    Res.KriMemory = GetMemory(HtmlEmt.Children[i].Children[2].InnerText.Replace(".", ""));//晶体罐头
                    Res.DeuMemory = GetMemory(HtmlEmt.Children[i].Children[3].InnerText.Replace(".", ""));//重氢罐头
                }
                else if (Caption == "Sum")
                {
                    Res.Metall = Convert.ToDecimal(HtmlEmt.Children[i].Children[1].InnerText.Replace(".", ""));//金属
                    Res.Kristall = Convert.ToDecimal(HtmlEmt.Children[i].Children[2].InnerText.Replace(".", "")); /// 晶体
                    Res.Deuterium = Convert.ToDecimal(HtmlEmt.Children[i].Children[3].InnerText.Replace(".", "")); /// 重氢

                    Res.Energie = HtmlEmt.Children[i].Children[4].InnerText + "/" + Res.Energie;//电量
                }

                else if (Caption == "Day")
                {
                    Res.MetDay = Convert.ToDecimal(HtmlEmt.Children[i].Children[1].InnerText.Replace(".", ""));
                    Res.KriDay = Convert.ToDecimal(HtmlEmt.Children[i].Children[2].InnerText.Replace(".", ""));
                    Res.DeuDay = Convert.ToDecimal(HtmlEmt.Children[i].Children[3].InnerText.Replace(".", ""));
                }
                else if (Caption == "Week")
                {
                    Res.MetWeek = Convert.ToDecimal(HtmlEmt.Children[i].Children[1].InnerText.Replace(".", ""));
                    Res.KriWeek = Convert.ToDecimal(HtmlEmt.Children[i].Children[2].InnerText.Replace(".", ""));
                    Res.DeuWeek = Convert.ToDecimal(HtmlEmt.Children[i].Children[3].InnerText.Replace(".", ""));
                }

            }
            return Res;
        }

        /// <summary>
        /// 选择框中得到内容
        /// </summary>
        /// <param name="Select"></param>
        /// <returns></returns>
        private string GetProduction(string Select)
        {
            Select = Select.Replace(" ", "");
            string LastStr = Select.Substring(Select.IndexOf("selected>") + "selected>".Length);
            return LastStr.Substring(0, LastStr.IndexOf("<"));
        }

        private decimal GetMemory(string MemoryStr)
        {
            MemoryStr = MemoryStr.ToLower().Replace("k", "000");
            return Convert.ToDecimal( MemoryStr.Replace(".", ""));
        }

        private string GetLevel(string LevelStr)
        {
           LevelStr= LevelStr.Replace(" ", "");
           return LevelStr.Substring(LevelStr.IndexOf("("));
        }
    }
}
