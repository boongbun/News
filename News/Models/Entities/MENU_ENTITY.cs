using System;

namespace News.Models.Entities
{
    public class MENU_ENTITY
    {
        public int STT { get; set; }
        public int MENU_ID { get; set; }
        public int MENU_PARENT_ID { get; set; }
        public string MENU_NAME { get; set; }
        public string MENU_PATH { get; set; }
        public string MENU_ICON_MATERIAL { get; set; }
    }
}

