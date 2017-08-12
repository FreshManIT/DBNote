#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote.DataBase
//文件名称：MySqlDataAccess
//创 建 人：FreshMan
//创建日期：2017/8/12 20:35:56
//用    途：记录类的用途
//======================================================================
#endregion

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using DBNote.IDataBase;
using DBNote.Models;
using DBNote.Util;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.DataConvert;
using FreshCommonUtility.SqlHelper;
using MySql.Data.MySqlClient;

namespace DBNote.DataBase
{
    public class MySqlDataAccess : IDataBaseTableAccess
    {
        /// <summary>
        /// 获取数据库列表
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public List<DataBaseModel> GetDataBaseModels(string connectionString)
        {
            var searchSql = " SELECT `SCHEMA_NAME` as name FROM `information_schema`.`SCHEMATA` where SCHEMA_NAME!= 'sys' and SCHEMA_NAME!='information_schema' ";
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
            var connection = SqlConnectionHelper.GetOpenConnection(connectionString);
            var dataBaseList = connection.Query<DataBaseModel>(searchSql);
            return dataBaseList.ToList();
        }

        /// <summary>
        /// 获取数据库中的表
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public List<Table> GetTableList(string connectionstring, string databaseName)
        {
            var connBuilder = BuildConnection(connectionstring);

            List<Table> tables = new List<Table>();

            string sqlCmd = $"SELECT TABLE_NAME, TABLE_COMMENT FROM TABLES WHERE TABLE_SCHEMA = '{databaseName}'";
            MySqlDataReader dr = MySqlHelper.ExecuteReader(connBuilder.ConnectionString, sqlCmd);
            while (dr.Read())
            {
                string id = dr.GetString(0);
                string displayName = dr.GetString(0);
                string name = dr.GetString(0);
                string comment = dr.IsDBNull(1) ? string.Empty : dr.GetString(1);

                Table table = new Table(id, displayName, name, comment)
                {
                    OriginalName = name
                };
                tables.Add(table);
            }
            dr.Close();

            return tables;
        }

        /// <summary>
        /// GetViews
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public List<View> GetViews(string connectionString, string databaseName)
        {
            var connBuilder = BuildConnection(connectionString);
            List<View> views = new List<View>();

            string sqlCmd = $"SELECT TABLE_NAME FROM VIEWS WHERE TABLE_SCHEMA = '{databaseName}'";
            MySqlDataReader dr = MySqlHelper.ExecuteReader(connBuilder.ConnectionString, sqlCmd);
            while (dr.Read())
            {
                string id = dr.GetString(0);
                string displayName = dr.GetString(0);
                string name = dr.GetString(0);
                string comment = string.Empty;

                View view = new View(id, displayName, name, comment)
                {
                    OriginalName = name
                };
                views.Add(view);
            }
            dr.Close();

            return views;
        }

        /// <summary>
        /// 获取表的字段
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <param name="databaseName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Table GetTableInfo(string connectionstring, string databaseName, string tableName)
        {
            var connBuilder = BuildConnection(connectionstring);
            var tableList = GetTableList(connBuilder.ConnectionString, databaseName);
            if (tableList == null || !tableList.Any()) return null;
            var table = tableList.FirstOrDefault(f => f.Name == tableName);
            if (table == null) return null;
            table.Columns = GetColumns(table.Name, connBuilder.ConnectionString, databaseName);
            table.PrimaryKeys = GetPrimaryKeys(table.Name, connBuilder.ConnectionString, databaseName);
            return table;
        }

        /// <summary>
        /// 获取表的字段
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <param name="databaseName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public View GetViewInfo(string connectionstring, string databaseName, string tableName)
        {
            var connBuilder = BuildConnection(connectionstring);
            var tableList = GetViews(connBuilder.ConnectionString, databaseName);
            if (tableList == null || !tableList.Any()) return null;
            var table = tableList.FirstOrDefault(f => f.Name == tableName);
            if (table == null) return null;
            table.Columns = GetColumns(table.Name, connBuilder.ConnectionString, databaseName);
            return table;
        }

