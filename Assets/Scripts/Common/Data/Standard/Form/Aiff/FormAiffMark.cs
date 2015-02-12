using System;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Form.Aiff
{
	public class FormAiffMark : FormChunk
	{
		public const string ID = "MARK";

		public FormAiffMark( string aId, UInt32 aSize, AByteArray aByteArray, FormChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			aByteArray.AddPosition( ( int )Size );
		}
	}
}
