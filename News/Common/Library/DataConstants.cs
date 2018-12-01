namespace News.Common.Library
{
    public sealed class SessionKeys
    {
        /// <summary>
        /// Thông tin người dùng hiện tại
        /// </summary>
        public const string UserInfo = "CURRENT_USER_INFO";

        public const string SocketClientName = "CURRENT_SOCKET_CLIENT_NAME";

        /// <summary>
        /// Thông tin ngôn ngữ hiện tại
        /// </summary>
        public const string CultureInfo = "CURRENT_CULTURE_INFO";
    }

    public sealed class UserRole
    {
        public const int Admin = 1;
    }

    public sealed class DataConstants
    {
    }

    public static class AppSettingValue
    {
        public const int Otp_Authen = 0;
        //Thông tin gọi ws http://10.10.41.18/LDAPWebService/WSLDAP.asmx?op=TraThongTinVNPTPortal
        public const string UserName = "ptpm2";
        public const string PassWord = "ptpm2@123";
    }
    public static class ErrorCode
    {
        #region error code service tập đoàn
        public const int Successful = 0;
        public const int Error = 1;
        public const int UserNotExist = 2;
        public const int UserNotExist_WrongPassword = 3;
        public const int UserLocked = 4;
        #endregion

        public const int MissingUserNameOrPassword = 5;
        public const int WrongPassword = 6;

        public const int InvalidInput = 999;
        public const int ApiError = -999;

    }
    public static class SSOConfig
    {
        public const int MinuteExpired = 60;
        public const int SSOCookieDayExpired = 1;
        public const string SSOCookieKeyEncrypt = "User@SSO";
        public const string SSOTokenKeyEncrypt = "Token@SSO";
        public const string SSOCookieName = "VNPTSSO";
        public const string SSOCookieKey = "UserId";
        public const string SSOUpdateStatusLogOnAppKey = "SSOUpdateStatusLogOn";

    }
}