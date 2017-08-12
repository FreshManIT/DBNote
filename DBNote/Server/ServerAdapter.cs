#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote.Server
//文件名称：ServerAdapter
//创 建 人：FreshMan
//创建日期：2017/8/12 20:44:27
//用    途：记录类的用途
//======================================================================
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBNote.DataBase;
using DBNote.Enum;
using FreshCommonUtility.Configure;

namespace DBNote.Server
{
    /// <summary>
    /// 服务适配器
    /// </summary>
    public class ServerAdapter
    {
        /// <summary>
        /// 服务列表
        /// </summary>
        public List<BaseServer> ServerList { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataBaseType"></param>
        public ServerAdapter(DataBaseTypeEnum dataBaseType)
        {
            if (dataBaseType == DataBaseTypeEnum.NotDesignated) return;
            ServerList = new List<BaseServer>();
            if ((dataBaseType & DataBaseTypeEnum.SqlServer) == DataBaseTypeEnum.SqlServer)
            {
                ServerList.Add(new BaseServer());
            }
            if ((dataBaseType & DataBaseTypeEnum.MySql) == DataBaseTypeEnum.MySql)
            {
                var dbBaseTableAccess = new MySqlDataAccess();
                var connectionString = AppConfigurationHelper.GetString("MySqlConnectionstring", null);
                const DataBaseTypeEnum currentDataBaseTypeEnum = DataBaseTypeEnum.MySql;
                ServerList.Add(new BaseServer(connectionString, dbBaseTableAccess, currentDataBaseTypeEnum));
            }
        }
    }
}