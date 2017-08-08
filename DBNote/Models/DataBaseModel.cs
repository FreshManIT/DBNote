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

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataBaseModel() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tables"></param>
        public DataBaseModel(Tables tables)
        {
            Tables = tables;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="views"></param>
        public DataBaseModel(Views views)
        {
            Views = views;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="views"></param>
        public DataBaseModel(Tables tables, Views views)
            : this(tables)
        {
            Views = views;
        }

        /// <summary>
        /// 表格
        /// </summary>
        public Tables Tables { get; set; }

        /// <summary>
        /// 视图
        /// </summary>
        public Views Views { get; set; }
    }
}