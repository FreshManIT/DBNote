#region	Vesion Info
//======================================================================
//Copyright(C) FreshMan.All right reserved.
//命名空间：DBNote.Models
//文件名称：MenuModel
//创 建 人：FreshMan
//创建日期：2017/8/7 22:03:45
//用    途：记录类的用途
//======================================================================
#endregion

using System.Collections.Generic;
using Newtonsoft.Json;

namespace DBNote.Models
{
    /// <summary>
    /// 菜单实体
    /// </summary>
    public class MenuModel
    {
        /// <summary>
        /// 菜单id
        /// </summary>
        [JsonProperty("menuid")]
        public int Menuid { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [JsonProperty("menuname")]
        public string Menuname { get; set; }

        /// <summary>
        /// 菜单连接
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// 菜单子项
        /// </summary>
        [JsonProperty("menus")]
        public List<MenuModel> Menus { get; set; } 
    }

}