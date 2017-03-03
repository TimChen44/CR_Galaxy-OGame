using System;
using System.Collections.Generic;
using System.Text;
using CR_Soft.ClassLibrary.Data;
using System.Data;
using System.Windows.Forms;
using System.IO;


namespace CR_Galaxy
{
    public class IOData
    {
        private CR_Soft.ClassLibrary.Data.Access _Access;
        private string ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}";

        private CR_Soft.ClassLibrary.Log.Log _Log;

        public IOData(string spPath, CR_Soft.ClassLibrary.Log.Log spLog)
        {
            _Access = new Access(string.Format(ConStr, spPath), false);
            _Access.Connection();
            _Log = spLog;
        }

        public void OutData(CR_Galaxy.ExPlanet[] spExInfo)
        {
            //作废，心疼啊
            //string OutStr = "INSERT INTO galaxy ( [Galaxy], [System], [Location], [PlanetName], [Username], [Rankings], [Union], [UnionRankings], [Members], [Moon], [vacation], [inactive], [longinactive], [banned], [Spy], [Date],[Memo],[GalaxySystem] ) VALUES (" +
            //    "{0}, {1}, {2}, '{3}', '{4}', {5}, '{6}', {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, '{15}',{16},'{17}')";

            //string UpDataStr = "UPDATE galaxy SET [PlanetName]='{3}', [Username]='{4}', [Rankings]={5}, [Union]='{6}', [UnionRankings]={7}, [Members]={8}, [Moon]={9}, [vacation]={10}, [inactive]={11}," +
            //    "[longinactive]={12}, [banned]= {13}, [Spy]={14}, [Date]= '{15}',[Memo]={16},[GalaxySystem]='{17}' where  [Galaxy]={0} and [System]={1} and [Location]={2}";

            string OutStr = "INSERT INTO galaxy ( GalaxySystem, Galaxy, System, Location, PlanetName, Moon, MoonSize, Metal, Crystal, Username, [Union], vacation, inactive, longinactive, banned, Spy, [Date], [Memo],Ni,[Rankings], [UnionRankings], [Members] ) VALUES (" +
            "'{0}', {1},{2}, {3}, '{4}', {5}, {6}, {7}, {8}, '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}', '{16}', '{17}',{18},{19},{20},{21})";

            string UpDataStr = "UPDATE galaxy SET galaxy.GalaxySystem = '{0}', galaxy.Galaxy = {1}, galaxy.System = {2}, galaxy.Location = {3}, galaxy.PlanetName = '{4}', galaxy.Moon = {5}, galaxy.MoonSize = {6}, galaxy.Metal = {7}, " +
                "galaxy.Crystal = {8}, galaxy.Username = '{9}', galaxy.[Union] = '{10}', galaxy.vacation = {11}, galaxy.inactive = {12}, galaxy.longinactive = {13}, galaxy.banned = {14}, galaxy.Spy = '{15}', galaxy.[Date] = '{16}', galaxy.[Memo] = '{17}',galaxy.Ni = {18} ,[Rankings]={19}, [UnionRankings]={20} ,[Members] = {21} " +
                "where  GalaxySystem='{0}' ";

            string HasStr = "SELECT galaxy.ID,galaxy.inactive,galaxy.Username FROM galaxy WHERE Galaxy={0} and System ={1} and Location={2};";


            for (int i = 0; i <= 14; i++)
            {
                try
                {
                    if ((spExInfo[i].Galaxy == 0) || (spExInfo[i].System == 0) || (spExInfo[i].Location == 0)) continue;
                    string[] Par ={
                        spExInfo[i].Galaxy.ToString() + ":" + spExInfo[i].System.ToString() + ":" + spExInfo[i].Location.ToString(),
                        spExInfo[i].Galaxy.ToString() ,
                        spExInfo[i].System.ToString(),
                        spExInfo[i].Location.ToString() ,
                        spExInfo[i].PlanetName ,
                        spExInfo[i].Moon.ToString(),
                        spExInfo[i].MoonSize.ToString(),
                        spExInfo[i].Metal.ToString(),
                        spExInfo[i].Crystal .ToString(),
                        spExInfo[i].Username,
                        spExInfo[i].Union ,
                        spExInfo[i].Vacation.ToString() ,
                        spExInfo[i].Inactive.ToString() ,
                        spExInfo[i].LongInactive.ToString() ,
                        spExInfo[i].Banned.ToString() ,
                        "",
                        DateTime.Now.ToString("yyyy-M-d"),
                        "",
                        "False",
                        spExInfo[i].Rankings .ToString(),
                        spExInfo[i].UnionRankings .ToString(),
                        spExInfo[i].Members .ToString()
                   };

                    DataTable s = (_Access.TableSqlEcx(HasStr, new string[] { spExInfo[i].Galaxy.ToString(), spExInfo[i].System.ToString(), spExInfo[i].Location.ToString() }));
                    //检查搜索到的行数
                    if (s.Rows.Count >= 1)
                    {
                        if (((bool)s.Rows[0]["inactive"] == false) && (spExInfo[i].Inactive == true))
                            Par[18] = "True";
                        if (spExInfo[i].Username == s.Rows[0]["Username"].ToString())
                            spExInfo[i].Spy = s.Rows[0]["Spy"].ToString();
                        _Access.ReutnSqlEcx(UpDataStr, Par);
                    }
                    else
                    {
                        _Access.ReutnSqlEcx(OutStr, Par);
                    }
                }
                catch (Exception ee)
                {
                    _Log.AddError("<OutData>" + ee.Message);
                }
            }
        }

