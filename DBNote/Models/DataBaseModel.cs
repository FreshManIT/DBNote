#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote.Models
//文件名称：DataBaseModel
//创 建 人：FreshMan
//创建日期：2017/8/7 23:04:20
//用    途：记录类的用途
//======================================================================
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBNote.Models
{
    public class DataBaseModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Database name
        /// </summary>
        public string Name { get; set; }
    }
}