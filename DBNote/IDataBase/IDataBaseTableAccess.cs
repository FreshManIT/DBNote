#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote
//文件名称：IDataBaseTableAccess
//创 建 人：FreshMan
//创建日期：2017/8/7 23:23:43
//用    途：记录类的用途
//======================================================================
#endregion

using System.Collections.Generic;
using DBNote.Models;

namespace DBNote.IDataBase
{
    public interface IDataBaseTableAccess
    {
        /// <summary>
        /// 获取数据库列表
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        List<DataBaseModel> GetDataBaseModels(string connectionString);

        /// <summary>
        /// 获取数据库中的表
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        List<Table> GetTableList(string connectionstring, string databaseName);

        /// <summary>
        /// GetViews
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        List<View> GetViews(string connectionString, string databaseName);
    }
}
