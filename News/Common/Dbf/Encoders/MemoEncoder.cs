﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Common.Dbf.Encoders
{
	internal class MemoEncoder: IEncoder
	{
		private static MemoEncoder instance = null;

		private MemoEncoder() { }

		public static MemoEncoder Instance
		{
			get
			{
				if (instance == null) instance = new MemoEncoder();
				return instance;
			}
		}

		public byte[] Encode(DbfField field, object data)
		{
			return null;
		}

        public object Decode(byte[] buffer, byte[] memoData)
        {
            int index = 0;
            // Memo fields of 5+ byts in length store their index in text, e.g. "     39394"
            // Memo fields of 4 bytes store their index as an int.
            if (buffer.Length > 4)
            {
                string text = Encoding.ASCII.GetString(buffer).Trim();
                if (text.Length == 0) return null;
                index = Convert.ToInt32(text);
            }
            else
            {
                index = BitConverter.ToInt32(buffer, 0);
                if (index == 0) return null;
            }
            return findMemo(index, memoData);
        }

        private static string findMemo(int index, byte[] memoData)
        {
            // The index is measured from the start of the file, even though the memo file header blocks takes
            // up the first few index positions.
            UInt16 blockSize = BitConverter.ToUInt16(memoData.Skip(6).Take(2).Reverse().ToArray(), 0);
            int type = (int)BitConverter.ToUInt32(memoData.Skip(index * blockSize).Take(4).Reverse().ToArray(), 0);
            int length = (int)BitConverter.ToUInt32(memoData.Skip(index * blockSize + 4).Take(4).Reverse().ToArray(), 0);
            string text = Encoding.ASCII.GetString(memoData.Skip(index * blockSize + 8).Take(length).ToArray()).Trim();
            return text;
        }
    }
}
