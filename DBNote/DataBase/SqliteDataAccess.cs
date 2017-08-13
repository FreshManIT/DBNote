#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote.DataBase
//文件名称：SqliteDataAccess
//创 建 人：FreshMan
//创建日期：2017/8/13 11:32:10
//用    途：记录类的用途
//======================================================================
#endregion
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Web;
using Dapper;
using DBNote.IDataBase;
using DBNote.Models;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.DataConvert;
using FreshCommonUtility.SqlHelper;

namespace DBNote.DataBase
{
    public class SqliteDataAccess : IDataBaseTableAccess
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        private readonly string _filePath;

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SqliteDataAccess()
        {
            _filePath = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath + "\\MyDatabase.sqlite");
            ConnectionString = $"Data Source={_filePath};Version=3;";
            CreateSqliteDb();
        }
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <returns></returns>
        public bool CreateSqliteDb()
        {
            if (!File.Exists(_filePath))
            {
                SQLiteConnection.CreateFile(_filePath);
            }
            var connection = new SQLiteConnection(ConnectionString);
            using (connection)
            {
                connection.Open();
                var tableCount = connection.ExecuteScalar(
                    " SELECT COUNT(*) FROM sqlite_master where type='table' and name='DataBaseLinkConfigModel' ");
                if (DataTypeConvertHelper.ToInt(tableCount) == 1) return true;
                connection.Execute(@" CREATE TABLE DataBaseLinkConfigModel (
                                            Id  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                            DbType  INTEGER,
                                            LinkName  TEXT,
                                            LinkConnectionString  TEXT
                                            ); ");

            }
            return true;
        }

        /// <summary>
        /// 查询配置文件信息
        /// </summary>
        /// <returns></returns>
        public List<DataBaseLinkConfigModel> GetConfigModels()
        {
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLite);
            var conn = SqlConnectionHelper.GetOpenConnection(ConnectionString);
            var result = conn.GetList<DataBaseLinkConfigModel>();
            return result.ToList();
        }

        /// <summary>
        /// 更新配置文件
        /// </summary>
        /// <param name="newModel"></param>
        /// <returns></returns>
        public bool UpdateConfigModel(DataBaseLinkConfigModel newModel)
        {
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLite);
            var conn = SqlConnectionHelper.GetOpenConnection(ConnectionString);
            var result = conn.Update(newModel);
            return result > 0;
        }

        /// <summary>
        /// 查询指定条件配置信息
        /// </summary>
        /// <param name="linkName"></param>
        /// <returns></returns>
        public DataBaseLinkConfigModel GetConfigModelByLinkName(string linkName)
        {
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLite);
            var conn = SqlConnectionHelper.GetOpenConnection(ConnectionString);
            var result = conn.Get<DataBaseLinkConfigModel>($" where LinkName={linkName} ");
            return result;
        }

        /// <summary>
        /// 删除配置文件
        /// </summary>
        /// <param name="deletModel"></param>
        /// <returns></returns>
        public bool DeleteConfigModel(DataBaseLinkConfigModel deletModel)
        {
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLite);
            var conn = SqlConnectionHelper.GetOpenConnection(ConnectionString);
            var result = conn.Delete(deletModel);
            return result > 0;
        }

        public List<DataBaseModel> GetDataBaseModels(string connectionString)
        {
            throw new NotImplementedException();
        }

        public List<Table> GetTableList(string connectionstring, string databaseName)
        {
            throw new NotImplementedException();
        }

        public List<View> GetViews(string connectionString, string databaseName)
        {
            throw new NotImplementedException();
        }

        public Table GetTableInfo(string connectionstring, string databaseName, string tableName)
        {
            throw new NotImplementedException();
        }

        public View GetViewInfo(string connectionstring, string databaseName, string tableName)
        {
            throw new NotImplementedException();
        }
    }
}