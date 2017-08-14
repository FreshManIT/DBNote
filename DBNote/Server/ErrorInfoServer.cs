#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote.Server
//文件名称：ErrorInfoServer
//创 建 人：FreshMan
//创建日期：2017/8/14 20:30:10
//用    途：记录类的用途
//======================================================================
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBNote.Models;

namespace DBNote.Server
{
    public static class ErrorInfoServer
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        static ErrorInfoServer()
        {
            ErrorInfoList = new List<ErrorModel>();
        }

        /// <summary>
        /// 错误列表
        /// </summary>
        private static List<ErrorModel> ErrorInfoList { get; set; }

        /// <summary>
        /// push错误
        /// </summary>
        /// <param name="ex"></param>
        public static void Push(Exception ex)
        {
            if (ex == null) return;
            ErrorInfoList.Add(new ErrorModel { ErrorType = ex.GetType().ToString(), ErrorStack = ex.Message });
        }

        /// <summary>
        /// 获得错误信息
        /// </summary>
        /// <returns></returns>
        public static List<ErrorModel> GetErrorModels()
        {
            return ErrorInfoList;
        } 
    }
}