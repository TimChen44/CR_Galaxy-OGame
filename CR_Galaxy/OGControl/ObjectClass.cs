using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CR_Galaxy.OGControl
{
   public  class ObjectClass
    {

    }
    /// <summary>
    /// 舰队飞行列表信息
    /// </summary>
    public class FleetFlyInfo
    {
        /// <summary>
        /// 0、到达时间
        /// </summary>
        public DateTime ArrivalTime;
        public string TaskContent;     //1、任务内容
        public string TaskID; //2、任务ID
        public string Content;  //3、精确任务内容（再次定义的任务内容，比如遇到间谍流攻击任务内容从定义为探测）
        public string StartPlace; //4、出发地点
        public string StartPlanet;   //5、出发星球
        public string PlayerName;  //6、玩家姓名
        public string PID;  //7、玩家PID
        public string Location; //8、目标地点
        public string ReachPlanet; //9、到达星球
        public string FleetCategory;  //10、舰队总类
        public Color FontColor; //11、字体颜色
        public Color BackgroundColor; //12、背景颜色
        public bool Group; //13、联合分组
    }
}
