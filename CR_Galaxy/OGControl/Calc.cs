using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace CR_Galaxy.OGControl
{
    /// <summary>
    /// 建筑物当前状态
    /// </summary>
    public enum EBuildState
    {
        /// <summary>
        /// 可以建造
        /// </summary>
        Enabled,
        /// <summary>
        /// 不可以建造
        /// </summary>
        Disabled,
        /// <summary>
        /// 正在升级
        /// </summary>
        Uping
    }


    /// <summary>
    /// 对象信息
    /// </summary>
    public class ObjectInfo
    {
        /// <summary>
        /// 对象名字
        /// </summary>
        public string Text;
        /// <summary>
        /// 需要金属
        /// </summary>
        public double Metall = 0;//
        /// <summary>
        /// 需要晶体
        /// </summary>
        public double Kristall = 0;//
        /// <summary>
        /// 需要重氢
        /// </summary>
        public double Deuterium = 0;//
        /// <summary>
        /// 建造时间
        /// </summary>
        public DateTime  Period;//
        /// <summary>
        /// 当前对象等级
        /// </summary>
        public double Level;
        /// <summary>
        /// 建造链接
        /// </summary>
        public string Url="";
        /// <summary>
        /// 建筑物当前状态
        /// </summary>
        public EBuildState State = EBuildState.Disabled;

    }

    public class Calc
    {
        /// <summary>
        /// 建筑物需要资源及时间
        /// </summary>
        /// <param name="DR"></param>
        /// <param name="Level"></param>
        /// <param name="Rot"></param>
        /// <param name="NanoRot"></param>
        /// <returns></returns>
        public ObjectInfo CalcBuildRes(DataRow DR, double Level, double Rot, double NanoRot)
        {
            ObjectInfo ORes = new ObjectInfo();
            CalcForschungRes(ORes, DR, Level);
            ORes.Period = new DateTime((long)(((ORes.Metall + ORes.Kristall) / 2500) * (1 / (Rot + 1)) * Math.Pow(0.5, NanoRot) * 60 * 60 * 10000000));

            ORes.Level =Level;
            return ORes;
        }

        /// <summary>
        /// 研究需要资源及时间
        /// </summary>
        /// <param name="DR"></param>
        /// <param name="Level"></param>
        /// <param name="Rot"></param>
        /// <param name="NanoRot"></param>
        /// <returns></returns>
        public ObjectInfo CalcForschungRes(DataRow DR, double Level,string ForschungInfo)
        {
            ObjectInfo ORes = new ObjectInfo();
            CalcForschungRes(ORes, DR, Level);

            ORes.Period = Info.GetBDateTime(ForschungInfo);

            ORes.Level = Level;
            return ORes;
        }

        //建造和研究所需资源
        public void CalcForschungRes(ObjectInfo ORes, DataRow DR, double Level)
        {
            int CalcType = Convert.ToInt32(DR["Calc"]);
            double Metall = Convert.ToDouble(DR["JS"]);
            double Kristall = Convert.ToDouble(DR["JT"]);
            double Deuterium = Convert.ToDouble(DR["HH"]);

            switch (CalcType)
            {

                case 1://金属
                    ORes.Metall = 60 * Math.Pow(1.5, Level);
                    ORes.Kristall = 15 * Math.Pow(1.5, Level);
                    break;
                case 2://晶体
                    ORes.Metall = 48 * Math.Pow(1.6, Level);
                    ORes.Kristall = 24 * Math.Pow(1.6, Level);
                    break;
                case 3://重氢
                    ORes.Metall = 225 * Math.Pow(1.5, Level);
                    ORes.Kristall = 75 * Math.Pow(1.5, Level);
                    break;
                case 4://太阳能
                    ORes.Metall = 75 * Math.Pow(1.5, Level);
                    ORes.Kristall = 30 * Math.Pow(1.5, Level);
                    break;
                case 5://核电站
                    ORes.Metall = 900 * Math.Pow(1.8, Level);
                    ORes.Kristall = 360 * Math.Pow(1.8, Level);
                    ORes.Deuterium = 180 * Math.Pow(1.8, Level);
                    break;
                case 6://引力技术

                    break;
                default://其他建筑物
                    ORes.Metall = Metall * Math.Pow(2, Level);
                    ORes.Kristall = Kristall * Math.Pow(2, Level);
                    ORes.Deuterium = Deuterium * Math.Pow(2, Level);
                    break;
            }
        }



        
    }
}
