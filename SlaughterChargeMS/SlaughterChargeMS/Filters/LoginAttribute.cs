/****************************************************************************
 * 单元名称：登录授权过滤类
 * 单元描述：登录授权过滤类 判断在一切Action执行之前判断是否已经登录
 * 作者:史正
 * 创建日期：2017年8月1日
 * 最后修改：（请最后修改的人填写）
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SlaughterChargeMS.Filters
{
    public class LoginAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 自动以授权检测
        /// </summary>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");
            //result为true的条件为 当前http连接的用户已经被验证  或者当前Session中存储的登录用户不为空
            var result = httpContext.User.Identity.IsAuthenticated ||
                (httpContext.Session != null && httpContext.Session["LoginAccount"] != null);
            if (!result)
                httpContext.Response.StatusCode = 403;
            return result;
        }
        /// <summary>
        /// 处理未经过授权的Http请求
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Login/Index");
        }
    }
}