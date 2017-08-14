#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote.Models
//文件名称：ErrorModel
//创 建 人：FreshMan
//创建日期：2017/8/14 20:31:11
//用    途：记录类的用途
//======================================================================
#endregion

using System;

namespace DBNote.Models
{
    public class ErrorModel
    {
        /// <summary>
        /// 错误类型
        /// </summary>
        public string ErrorType { get; set; }

        /// <summary>
        /// 错误堆栈
        /// </summary>
        public string ErrorStack { get; set; }

        /// <summary>
        /// 错误时间
        /// </summary>
        public string ErrorTime => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}