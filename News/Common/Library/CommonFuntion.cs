using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using News.Hubs;
using News.Models;
using News.Models.Entities;
using dBASE.NET;

namespace News.Common.Library
{
    #region ConverCommon
    public class CommonClass
    {
        public static string PaginatedData(string mainSql)
        {
            string pagingStr = @"SELECT *  FROM ( " + mainSql + @" ) WHERE STT >= ( (:pPageIndex - 1) * :pPageSize + 1)
                        AND STT < ( (:pPageIndex - 1) * :pPageSize + 1 + :pPageSize)";

            return pagingStr;
        }

        public static int ConvertNumber(object obj, int defaultValue)
        {
            try
            {
                return int.Parse(obj.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ChuyenSo(string number)
        {
            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc;
            int i, j, k, n, len, found, ddv, rd;

            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "lẻ ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;
                            case '5':
                                if (i + j == len - 1)
                                    doc += "lăm ";
                                else
                                    doc += cs[5] + " ";
                                break;
                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Upper first char
                        if (i == 0)

                        {
                            char[] array = doc.ToCharArray();
                            // Handle the first letter in the string.  
                            if (array.Length >= 1)
                            {
                                if (char.IsLower(array[0]))
                                {
                                    array[0] = char.ToUpper(array[0]);
                                }
                                doc = new string(array);
                            }
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += dv[n - j - 1] + " ";
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                    if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];

            return doc;
        }

        #region Conver Number To Money String

        private static string Chu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;
                case "1":
                    result = "một";
                    break;
                case "2":
                    result = "hai";
                    break;
                case "3":
                    result = "ba";
                    break;
                case "4":
                    result = "bốn";
                    break;
                case "5":
                    result = "năm";
                    break;
                case "6":
                    result = "sáu";
                    break;
                case "7":
                    result = "bảy";
                    break;
                case "8":
                    result = "tám";
                    break;
                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }
        private static string Donvi(string so)
        {
            string Kdonvi = "";

            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";

            return Kdonvi;
        }
        private static string Tach(string tach3)
        {
            string Ktach = "";
            if (tach3.Equals("000"))
                return "";
            if (tach3.Length == 3)
            {
                string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    Ktach = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    Ktach = " không trăm mười lăm ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";

            }


            return Ktach;

        }
        public static string ConverMoneyNumberToString(double gNum)
        {
            if (gNum == 0)
                return "Không đồng";

            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            double Num = Math.Round(gNum, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (gNum < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            //don vi hang mod
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            // Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + " đồng chẵn.";

            return lso_chu.ToString().Trim();

        }

        #endregion

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static List<T> DataSetToObject<T>(DataSet dataSet)
        {
            var array = new List<T>();
            var dataTable = dataSet.Tables;
            var dt = dataTable[0];


            foreach (DataRow dataRow in dt.Rows)
            {
                var row = Activator.CreateInstance<T>();

                var propertyInfos = typeof(T).GetProperties();

                foreach (var ppi in propertyInfos)
                {
                    var name = ppi.Name.ToUpper();
                    //try
                    //{
                    //bo sung kiem tra ten cot co ton tai khong o day
                    if (!dataRow.Table.Columns.Contains(name)) continue;
                    var value = dataRow[name];
                    ppi.SetValue(row, Convert.ChangeType(value, ppi.PropertyType), null);
                    //}
                    //catch (Exception ex)
                    //{
                    //}
                }
                array.Add((T)row);
            }
            return array;
        }

        public static List<T> DataTableToObject<T>(DataTable dataTable)
        {
            var array = new List<T>();
            string errStr = string.Empty;
            string errStrDetail = string.Empty;
            try
            {
                int demRow = 1;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    var row = Activator.CreateInstance<T>();

                    var propertyInfos = typeof(T).GetProperties();
                    errStr = "row: " + demRow;
                    foreach (var ppi in propertyInfos)
                    {
                        errStrDetail = string.Empty;
                        var name = ppi.Name.ToUpper();
                        //try
                        //{
                        //bo sung kiem tra ten cot co ton tai khong o day
                        if (!dataRow.Table.Columns.Contains(name)) continue;
                        var value = dataRow[name];

                        errStrDetail = ". name: " + name + ", value: " + dataRow[name] + ", properties: " + ppi.PropertyType;

                        ppi.SetValue(row, Convert.ChangeType(value, ppi.PropertyType), null);
                        //}
                        //catch (Exception ex)
                        //{
                        //}
                    }
                    array.Add((T)row);

                    CommonFunction.SendProgress("Convert Excel to List ...", demRow, dataTable.Rows.Count);
                    demRow++;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + ". " + errStr + errStrDetail);
            }
            
            return array;
        }

        public static ResultModel<ViewFileModel> GetViewFileModel(FileInfo[] fileInfo)
        {
            var result = new ResultModel<ViewFileModel>()
            {
                Data = new ViewFileModel() { ListFileInfos = new List<FILE_INFO>() }
            };
            try
            {
                if (fileInfo.Length > 0)
                {
                    var dem = 1;
                    foreach (var itemFileInfo in fileInfo)
                    {
                        var itemFile = new FILE_INFO()
                        {
                            FILE_ID = dem,
                            DIRECTORY_PATH = itemFileInfo.DirectoryName,
                            FILE_NAME = itemFileInfo.Name,
                            FILE_PATH = itemFileInfo.FullName,
                            CREATE_DATE = itemFileInfo.CreationTime
                        };

                        result.Data.ListFileInfos.Add(itemFile);
                        dem++;
                    }

                    result.Message = "Get file done !";
                }
                else
                {
                    result.IsError = true;
                    result.Data.ListFileInfos = null;
                }
            }
            catch (Exception e)
            {
                result.IsError = true;
                result.Message = e.Message;
                result.Data = null;
            }

            return result;
        }
    }


    #endregion ConvertCommon

    #region ConvertFont
    public class Uconvert119
    {
        public static string Uconvert(string strConv)
        {

            Int32 doclength;
            Int32 counter;
            char chr;
            string strCnv;

            if (strConv != "")
            {
                doclength = strConv.Length;
                strCnv = "";
                for (counter = 0; counter < doclength; counter++)
                {
                    chr = Convert.ToChar(strConv.Substring(counter, 1));

                    switch (chr)
                    {

                        case 'µ':
                            chr = 'à'; break;
                        case '¸':
                            chr = 'á'; break;
                        case '¶':
                            chr = 'ả'; break;
                        case '¹':
                            chr = 'ạ'; break;
                        case '·':
                            chr = 'ã'; break;

                        //'--aw--
                        case '¨':
                            chr = 'ă'; break;
                        case '»':
                            chr = 'ằ'; break;
                        case '¾':
                            chr = 'ắ'; break;
                        case '¼':
                            chr = 'ẳ'; break;
                        case 'Æ':
                            chr = 'ặ'; break;
                        case '½':
                            chr = 'ẵ'; break;


                        //'--aa--
                        case '©':
                            chr = 'â'; break;
                        case 'Ç':
                            chr = 'ầ'; break;
                        case 'Ê':
                            chr = 'ấ'; break;
                        case 'È':
                            chr = 'ẩ'; break;
                        case 'Ë':
                            chr = 'ậ'; break;
                        case 'É':
                            chr = 'ẫ'; break;

                        //'--e--
                        case 'e':
                            chr = 'e'; break;
                        case 'Ì':
                            chr = 'è'; break;
                        case 'Ð':
                            chr = 'é'; break;
                        case 'Î':
                            chr = 'ẻ'; break;
                        case 'Ñ':
                            chr = 'ẹ'; break;
                        case 'Ï':
                            chr = 'ẽ'; break;

                        //'--ee--
                        case 'ª':
                            chr = 'ê'; break;
                        case 'Ò':
                            chr = 'ề'; break;
                        case 'Õ':
                            chr = 'ế'; break;
                        case 'Ó':
                            chr = 'ể'; break;
                        case 'Ö':
                            chr = 'ệ'; break;
                        case 'Ô':
                            chr = 'ễ'; break;


                        //'--i--
                        case '×':
                            chr = 'ì'; break;
                        case 'Ý':
                            chr = 'í'; break;
                        case 'Ø':
                            chr = 'ỉ'; break;
                        case 'Þ':
                            chr = 'ị'; break;
                        case 'Ü':
                            chr = 'ĩ'; break;


                        //'--o--
                        case 'ß':
                            chr = 'ò'; break;
                        case 'ã':
                            chr = 'ó'; break;
                        case 'á':
                            chr = 'ỏ'; break;
                        case 'ä':
                            chr = 'ọ'; break;
                        case 'â':
                            chr = 'õ'; break;


                        //'--oo--
                        case '«':
                            chr = 'ô'; break;
                        case 'å':
                            chr = 'ồ'; break;
                        case 'è':
                            chr = 'ố'; break;
                        case 'æ':
                            chr = 'ổ'; break;
                        case 'é':
                            chr = 'ộ'; break;
                        case 'ç':
                            chr = 'ỗ'; break;

                        //'--ow--
                        case '¬':
                            chr = 'ơ'; break;
                        case 'ê':
                            chr = 'ờ'; break;
                        case 'í':
                            chr = 'ớ'; break;
                        case 'ë':
                            chr = 'ở'; break;
                        case 'î':
                            chr = 'ợ'; break;
                        case 'ì':
                            chr = 'ỡ'; break;

                        //'--u--
                        case 'ï':
                            chr = 'ù'; break;
                        case 'ó':
                            chr = 'ú'; break;
                        case 'ñ':
                            chr = 'ủ'; break;
                        case 'ô':
                            chr = 'ụ'; break;
                        case 'ò':
                            chr = 'ũ'; break;

                        //'--w--
                        case '­':
                            chr = 'ư'; break;
                        case 'õ':
                            chr = 'ừ'; break;
                        case 'ø':
                            chr = 'ứ'; break;
                        case 'ö':
                            chr = 'ử'; break;
                        case 'ù':
                            chr = 'ự'; break;
                        case '÷':
                            chr = 'ữ'; break;


                        //'--y--
                        case 'ú':
                            chr = 'ỳ'; break;
                        case 'ý':
                            chr = 'ý'; break;
                        case 'û':
                            chr = 'ỷ'; break;
                        case 'þ':
                            chr = 'ỵ'; break;
                        case 'ü':
                            chr = 'ỹ'; break;

                        //'--dd DD--
                        case '®':
                            chr = 'đ'; break;
                        case '§':
                            chr = 'Đ'; break;

                        //'--AW AA EE OO OW W--
                        case '¡':
                            chr = 'Ă'; break;
                        case '¢':
                            chr = 'Â'; break;
                        case '£':
                            chr = 'Ê'; break;
                        case '¤':
                            chr = 'Ô'; break;
                        case '¥':
                            chr = 'Ơ'; break;
                        case '¦':
                            chr = 'Ư'; break;

                    }
                    strCnv = strCnv + chr;

                }

                return strCnv;
            }
            else
                return "";
        }

        public static string Vconvert(string strConv)
        {
            Int32 doclength;
            Int32 counter;
            char chr;
            string strCnv;

            if (strConv != "")
            {
                doclength = strConv.Length;
                strCnv = "";
                for (counter = 0; counter < doclength; counter++)
                {
                    chr = Convert.ToChar(strConv.Substring(counter, 1));

                    switch (chr)
                    {

                        case 'à': chr = 'µ'; break;
                        case 'á': chr = '¸'; break;
                        case 'ạ': chr = '¹'; break;
                        case 'ả': chr = '¶'; break;
                        case 'ã': chr = '·'; break;

                        case 'ò': chr = 'ß'; break;
                        case 'ó': chr = 'ã'; break;
                        case 'ọ': chr = 'ä'; break;
                        case 'ỏ': chr = 'á'; break;
                        case 'õ': chr = 'â'; break;
                        case 'ì': chr = '×'; break;
                        case 'í': chr = 'Ý'; break;
                        case 'ị': chr = 'Þ'; break;
                        case 'ỉ': chr = 'Ø'; break;
                        case 'ĩ': chr = 'Ü'; break;

                        case 'ù': chr = 'ï'; break;

                        case 'ú': chr = 'ó'; break;

                        case 'ụ': chr = 'ô'; break;

                        case 'ủ': chr = 'ñ'; break;

                        case 'ũ': chr = 'ò'; break;

                        case 'è': chr = 'Ì'; break;

                        case 'é': chr = 'Ð'; break;

                        case 'ẹ': chr = 'Ñ'; break;

                        case 'ẻ': chr = 'Î'; break;

                        case 'ẽ': chr = 'Ï'; break;

                        case 'ồ': chr = 'å'; break;

                        case 'ố': chr = 'è'; break;

                        case 'ộ': chr = 'é'; break;

                        case 'ổ': chr = 'æ'; break;

                        case 'ỗ': chr = 'ç'; break;

                        case 'ờ': chr = 'ê'; break;

                        case 'ớ': chr = 'í'; break;

                        case 'ợ': chr = 'î'; break;

                        case 'ở': chr = 'ë'; break;

                        case 'ỡ': chr = 'ì'; break;

                        case 'ừ': chr = 'õ'; break;

                        case 'ứ': chr = 'ø'; break;

                        case 'ự': chr = 'ù'; break;

                        case 'ử': chr = 'ö'; break;

                        case 'ữ': chr = '÷'; break;

                        case 'ề': chr = 'Ò'; break;

                        case 'ế': chr = 'Õ'; break;

                        case 'ệ': chr = 'Ö'; break;

                        case 'ể': chr = 'Ó'; break;

                        case 'ễ': chr = 'Ô'; break;

                        case 'ằ': chr = '»'; break;

                        case 'ắ': chr = '¾'; break;

                        case 'ặ': chr = 'Æ'; break;

                        case 'ẳ': chr = '¼'; break;

                        case 'ẵ': chr = '½'; break;// '(awx)

                        case 'ầ': chr = 'Ç'; break;

                        case 'ấ': chr = 'Ê'; break;

                        case 'ậ': chr = 'Ë'; break;

                        case 'ẩ': chr = 'È'; break;

                        case 'ẫ': chr = 'É'; break;

                        case 'đ': chr = '®'; break;
                        case 'Đ': chr = '§'; break;// '(DD)

                        case 'ư': chr = '­'; break;
                        case 'ơ': chr = '¬'; break;
                        case 'ă': chr = '¨'; break;
                        case 'â': chr = '©'; break;
                        case 'ô': chr = '«'; break;
                        case 'ê': chr = 'ª'; break;

                        case 'Ơ': chr = '¥'; break;// '(OW)
                        case 'Ư': chr = '¦'; break;// '(UW)
                        case 'Ă': chr = '¡'; break;// '(AW)
                        case 'Â': chr = '¢'; break;
                        case 'Ô': chr = '¤'; break;
                        case 'Ê': chr = '£'; break;
                        case 'ỳ': chr = 'ú'; break;
                        case 'ý': chr = 'ý'; break;
                        case 'ỵ': chr = 'þ'; break;
                        case 'ỷ': chr = 'û'; break;
                        case 'ỹ': chr = 'ü'; break;

                        case 'Ỳ': chr = 'ú'; break;
                        case 'Ý': chr = 'ý'; break;
                        case 'Ỵ': chr = 'þ'; break;
                        case 'Ỷ': chr = 'û'; break;
                        case 'Ỹ': chr = 'ü'; break;

                            //default: chr = chr;

                    }
                    strCnv = strCnv + chr;

                }

                return strCnv;
            }
            else
                return "";
        }

        public string Convert_Date(string strDate)
        {
            //format date input mm/dd/yyyy
            //format output dd/mm/yyyy
            string convert_date = "";
            string mm = "", dd = "";
            if (strDate == "")
            {
                convert_date = "";
            }
            else
            //elseif strDate<>"" then
            {
                if (strDate.IndexOf('/') > 0)
                //if instr(strDate,"/")>0 then
                {
                    mm = strDate.Substring(0, strDate.IndexOf('/'));
                    //mm=left(strDate,instr(strDate,"/")-1);
                    strDate = strDate.Substring(strDate.IndexOf('/') + 1, strDate.Length - 1 - strDate.IndexOf('/'));
                    //strdate=right(strDate,len(strDate)-instr(strDate,"/"));
                    dd = strDate.Substring(0, strDate.IndexOf('/'));
                    //dd = left(strDate, instr(strDate, "/") - 1);
                }
                else
                    convert_date = "";
                //end if
                if (strDate.IndexOf('/') > 0)
                {
                    strDate = strDate.Substring(strDate.IndexOf('/') + 1, strDate.Length - 1 - strDate.IndexOf('/'));
                    convert_date = dd + "/" + mm + "/" + strDate;
                }
                else
                    convert_date = "";

            }
            return convert_date;
        }

        public static string UconvertVNI(string strConv)
        {
            string strConverted = "";

            string[] strUni = { "à", "á", "ả", "ã", "ạ", "ă", "ằ", "ắ", "ẳ", "ẵ", "ặ", "â", "ầ", "ấ", "ẩ", "ẫ", "ậ", "đ", "è",
                                "é", "ẻ", "ẽ", "ẹ", "ê", "ề", "ế", "ể", "ễ", "ệ", "ì", "í", "ỉ", "ĩ", "ị", "ò", "ó", "ỏ", "õ",
                                "ọ", "ô", "ồ", "ố", "ổ", "ỗ", "ộ", "ơ", "ờ", "ớ", "ở", "ỡ", "ợ", "ù", "ú", "ủ", "ũ", "ụ", "ư",
                                "ừ", "ứ", "ử", "ữ", "ự", "ỳ", "ý", "ỷ", "ỹ", "ỵ", "Ă", "Â", "Đ", "Ê", "Ô", "Ơ", "Ư" };

            string[] strVni = { "aø", "aù", "aû", "aõ", "aï", "aê", "aè", "aè", "aú", "aü", "aë", "aâ", "aá", "aà", "aå", "aã", "aä", "eù"
                                  , "eø", "eû", "eõ", "eï", "eâ", ""
                              ,"","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","",""
                              ,"","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","","",""};
            string[] strTemp = new string[1000];
            string[] strTempConverted = new string[1000];
            string strTemp1 = "";
            int strIndex;

            if (strConv != "")
            {
                strTemp = strConv.Select(c => c.ToString()).ToArray();
            }
            foreach (string Temp in strTemp)
            {
                strIndex = Array.IndexOf(strUni, Temp, 0);
                if (strIndex >= 0)
                {
                    strTemp1 = strVni[strIndex];
                    strConverted = strConverted + strTemp1;
                }
                else
                {
                    strConverted = strConverted + Temp;
                }
            }


            return strConverted;
        }

        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                "í","ì","ỉ","ĩ","ị",
                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e","e","e","e","e","e","e","e","e","e","e",
                "i","i","i","i","i",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                "u","u","u","u","u","u","u","u","u","u","u",
                "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }
    }
    #endregion ConvertFont

    #region SignalR

    public class CommonFunction
    {
        public static void SendProgress(string progressMessage, int progressCount, int totalItems)
        {
            //IN ORDER TO INVOKE SIGNALR FUNCTIONALITY DIRECTLY FROM SERVER SIDE WE MUST USE THIS
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();

            //CALCULATING PERCENTAGE BASED ON THE PARAMETERS SENT
            var percentage = (progressCount * 100) / totalItems;

            //PUSHING DATA TO ALL CLIENTS
            hubContext.Clients.All.AddProgress(progressMessage, percentage + "%");

        }
    }

    #endregion

    #region CommonFileFunction

    public class CommonFileFunction
    {
        public static ResultModel<List<T>> GetListFromDbfReader<T>(Dbf readerDbf, bool ignoreCaseMapping = false)
        {
            var result =  new ResultModel<List<T>>()
            {
                Data = new List<T>()
            };
            try
            {
                //var dr = ExecuteReader(query, commandType);
                if (readerDbf == null || readerDbf.Records.Count == 0)
                    return null;
                var fCount = 1;
                var mType = typeof(T);
                var mList = new List<T>();
                
                foreach (DbfRecord record in readerDbf.Records)
                {
                    var obj = Activator.CreateInstance(mType);
                    var i = 0;
                    foreach (DbfField field in readerDbf.Fields)
                    {
                        if (record[i] == null)
                        {
                            i++;
                            continue;
                        }

                        PropertyInfo p = null;
                        if (ignoreCaseMapping)
                            p = mType.GetProperty(field.Name,
                                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        else
                            p = mType.GetProperty(field.Name);
                        if (p == null)
                        {
                            i++;
                            continue;
                        }

                        try
                        {
                            var type = p.PropertyType;
                            var value = ChangeType(record[i], type);
                            if (type.Name == "String")
                            {
                                
                            }
                            p.SetValue(obj, value, null);
                        }
                        catch (Exception ex)
                        {
                            return new ResultModel<List<T>>(){Data = null,IsError = true, Message = ex.Message};
                        }

                        
                        i++;
                    }
                    
                    mList.Add((T)obj);
                    CommonFunction.SendProgress("Convert dbf file to list ...", fCount, readerDbf.Records.Count);
                    fCount++;
                }

                result.Data = mList;

                readerDbf = null;
                return result;
            }
            catch (Exception ex)
            {
                return new ResultModel<List<T>>() { Data = null, IsError = true, Message = ex.Message };
            }
            finally
            {
                readerDbf = null;
            }
        }
        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t); ;
            }

            return Convert.ChangeType(value, t);
        }

        public static string CheckFileSameNameUpload(string fullPath)
        {
            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(fullPath);
            string extension = Path.GetExtension(fullPath);
            string path = Path.GetDirectoryName(fullPath);
            string newFullPath = fullPath;

            while (File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);
            }

            return newFullPath;
        }
    }

