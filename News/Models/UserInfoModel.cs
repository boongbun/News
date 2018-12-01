using System;
using System.Collections.Generic;
using News.Models.Entities;


namespace News.Models
{
    public class UserInfoModel : SSO_USER
    {
        public string ReturnUrl { get; set; }
        public bool IsRedirect { get; set; }

        public string SessionId { get; set; }
        public bool IsUpdateStatusLogOut { get; set; }

        public string TokenKey { get; set; }
        public DateTime ExpiredDateTime { get; set; }

        /// <summary>
        /// Ghi nhớ cookie
        /// </summary>
        public bool IsRemember { get; set; }
        /// <summary>
        /// Check remote server to generate token key
        /// </summary>
        public bool IsRemoteLogOn { get; set; }

        #region Cac thuoc tinh them cho session


        public int Role { get; set; }

        public List<long> Actions { get; set; }

        public string Menu { get; set; }

        #endregion
    }


}
