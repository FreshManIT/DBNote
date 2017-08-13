using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using DBNote.Enum;
using DBNote.Models;
using DBNote.Server;
using FreshCommonUtility.Enum;
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
            var dbType = System.Web.HttpContext.Current.GetIntFromParameters("dbType");
            ViewBag.dbType = dbType;
            var dbTypeEnum = EnumHelper.GetEnumByValue<DataBaseTypeEnum>(dbType);
            ViewBag.dbTypeDescription = EnumHelper.GetDescriptionByEnum(dbTypeEnum);
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
            var dbType = System.Web.HttpContext.Current.GetIntFromParameters("dbType");
            var dbTypeEnum = EnumHelper.GetEnumByValue<DataBaseTypeEnum>(dbType);
            BaseTable baseInfo = null;
            if (type == "table")
            {
                baseInfo = TableViewServer.GetTableInfo(databaseName, tableName, dbTypeEnum);
            }
            else if (type == "view")
            {
                baseInfo = TableViewServer.GetViewInfo(databaseName, tableName, dbTypeEnum);
            }
            var result = new
            {
                total = baseInfo?.Columns.Count,
                rows = baseInfo?.Columns.Values
            };
            return Json(result);
        }

        /// <summary>
        /// 获得数据库配置文件
        /// </summary>
        /// <returns></returns>
        public JsonResult DataBaseConfigJson()
        {
            var configList = DataBaseConfigServer.GetConfigModels();
            var result = new
            {
                total = configList?.Count,
                rows = configList
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteConfig()
        {
            var result = new BaseResultModel();
            var data = System.Web.HttpContext.Current.GetStringFromParameters("requestData");
            if (string.IsNullOrEmpty(data))
            {
                result.Code = "0001";
                result.Des = "请求参数错误";
                return Json(result);
            }
            var configModel = JsonConvert.DeserializeObject<List<DataBaseLinkConfigModel>>(data);
            if (configModel == null || !configModel.Any())
            {
                result.Code = "0001";
                result.Des = "请求参数解析失败";
                return Json(result);
            }
            configModel.ForEach(f =>
            {
                DataBaseConfigServer.DeleteConfigModel(f);
            });
            result.Code = "0000";
            result.Des = "删除成功";
            return Json(result);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateOrAddConfig()
        {
            var id = System.Web.HttpContext.Current.GetIntFromParameters("Id");
            var linkName = System.Web.HttpContext.Current.GetStringFromParameters("LinkName");
            var dbType = System.Web.HttpContext.Current.GetIntFromParameters("DbType");
            var linkConnectionString = System.Web.HttpContext.Current.GetStringFromParameters("LinkConnectionString");
            var model = new DataBaseLinkConfigModel
            {
                DbType = EnumHelper.GetEnumByValue<DataBaseTypeEnum>(dbType),
                LinkConnectionString = linkConnectionString,
                Id = id,
                LinkName = linkName
            };
            var success = DataBaseConfigServer.UpdateConfigModel(model);

            var result = new BaseResultModel
            {
                Code = success ? "0000" : "0001",
                Des = success ? "成功" : "失败"
            };
            return Json(result);
        }
    }
}