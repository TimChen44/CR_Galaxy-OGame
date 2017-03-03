using System;
using System.Collections.Generic;
using System.Text;

namespace CR_Galaxy
{
    public struct Rankings
    {
        /// <summary>
        /// 扫分时间
        /// </summary>
        public string Date;
        /// <summary>
        /// 排名变化
        /// </summary>
        public int EesC;
        /// <summary>
        /// 排名
        /// </summary>
        public int EesR;
        /// <summary>
        /// 得分
        /// </summary>
        public int EesS;
        /// <summary>
        /// 玩家或舰队名
        /// </summary>
        public string Name;
    
        //public Rankings()
        //{
        //    Date = new DateTime();
        //    EesC=0;
        //    EesR=0;
        //    EesS = 0;
        //    Name = "";
        //}
    }
}
