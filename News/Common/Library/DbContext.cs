using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using News.BaseCore.Bussiness;
using News.Models;

namespace News.Common.Library
{
    public sealed class DataBaseConnection
    {
        public const string SsoConnectionString =
            "Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 10.10.10.10)(PORT = 1521))) (CONNECT_DATA = (SERVICE_NAME = xxx)));User Id=xxx;Password=xxx";
    }

    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Thông tin Action cần check phân quyền   
        /// </summary>
        public long[] Action { get; set; }

        private bool _hasPermission;

        private bool _isLogged;

        //private readonly string _ssoUrl = ConfigurationManager.AppSettings[AppSettingKeys.SSOLogOnUrl];
        //private readonly string _ssoEdhscUrl = ConfigurationManager.AppSettings[AppSettingKeys.SSOLogOnEdhscUrl];
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.Session?[SessionKeys.UserInfo] as UserInfoModel;
            if (user == null || user.ID == 0)
            {
                _isLogged = false;
                return false;
            }

            _isLogged = !Convert.ToBoolean(ConfigurationManager.AppSettings[SSOConfig.SSOUpdateStatusLogOnAppKey]) || CheckValidateSession(user).Data; ;
            if (user.Role == UserRole.Admin || Action == null)
            {
                _hasPermission = true;
            }
            else
            {
                if (user.Actions != null)
                {
                    _hasPermission = Action.Any(t => user.Actions.Contains(t)
                    //|| 
                    //(
                    //    (user.Privilege.ListMenu != null)
                    //    &&
                    //    (user.Privilege.ListMenu.Where(x => x.MENU_ID == t).ToList().Count > 0)
                    //)
                    );
                }
                else
                {
                    _hasPermission = false;
                }

            }

            return _isLogged && _hasPermission;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            if (_isLogged == false)
            {
                //Kiểm tra có phải đăng nhập qua SSO hay không
                if (filterContext.HttpContext.Request.Url != null)
                {
                    var url = filterContext.HttpContext.Request.Url.AbsoluteUri;
                    var key = filterContext.HttpContext.Request.QueryString["tokenkey"];
                    var idx = url.ToLower().IndexOf("tokenkey", StringComparison.Ordinal);
                    if (idx > 0)
                        url = url.Substring(0, idx - 1);

                    if (!String.IsNullOrEmpty(key))
                    {
                        var userIdQueryString = filterContext.HttpContext.Request.QueryString["userid"];
                        long userId = 0;
                        if (!string.IsNullOrWhiteSpace(userIdQueryString) &&
                            long.TryParse(userIdQueryString, out userId))
                        {

                            _isLogged = LogOn(new UserInfoModel(), filterContext.HttpContext);

                            //Kiểm tra lại xem đã đăng nhập thành công hay chưa
                            if (_isLogged)
                            {
                                filterContext.Result = new RedirectResult(url);
                                return;
                            }

                        }
                    }
                    else
                    {
                        if (filterContext.HttpContext.Request.IsAjaxRequest())
                        {
                            UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);
                            filterContext.HttpContext.Response.StatusCode = 401;
                            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                            filterContext.HttpContext.Response.ContentType = "application/json";
                            filterContext.Result = new JsonResult
                            {
                                Data = new
                                {
                                    ErrorCode = "-1",
                                    ErrorMessage = "NotAuthorized",
                                    Url = urlHelper.Action("LogOff", "Account", new { returnUrl = url, area = "" })
                                },
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            };
                        }
                        else
                        {

                            filterContext.Result = new RedirectResult($"Account?returnUrl={url}");

                        }
                    }
                }
            }
            else
            {
                UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);
                filterContext.Result = new RedirectResult(urlHelper.Action("Index", "Home"));
            }

        }

        #region SSO

        private readonly AccountBussiness _accountBussiness = new AccountBussiness();
        /// <summary>
        /// Kiểm tra key xác thực có hợp lệ hay không
        /// </summary>
        /// <param name="moduleIdToGetPrivilege"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private ResultModel<UserInfoModel> ValidateTokenKey(int moduleIdToGetPrivilege, string key)
        {

            var result = _accountBussiness.CheckValidateKey(key);


            return result;
        }

        private ResultModel<bool> CheckValidateSession(UserInfoModel param)
        {

            var result = _accountBussiness.CheckValidateSession(param);


            return result;
        }

        private bool LogOn(UserInfoModel userParam, HttpContextBase httpContext)
        {
            if (userParam != null)
            {
                var getInfoUserExists = new AccountBussiness().AuthenticationUserExists(userParam);

                //Lưu vào Session
                var userinfo = new UserInfoModel
                {
                    ID = getInfoUserExists.Data.ID,
                    USER_NAME = getInfoUserExists.Data.USER_NAME
                };

                //Khởi tạo Session
                httpContext.Session[SessionKeys.UserInfo] = userinfo;

                return true;
            }
            return false;
        }

        #endregion SSO
    }

    public sealed class TableNameSource
    {
        public const string AccountTable = " TMP_ACCOUNT ";
        public const string BaoNoTable = " TMP_BAO_NO ";
        public const string BaoNoCapTable = " TMP_BAO_NO_CAP ";
        public const string BaoNoNvTable = " TMP_BAO_NO_NV ";
        public const string BaoNoDtTable = " TMP_BAO_NO_DT ";

    }
}
