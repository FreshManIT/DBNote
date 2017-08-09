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

using DBNote.Models;

namespace DBNote.Server
{
    /// <summary>
    /// 表，视图服务
    /// </summary>
    public class TableViewServer
    {
        /// <summary>
        /// 获取Table的信息
        /// </summary>
        /// <param name="dataBaseName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static Table GetTableInfo(string dataBaseName, string tableName)
        {
            return BaseServer.DbBaseTableAccess.GetTableInfo(BaseServer.ConnectionString, dataBaseName, tableName);
        }

        /// <summary>
        /// 获取视图信息
        /// </summary>
        /// <param name="dataBaseName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static View GetViewInfo(string dataBaseName, string tableName)
        {
            return BaseServer.DbBaseTableAccess.GetViewInfo(BaseServer.ConnectionString, dataBaseName, tableName);
        }
    }
}