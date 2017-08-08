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

using System.Collections.Generic;
using System.Linq;
using DBNote.Models;

namespace DBNote.Server
{
    /// <summary>
    /// Menu server
    /// </summary>
    public class MenuServer
    {
        /// <summary>
        /// Get Menu info.
        /// </summary>
        /// <returns></returns>
        public static MenuModel GetMenuInfo()
        {
            var resultMenu = new List<MenuModel>();
            var dbList = BaseServer.DbBaseTableAccess.GetDataBaseModels(BaseServer.ConnectionString);
            if (dbList != null && dbList.Any())
            {
                var i = 2;
                dbList.ForEach(f =>
                {
                    var childMenu = new List<MenuModel>();
                    var tables = BaseServer.DbBaseTableAccess.GetTableList(BaseServer.ConnectionString, f.Name);
                    var views = BaseServer.DbBaseTableAccess.GetViews(BaseServer.ConnectionString, f.Name);
                    if (tables != null && tables.Any())
                    {
                        childMenu.AddRange(tables.Select(r => new MenuModel { Menuid = i++, Icon = "icon-table", Menuname = r.Name, Url = $"/home/ShowTableInfo?datatableName={f.Name}&tableName={r.Name}&type=table" }).ToList());
                    }

                    if (views != null && views.Any())
                    {
                        childMenu.AddRange(views.Select(r => new MenuModel { Menuid = i++, Icon = "icon-table", Menuname = r.Name, Url = $"/home/ShowTableInfo?datatableName={f.Name}&tableName={r.Name}&type=view" }).ToList());
                    }
                    resultMenu.Add(new MenuModel { Menuid = i++, Menuname = f.Name, Icon = "icon-database", Menus = childMenu });
                });
            }

            return new MenuModel { Menus = resultMenu };
        }
    }
}