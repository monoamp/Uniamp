using System;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Riff.Wave
{
	public class RiffWaveInst : RiffChunk
	{
		public const string ID = "inst";

		public readonly UInt32 name;
		public readonly string data;

		public readonly Byte unshiftedNote;
		public readonly SByte fineTune;
		public readonly SByte gain;
		public readonly Byte lowNote;
		public readonly Byte highNote;
		public readonly Byte lowVelocity;
		public readonly Byte highVelocity;

		public RiffWaveInst( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			unshiftedNote = aByteArray.ReadByte();
			fineTune = aByteArray.ReadSByte();
			gain = aByteArray.ReadSByte();
			lowNote = aByteArray.ReadByte();
			highNote = aByteArray.ReadByte();
			lowVelocity = aByteArray.ReadByte();
			highVelocity = aByteArray.ReadByte();

			informationList.Add( "    Unshifted Note:" + unshiftedNote );
			informationList.Add( "    Fine Tune:" + fineTune );
			informationList.Add( "    Gain:" + gain );
			informationList.Add( "    Low Note:" + lowNote );
			informationList.Add( "    High Note:" + highNote );
			informationList.Add( "    Low Velocity:" + lowVelocity );
			informationList.Add( "    High Velocity:" + highVelocity );
		}
	}
}
