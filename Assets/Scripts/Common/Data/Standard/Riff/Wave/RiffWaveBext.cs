using System;

using Monoamp.Common.system.io;

namespace Monoamp.Common.Data.Standard.Riff.Wave
{
	public class RiffWaveBext : RiffChunk
	{
		public const string ID = "bext";

		public readonly string description;			// ASCII or S-JIS, Description of the sound sequence
		public readonly string originator;			// ASCII or S-JIS, 制作会社、制作者名
		public readonly string originatorReference;	// ASCII or S-JIS, 制作ユニークコード
		public readonly string originationDate;		// ASCII, 制作年月日 yyyy-mm-dd
		public readonly string originationTime;		// ASCII, 制作時刻 hh:mm:ss
		public readonly UInt32 timeReferenceLow;	// First sample count since midnight, low word
		public readonly UInt32 timeReferenceHigh;	// First sample count since midnight, high word
		public readonly UInt16 version;				// BWF-Jのバージョン番号
		public readonly Byte[] umid;				// Binary Byte 0 of SMPTE UMID
		//public readonly Byte umid_63;				// Binary Byte 63 of SMPTE UMID
		public readonly Byte[] reserved;			// 190 Bytes, reserved for future use, set to “NULL”
		public readonly string codingHistory;		// ASCII or S-JIS, Coding history

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