        public DataTable ShowAll()
        {
            return _Access.TableSqlEcx("SELECT * FROM Galaxy ORDER BY Galaxy.Galaxy, Galaxy.System, Galaxy.Location;");
        }

        public DataTable FastUser(string spUser, bool spMH)
        {
            if (spMH == true)//模糊查询

                return _Access.FuzzyFind("Galaxy", "Username", spUser);
            else
                return _Access.Find("Galaxy", "Username", spUser);
        }

        public DataTable FastUnion(string spUnion, bool spMH)
        {
            if (spMH == true)//模糊查询

                return _Access.FuzzyFind("Galaxy", "[Union]", spUnion);
            else
                return _Access.Find("Galaxy", "[Union]", spUnion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spPlanet"></param>
        /// <param name="spMH"></param>
        /// <returns></returns>
        public DataTable FastPlanet(string spPlanet, bool spMH)
        {
            if (spMH == true)//模糊查询

                return _Access.FuzzyFind("Galaxy", "PlanetName", spPlanet);
            else
                return _Access.Find("Galaxy", "PlanetName", spPlanet);
        }

        /// <summary>
        /// 快速定位星系
        /// </summary>
        /// <param name="spGalaxy"></param>
        /// <returns></returns>
        public DataTable FastGalaxy(int spGalaxy)
        {
            return _Access.Find("Galaxy", "Galaxy", spGalaxy);

        }

        /// <summary>
        /// 搜索i羊
        /// </summary>
        /// <param name="spGalaxy"></param>
        /// <returns></returns>
        public DataTable Findi(int spGalaxy)
        {
            string SqlStr = "SELECT * FROM Galaxy WHERE Galaxy={0} and inactive={1}  ORDER BY Galaxy.Galaxy, Galaxy.System, Galaxy.Location;";
            return _Access.TableSqlEcx(SqlStr, new string[] { spGalaxy.ToString(), "True" });
        }
        /// <summary>
        /// 搜索I羊
        /// </summary>
        /// <param name="spGalaxy"></param>
        /// <returns></returns>
        public DataTable FindI(int spGalaxy)
        {
            string SqlStr = "SELECT * FROM Galaxy WHERE Galaxy={0} and longinactive={1}  ORDER BY Galaxy.Galaxy, Galaxy.System, Galaxy.Location;";
            return _Access.TableSqlEcx(SqlStr, new string[] { spGalaxy.ToString(), "True" });
        }

        public DataTable FindNi(int spGalaxy)
        {
            string SqlStr = "SELECT * FROM Galaxy WHERE Galaxy={0} and Ni={1}  ORDER BY Galaxy.Galaxy, Galaxy.System, Galaxy.Location;";
              return _Access.TableSqlEcx(SqlStr,  spGalaxy.ToString(), "True" );
        }

        public DataTable Find4Planet(int spGalaxy, string spLocation)
        {
            string SqlStr = "SELECT Galaxy.* FROM Galaxy where Galaxy.Galaxy={0} and Galaxy.Location ={1}";
            return _Access.TableSqlEcx(SqlStr, spGalaxy.ToString(), spLocation);
        }

        public DataTable FindNullSystem(int spGalaxy)
        {
            return null;

        }

        /// <summary>
        /// 用户其他星球
        /// </summary>
        /// <param name="spUserName"></param>
        /// <returns></returns>
        public DataTable FindUserOtherPlanet(string spUserName)
        {
            return _Access.Find("Galaxy", "Username", spUserName);
        }

        /// <summary>
        /// 邻居星球
        /// </summary>
        /// <param name="spGalaxy"></param>
        /// <param name="spSystem"></param>
        /// <returns></returns>
        public DataTable FindPlanetNeighbors(string spGalaxy, string spSystem)
        {
            string SqlStr = "SELECT * FROM Galaxy WHERE Galaxy={0} and System={1}  ORDER BY Galaxy.Galaxy, Galaxy.System, Galaxy.Location;";
            return _Access.TableSqlEcx(SqlStr, new string[] { spGalaxy, spSystem });
        }
        /// <summary>
        /// 用户排名
        /// </summary>
        /// <param name="Min"></param>
        /// <param name="Max"></param>
        /// <returns></returns>
        public DataTable FindUserRankings(string Min, string Max)
        {
            string SqlStr = "SELECT * FROM Galaxy WHERE Rankings>={0} and Rankings<={1}  ORDER BY Rankings;";
            return _Access.TableSqlEcx(SqlStr, new string[] { Min.ToString(), Max.ToString() });
        }

        public DataTable FindUserUnionRankings(string Min, string Max)
        {
            string SqlStr = "SELECT * FROM Galaxy WHERE UnionRankings>={0} and UnionRankings<={1}  ORDER BY UnionRankings;";
            return _Access.TableSqlEcx(SqlStr, new string[] { Min.ToString(), Max.ToString() });
        }

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <param name="spGalaxy"></param>
        /// <param name="spUserName"></param>
        /// <param name="spRankings"></param>
        /// <param name="spUnion"></param>
        /// <param name="spUnionRankings"></param>
        /// <param name="spi"></param>
        /// <param name="spLi"></param>
        /// <param name="spVacation"></param>
        /// <param name="spBanned"></param>
        /// <returns></returns>
        public DataTable FindAdvancedFind(string spGalaxy, string spUserName, string spRankings, string spUnion, string spUnionRankings, string spi, string spLi, string spVacation, string spBanned)
        {
            string SqlStr = "SELECT * FROM Galaxy WHERE {0} and {1} and {2} and {3} and {4} and {5} and {6} and {7} and {8}  ORDER BY Galaxy.Galaxy, Galaxy.System, Galaxy.Location;";
            return _Access.TableSqlEcx(SqlStr, new string[] { spGalaxy, spUserName, spRankings, spUnion, spUnionRankings, spi, spLi, spVacation, spBanned });
        }

        /// <summary>
        /// 导出数据-全部数据
        /// </summary>
        /// <returns></returns>
        public void OutAllData(string spPath)
        {
            DataTable DT = _Access.ShowAll("galaxy");
            DT.WriteXml(spPath);

        }

        public void OutNowData(string spPath)
        {
            string SqlStr = "SELECT * FROM Galaxy WHERE Date = #{0}#";
            string MyDate = DateTime.Now.ToString("yyyy-M-d");
            DataTable DT = _Access.TableSqlEcx(SqlStr, new string[] { MyDate });
            DT.WriteXml(spPath);
        }

        public void OutGalaxyAllData(string spPath, string spGalaxy)
        {
            string SqlStr = "SELECT * FROM Galaxy WHERE [galaxy] = {0}";
            DataTable DT = _Access.TableSqlEcx(SqlStr, new string[] { spGalaxy });
            DT.WriteXml(spPath);
        }

        public void OutGalaxyNowData(string spPath, string spGalaxy)
        {
            string SqlStr = "SELECT * FROM Galaxy WHERE Date = #{0}# and  [galaxy] = {1}";
            string MyDate = DateTime.Now.ToString("yyyy-M-d");
            DataTable DT = _Access.TableSqlEcx(SqlStr, new string[] { MyDate, spGalaxy });
            DT.WriteXml(spPath);
        }


        /// <summary>
        /// 导入数据用
        /// </summary>
        /// <param name="spPath"></param>
        /// <param name="spPBar"></param>
        public void InData(string spPath, ProgressBar spPBar)
        {

            DataTable DT = _Access.Find("galaxy", "ID", -1);
            // DT.ReadXmlSchema(spPath);
            DT.ReadXml(spPath);

            spPBar.Maximum = DT.Rows.Count;


            string OutStr = "INSERT INTO galaxy ( GalaxySystem, Galaxy, System, Location, PlanetName, Moon, MoonSize, Metal, Crystal, Username, [Union], vacation, inactive, longinactive, banned, Spy, [Date], [Memo],[Rankings],[UnionRankings],[Members],[Ni] ) VALUES (" +
"'{0}', {1},{2}, {3}, '{4}', {5}, {6}, {7}, {8}, '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}', '{16}', '{17}',{18},{19},{20},{21})";

            string UpDataStr = "UPDATE galaxy SET galaxy.GalaxySystem = '{0}', galaxy.Galaxy = {1}, galaxy.System = {2}, galaxy.Location = {3}, galaxy.PlanetName = '{4}', galaxy.Moon = {5}, galaxy.MoonSize = {6}, galaxy.Metal = {7}, " +
                "galaxy.Crystal = {8}, galaxy.Username = '{9}', galaxy.[Union] = '{10}', galaxy.vacation = {11}, galaxy.inactive = {12}, galaxy.longinactive = {13}, galaxy.banned = {14}, galaxy.Spy = '{15}', galaxy.[Date] = '{16}', galaxy.[Memo] = '{17}',Rankings = {18} ,UnionRankings = {19} , Members ={20} , Ni={21}" +
                "where  GalaxySystem='{0}' ";

            //作废，再次心痛
            ////添加
            //string OutStr = "INSERT INTO galaxy ( [Galaxy], [System], [Location], [PlanetName], [Username], [Rankings], [Union], [UnionRankings], [Members], [Moon], [vacation], [inactive], [longinactive], [banned], [Spy], [Date],[Memo],[GalaxySystem] ) VALUES (" +
            //    "{0}, {1}, {2}, '{3}', '{4}', {5}, '{6}', {7}, {8}, {9}, {10}, {11}, {12}, {13}, '{14}', '{15}',{16},'{17}')";

            ////更新
            //string UpDataStr = "UPDATE galaxy SET [PlanetName]='{3}', [Username]='{4}', [Rankings]={5}, [Union]='{6}', [UnionRankings]={7}, [Members]={8}, [Moon]={9}, [vacation]={10}, [inactive]={11}," +
            //    "[longinactive]={12}, [banned]= {13}, [Spy]='{14}', [Date]= '{15}',[Memo]={16},[GalaxySystem]='{17}' where  [Galaxy]={0} and [System]={1} and [Location]={2}";

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                ExPlanet spInfo = new ExPlanet();
                spInfo.Galaxy = Convert.ToInt32(DT.Rows[i]["Galaxy"]);
                spInfo.System = Convert.ToInt32(DT.Rows[i]["System"].ToString());
                spInfo.Location = Convert.ToInt32(DT.Rows[i]["Location"].ToString());
                spInfo.PlanetName = DT.Rows[i]["PlanetName"].ToString();
                spInfo.Moon = Convert.ToBoolean(DT.Rows[i]["Moon"]);
                spInfo.MoonSize = Convert.ToInt32(DT.Rows[i]["MoonSize"]);
                spInfo.Metal = Convert.ToInt32(DT.Rows[i]["Metal"]);
                spInfo.Crystal = Convert.ToInt32(DT.Rows[i]["Crystal"]);
                spInfo.Username = DT.Rows[i]["Username"].ToString();
                spInfo.Union = DT.Rows[i]["Union"].ToString();
                spInfo.Vacation = Convert.ToBoolean(DT.Rows[i]["vacation"]);
                spInfo.Inactive = Convert.ToBoolean(DT.Rows[i]["inactive"]);
                spInfo.LongInactive = Convert.ToBoolean(DT.Rows[i]["longinactive"]);
                spInfo.Banned = Convert.ToBoolean(DT.Rows[i]["banned"]);
                spInfo.Spy = DT.Rows[i]["Spy"].ToString();
                spInfo.Date = Convert.ToDateTime(DT.Rows[i]["Date"].ToString());
                spInfo.Memo = DT.Rows[i]["Memo"].ToString();


                string Rankings = "0";
                string UnionRankings = "0";
                string Members = "0";

                if (DT.Rows[i]["Rankings"].ToString() != "") Rankings = DT.Rows[i]["Rankings"].ToString();
                if (DT.Rows[i]["UnionRankings"].ToString() != "") UnionRankings = DT.Rows[i]["UnionRankings"].ToString();
                if (DT.Rows[i]["Members"].ToString() != "") Members = DT.Rows[i]["Members"].ToString();

                string[] Par ={spInfo.Galaxy.ToString() + ":" + spInfo.System.ToString() + ":" + spInfo.Location.ToString(),
                    spInfo.Galaxy.ToString() ,spInfo.System.ToString(),spInfo.Location.ToString() ,spInfo.PlanetName ,spInfo.Moon.ToString(),spInfo.MoonSize.ToString(),  
                    spInfo.Metal.ToString(),spInfo.Crystal .ToString(),spInfo.Username,spInfo.Union ,spInfo.Vacation.ToString() ,spInfo.Inactive.ToString() ,
                    spInfo.LongInactive.ToString() ,spInfo.Banned.ToString() ,spInfo.Spy,DateTime.Now.ToString("yyyy-M-d"),"''",
                           Rankings,UnionRankings,  Members,DT.Rows[i]["Ni"].ToString() };

                try
                {
                    DataTable DT2 = _Access.Find("galaxy", "GalaxySystem", DT.Rows[i]["GalaxySystem"].ToString());
                    if (DT2.Rows.Count == 1)
                    {//有相同数据，采用更新
                        if (spInfo.Date > Convert.ToDateTime(DT2.Rows[0]["Date"].ToString()))
                            _Access.ReutnSqlEcx(UpDataStr, Par);
                    }
                    else if (DT2.Rows.Count == 0)
                    {//没有数据，采用添加
                        _Access.ReutnSqlEcx(OutStr, Par);

                    }
                    else if (DT2.Rows.Count > 1)
                    {//有多条数据，先删除在添加
                        _Access.FastDel("galaxy", "GalaxySystem", DT.Rows[i]["GalaxySystem"].ToString());
                        _Access.ReutnSqlEcx(OutStr, Par);
                    }
                }
                catch (Exception ee)
                {
                    _Log.AddError("<InData>" + ee.Message);
                }
                Application.DoEvents();
                spPBar.Value = i + 1;
            }
        }

        public void IODataTest()
        {

        }

        /// <summary>
        /// Spy报告处理
        /// </summary>
        /// <param name="spSpy"></param>
        /// <param name="spGalaxySystem"></param>
        public void UpDataSpy(string spSpy, string spGalaxySystem)
        {

            string UpSpyStr = "UPDATE galaxy SET [Spy]='{0}' where  [GalaxySystem]='{1}'";
            _Access.TableSqlEcx(UpSpyStr, new string[] { spSpy, spGalaxySystem });
        }

        public string GetDataSpy(string spGalaxySystem)
        {
            try
            {

                string GetSpyStr = "SELECT galaxy.Spy FROM Galaxy where GalaxySystem ='{0}'";
                DataTable DT = _Access.TableSqlEcx(GetSpyStr, new string[] { spGalaxySystem });
                if (DT.Rows.Count == 1)
                {
                    return DT.Rows[0]["Spy"].ToString();
                }
                else
                {
                    return "";
                }

            }
            catch (Exception ee)
            {
                _Log.AddError("<GetDataSpy>" + ee.Message);
                return "";
            }
        }

        /// <summary>
        /// 设置读取
        /// </summary>
        /// <returns></returns>
        public void SetRelationsRead(ListBox spFriend, ListBox spEnemy, ListBox spUnion, ListBox spEnemyUnion)
        {
            DataTable DT = _Access.ShowAll("SetRelations");
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                if (DT.Rows[i]["Friend"].ToString() != "")
                    spFriend.Items.Add(DT.Rows[i]["Friend"].ToString());
                else if (DT.Rows[i]["Enemy"].ToString() != "")
                    spEnemy.Items.Add(DT.Rows[i]["Enemy"].ToString());
                if (DT.Rows[i]["Union"].ToString() != "")
                    spUnion.Items.Add(DT.Rows[i]["Union"].ToString());
                if (DT.Rows[i]["EnemyUnion"].ToString() != "")
                    spEnemyUnion.Items.Add(DT.Rows[i]["EnemyUnion"].ToString());
            }
        }

