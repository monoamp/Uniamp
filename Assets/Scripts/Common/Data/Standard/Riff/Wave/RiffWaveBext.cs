using System;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Riff.Wave
{
	public class RiffWaveBext : RiffChunk
	{
		public const string ID = "bext";

		public readonly string description;	
		public readonly string originator;
		public readonly string originatorReference;
		public readonly string originationDate;
		public readonly string originationTime;
		public readonly UInt32 timeReferenceLow;
		public readonly UInt32 timeReferenceHigh;
		public readonly UInt16 version;
		public readonly Byte[] umid;
		//public readonly Byte umid_63;
		public readonly Byte[] reserved;
		public readonly string codingHistory;

		public RiffWaveBext( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			description = aByteArray.ReadString( 256 );
			originator = aByteArray.ReadString( 32 );
			originatorReference = aByteArray.ReadString( 32 );
			originationDate = aByteArray.ReadString( 10 );
			originationTime = aByteArray.ReadString( 8 );
			timeReferenceLow = aByteArray.ReadUInt32();
			timeReferenceHigh = aByteArray.ReadUInt32();
			version = aByteArray.ReadUInt16();
			umid = aByteArray.ReadBytes( 64 );
			reserved = aByteArray.ReadBytes( 190 );
			codingHistory = aByteArray.ReadString( ( int )( Size - 256 - 32 - 32 - 10 - 8 - 4 - 4 - 2 - 64 - 190 ) );

			informationList.Add( "Description:" + description );
			informationList.Add( "Originator:" + originator );
			informationList.Add( "Originator Reference:" + originatorReference );
			informationList.Add( "Origination Date:" + originationDate );
			informationList.Add( "Origination Time:" + originationTime );
			informationList.Add( "Time Reference Low:" + timeReferenceLow.ToString() );
			informationList.Add( "Time Reference High:" + timeReferenceHigh.ToString() );
			informationList.Add( "Version:" + version.ToString() );
			informationList.Add( "UMID:" + umid.ToString() );
			informationList.Add( "Reserved:" + reserved.ToString() );
			informationList.Add( "Coding History:" + codingHistory );
		}

		class broadcast_audio_extension
		{

		}
	}
}
