using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CR_Galaxy
{
    public class Navigate
    {
        /// <summary>
        /// 记入最后的链接
        /// </summary>
        public string EndUrl;
        /// <summary>
        /// 当前状态
        /// 0为空闲
        /// 1为忙碌
        /// -1为不可用
        /// </summary>
        public int State;

        /// <summary>
        /// 存储当前WEB在做什么
        /// 0：什么都不做
        /// 1：扫描星图
        /// 2：扫描个人排名
        /// 3：扫描舰队排名
        /// 10：首次登入
        /// </summary>
        public int Work;

        /// <summary>
        /// 当前状态
        /// True继续扫描
        /// False扫描结束
        /// </summary>
        public bool WorkState;

        /// <summary>
        /// 
        /// </summary>
        public string _Session;

        public Navigate()
        {

        }

        /// <summary>
        /// 获得session
        /// 将来这里还要获得星球信息
        /// </summary>
        /// <param name="spHD"></param>
        /// <returns></returns>
        public bool SaveMainSession(HtmlDocument spHD)
        {
            HtmlElementCollection dom = spHD.GetElementsByTagName("option");
            foreach (HtmlElement HE in dom)
            {
                if (HE.GetAttribute("Value") != null)
                {
                    if (HE.GetAttribute("Value").ToString().ToLower().IndexOf("session") >= 0)
                    {
                        string Tmp = HE.GetAttribute("Value").ToString().ToLower();
                        _Session = Tmp.Substring(Tmp.IndexOf("session=") + 8, 12);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool SaveSession(HtmlDocument spHD)
        {
            HtmlElementCollection dom = spHD.GetElementsByTagName("a");
            foreach (HtmlElement HE in dom)
            {
                if (HE.InnerHtml.IndexOf("session=") > 0)
                {
                    _Session = HE.InnerHtml.Substring(HE.InnerHtml.IndexOf("session=") + 8, 12);
                    return true;
                }

            }
            return false;
        }
    }
}
