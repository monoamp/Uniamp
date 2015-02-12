using System;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Form.Aiff
{
	public class FormAiffComm : FormChunk
	{
		public const string ID = "COMM";

		public readonly UInt16 numberOfChannels;
		public readonly UInt32 numberOfFrames;
		public readonly UInt16 bitsPerSamples;
		public readonly Double sampleRate;

		public FormAiffComm( string aId, UInt32 aSize, AByteArray aByteArray, FormChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			numberOfChannels = aByteArray.ReadUInt16();
			numberOfFrames = aByteArray.ReadUInt32();
			bitsPerSamples = aByteArray.ReadUInt16();
			sampleRate = aByteArray.ReadExtendedFloatPoint();

			informationList.Add( "Number Of Channels:" + numberOfChannels );
			informationList.Add( "Number Of Frames:" + numberOfFrames );
			informationList.Add( "Bits Per Samples:" + bitsPerSamples );
			informationList.Add( "Sample Rate:" + sampleRate );
		}
	}
}
