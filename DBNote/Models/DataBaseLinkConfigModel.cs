#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote.Models
//文件名称：DataBaseLinkConfigModel
//创 建 人：FreshMan
//创建日期：2017/8/13 11:33:49
//用    途：记录类的用途
//======================================================================
#endregion

using DBNote.Enum;
using FreshCommonUtility.Dapper;

namespace DBNote.Models
{
    /// <summary>
    /// 数据库配置文件
    /// </summary>
    public class DataBaseLinkConfigModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseTypeEnum DbType { get; set; }

        /// <summary>
        /// 连接名称
        /// </summary>
        public string LinkName { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string LinkConnectionString { get; set; }
    }
}