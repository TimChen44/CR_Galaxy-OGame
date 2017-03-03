using System;
using System.Collections.Generic;
using System.Text;
using CR_Soft.Windows.Web;

namespace CR_Galaxy
{
    public class GalaxySystem : CR_Soft.Windows.Web.ClassWB
    {
        private int _Galaxy;
        private int _System;
        /// <summary>
        /// 起始银河
        /// </summary>
        private int _BeginGalaxy;
        /// <summary>
        /// 起始太阳系
        /// </summary>
        private int _BeginSystem;
        /// <summary>
        /// 结束银河
        /// </summary>
        private int _EndGalaxy;
        /// <summary>
        /// 结束太阳系
        /// </summary>
        private int _EndSystem;

        private string GalaxyURL = "{0}index.php?page=galaxy&no_header=1&session={1}&galaxy={2}&system={3}";

        /// <summary>
        /// 构造
        /// </summary>
        public GalaxySystem(System.Windows.Forms.WebBrowser spWB)
            : base(spWB)
        {

        }

        /// <summary>
        /// 太阳系
        /// </summary>
        public int System
        {
            get
            {
                return _System;
            }
            set
            {
                _System = value;
            }
        }

        /// <summary>
        /// 银河
        /// </summary>
        public int Galaxy
        {
            get
            {
                return _Galaxy;
            }
            set
            {
                _Galaxy = value;
            }
        }

        public int BeginGalaxy
        {
            get
            {
                return _BeginGalaxy;
            }
            set
            {
                _BeginGalaxy = value;
            }
        }

        public int BeginSystem
        {
            get
            {
                return _BeginSystem;
            }
            set
            {
                _BeginSystem = value;
            }
        }

        public int EndGalaxy
        {
            get
            {
                return _EndGalaxy;
            }
            set
            {
                _EndGalaxy = value;
            }
        }

        public int EndSystem
        {
            get
            {
                return _EndSystem;
            }
            set
            {
                _EndSystem = value;
            }
        }

        /// <summary>
        /// 太阳系加一
        /// </summary>
        public void SystemInc()
        {
            _System =Convert.ToInt32 ( this.GetSystme());//矫正太阳系编号
            if (_System >= 499)
            {
                _Galaxy++;
                base.ModValueName("galaxy", _Galaxy.ToString());
                _System = 1;
                Goto();
            }
            else
            {
                base.ClickName("systemRight");
                _System++;
            }
        }

        /// <summary>
        /// 银河加一
        /// </summary>
        public void GalaxyInc()
        {
            base.ClickName("galaxyRight");
        }

        /// <summary>
        /// 跳转至
        /// </summary>
        /// <param name="spGalaxy">银河</param>
        /// <param name="spSystem">太阳系</param>
        public void Goto(int spGalaxy, int spSystem)
        {
            _Galaxy = spGalaxy;
            _System = spSystem;
            Goto();
        }

        /// <summary>
        /// 直接转到
        /// </summary>
        public void Goto()
        {
            base.ModValueName("galaxy", _Galaxy.ToString());
            base.ModValueName("system",Convert.ToString(_System-1));
            //base.ClickValue("显示");//不知为什么按显示无效
            base.ClickName("systemRight"); //所以用这个代替
        }

        public void Goto(string Server, string spSession, int spGalaxy, int spSystem)
        {
            _Galaxy = spGalaxy;
            _System = spSystem;
            base.Navigate(string.Format(GalaxyURL, new string[] { Server, spSession, spGalaxy.ToString(), spSystem.ToString() }));
        }

        public void Goto(string Server, string spSession)
        {
            base.Navigate(string.Format(GalaxyURL, new string[] { Server, spSession, _Galaxy.ToString(), _System.ToString () }));
        }

        /// <summary>
        /// 从页面读取太阳系地址
        /// </summary>
        /// <returns></returns>
        public string GetSystme()
        {
           return base.ReadValueName("system");
        }

        /// <summary>
        /// 从页面读取银河系地址
        /// </summary>
        /// <returns></returns>
        public string GetGalaxy()
        {
            return base.ReadValueName("galaxy");
        }

        /// <summary>
        /// 星系地址是否溢出
        /// </summary>
        /// <returns>
        /// 0、正常
        /// 1、银河地址溢出
        /// 2、太阳系地址溢出
        /// 3、都溢出
        /// 4、超出搜索范围
        /// </returns>
        public int Overflow(int spGalaxy, int spSystem)
        {
            if (spGalaxy > _EndGalaxy) return 4;
            if (spGalaxy >= _EndGalaxy)
            {
                if (spSystem > _EndSystem)
                {
                    return 4;
                }

            }
            return 0;
        }
        /// <summary>
        /// 地址是否溢出
        /// </summary>
        /// <returns></returns>
        public int Overflow()
        {
            if (_Galaxy > _EndGalaxy) return 4;

            if (_Galaxy >= _EndGalaxy)
            {
                if (_System > _EndSystem)
                {
                    return 4;
                }

            }
            return 0;
        }

        /// <summary>
        /// 获得星系总数
        /// </summary>
        /// <returns></returns>
        public int GetSystemCount()
        {
            if (_BeginGalaxy == _EndGalaxy)
            {
                return _EndSystem - _BeginSystem;

            }
            else
            {
                return (_EndGalaxy - _BeginGalaxy - 1) * 499 + (499 - _BeginSystem) + _EndSystem + 1 ;
            }
        }
    }
}