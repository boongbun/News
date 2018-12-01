using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace News.Models.Parameter
{
    public class ExcelParameters
    {
        public ExcelParameters()
        {
            SHEET_INDEX = 0;
            SHEET_NUMBERS = 0;
            SHEET_TOTAL = 0;
            SHEET_ROWS = 0;
            SHEET_COLUMNS = 0;
        }
        public int STT { get; set; }
        public int SHEET_INDEX { get; set; }
        public int SHEET_NUMBERS { get; set; }
        public int SHEET_TOTAL { get; set; }
        public int SHEET_ROWS { get; set; }
        public int SHEET_COLUMNS { get; set; }

        public string FULL_PATH { get; set; }
        public string FULL_NAME { get; set; }
       
    }
}