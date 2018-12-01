using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Models.Entities
{
    public class SSO_SECURITY_CODE
    {
        public long USER_ID { get; set; }
        public string SECURITY_CODE { get; set; }
        public string SESSION_ID { get; set; }
        public DateTime EXPIRED_TIME { get; set; }

       
    }
}

