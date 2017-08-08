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
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using Dapper;
using DBNote.IDataBase;
using DBNote.Models;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;

namespace DBNote.DataBase
{
    public class SqlDataBaseTableAccess : IDataBaseTableAccess
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

        /// <summary>
        /// 获取数据库中的表
        /// </summary>
        /// <param name="connectionstring"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public List<Table> GetTableList(string connectionstring, string databaseName)
        {
            List<Table> tables = new List<Table>();

            string sqlCmd = $"select [name],[object_id] from [{databaseName}].sys.tables where type='U'";
            SqlDataReader dr = SqlHelper.ExecuteReader(connectionstring, CommandType.Text, sqlCmd);
            while (dr.Read())
            {
                string id = dr.GetString(0);
                string displayName = dr.GetString(0);
                string name = dr.GetString(0);
                string comment = string.Empty;
                int objectId = dr.GetInt32(1);

                Table table = new Table(id, displayName, name, comment)
                {
                    OriginalName = name,
                    Columns = GetColumns(objectId, connectionstring)
                };
                table.PrimaryKeys = GetPrimaryKeys(objectId, connectionstring, table.Columns);
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
            List<View> views = new List<View>();

            string sqlCmd = $"select [name],[object_id] from [{databaseName}].sys.views where type='V'";
            SqlDataReader dr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlCmd);
            while (dr.Read())
            {
                string id = dr.GetString(0);
                string displayName = dr.GetString(0);
                string name = dr.GetString(0);
                string comment = string.Empty;
                int objectId = dr.GetInt32(1);

                View view = new View(id, displayName, name, comment)
                {
                    OriginalName = name,
                    Columns = GetColumns(objectId, connectionString)
                };
                views.Add(view);
            }
            dr.Close();

            return views;
        }

        /// <summary>
        /// GetColumns
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private Columns GetColumns(int objectId, string connectionString)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select c.object_id,c.column_id,c.name,c.max_length,c.is_identity,c.is_nullable,c.is_computed,");
            sqlBuilder.Append("t.name as type_name,p.value as description,d.definition as default_value ");
            sqlBuilder.Append("from sys.columns as c ");
            sqlBuilder.Append("inner join sys.types as t on c.user_type_id =  t.user_type_id ");
            sqlBuilder.Append("left join sys.extended_properties as p on p.major_id = c.object_id and p.minor_id = c.column_id ");
            sqlBuilder.Append("left join sys.default_constraints as d on d.parent_object_id = c.object_id and d.parent_column_id = c.column_id ");
            sqlBuilder.AppendFormat("where c.object_id={0}", objectId);

            return GetColumns(connectionString, sqlBuilder.ToString());
        }

        /// <summary>
        /// GetPrimaryKeys
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="connectionString"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        private Columns GetPrimaryKeys(int objectId, string connectionString, Columns columns)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select syscolumns.name from syscolumns,sysobjects,sysindexes,sysindexkeys ");
            sqlBuilder.AppendFormat("where syscolumns.id ={0} ", objectId);
            sqlBuilder.Append("and sysobjects.xtype = 'PK' and sysobjects.parent_obj = syscolumns.id ");
            sqlBuilder.Append("and sysindexes.id = syscolumns.id and sysobjects.name = sysindexes.name and ");
            sqlBuilder.Append("sysindexkeys.id = syscolumns.id and sysindexkeys.indid = sysindexes.indid and syscolumns.colid = sysindexkeys.colid");

            Columns primaryKeys = new Columns(4);
            SqlDataReader dr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlBuilder.ToString());
            while (dr.Read())
            {
                string name = dr.IsDBNull(0) ? string.Empty : dr.GetString(0);
                if (columns.ContainsKey(name)) primaryKeys.Add(name, columns[name]);
            }
            dr.Close();

            return primaryKeys;
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
            SqlDataReader dr = SqlHelper.ExecuteReader(connectionString, CommandType.Text, sqlCmd);
            while (dr.Read())
            {
                string id = dr.IsDBNull(2) ? string.Empty : dr.GetString(2);
                string displayName = dr.IsDBNull(2) ? string.Empty : dr.GetString(2);
                string name = dr.IsDBNull(2) ? string.Empty : dr.GetString(2);
                int length = dr.IsDBNull(3) ? 0 : dr.GetInt16(3);
                bool identity = !dr.IsDBNull(4) && dr.GetBoolean(4);
                bool isNullable = !dr.IsDBNull(5) && dr.GetBoolean(5);
                bool isComputed = !dr.IsDBNull(6) && dr.GetBoolean(6);
                string dataType = dr.IsDBNull(7) ? string.Empty : dr.GetString(7);
                string comment = dr.IsDBNull(8) ? string.Empty : dr.GetString(8);
                string defaultValue = dr.IsDBNull(9) ? string.Empty : dr.GetString(9);

                Column column = new Column(id, displayName, name, dataType, comment)
                {
                    Length = length,
                    IsAutoIncremented = identity,
                    IsNullable = isNullable,
                    DefaultValue = defaultValue,
                    DataType = dataType,
                    OriginalName = name,
                    IsComputed = isComputed
                };
                columns.Add(id, column);
            }
            dr.Close();

            return columns;
        }
    }
}