using System;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff.Wave
{
	public class RiffWaveFmt_ : RiffChunk
	{
		public const string ID = "fmt ";

		public readonly FormatTag formatTag;
		public readonly UInt16 channels;
		public readonly UInt32 samplesPerSec;
		public readonly UInt32 averageBytesPerSec;
		public readonly UInt16 blockAlign;
		public readonly UInt16 bitsPerSample;

		public RiffWaveFmt_( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			formatTag = ( FormatTag )aByteArray.ReadUInt16();
			channels = aByteArray.ReadUInt16();
			samplesPerSec = aByteArray.ReadUInt32();
			averageBytesPerSec = aByteArray.ReadUInt32();
			blockAlign = aByteArray.ReadUInt16();
			bitsPerSample = aByteArray.ReadUInt16();

			informationList.Add( "Format Tag:" + formatTag );
			informationList.Add( "Channels:" + channels );
			informationList.Add( "Samples Per Sec:" + samplesPerSec );
			informationList.Add( "Average Bytes Per Sec:" + averageBytesPerSec );
			informationList.Add( "Block Align:" + blockAlign );
			informationList.Add( "Bits Per Sample:" + bitsPerSample );
			
			Logger.BreakDebug( "Size" + aSize );
		}

		public RiffWaveFmt_( FormatTag aTag, UInt16 aChannels, UInt32 aSamplesPerSec, UInt32 aAverageBytesPerSec, UInt16 aBlockAlign, UInt16 aBitsPerSample, RiffChunkList aParent )
			: base( ID, 16, null, null )
		{
			formatTag = aTag;
			channels = aChannels;
			samplesPerSec = aSamplesPerSec;
			averageBytesPerSec = aAverageBytesPerSec;
			blockAlign = aBlockAlign;
			bitsPerSample = aBitsPerSample;
		}

		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			for( int i = 0; i < id.Length; i++ )
			{
				aByteArray.WriteUByte( ( Byte )id[i] );
			}

			aByteArray.WriteUInt32( ( UInt32 )Size );

			aByteArray.WriteUInt16( ( UInt16 )formatTag );
			aByteArray.WriteUInt16( channels );
			aByteArray.WriteUInt32( samplesPerSec );
			aByteArray.WriteUInt32( averageBytesPerSec );
			aByteArray.WriteUInt16( blockAlign );
			aByteArray.WriteUInt16( bitsPerSample );

			for( int i = 0; i < Size - 16; i++ )
			{
				aByteArray.WriteUByte( 0x00 );
			}
		}

		public enum FormatTag
		{
			WAVE_FORMAT_G723_ADPCM =			0x0014,			/* Antex Electronics Corporation */
			WAVE_FORMAT_ANTEX_ADPCME =			0x0033,			/* Antex Electronics Corporation */
			WAVE_FORMAT_G721_ADPCM =			0x0040,			/* Antex Electronics Corporation */
			WAVE_FORMAT_APTX =					0x0025,			/* Audio Processing Technology */
			WAVE_FORMAT_AUDIOFILE_AF36 =		0x0024,			/* Audiofile, Inc. */
			WAVE_FORMAT_AUDIOFILE_AF10 =		0x0026,			/* Audiofile, Inc. */
			WAVE_FORMAT_CONTROL_RES_VQLPC =		0x0034,			/* Control Resources Limited */
			WAVE_FORMAT_CONTROL_RES_CR10 =		0x0037,			/* Control Resources Limited */
			WAVE_FORMAT_CREATIVE_ADPCM =		0x0200,			/* Creative Labs, Inc */
			WAVE_FORMAT_DOLBY_AC2 =				0x0030,			/* Dolby Laboratories */
			WAVE_FORMAT_DSPGROUP_TRUESPEECH =	0x0022,			/* DSP Group, Inc */
			WAVE_FORMAT_DIGISTD =				0x0015,			/* DSP Solutions, Inc. */
			WAVE_FORMAT_DIGIFIX =				0x0016,			/* DSP Solutions, Inc. */
			WAVE_FORMAT_DIGIREAL =				0x0035,			/* DSP Solutions, Inc. */
			WAVE_FORMAT_DIGIADPCM =				0x0036,			/* DSP Solutions, Inc. */
			WAVE_FORMAT_ECHOSC1 =				0x0023,			/* Echo Speech Corporation */
			WAVE_FORMAT_FM_TOWNS_SND =			0x0300,			/* Fujitsu Corp. */
			WAVE_FORMAT_IBM_CVSD =				0x0005,			/* IBM Corporation */
			WAVE_FORMAT_OLIGSM =				0x1000,			/* Ing C. Olivetti & C., S.p.A. */
			WAVE_FORMAT_OLIADPCM =				0x1001,			/* Ing C. Olivetti & C., S.p.A. */
			WAVE_FORMAT_OLICELP =				0x1002,			/* Ing C. Olivetti & C., S.p.A. */
			WAVE_FORMAT_OLISBC =				0x1003,			/* Ing C. Olivetti & C., S.p.A. */
			WAVE_FORMAT_OLIOPR =				0x1004,			/* Ing C. Olivetti & C., S.p.A. */
			WAVE_FORMAT_IMA_ADPCM =				0x0011,			/* Intel Corporation */
			WAVE_FORMAT_DVI_ADPCM =				0x0011,			/* Intel Corporation */
			WAVE_FORMAT_UNKNOWN =				0x0000,			/* Microsoft Corporation */
			WAVE_FORMAT_PCM =					0x0001,			/* Microsoft Corporation */
			WAVE_FORMAT_ADPCM =					0x0002,			/* Microsoft Corporation */
			WAVE_FORMAT_ALAW =					0x0006,			/* Microsoft Corporation */
			WAVE_FORMAT_MULAW =					0x0007,			/* Microsoft Corporation */
			WAVE_FORMAT_GSM610 =				0x0031,			/* Microsoft Corporation */
			WAVE_FORMAT_MPEG =					0x0050,			/* Microsoft Corporation */
			WAVE_FORMAT_NMS_VBXADPCM =			0x0038,			/* Natural MicroSystems */
			WAVE_FORMAT_OKI_ADPCM =				0x0010,			/* OKI */
			WAVE_FORMAT_SIERRA_ADPCM =			0x0013,			/* Sierra Semiconductor Corp */
			WAVE_FORMAT_SONARC =				0x0021,			/* Speech Compression */
			WAVE_FORMAT_MEDIASPACE_ADPCM =		0x0012,			/* Videologic */
			WAVE_FORMAT_YAMAHA_ADPCM =			0x0020			/* Yamaha Corporation of America */
		};
	}
}
