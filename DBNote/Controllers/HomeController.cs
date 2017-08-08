using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DBNote.Models;
using DBNote.Server;
using FreshCommonUtility.Web;
using Newtonsoft.Json;

namespace DBNote.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var menu = MenuServer.GetMenuInfo();
            ViewBag.MenuJson = JsonConvert.SerializeObject(menu);
            return View();
        }

        /// <summary>
        /// 显示表格详细信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowTableInfo()
        {
            ViewBag.databaseName = System.Web.HttpContext.Current.GetStringFromParameters("datatableName");
            ViewBag.tableName = System.Web.HttpContext.Current.GetStringFromParameters("tableName");
            ViewBag.type = System.Web.HttpContext.Current.GetStringFromParameters("type");
            return View();
        }

        /// <summary>
        /// 显示表格详细信息
        /// </summary>
        /// <returns></returns>
        public JsonResult TableInfoJson()
        {
            var databaseName = System.Web.HttpContext.Current.GetStringFromParameters("datatableName");
            var tableName = System.Web.HttpContext.Current.GetStringFromParameters("tableName");
            var type = System.Web.HttpContext.Current.GetStringFromParameters("type");
            BaseTable baseInfo = null;
            if (type == "table")
            {
                baseInfo = TableViewServer.GetTableInfo(databaseName, tableName);
            }
            else if (type == "view")
            {
                baseInfo = TableViewServer.GetViewInfo(databaseName, tableName);
            }
            var result = new
            {
                total = baseInfo?.Columns.Count,
                rows = baseInfo?.Columns.Values
            };
            return Json(result);
        }
    }
}