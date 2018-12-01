﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Common.Dbf.Encoders
{
	internal class LogicalEncoder: IEncoder
	{
		private static LogicalEncoder instance = null;

		private LogicalEncoder() { }

		public static LogicalEncoder Instance
		{
			get
			{
				if (instance == null) instance = new LogicalEncoder();
				return instance;
			}
		}

		public byte[] Encode(DbfField field, object data)
		{
			// Convert boolean value to string.
			string text = "?";
			if(data != null) { 
				text = (bool)data == true ? "Y" : "N";
		  }

		  // Grow string to fill field length.
		  text = text.PadLeft(field.Length, ' ');

			// Convert string to byte array.
			return Encoding.ASCII.GetBytes(text);
		}

        public object Decode(byte[] buffer, byte[] memoData)
        {
            string text = Encoding.ASCII.GetString(buffer).Trim().ToUpper();
            if (text == "?") return null;
            return (text == "Y" || text == "T");
        }
    }
}