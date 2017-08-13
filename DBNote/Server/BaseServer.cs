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

using DBNote.Enum;
using DBNote.IDataBase;

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
        public IDataBaseTableAccess DbBaseTableAccess { get; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; }

        /// <summary>
        /// 当前数据库类型
        /// </summary>
        public DataBaseTypeEnum CurrentDataBaseTypeEnum { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="dbBaseTableAccessType"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public BaseServer(string connectionString, IDataBaseTableAccess dbBaseTableAccessType, DataBaseTypeEnum dbType)
        {
            ConnectionString = connectionString;
            DbBaseTableAccess = dbBaseTableAccessType;
            CurrentDataBaseTypeEnum = dbType;
        }
    }
}