        public DataTable SetMy()
        {
            return _Access.ShowAll("SetMy");
        }

        public void SetWrite(ListBox spFriend, ListBox spEnemy, ListBox spUnion, ListBox spEnemyUnion, string[] spPar)
        {
            string InsStr = "INSERT INTO SetRelations ( [{0}] ) VALUES ( '{1}')";

            _Access.DelAll("SetRelations");
            int i;
            for (i = 0; i < spFriend.Items.Count; i++)
                _Access.ReutnSqlEcx(InsStr, new string[] { "Friend", spFriend.Items[i].ToString() });
            for (i = 0; i < spEnemy.Items.Count; i++)
                _Access.ReutnSqlEcx(InsStr, new string[] { "Enemy", spEnemy.Items[i].ToString() });
            for (i = 0; i < spUnion.Items.Count; i++)
                _Access.ReutnSqlEcx(InsStr, new string[] { "Union", spUnion.Items[i].ToString() });
            for (i = 0; i < spEnemyUnion.Items.Count; i++)
                _Access.ReutnSqlEcx(InsStr, new string[] { "EnemyUnion", spEnemyUnion.Items[i].ToString() });

            string MySetStr = "UPDATE SetMy SET SetMy.My = '{0}', SetMy.MyUnion = {1}, SetMy.Friend = {2}, SetMy.Enemy = {3}, SetMy.[Union] = {4}, SetMy.EnemyUnion = {5}, SetMy.i = {6}, SetMy.u = {7}, SetMy.g = {8}, SetMy.MyUnionColor = {9}, SetMy.FriendColor = {10}, SetMy.EnemyColor = {11}, SetMy.UnionColor = {12}, SetMy.EnemyUnionColor = {13}, SetMy.iColor = {14}, SetMy.uColor = {15}, SetMy.gColor = {16} , SetMy.GridBack = {17} , SetGridFore = {18},Custom1Color={19},Custom2Color={20};";
            _Access.ReutnSqlEcx(MySetStr, spPar);

        }

