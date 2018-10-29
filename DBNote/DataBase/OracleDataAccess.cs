using Dapper;
using DBNote.IDataBase;
using DBNote.Models;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FreshCommonUtility.DataConvert;
using DBNote.Util;

namespace DBNote.DataBase
{
    public class OracleDataAccess : IDataBaseTableAccess
    {
        /// <summary>
        /// 获取数据库列表
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public List<DataBaseModel> GetDataBaseModels(string connectionString)
        {
            string host = "";
            if (string.IsNullOrEmpty(connectionString)) return null;
            if (connectionString.Contains("HOST"))
            {
                int index = connectionString.IndexOf("HOST")+5;
                if (index >-1)
                {
                    connectionString = connectionString.Substring(index);
                    int first = connectionString.IndexOf(")");
                    if (first > -1)
                    {
                        host = connectionString.Substring(0, first);
                    }
                }
                index = connectionString.IndexOf("User ID") + 8;
                if (index > -1)
                {
                    connectionString = connectionString.Substring(index);
                    int first = connectionString.IndexOf(";");
                    if (first > -1)
                    {
                        host += connectionString.Substring(0, first);
                    }
                }
            }
            var dataBaseList = new List<DataBaseModel>() { new DataBaseModel { Name = host } };
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
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.Oracle);
            using (IDbConnection conn = SqlConnectionHelper.GetOpenConnection(connectionstring))
            {
                List<Table> tables = new List<Table>();
                string sqlCmd = "SELECT table_name,cluster_name FROM user_tables";
                var dr = conn.ExecuteReader(sqlCmd);
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
        }

        /// <summary>
        /// GetViews
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public List<View> GetViews(string connectionString, string databaseName)
        {
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.Oracle);
            using (IDbConnection conn = SqlConnectionHelper.GetOpenConnection(connectionString))
            {
                List<View> views = new List<View>();
                string sqlCmd = $"select view_name,editioning_view from user_views";
                var dr = conn.ExecuteReader(sqlCmd);
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
            var tableList = GetTableList(connectionstring, databaseName);
            if (tableList == null || !tableList.Any()) return null;
            var table = tableList.FirstOrDefault(f => f.Name == tableName);
            if (table == null) return null;
            table.Columns = GetColumns(table.Name, connectionstring, databaseName);
            table.PrimaryKeys = GetPrimaryKeys(table.Name, connectionstring, databaseName);
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
            var tableList = GetViews(connectionstring, databaseName);
            if (tableList == null || !tableList.Any()) return null;
            var table = tableList.FirstOrDefault(f => f.Name == tableName);
            if (table == null) return null;
            table.Columns = GetColumns(table.Name, connectionstring, databaseName);
            return table;
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
            sqlBuilder.Append(@"SELECT
    utc.table_name,
    utc.COLUMN_ID,
    utc.column_name,
    utc.data_type,
    utc.data_length,
    utc.DATA_PRECISION,
    utc.NULLABLE,
    utc.data_length,
    utc.data_default,
    UCC.comments
FROM
    user_tab_columns utc
LEFT JOIN user_col_comments ucc ON utc.table_name = ucc.table_name
AND utc.column_name = ucc.column_name ");
            sqlBuilder.AppendFormat(@"
WHERE UTC.table_name = '{0}'
ORDER BY
    table_name,
    COLUMN_ID ", tableOrViewName);

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
            sqlBuilder.Append(@"select  col.*
from user_constraints con, user_cons_columns col
where
con.constraint_name = col.constraint_name and con.constraint_type = 'P' ");
            sqlBuilder.AppendFormat(" and col.table_name = '{0}' ", tableName);
            return GetKeys(connectionString, sqlBuilder.ToString());
        }

        /// <summary>
        /// GetColumns
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sqlCmd"></param>
        /// <returns></returns>
        private Columns GetColumns(string connectionString, string sqlCmd)
        {
            Columns columns = new Columns(500);
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.Oracle);
            using (var conn = SqlConnectionHelper.GetOpenConnection(connectionString))
            {
                var dr = conn.ExecuteReader(sqlCmd);
                while (dr.Read())
                {
                    string id = dr.IsDBNull(2) ? string.Empty : dr.GetString(2);
                    string displayName = dr.IsDBNull(2) ? string.Empty : dr.GetString(2);
                    string name = dr.IsDBNull(2) ? string.Empty : dr.GetString(2);
                    string dataType = dr.IsDBNull(3) ? string.Empty : dr.GetString(3);
                    string defaultValue = dr.IsDBNull(8) ? string.Empty : dr.GetValue(8).ToString();
                    string isNullable = dr.IsDBNull(6) ? string.Empty : dr.GetString(6);
                    string length = dr.IsDBNull(7) ? string.Empty : dr.GetValue(7).ToString();
                    string identity =  string.Empty;
                    string comment = dr.IsDBNull(9) ? string.Empty : dr.GetValue(9).ToString();

                    Column column = new Column(id, displayName, name, dataType, comment);
                    column.Length = DataTypeConvertHelper.ToInt(length);
                    column.IsAutoIncremented = false;
                    column.IsNullable = isNullable.Equals("Y");
                    column.DefaultValue = defaultValue.ToEmpty();
                    column.DataType = dataType;
                    column.OriginalName = name;
                    columns.Add(id, column);
                }
                dr.Close();

                return columns;
            }
        }

        /// <summary>
        /// GetKeys
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sqlCmd"></param>
        /// <returns></returns>
        private Columns GetKeys(string connectionString, string sqlCmd)
        {
            Columns columns = new Columns(5);
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.Oracle);
            using (var conn = SqlConnectionHelper.GetOpenConnection(connectionString))
            {
                var dr = conn.ExecuteReader(sqlCmd);
                while (dr.Read())
                {
                    string id = dr.IsDBNull(3) ? string.Empty : dr.GetValue(3).ToString();
                    string displayName = dr.IsDBNull(3) ? string.Empty : dr.GetValue(3).ToString();
                    string name = dr.IsDBNull(3) ? string.Empty : dr.GetValue(3).ToString();
                    string dataType = "";
                    string defaultValue = "";
                    string isNullable = "Y";
                    string length = "";
                    string identity = string.Empty;
                    string comment = "主键";

                    Column column = new Column(id, displayName, name, dataType, comment);
                    column.Length = DataTypeConvertHelper.ToInt(length);
                    column.IsAutoIncremented = false;
                    column.IsNullable = isNullable.Equals("Y");
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
}