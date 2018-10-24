#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote.Server
//文件名称：ServerAdapter
//创 建 人：FreshMan
//创建日期：2017/8/12 20:44:27
//用    途：记录类的用途
//======================================================================
#endregion
using System.Collections.Generic;
using System.Linq;
using DBNote.DataBase;
using DBNote.Enum;

namespace DBNote.Server
{
    /// <summary>
    /// 服务适配器
    /// </summary>
    public class ServerAdapter
    {
        /// <summary>
        /// 服务列表
        /// </summary>
        public List<BaseServer> ServerList { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataBaseType"></param>
        public ServerAdapter(DataBaseTypeEnum dataBaseType)
        {
            var config = new SqliteDataAccess().GetConfigModels();
            if (config == null || !config.Any()) return;
            if (dataBaseType == DataBaseTypeEnum.NotDesignated) return;
            ServerList = new List<BaseServer>();

            if ((dataBaseType & DataBaseTypeEnum.SqlServer) == DataBaseTypeEnum.SqlServer)
            {
                var tempConfig = config.FirstOrDefault(f => f.DbType == DataBaseTypeEnum.SqlServer && f.IsEnable==IsEnableEnum.Enable);
                if (!string.IsNullOrEmpty(tempConfig?.LinkConnectionString))
                {
                    ServerList.Add(new BaseServer(tempConfig.LinkConnectionString, new SqlServerDataAccess(), DataBaseTypeEnum.SqlServer));
                }
            }
            if ((dataBaseType & DataBaseTypeEnum.MySql) == DataBaseTypeEnum.MySql)
            {
                var tempConfig = config.FirstOrDefault(f => f.DbType == DataBaseTypeEnum.MySql && f.IsEnable == IsEnableEnum.Enable);
                if (!string.IsNullOrEmpty(tempConfig?.LinkConnectionString))
                {
                    ServerList.Add(new BaseServer(tempConfig.LinkConnectionString, new MySqlDataAccess(), DataBaseTypeEnum.MySql));
                }
            }
            if((dataBaseType & DataBaseTypeEnum.Oracle) == DataBaseTypeEnum.Oracle)
            {
                var tempConfig = config.FirstOrDefault(f => f.DbType == DataBaseTypeEnum.Oracle && f.IsEnable == IsEnableEnum.Enable);
                if (!string.IsNullOrEmpty(tempConfig?.LinkConnectionString))
                {
                    ServerList.Add(new BaseServer(tempConfig.LinkConnectionString, new OracleDataAccess(), DataBaseTypeEnum.Oracle));
                }
            }
        }
    }
}