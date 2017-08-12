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

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DBNote.Enum;
using DBNote.Models;
using WebGrease.Css.Extensions;

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
            var serverAdapter = new ServerAdapter(DataBaseTypeEnum.MySql | DataBaseTypeEnum.SqlServer);
            if (serverAdapter.ServerList == null || !serverAdapter.ServerList.Any()) return null;

            var resultMenu = new ConcurrentBag<MenuModel>();
            serverAdapter.ServerList.AsParallel().ForEach(baseserver =>
            {
                var dbList = baseserver.DbBaseTableAccess.GetDataBaseModels(baseserver.ConnectionString);
                if (dbList != null && dbList.Any())
                {
                    var i = 2;
                    var baseserver1 = baseserver;
                    dbList.ForEach(f =>
                    {
                        var childMenu = new List<MenuModel>();
                        var tables = baseserver1.DbBaseTableAccess.GetTableList(baseserver1.ConnectionString, f.Name);
                        var views = baseserver1.DbBaseTableAccess.GetViews(baseserver1.ConnectionString, f.Name);
                        if (tables != null && tables.Any())
                        {
                            childMenu.AddRange(tables.Select(r => new MenuModel { Menuid = i++, Icon = "icon-table", Menuname = r.Name, Url = $"/home/ShowTableInfo?datatableName={f.Name}&tableName={r.Name}&type=table&dbType={baseserver.CurrentDataBaseTypeEnum.GetHashCode()}" }).ToList());
                        }

                        if (views != null && views.Any())
                        {
                            childMenu.AddRange(views.Select(r => new MenuModel { Menuid = i++, Icon = "icon-table", Menuname = r.Name, Url = $"/home/ShowTableInfo?datatableName={f.Name}&tableName={r.Name}&type=view&dbType={baseserver.CurrentDataBaseTypeEnum.GetHashCode()}" }).ToList());
                        }
                        resultMenu.Add(new MenuModel { Menuid = i++, Menuname = f.Name, Icon = "icon-database", Menus = childMenu });
                    });
                }
            });

            return new MenuModel { Menus = resultMenu.ToList() };
        }
    }
}