#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote.Server
//文件名称：BaseServer
//创 建 人：FreshMan
//创建日期：2017/8/8 22:59:00
//用    途：记录类的用途
//======================================================================
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using DBNote.DataBase;
using DBNote.IDataBase;
using FreshCommonUtility.Configure;

namespace DBNote.Server
{
    /// <summary>
    /// 基础服务
    /// </summary>
    public class BaseServer
    {
        /// <summary>
        /// 数据库访问对象
        /// </summary>
        public static IDataBaseTableAccess DbBaseTableAccess;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString;

        /// <summary>
        /// 构造函数
        /// </summary>
        static BaseServer()
        {
            DbBaseTableAccess = new SqlDataBaseTableAccess();
            ConnectionString = AppConfigurationHelper.GetString("sqlconnectionstring", null);
        }

        /// <summary>
        /// 设置接口类型
        /// </summary>
        /// <param name="dbBaseTableAccessType"></param>
        /// <returns></returns>
        public static bool SetDbType(IDataBaseTableAccess dbBaseTableAccessType)
        {
            DbBaseTableAccess = dbBaseTableAccessType;
            return true;
        }

        /// <summary>
        /// 设置连接字符串
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static bool SetDbConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
            return true;
        }
    }
}