        /// <summary>
        /// 添加用户排名信息
        /// </summary>
        /// <param name="spRankings"></param>
        /// <param name="spTable"></param>
        public void OutRankings(Rankings[] spRankings, string spTable)
        {
            string OutStr = "INSERT INTO {0} ( UserName, R, C, S, [Date] ) VALUES ('{1}',{2}, {3}, {4} , '{5}')";
            string SelStr = "select ID,[Date] from {0} where UserName = '{1}' ORDER BY [ID]";


            for (int i = 0; i < 100; i++)
            {
                try
                {
                    DataTable DT = _Access.TableSqlEcx(SelStr, spTable, spRankings[i].Name);
                    if (DT.Rows.Count > 0)
                    {
                        if ((DateTime)DT.Rows[DT.Rows.Count - 1]["Date"] == Convert.ToDateTime(spRankings[i].Date))
                        {
                            continue;
                        }
                    }
                     if (DT.Rows.Count >= 30)
                    {
                        _Access.FastDel(spTable,"ID",Convert.ToInt32(DT.Rows[0]["ID"].ToString()));
                    }

                    string[] Par ={ spTable, spRankings[i].Name, spRankings[i].EesR.ToString(), spRankings[i].EesC.ToString(), spRankings[i].EesS.ToString(), spRankings[0].Date };
                    _Access.ReutnSqlEcx(OutStr, Par);
                }
                catch (Exception ee)
                {
                    _Log.AddError("<OutRankings>" + ee.Message);
                }
            }
        }

