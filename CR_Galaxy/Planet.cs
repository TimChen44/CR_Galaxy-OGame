using System;
using System.Collections.Generic;
using System.Text;

namespace CR_Galaxy
{
    /// <summary>
    /// 作废-早期
    /// </summary>
    /// <remarks></remarks>
    public struct Planet
    {
        /// <summary>
        /// 位置
        /// </summary>
        public int Location;
        /// <summary>
        /// 月亮
        /// </summary>
        public bool Moon ;
        /// <summary>
        /// 星球名
        /// </summary>
        public string PlanetName;
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username;
        /// <summary>
        /// 排名
        /// </summary>
        public int Rankings;
        /// <summary>
        /// 联盟
        /// </summary>
        public string Union;
        /// <summary>
        /// 联盟排名
        /// </summary>
        public int UnionRankings;
        /// <summary>
        /// 联盟成员
        /// </summary>
        public int Members;
        ///// <summary>
        ///// 低等级
        ///// </summary>
        //public bool Moon;
        /// <summary>
        /// 假期
        /// </summary>
        public bool Vacation ;
        /// <summary>
        /// 7天不在线
        /// </summary>
        public bool Inactive ;
        /// <summary>
        /// 30天不在线
        /// </summary>
        public bool LongInactive;
        /// <summary>
        /// 被封
        /// </summary>
        public bool Banned ;
        /// <summary>
        /// 银河
        /// </summary>
        public int Galaxy;
        /// <summary>
        /// 太阳系
        /// </summary>
        public int System;
        /// <summary>
        /// 日期
        /// </summary>
        public string Date;

        public string Memo;

        public string Spy;

        public Planet(bool spbool)
        {
            Location = 0;
            Rankings = 0;
            UnionRankings = 0;
            Members = 0;
            this.Galaxy = 0;
            System = 0;

            PlanetName = "";
            Username = "";
            Union = "";

            Banned = false;
            LongInactive = false;
            Inactive = false;
            Vacation = false;
            Moon = false;
            Moon = false;

            Date = "";
            Memo = "";
            Spy = "";
        }



    }

}
