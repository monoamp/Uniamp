using System;
using System.Collections.Generic;

using Monoamp.Common.system.io;
using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Form
{
	public class FormUnknown : FormChunk
	{
		public readonly Byte[] dataArray;

		public FormUnknown( string aId, UInt32 aSize, AByteArray aByteArray, FormChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			Logger.Debug( "Unknown ID:" + aId + ", Size:" + aSize );

			if( position + Size <= aByteArray.Length )
			{
				dataArray = aByteArray.ReadBytes( ( int )Size );
			}
		}
	}
}
