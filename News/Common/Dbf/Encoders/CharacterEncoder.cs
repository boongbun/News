using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Common.Dbf.Encoders
{
	internal class CharacterEncoder: IEncoder
	{
		private static CharacterEncoder instance = null;

		private CharacterEncoder() { }

		public static CharacterEncoder Instance
		{
			get
			{
				if (instance == null) instance = new CharacterEncoder();
				return instance;
			}
		}

		public byte[] Encode(DbfField field, object data)
		{
			// Convert data to string. NULL is the empty string.
			string text = data == null ? "" : (string) data;
			// Pad string with spaces.
			while (text.Length < field.Length) text = text + " ";
			// Convert string to byte array.
			return Encoding.ASCII.GetBytes((string)text);
		}

        public object Decode(byte[] buffer, byte[] memoData)
        {
            //string text = Encoding.ASCII.GetString(buffer).Trim();
            string text = Encoding.Default.GetString(buffer).Trim();
            text = Uconvert(text);
            if (text.Length == 0) return null;
            return text;
        }

        public string Uconvert(string strConv)
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
    }
}
