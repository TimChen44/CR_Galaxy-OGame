using System;
using System.Collections.Generic;
using System.Text;

namespace CR_Galaxy
{
    public struct ExPlanet
    {
        /// <summary>
        /// 银河
        /// </summary>
        public int Galaxy;
        /// <summary>
        /// 太阳系
        /// </summary>
        public int System;
        /// <summary>
        /// 位置
        /// </summary>
        public int Location;
        /// <summary>
        /// 星球名
        /// </summary>
        public string PlanetName;
        /// <summary>
        /// 月亮
        /// </summary>
        public bool Moon;
        /// <summary>
        /// 体积
        /// </summary>
        public int MoonSize;
        /// <summary>
        /// 金属
        /// </summary>
        public int Metal;
        /// <summary>
        /// 晶体
        /// </summary>
        public int Crystal;
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username;
        /// <summary>
        /// 联盟
        /// </summary>
        public string Union;
        /// <summary>
        /// 假期
        /// </summary>
        public bool Vacation;
        /// <summary>
        /// 7天不在线
        /// </summary>
        public bool Inactive;
        /// <summary>
        /// 30天不在线
        /// </summary>
        public bool LongInactive;
        /// <summary>
        /// 被封
        /// </summary>
        public bool Banned;
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date;

        public string Memo;

        public string Spy;

        /// <summary>
        /// 联盟排名
        /// </summary>
        public int UnionRankings;
        /// <summary>
        /// 联盟成员
        /// </summary>
        public int Members;
        /// <summary>
        /// 排名
        /// </summary>
        public int Rankings;

        public ExPlanet(bool spbool)
        {
            this.Galaxy = 0;
            System = 0;
            Location = 0;
            MoonSize = 0;
            Metal = 0;
            Crystal = 0;

            UnionRankings = 0;
            Members = 0;
            this.Rankings = 0;

            PlanetName = "";
            Username = "";
            Union = "";


            Banned = false;
            LongInactive = false;
            Inactive = false;
            Vacation = false;
            Moon = false;

            Date = new DateTime() ;
            Memo = "";
            Spy = "";
        }
    }
}