        /// <summary>
        /// 添加联盟
        /// </summary>
        /// <param name="spRankings"></param>
        /// <param name="spTable"></param>
        public void OutUnionRankings(Rankings[] spRankings, string spTable)
        {
            string OutStr = "INSERT INTO {0} ( [Union], R, C, S, [Date] ) VALUES ('{1}',{2}, {3}, {4} , '{5}')";
            string SelStr = "select ID,[Date] from {0} where [Union] = '{1}' ORDER BY [ID]";

            for (int i = 0; i < 100; i++)
            {
                try
                {
                    DataTable DT = _Access.TableSqlEcx(SelStr, spTable, spRankings[i].Name);
                    if (DT.Rows.Count > 0)
                    {
                        if ((DateTime)DT.Rows[DT.Rows.Count - 1]["Date"] == Convert.ToDateTime(spRankings[i].Date))
                        {
                            continue;
                        }
                    }
                 if (DT.Rows.Count >= 30)
                    {
                        _Access.FastDel(spTable, "ID", Convert.ToInt32(DT.Rows[0]["ID"].ToString()));
                    }

                    string[] Par ={ spTable, spRankings[i].Name, spRankings[i].EesR.ToString(), spRankings[i].EesC.ToString(), spRankings[i].EesS.ToString(), spRankings[0].Date };
                    _Access.ReutnSqlEcx(OutStr, Par);
                }
                catch (Exception ee)
                {
                    _Log.AddError("<OutUnionRankings>" + ee.Message);
                }
            }
        }

