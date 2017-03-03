using System;
using System.Collections.Generic;
using System.Text;

namespace CR_Galaxy.OGControl
{
    public static class InfoFleet
    {
        /// <summary>
        /// 舰队飞行列表信息
        /// </summary>
        public static class FFLtColumn
        {
            /// <summary>
            /// 剩余时间
            /// </summary>
            static public string SurplusTime;
            /// <summary>
            /// 0、到达时间
            /// </summary>
            static public string ArrivalTime;
            /// <summary>
            /// 任务内容
            /// </summary>
            static public string TaskContent;     //1、
            /// <summary>
            /// 任务ID
            /// </summary>
            static public string TaskID; //2、
            /// <summary>
            /// 精确任务内容（再次定义的任务内容，比如遇到间谍流攻击任务内容从定义为探测）
            /// </summary>
            static public string Content;  //3、
            /// <summary>
            /// 出发地点
            /// </summary>
            static public string StartPlace; //4、
            /// <summary>
            /// 出发星球
            /// </summary>
            static public string StartPlanet;   //5、
            /// <summary>
            /// 玩家姓名
            /// </summary>
            static public string PlayerName;  //6、
            /// <summary>
            /// 玩家PID
            /// </summary>
            static public string PID;  //7、
            /// <summary>
            /// 目标地点
            /// </summary>
            static public string ReachPlace; //8、
            /// <summary>
            /// 到达星球
            /// </summary>
            static public string ReachPlanet; //9、
            /// <summary>
            /// 舰队总类
            /// </summary>
            static public string FleetCategory;  //10、
            /// <summary>
            /// 携带的资源
            /// </summary>
            static public string Res;
            /// <summary>
            /// 字体颜色
            /// </summary>
            static public string FontColor; //11、
            /// <summary>
            /// 背景颜色
            /// </summary>
            static public string BackgroundColor; //12、
            /// <summary>
            /// /联合分组
            /// </summary>
            static public string Group; //13、
            /// <summary>
            /// 判断当前星球是否已经FS
            /// </summary>
            static public string FS;
            /// <summary>
            /// 发送FS前警告
            /// </summary>
            static public string FSWaring ;
            /// <summary>
            /// 航线被发现时间
            /// </summary>
            static public string CreateTime;
        }

        public static int FFLtColumnCount = 14;

        public static void CN()
        {
            FFLtColumn.SurplusTime = "剩余时间";
            FFLtColumn.ArrivalTime = "到达时间";
            FFLtColumn.TaskContent = "任务内容";
            FFLtColumn.TaskID = "任务ID";
            FFLtColumn.Content = "精确任务内容";
            FFLtColumn.StartPlace = "出发地点";
            FFLtColumn.StartPlanet = "出发星球";
            FFLtColumn.PlayerName = "玩家姓名";
            FFLtColumn.PID = "玩家PID";
            FFLtColumn.ReachPlace = "目标地点";
            FFLtColumn.ReachPlanet = "到达星球";
            FFLtColumn.FleetCategory = "舰队总类";
            FFLtColumn.Res = "携带的资源";
            FFLtColumn.FontColor = "字体颜色";
            FFLtColumn.BackgroundColor = "背景颜色";
            FFLtColumn.Group = "联合分组";
            FFLtColumn.FS = "是否FS";
            FFLtColumn.FSWaring = "是否FS前警告";
            FFLtColumn.CreateTime = "航线被发现时间";
        }
    }
}