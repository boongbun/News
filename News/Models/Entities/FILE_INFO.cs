using System;

namespace News.Models.Entities
{
    public class FILE_INFO
    {
        public int FILE_ID { get; set; }
        public string DIRECTORY_NAME { get; set; }
        public string DIRECTORY_PATH { get; set; }
        public string FILE_NAME { get; set; }
        public string FILE_PATH { get; set; }
        public string FILE_EXT { get; set; }
        public string FILE_TYPE { get; set; }
        public string FILE_BLOB { get; set; }

        public DateTime CREATE_DATE { get; set; }
    }
}

