#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote.Models
//文件名称：BaseResultModel
//创 建 人：FreshMan
//创建日期：2017/8/13 21:20:22
//用    途：记录类的用途
//======================================================================
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBNote.Models
{
    /// <summary>
    /// 返回对象
    /// </summary>
    public class BaseResultModel
    {
        /// <summary>
        /// 返回说明
        /// </summary>
        public string Des { get; set; }

        /// <summary>
        /// 返回code
        /// </summary>
        public string Code { get; set; }
    }
}