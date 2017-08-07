#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote.DataBase
//文件名称：DataBaseTableAccess
//创 建 人：FreshMan
//创建日期：2017/8/7 22:51:21
//用    途：记录类的用途
//======================================================================
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using DBNote.IDataBase;
using DBNote.Models;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;

namespace DBNote.DataBase
{
    public class DataBaseTableAccess : IDataBaseTableAccess
    {
        /// <summary>
        /// 获取数据库列表
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public List<DataBaseModel> GetDataBaseModels(string connectionString)
        {
            var searchSql = " select name from master..sysdatabases where name!= 'master' ";
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLServer);
            var connection = SqlConnectionHelper.GetOpenConnection(connectionString);
            var dataBaseList = connection.Query<DataBaseModel>(searchSql);
            return dataBaseList.ToList();
        }
    }
}