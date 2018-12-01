using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Common.Dbf.Encoders
{
	internal interface IEncoder
	{
		byte[] Encode(DbfField field, object data);
        object Decode(byte[] buffer, byte[] memoData);
    }
}
