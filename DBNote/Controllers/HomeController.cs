using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using DBNote.Enum;
using DBNote.Models;
using DBNote.Server;
using FreshCommonUtility.Enum;
using FreshCommonUtility.Web;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;

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
            try
            {
                var menu = MenuServer.GetMenuInfo();
                ViewBag.MenuJson = JsonConvert.SerializeObject(menu);
            }
            catch (Exception ex)
            {
                ViewBag.MenuJson = "null";
                ErrorInfoServer.Push(ex);
            }
            return View();
        }

        /// <summary>
        /// 显示表格详细信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowTableInfo()
        {
            ViewBag.databaseName = System.Web.HttpContext.Current.GetStringFromParameters("datatableName");
            ViewBag.tableName = System.Web.HttpContext.Current.GetStringFromParameters("tableName").Replace("**", "#");
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
            tableName = tableName.Replace("**", "#");
            var type = System.Web.HttpContext.Current.GetStringFromParameters("type");
            var dbType = System.Web.HttpContext.Current.GetIntFromParameters("dbType");
            var dbTypeEnum = EnumHelper.GetEnumByValue<DataBaseTypeEnum>(dbType);
            BaseTable baseInfo = null;
            try
            {
                if (type == "table")
                {
                    baseInfo = TableViewServer.GetTableInfo(databaseName, tableName, dbTypeEnum);
                }
                else if (type == "view")
                {
                    baseInfo = TableViewServer.GetViewInfo(databaseName, tableName, dbTypeEnum);
                }
            }
            catch (Exception ex)
            {
                ErrorInfoServer.Push(ex);
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
            List<DataBaseLinkConfigModel> configList = null;
            try
            {
                configList = DataBaseConfigServer.GetConfigModels();
            }
            catch (Exception ex)
            {
                ErrorInfoServer.Push(ex);
            }
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
            try
            {
                configModel.ForEach(f =>
                {
                    DataBaseConfigServer.DeleteConfigModel(f);
                });
                result.Code = "0000";
                result.Des = "删除成功";
            }
            catch (Exception ex)
            {
                ErrorInfoServer.Push(ex);
                result.Code = "0000";
                result.Des = ex.Message;
            }
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
            var isEnable = System.Web.HttpContext.Current.GetIntFromParameters("IsEnable");
            var model = new DataBaseLinkConfigModel
            {
                DbType = EnumHelper.GetEnumByValue<DataBaseTypeEnum>(dbType),
                LinkConnectionString = linkConnectionString,
                Id = id,
                LinkName = linkName,
                IsEnable = EnumHelper.GetEnumByValue<IsEnableEnum>(isEnable)
            };
            var result = new BaseResultModel();
            if (model.Id < 1)
            {
                var oldModel = DataBaseConfigServer.GetConfigModelByLinkName(model.LinkName);
                if (oldModel != null && oldModel.Id > 0)
                {
                    result.Code = "0001";
                    result.Des = "连接名称已被占用，请重新输入。";
                    return Json(result);
                }
            }
            var success = DataBaseConfigServer.UpdateConfigModel(model);
            result.Code = success ? "0000" : "0001";
            result.Des = success ? "成功" : "失败";
            return Json(result);
        }

        /// <summary>
        /// 获得错误信息
        /// </summary>
        /// <returns></returns>
        public JsonResult GetErrorInfo()
        {
            var errorInfo = ErrorInfoServer.GetErrorModels();
            var result = new
            {
                total = errorInfo?.Count,
                rows = errorInfo
            };
            return Json(result);
        }
    }
}