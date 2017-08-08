#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote.Server
//文件名称：MenuServer
//创 建 人：FreshMan
//创建日期：2017/8/7 22:27:52
//用    途：记录类的用途
//======================================================================
#endregion
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using DBNote.DataBase;
using DBNote.IDataBase;
using DBNote.Models;
using FreshCommonUtility.Configure;

namespace DBNote.Server
{
    /// <summary>
    /// Menu server
    /// </summary>
    public class MenuServer
    {
        /// <summary>
        /// 数据库访问对象
        /// </summary>
        private static IDataBaseTableAccess dbBaseTableAccess;

        private static NameValueCollection AppSettings;
        /// <summary>
        /// Get Menu info.
        /// </summary>
        /// <returns></returns>
        public static MenuModel GetMenuInfo()
        {
            var resultMenu = new List<MenuModel>();

            SetDbType(new SqlDataBaseTableAccess());
            var connectionString = AppConfigurationHelper.GetString("sqlconnectionstring", null);
            var dbList = dbBaseTableAccess.GetDataBaseModels(connectionString);
            if (dbList != null && dbList.Any())
            {
                var i = 2;
                dbList.ForEach(f =>
                {
                    var childMenu = new List<MenuModel>();
                    var tables = dbBaseTableAccess.GetTableList(connectionString, f.Name);
                    var views = dbBaseTableAccess.GetViews(connectionString, f.Name);
                    if (tables != null && tables.Any())
                    {
                        childMenu.AddRange(tables.Select(r => new MenuModel { Menuid = i++, Icon = "icon-table",Menuname = r.Name}).ToList());
                    }

                    if (views != null && views.Any())
                    {
                        childMenu.AddRange(views.Select(r => new MenuModel { Menuid = i++, Icon = "icon-table", Menuname = r.Name }).ToList());
                    }
                    resultMenu.Add(new MenuModel { Menuid = i++, Menuname = f.Name, Icon = "icon-database", Menus = childMenu });
                });
            }

            return new MenuModel { Menus = resultMenu };
        }

        /// <summary>
        /// 设置接口类型
        /// </summary>
        /// <param name="dbBaseTableAccessType"></param>
        /// <returns></returns>
        public static bool SetDbType(IDataBaseTableAccess dbBaseTableAccessType)
        {
            dbBaseTableAccess = dbBaseTableAccessType;
            return true;
        }
    }
}