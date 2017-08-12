#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote.Enum
//文件名称：DataBaseTypeEnum
//创 建 人：FreshMan
//创建日期：2017/8/12 20:51:05
//用    途：记录类的用途
//======================================================================
#endregion
using System;
using System.ComponentModel;

namespace DBNote.Enum
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    [Flags]
    public enum DataBaseTypeEnum
    {
        /// <summary>
        /// 未指定
        /// </summary>
        [Description("未指定数据库类型")]
        NotDesignated = 0,

        /// <summary>
        /// SqlServer数据库
        /// </summary>
        [Description("SqlServer数据库")]
        SqlServer = 1 << 0,

        /// <summary>
        /// MySql数据库
        /// </summary>
        [Description("MySql数据库")]
        MySql = 1 << 1,

        /// <summary>
        /// Sqlite数据库
        /// </summary>
        [Description("Sqlite数据库")]
        Sqlite = 1 << 2,
    }
}