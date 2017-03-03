using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace CR_Galaxy.OGControl
{
    static class Info
    {
        //记录名字，用来定位当前行属于哪个资源
        //因为金属、晶体等一些东西价格
        static public Hashtable ResLocation = new Hashtable();


        //处理用文字，为了支持多服务而加的
        static public string Level = "";
        static public string Rot;
        static public string NanoRot;
        static public string Day;
        static public string Hours;
        static public string Minutes;

        /// <summary>
        /// 得到等级
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        static public double GetLevelValue(string Content)
        {
            if (Content.IndexOf(Level) == -1) return 0;
            Content = Content.Substring(Content.IndexOf(Level) + Level.Length);
            Content = Content.Substring(0, Content.IndexOf(")"));
            return Convert.ToDouble(Content.Trim());
        }

        static public string GetFirstNum(string Content)
        {
            Regex Rx = new Regex("[0-9]*");
            Match MC = Rx.Match(Content);
            if (MC == null) return "-1";
            return MC.Value.Trim();
        }

        static public string GetEndNum(string Content)
        {
            Regex Rx = new Regex("\\d[0-9]*");
            Match MC = Rx.Match(Content);
            if (MC == null) return "-1";
            return MC.Value.Trim();
        }

        /// <summary>
        /// 得到时间
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        static public DateTime GetBDateTime(string Content)
        {
            Regex RxMin = new Regex("[0-9]*" + Minutes);
            Match MinMC = RxMin.Match(Content);
            int DMin = Convert.ToInt32(GetFirstNum(MinMC.Value));

            Regex RxHou = new Regex("[0-9]*" + Hours);
            Match HouMC = RxHou.Match(Content);
            int DHou;
            if (HouMC.Length == 0) DHou = 0;
            else DHou = Convert.ToInt32(GetFirstNum(HouMC.Value));

            Regex RxDay = new Regex("[0-9]*" + Day);
            Match DayMC = RxDay.Match(Content);
            int DDay;
            if (DayMC.Length == 0) DDay = 1;
            else DDay = Convert.ToInt32(GetFirstNum(DayMC.Value));

            return new DateTime(1, 1, DDay, DHou, DMin, 0);

        }

        /// <summary>
        /// 获得名字
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        static public string GetCaption(string Content)
        {
            Match Mc = Regex.Match(Content, @".+?((?=\()|(?=：)|(?=:))");
            if (Mc.Value == "") return Content.Trim();
            return Mc.Value.Trim();
        }

        /// <summary>
        /// 获得等级
        /// </summary>
        /// <param name="Ht"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        static public string GetHashtableValue(Hashtable Ht, string Key)
        {
            Match Mc = Regex.Match("Content", @"(?<=().+?(?=\))");
            return Mc.Value.Trim();
            //if (Ht.Contains(Key) == true)
            //    return Ht[Key].ToString();
            //else
            //    return "";
        }

        /// <summary>
        /// 获得飞船或防御数量
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        static public double  GetObjectNum(string Content)
        {
            Regex Rx = new Regex("(?<=\\()[0-9]*");
            Match MC = Rx.Match(Content);
            if ((MC == null) || (MC.Value=="")) return 0;
            return Convert.ToDouble(MC.Value.Trim());
        }

        static public List<string[]> GetBuildListInfo(string Content)
        {
            if (Content == null) return null;
            List<string[]> BuildListInfo = new List<string[]>();
            MatchCollection Mcs = Regex.Matches (Content, @"(?<=Array\().+?(?=\))");
            if (Mcs.Count < 3) return null;
            string Time = Mcs[0].Value.Replace("\"", "");
            string Build = Mcs[1].Value.Replace("\"", "");
            string Count = Mcs[2].Value.Replace("\"", "");

            string[] Times = Regex.Split(Time,",");
            string[] Builds = Regex.Split(Build, ",");
            string[] Counts = Regex.Split(Count, ",");

            for (int i = 0; i < Times.Length; i++)
            {
                if (Times[0].Trim().Length == 0) continue;
                BuildListInfo.Add(new string[] { Times[i], Builds[i], Counts[i] });
            }
            return BuildListInfo;
        }


        static public void CN()
        {
            //初始化资源定位
            ResLocation.Add("金属矿", "Metall");
            ResLocation.Add("晶体矿", "Kristall");
            ResLocation.Add("重氢分离器", "Deuterium");
            ResLocation.Add("太阳能发电站", "Energie");
            ResLocation.Add("核电站", "Atomic");
            ResLocation.Add("太阳能卫星", "Satellite");
            ResLocation.Add("储存器容量", "Memory");
            ResLocation.Add("总和", "Sum");
            ResLocation.Add("每天的资源", "Day");
            ResLocation.Add("每星期的资源", "Week");


            //分离级别用文字
            Level = "等级";

            Rot = "机器人工厂";
            NanoRot = "纳米机器人工厂";

            Day = "天";
            Hours = "小时";
            Minutes = "分钟";

        }


    }
}