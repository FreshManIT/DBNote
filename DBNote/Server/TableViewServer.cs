#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote.Server
//文件名称：TableViewServer
//创 建 人：FreshMan
//创建日期：2017/8/8 22:56:56
//用    途：记录类的用途
//======================================================================
#endregion

using System.Linq;
using DBNote.Enum;
using DBNote.Models;

namespace DBNote.Server
{
    /// <summary>
    /// 表，视图服务
    /// </summary>
    public static class TableViewServer
    {
        /// <summary>
        /// 获取Table的信息
        /// </summary>
        /// <param name="dataBaseName"></param>
        /// <param name="tableName"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static Table GetTableInfo(string dataBaseName, string tableName, DataBaseTypeEnum dbType)
        {
            var serverAdapter = new ServerAdapter(dbType);
            if (serverAdapter.ServerList == null || !serverAdapter.ServerList.Any()) return null;
            var baseServer = serverAdapter.ServerList.First();
            return baseServer.DbBaseTableAccess.GetTableInfo(baseServer.ConnectionString, dataBaseName, tableName);
        }

        /// <summary>
        /// 获取视图信息
        /// </summary>
        /// <param name="dataBaseName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static View GetViewInfo(string dataBaseName, string tableName, DataBaseTypeEnum dbType)
        {
            var serverAdapter = new ServerAdapter(dbType);
            if (serverAdapter.ServerList == null || !serverAdapter.ServerList.Any()) return null;
            var baseServer = serverAdapter.ServerList.First();
            return baseServer.DbBaseTableAccess.GetViewInfo(baseServer.ConnectionString, dataBaseName, tableName);
        }
    }
}