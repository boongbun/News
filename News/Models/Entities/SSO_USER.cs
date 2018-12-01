using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models.Entities
{
    public class SSO_USER
    {
        public long ID { get; set; }
        public int VAI_TRO_ID { get; set; }
        public long CHUC_NANG_ID { get; set; }

        public string HO_TEN { get; set; }
        public string USER_NAME { get; set; }
        public string PASS_WORD { get; set; }

        public string MA_DON_VI { get; set; }
        public string TEN_DON_VI { get; set; }
        public string MA_TINH_THANH { get; set; }
        public string TEN_TINH_THANH { get; set; }
    }
}

