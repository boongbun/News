﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Common.Dbf.Encoders
{
	internal class FloatEncoder: IEncoder
	{
		private static FloatEncoder instance = null;

		private FloatEncoder() { }

		public static FloatEncoder Instance
		{
			get
			{
				if (instance == null) instance = new FloatEncoder();
				return instance;
			}
		}

		public byte[] Encode(DbfField field, object data)
		{
			string text = Convert.ToString(data).PadLeft(field.Length, ' ');
			if (text.Length > field.Length) text.Substring(0, field.Length);
			return Encoding.ASCII.GetBytes(text);
		}

        public object Decode(byte[] buffer, byte[] memoData)
        {
            string text = Encoding.ASCII.GetString(buffer).Trim();
            if (text.Length == 0) return null;
            return Convert.ToSingle(text);
        }
    }
}
