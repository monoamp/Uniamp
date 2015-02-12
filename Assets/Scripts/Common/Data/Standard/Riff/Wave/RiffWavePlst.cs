using System;
using System.Collections.Generic;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff.Wave
{
	public class RiffWavePlst : RiffChunk
	{
		public const string ID = "plst";

		public readonly UInt32 segments;
		public readonly List<PlaySegment> PlaySegments;

		public RiffWavePlst( string aId, UInt32 aSize, AByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			segments = aByteArray.ReadUInt32();

			informationList.Add( "Segments:" + segments );

			PlaySegments = new List<PlaySegment>();

			for( int i = 0; i < segments; i++ )
			{
				informationList.Add( "----------------" );

				PlaySegments.Add( new PlaySegment( aByteArray, informationList ) );
			}
		}
	}

	public class PlaySegment
	{
		public readonly UInt32 name;
		public readonly UInt32 length;
		public readonly UInt32 loops;

		public PlaySegment( AByteArray aByteArray, List<string> aInformationList )
		{
			name = aByteArray.ReadUInt32();
			length = aByteArray.ReadUInt32();
			loops = aByteArray.ReadUInt32();

			aInformationList.Add( "    Name:" + name );
			aInformationList.Add( "    Length:" + length );
			aInformationList.Add( "    Loops:" + loops );
		}
	}
}
