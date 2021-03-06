using System;
using System.Collections.Generic;
using System.Text;

namespace CR_Galaxy
{
    /// <summary>
    /// 不同的服务器有不同的文字，所以用这个来切换
    /// </summary>
    public class ServerInfo
    {
        /// <summary>
        /// 排名
        /// </summary>
        public string Rankings;

        /// <summary>
        /// " 有 "
        /// </summary>
        public string Have;

        /// <summary>
        /// "</td"
        /// </summary>
        public string td;

        /// <summary>
        /// "名会员"
        /// </summary>
        public string Members;

        /// <summary>
        /// 服务器
        /// </summary>
        public string Server;

        /// <summary>
        /// 域名
        /// </summary>
        public string Website;

        public string WebsiteEx;

        public string Moon;
        /// <summary>
        /// 大小
        /// </summary>
        /// 
        public string MoonSize;
        /// <summary>
        /// 大小\
        /// </summary>
        public string MoonSizeEnd = ")\\";//0.76c修改了显示方式，所以增加了")"

        /// <summary>
        /// 金属
        /// </summary>
        public string Metal;

        /// <summary>
        /// 晶体
        /// </summary>
        public string Crystal;

        /// <summary>
        /// 残骸结尾
        /// </summary>
        public string WreckageEnd;

        public string Login;

        public void CN()
        {
            Rankings = "排名";
            Have = " 有 ";
            td = "</td";
            Members = "名会员";
            Server = "中国";
            Login = @"http://uni{0}.cn.ogame.org/game/reg/login2.php?v=2&login={1}&pass={2}";
            WebsiteEx = @"http://uni{0}.cn.ogame.org/game/";
            Website = "www.ogame.com.cn";
            Moon = "月球";
            MoonSize = "大小：";
            Metal ="金属：</th><th>";
            Crystal = "晶体：</th><th>";
            WreckageEnd = "</th>";
        }

        public void TW()
        {
            Rankings = "排名";
            Have = " 有 ";
            td = "</td";
            Members = "名會員";
            Server = "台湾";
            Login = @"http://uni{0}.ogame.tw/game/reg/login2.php?v=2&login={1}&pass={2}";
            WebsiteEx = @"http://uni{0}.ogame.tw/game/";
            Website = "www.ogame.tw";
            Moon = "月球";
            MoonSize = "大小:";
            Metal = "金屬:</th><th>";
            Crystal = "晶體:</th><th>";
            WreckageEnd = "</th>";
        }
        public void DE()
        {
            Rankings = "auf Platz";
            Have = " mit ";
            td = "</td";
            Members = "Mitglieder";
            Server = "德国";
            Login = @"http://uni{0}.ogame.de/game/reg/login2.php?v=2&login={1}&pass={2}";
            WebsiteEx = @"http://uni{0}.ogame.de/game/";
            Website = "ogame.de";

            Moon = "Mond";
            MoonSize = "Größe:";
            Metal = "Metall:</th><th>";
            Crystal = "Kristall:</th><th>";
            WreckageEnd = "</th>";
        }

        public void EN()
        {
            Rankings = "ranked";
            Have = " consisting of ";
            td = "</td";
            Members = "member";
            Server = "英国";
            Login = @"http://uni{0}.ogame.org/game/reg/login2.php?v=2&login={1}&pass={2}";
            WebsiteEx = @"http://uni{0}.ogame.org/game/";
            Website = "www.ogame.org";

            Moon = "Moon";
            MoonSize = "Size:";
            Metal = "Metal:</th><th>";
            Crystal = "Crystal:</th><th>";
            WreckageEnd = "</th>";
        }

        public void AutoServer(string spServer)
        {
            switch (spServer)
            {
                case "中国":
                    CN();
                    break;
                case "台湾":
                    TW();
                    break;
                case "德国":
                    DE();
                    break;
                case "英国":
                    EN();
                    break;
            }
        }

        public string LoginUrl(string spU, string spLogin, string spPass)
        {
            WebsiteEx = string.Format(WebsiteEx, spU);
            string L=Login;
            return string.Format(L, spU, spLogin, spPass);

        }
    }
}