        /// <summary>
        /// 修改连接的数据库
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <returns></returns>
        private MySqlConnectionStringBuilder BuildConnection(string connectionstring)
        {
            MySqlConnectionStringBuilder connBuilder = new MySqlConnectionStringBuilder(connectionstring);
            connBuilder.Database = "information_schema";
            return connBuilder;
        }

        /// <summary>
        /// GetColumns
        /// </summary>
        /// <param name="tableOrViewName"></param>
        /// <param name="connectionString"></param>
        /// <param name="dataBaseName"></param>
        /// <returns></returns>
        private Columns GetColumns(string tableOrViewName, string connectionString, string dataBaseName)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT TABLE_SCHEMA,TABLE_NAME,COLUMN_NAME,DATA_TYPE,COLUMN_KEY,COLUMN_DEFAULT, ");
            sqlBuilder.Append("IS_NULLABLE,CHARACTER_MAXIMUM_LENGTH,EXTRA,COLUMN_COMMENT ");
            sqlBuilder.Append("FROM COLUMNS ");
            sqlBuilder.AppendFormat("WHERE TABLE_SCHEMA = '{0}' AND TABLE_NAME ='{1}' ", dataBaseName, tableOrViewName);

            return GetColumns(connectionString, sqlBuilder.ToString());
        }

        /// <summary>
        /// GetPrimaryKeys
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="connectionString"></param>
        /// <param name="originalDbName"></param>
        /// <returns></returns>
        private Columns GetPrimaryKeys(string tableName, string connectionString, string originalDbName)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT TABLE_SCHEMA,TABLE_NAME,COLUMN_NAME,DATA_TYPE,COLUMN_KEY,COLUMN_DEFAULT, ");
            sqlBuilder.Append("IS_NULLABLE,CHARACTER_MAXIMUM_LENGTH,EXTRA,COLUMN_COMMENT ");
            sqlBuilder.Append("FROM COLUMNS ");
            sqlBuilder.AppendFormat("WHERE TABLE_SCHEMA = '{0}' AND TABLE_NAME ='{1}' AND COLUMN_KEY='PRI'", originalDbName, tableName);

            return GetColumns(connectionString, sqlBuilder.ToString());
        }

        /// <summary>
        /// GetColumns
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sqlCmd"></param>
        /// <returns></returns>
        private Columns GetColumns(string connectionString, string sqlCmd)
        {
            Columns columns = new Columns(50);
            MySqlDataReader dr = MySqlHelper.ExecuteReader(connectionString, sqlCmd);
            while (dr.Read())
            {
                string id = dr.IsDBNull(2) ? string.Empty : dr.GetString(2);
                string displayName = dr.IsDBNull(2) ? string.Empty : dr.GetString(2);
                string name = dr.IsDBNull(2) ? string.Empty : dr.GetString(2);
                string dataType = dr.IsDBNull(3) ? string.Empty : dr.GetString(3);
                //string key = dr.IsDBNull(4) ? string.Empty : dr.GetString(4);
                string defaultValue = dr.IsDBNull(5) ? string.Empty : dr.GetString(5);
                string isNullable = dr.IsDBNull(6) ? string.Empty : dr.GetString(6);
                string length = dr.IsDBNull(7) ? string.Empty : dr.GetString(7);
                string identity = dr.IsDBNull(8) ? string.Empty : dr.GetString(8);
                string comment = dr.IsDBNull(9) ? string.Empty : dr.GetString(9);

                Column column = new Column(id, displayName, name, dataType, comment);
                column.Length = DataTypeConvertHelper.ToInt(length);
                column.IsAutoIncremented = identity.Equals("auto_increment");
                column.IsNullable = isNullable.Equals("YES");
                column.DefaultValue = defaultValue.ToEmpty();
                column.DataType = dataType;
                column.OriginalName = name;
                columns.Add(id, column);
            }
            dr.Close();

            return columns;
        }
    }
}