        /// <summary>
        /// 多一个成员名，所以说一种添加方式
        /// </summary>
        /// <param name="spRankings"></param>
        /// <param name="spTable"></param>
        public void OutUnionRankings(Rankings[] spRankings, int[] Members, string spTable)
        {
            string OutStr = "INSERT INTO {0} ( [Union], R, C, S,Members,  [Date]) VALUES ('{1}',{2}, {3}, {4} ,{5}, '{6}')";
            string SelStr = "select ID,[Date] from {0} where [Union] = '{1}' ORDER BY [ID]";

            for (int i = 0; i < 100; i++)
            {
                try
                {
                    DataTable DT = _Access.TableSqlEcx(SelStr, spTable, spRankings[i].Name);
                    if (DT.Rows.Count > 0)
                    {
                        if ((DateTime)DT.Rows[DT.Rows.Count - 1]["Date"] == Convert.ToDateTime(spRankings[i].Date))
                        {
                            continue;
                        }
                    }
                   if (DT.Rows.Count >= 30)
                    {
                        _Access.FastDel(spTable, "ID", Convert.ToInt32(DT.Rows[0]["ID"].ToString()));
                    }

                    string[] Par ={ spTable, spRankings[i].Name, spRankings[i].EesR.ToString(), spRankings[i].EesC.ToString(), spRankings[i].EesS.ToString(), Members[i].ToString(), spRankings[0].Date };
                    _Access.ReutnSqlEcx(OutStr, Par);
                }
                catch (Exception ee)
                {
                    _Log.AddError("<OutUnionRankings>" + ee.Message);
                }
            }
        }

