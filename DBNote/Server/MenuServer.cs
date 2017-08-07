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
            AppSettings = ConfigurationManager.AppSettings;
            var resultMenu = new List<MenuModel>();

            var tempDataBase = new MenuModel { Menuname = "DB Server", Menuid = 1, Icon = "icon-sys" };
            var childMenu = new List<MenuModel>()
            {
                new MenuModel {Menuid = 12,Menuname = "FreshMan",Icon = "icon-add",Url = "http://www.baidu.com"},
                new MenuModel {Menuid = 13,Menuname = "拖放",Icon = "icon-users",Url = "draggable.html"}
            };
            tempDataBase.Menus = childMenu;
            resultMenu.Add(tempDataBase);
            SetDbType(new DataBaseTableAccess());
            var connectionString = AppConfigurationHelper.GetString("sqlconnectionstring", null);
            var dbList = dbBaseTableAccess.GetDataBaseModels(connectionString);
            if (dbList != null && dbList.Any())
            {
                var i = 2;
                dbList.ForEach(f =>
                {
                    resultMenu.Add(new MenuModel { Menuid = i++, Menuname = f.Name, Icon = "icon-sys", Menus = childMenu });
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