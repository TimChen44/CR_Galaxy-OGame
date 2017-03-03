using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CR_Galaxy.OGControl
{
    public enum ENavigateOther
    {
        /// <summary>
        /// 刷新资源
        /// </summary>
        Res,
        /// <summary>
        /// 刷新舰队
        /// </summary>
        Fleet,
       /// <summary>
       /// 其他刷新
       /// </summary>
        Other

    }


    /// <summary>
    /// 全局控制综合管理类
    /// </summary>
    public class OGControlManage
    {
        /// <summary>
        /// 所有内政控制
        /// </summary>
        public Hashtable _OGControl = new Hashtable();

        /// <summary>
        /// 舰队控制
        /// </summary>
        public OGMilitary _OGMilitary = new OGMilitary();
        /// <summary>
        /// 计入身份ID
        /// </summary>
        public string _Session;
        /// <summary>
        /// 是否正在刷新资源页面
        /// </summary>
        public bool  _ResRefNow=false;

        /// <summary>
        /// 是否正在控制舰队
        /// </summary>
        public bool _FleetControlNow = false;

        /// <summary>
        /// 自动FS模式下禁止任何带有星球的刷新
        /// </summary>
        public bool _AutoFS = false;
        /// <summary>
        /// 当前拥有处理权限的星球
        /// </summary>
        public string _PlanetID = "";
        ///// <summary>
        ///// 当前连接数量，应为舰队控制时不能存在有任何正在连接的项目，所以必须要等待链接全部完成
        ///// </summary>
        //public int _LinkCount = 0;
        //没法判断啊。索性不用了，直接判断吧

        public bool GetNavigateAllow(ENavigateOther NavigateOther)
        {
            if (NavigateOther == ENavigateOther.Res)
            {//如果刷新的是资源，那么就要判断资源是否被占用
                return _ResRefNow || _FleetControlNow || _AutoFS; //只要一个处于使用中，那么就属于使用中
            }

            if (NavigateOther == ENavigateOther.Fleet)
            {
                return _ResRefNow || _FleetControlNow || _AutoFS;
            }
            else
            {
                return _FleetControlNow;
            }

        }
    }
}
