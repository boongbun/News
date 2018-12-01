using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using News.Models;
using News.Common.Library;
using ExcelDataReader;
using News.Models.Parameter;

namespace News.Common.FileReader.Excel
{
    public class ExcelReader
    {
        public static ListPagedResultModel<T> GetExcelToList<T>(SearchModel<ExcelParameters> param)
        {
            var result = new ListPagedResultModel<T>(){ListItem = new List<T>()};
            try
            {
                using (var stream = File.Open(param.Cretia.FULL_PATH, FileMode.Open, FileAccess.Read))
                {

                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var datasetExcel = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });

                        var tableExcel = datasetExcel.Tables[param.Cretia.SHEET_INDEX];

                        result.ListItem = CommonClass.DataTableToObject<T>(tableExcel);
                        result.TotalRecord = tableExcel.Rows.Count;
                    }
                }
            }
            catch (Exception e)
            {
                result=new ListPagedResultModel<T>(){ListItem = null, IsError = true, Message = e.Message};
            }
            

            return result;
        }

        public static ResultModel<ExcelTableModel> ShowInfoExcel(SearchModel<ExcelParameters> param)
        {
            var result = new ResultModel<ExcelTableModel>() { Data = new ExcelTableModel()
            {
                ExcelInfoSheets = new ExcelParameters(),
                ListExcelSheets = new List<ExcelParameters>()
            }};
            try
            {
                using (var stream = File.Open(param.Cretia.FULL_PATH, FileMode.Open, FileAccess.Read))
                {

                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var datasetExcel = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });

                        result.Data.ExcelInfoSheets = new ExcelParameters()
                        {
                            SHEET_TOTAL = datasetExcel.Tables.Count,
                            FULL_NAME = param.Cretia.FULL_NAME
                        };
                        int index = 0;
                        foreach (var item in datasetExcel.Tables)
                        {
                            var addItem = new ExcelParameters()
                            {
                                SHEET_INDEX = index,
                                FULL_NAME = ((DataTable)item).TableName,
                                SHEET_ROWS = ((DataTable)item).Rows.Count,
                                SHEET_COLUMNS = ((DataTable)item).Columns.Count,
                            };

                            result.Data.ListExcelSheets.Add(addItem);
                            index++;
                        }

                        result.Message = "Lấy thông tin file thành công !";
                    }
                }
            }
            catch (Exception e)
            {
                result = new ResultModel<ExcelTableModel>() { Data = null, IsError = true, Message = e.Message };
            }

            return result;
        }
    }
}