        /// <summary>
        /// 在主数据库中更新用户排名
        /// </summary>
        /// <param name="spRankings"></param>
        public void UpDataRankings(Rankings[] spRankings)
        {
            string UpRankingsStr = "UPDATE galaxy SET galaxy.Rankings = {0} where galaxy.[Username] = '{1}';";

            for (int i = 0; i < 100; i++)
            {
                try
                {
                    _Access.ReutnSqlEcx(UpRankingsStr,spRankings[i].EesR.ToString(), spRankings[i].Name);
                }
                catch (Exception ee)
                {
                    _Log.AddError("<UpDataRankings>" + ee.Message);
                }
            }
        }

        /// <summary>
        /// 在主数据库中更新联盟排名
        /// </summary>
        /// <param name="spRankings"></param>
        public void UpDataUnionRankings(Rankings[] spRankings, int[] spMembers)
        {
            string UpRankingsStr = "UPDATE galaxy SET galaxy.UnionRankings = {0}, galaxy.Members = {1} where galaxy.[Union] = '{2}';";

            for (int i = 0; i < 100; i++)
            {
                try
                {
                    _Access.ReutnSqlEcx(UpRankingsStr, spRankings[i].EesR.ToString(), spMembers[i].ToString(), spRankings[i].Name);
                }
                catch (Exception ee)
                {
                    _Log.AddError("<UpDataRankings>" + ee.Message);
                }
            }
        }

