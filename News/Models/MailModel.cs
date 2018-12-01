using System;
using System.Collections.Generic;
using System.Data;
using News.Models.Entities;

namespace News.Models
{
    public class MailModel
    {
        public string AddressClient { get; set; }
        public string SubjectClient { get; set; }
        public string BodyClient { get; set; }

        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] DataFileBytes { get; set; }

    }


}
