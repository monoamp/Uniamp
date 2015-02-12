using System;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Form.Aiff
{
	public class FormAiffChan : FormChunk
	{
		public const string ID = "CHAN";

		public FormAiffChan( string aId, UInt32 aSize, AByteArray aByteArray, FormChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			aByteArray.AddPosition( ( int )Size );
		}
	}
}
