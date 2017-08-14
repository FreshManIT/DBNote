#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote.Server
//文件名称：DataBaseConfigServer
//创 建 人：FreshMan
//创建日期：2017/8/13 18:18:34
//用    途：记录类的用途
//======================================================================
#endregion

using System.Collections.Generic;
using DBNote.DataBase;
using DBNote.Models;

namespace DBNote.Server
{
    public static class DataBaseConfigServer
    {
        /// <summary>
        /// 数据访问对象
        /// </summary>
        private static readonly SqliteDataAccess DataReader = new SqliteDataAccess();

        /// <summary>
        /// 查询配置文件信息
        /// </summary>
        /// <returns></returns>
        public static List<DataBaseLinkConfigModel> GetConfigModels()
        {
            return DataReader.GetConfigModels();
        }

        /// <summary>
        /// 更新配置文件
        /// </summary>
        /// <param name="newModel"></param>
        /// <returns></returns>
        public static bool UpdateConfigModel(DataBaseLinkConfigModel newModel)
        {
            if (newModel == null || newModel.Id < 0) return false;
            return newModel.Id < 1 ? DataReader.AddConfigModel(newModel) : DataReader.UpdateConfigModel(newModel);
        }

        /// <summary>
        /// 查询指定条件配置信息
        /// </summary>
        /// <param name="linkName"></param>
        /// <returns></returns>
        public static DataBaseLinkConfigModel GetConfigModelByLinkName(string linkName)
        {
            return DataReader.GetConfigModelByLinkName(linkName);
        }

        /// <summary>
        /// 删除配置文件
        /// </summary>
        /// <param name="deletModel"></param>
        /// <returns></returns>
        public static bool DeleteConfigModel(DataBaseLinkConfigModel deletModel)
        {
            return DataReader.DeleteConfigModel(deletModel);
        }
    }
}