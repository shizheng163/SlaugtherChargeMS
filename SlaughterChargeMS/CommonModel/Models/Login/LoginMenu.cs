/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017/7/21 14:39:57
*  文件名：LoginMenu
*  说明： 
*───────────────────────────────────
*  V0.01 2017/7/21 14:39:57 史正 初版
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModel.Models
{
    /// <summary>
    /// 返回前端菜单Json的菜单节点模型
    /// </summary>
    public class LoginMenu
    {
        public string menuid { get; set; }
        public string menuname { get; set; }
        public string url { get; set; }
        public string icon { get; set; }
        public List<LoginMenu> menus { get; set; }
    }
}
