using System;
using System.Collections.Generic;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff.Wave
{
	public class RiffWaveCue_ : RiffChunk
	{
		public const string ID = "cue ";

		public readonly UInt32 points;
		public readonly List<CuePoint> cuePoints;

		public override UInt32 Size {
			get{ return ( UInt32 )( 4 + 24 * cuePoints.Count ); }
			protected set{ base.Size = value; }
		}

		public RiffWaveCue_( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			points = aByteArray.ReadUInt32();

			informationList.Add( "Points:" + points );

			cuePoints = new List<CuePoint>();

			for( int i = 0; i < points; i++ )
			{
				informationList.Add( "----------------" );

				cuePoints.Add( new CuePoint( aByteArray, informationList ) );
			}
		}

		public RiffWaveCue_( List<CuePoint> aCuePoints )
			: base( ID, ( UInt32 )( 4 + 24 * aCuePoints.Count ), null, null )
		{
			points = ( UInt32 )aCuePoints.Count;
			cuePoints = aCuePoints;
		}

		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			for( int i = 0; i < id.Length; i++ )
			{
				aByteArray.WriteUByte( ( Byte )id[i] );
			}

			aByteArray.WriteUInt32( ( UInt32 )Size );

			aByteArray.WriteUInt32( points );

			for( int i = 0; i < points; i++ )
			{
				cuePoints[i].WriteByteArray( aByteArray );
			}
		}
	}

	public class CuePoint
	{
		public readonly UInt32 name;
		public readonly UInt32 position;
		public readonly string chunk;
		public readonly UInt32 chunkStart;
		public readonly UInt32 blockStart;
		public readonly UInt32 sampleOffset;

		public CuePoint( ByteArray aByteArray, List<string> aInformationList )
		{
			name = aByteArray.ReadUInt32();
			position = aByteArray.ReadUInt32();
			chunk = aByteArray.ReadString( 4 );
			chunkStart = aByteArray.ReadUInt32();
			blockStart = aByteArray.ReadUInt32();
			sampleOffset = aByteArray.ReadUInt32();

			aInformationList.Add( "    Name:" + name );
			aInformationList.Add( "    Position:" + position );
			aInformationList.Add( "    Chunk:" + chunk );
			aInformationList.Add( "    Chunk Start:" + chunkStart );
			aInformationList.Add( "    Block Start:" + blockStart );
			aInformationList.Add( "    Sample Offset:" + sampleOffset );
		}

		public CuePoint( UInt32 aIdentifier, UInt32 aPosition, string aId, UInt32 aChunkStart, UInt32 aBlockStart, UInt32 aSampleOffset )
		{
			name = aIdentifier;
			position = aPosition;
			chunk = aId;
			chunkStart = aChunkStart;
			blockStart = aBlockStart;
			sampleOffset = aSampleOffset;
		}

		public void WriteByteArray( ByteArray aByteArray )
		{
			aByteArray.WriteUInt32( name );
			aByteArray.WriteUInt32( position );
			aByteArray.WriteString( chunk );
			aByteArray.WriteUInt32( chunkStart );
			aByteArray.WriteUInt32( blockStart );
			aByteArray.WriteUInt32( sampleOffset );
		}
	}
}