    #endregion

    #region Cryto - Encrypt Decrypt

    public class Crypto
    {
        public static string Sha512Encode(String sInp)
        {
            String strpass = "";
            SHA512 shaM = SHA512.Create();
            ASCIIEncoding enc = new ASCIIEncoding();
            int i;
            shaM.ComputeHash(enc.GetBytes(sInp));
            for (i = 0; i <= shaM.Hash.Length - 1; i++)
                strpass += shaM.Hash[i].ToString("X");
            return strpass;
        }

        #region Encrypt and Decrypt string in MD5

        /// <summary>
        /// Mã hóa ký tự với kiểu mã hõa TripleDes - MD5
        /// </summary>
        /// <param name="key"></param>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public static string EncryptString(string key, string toEncrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            TripleDESCryptoServiceProvider tdes =
            new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Giải mã dữ liệu đã mã hóa TripleDes - MD5
        /// </summary>
        /// <param name="key"></param>
        /// <param name="toDecrypt"></param>
        /// <returns></returns>
        public static string DecryptString(string key, string toDecrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
            toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        #endregion Encrypt and Decrypt string in MD5

    }

    #endregion

    #region Ultility

    public class Utils
    {
        /// <summary>
        /// Convert String to Byte array
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        /// <summary>
        /// Convert byte array to string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        /// <summary>
        /// Convert xâu dd/mm/yyyy về kiểu DateTime
        /// </summary>
        /// <param name="strDate">Xâu dd/mm/yyyy</param>
        /// <returns>DateTime</returns>
        public static DateTime ConvertStringToDatetime(string strDate)
        {
            var arrayDate = strDate.Split('/');
            var tmpDate = new DateTime(Convert.ToInt32(arrayDate[2]), Convert.ToInt32(arrayDate[1]),
                Convert.ToInt32(arrayDate[0]));
            return tmpDate;
        }

        /// <summary>
        /// Convert datetime hệ thống về dạng "thứ ngày/tháng/năm"
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertDateToDateVn(DateTime dateTime)
        {
            return string.Format("{0} {1}", dateTime.ToString("dddd", new CultureInfo("vi-VN")),
                dateTime.ToString("dd/MM/yyyy"));
        }
        
        /// <summary>
        /// Tạo dãy số ngẫu nhiên
        /// </summary>
        /// <param name="size">Độ dài chuỗi</param>
        /// <returns></returns>
        public static string GenerateString(int size)
        {
            Random rand = new Random();
            //public const string Alphabet = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //string Alphabet = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string Alphabet = "0123456789";
            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = Alphabet[rand.Next(Alphabet.Length)];
            }
            return new string(chars);
        }
    }

    #endregion
}