        /// <summary>
        /// 积分历史记录
        /// </summary>
        /// <param name="spTable"></param>
        /// <param name="spName"></param>
        /// <returns></returns>
        public DataTable HistoryUserRankings(string spTable, string spName)
        {
            string SqlStr = "SELECT * FROM {0} where UserName = '{1}' ORDER BY [ID] DESC;";
            return _Access.TableSqlEcx(SqlStr, spTable, spName.Trim());
        }

        public DataTable HistoryUnionRankings(string spTable, string spUnion)
        {
            string SqlStr = "SELECT * FROM {0} where [Union] = '{1}' ORDER BY [ID] DESC;";
            return _Access.TableSqlEcx(SqlStr, spTable, spUnion.Trim());
        }

        public DataTable MetalCrystal()
        {
            string SqlStr = "SELECT galaxy.* FROM galaxy WHERE galaxy.Metal>0 OR galaxy.Crystal>0;";
           return  _Access.TableSqlEcx(SqlStr);

        }

        /// <summary>
        /// 标记颜色
        /// </summary>
        /// <param name="Sys">标记位置</param>
        public void Custom1Check(string Sys,bool value)
        {
            string SqlStr = "UPDATE galaxy SET galaxy.Custom1 = {1} where GalaxySystem ='{0}'";
            _Access.ReutnSqlEcx(SqlStr, Sys, Convert.ToString(!value));
        }

        public void Custom2Check(string Sys, bool value)
        {
            string SqlStr = "UPDATE galaxy SET galaxy.Custom2 = {1} where GalaxySystem ='{0}'";
            _Access.ReutnSqlEcx(SqlStr, Sys, Convert.ToString(!value));
        }
    }
}