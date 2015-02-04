using System;
using System.Collections.Generic;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff.Wave
{
	public class RiffWaveSmpl : RiffChunk
	{
		public const string ID = "smpl";

		public readonly UInt32 manufacturer;
		public readonly UInt32 product;
		public readonly UInt32 samplePeriod;
		public readonly UInt32 midiUnityNote;
		public readonly UInt32 midiPitchFraction;
		public readonly UInt32 smpteFormat;
		public readonly UInt32 smpteOffset;
		public readonly UInt32 sampleLoops;
		public readonly UInt32 samplerData;
		public readonly List<SampleLoop> sampleLoopList;

		public RiffWaveSmpl( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{
			manufacturer = aByteArray.ReadUInt32();
			product = aByteArray.ReadUInt32();
			samplePeriod = aByteArray.ReadUInt32();
			midiUnityNote = aByteArray.ReadUInt32();
			midiPitchFraction = aByteArray.ReadUInt32();
			smpteFormat = aByteArray.ReadUInt32();
			smpteOffset = aByteArray.ReadUInt32();
			sampleLoops = aByteArray.ReadUInt32();
			samplerData = aByteArray.ReadUInt32();

			informationList.Add( "Manufacturer:" + manufacturer );
			informationList.Add( "Product:" + product );
			informationList.Add( "Sample Period:" + samplePeriod );
			informationList.Add( "Midi Unity Note:" + midiUnityNote );
			informationList.Add( "Midi Pitch Fraction:" + midiPitchFraction );
			informationList.Add( "SMPTE Format:" + smpteFormat );
			informationList.Add( "SMPTE Offset:" + smpteOffset );
			informationList.Add( "Sample Loops:" + sampleLoops );
			informationList.Add( "Sampler Data:" + samplerData );

			sampleLoopList = new List<SampleLoop>();

			for( int i = 0; i < sampleLoops; i++ )
			{
				informationList.Add( "----------------" );

				sampleLoopList.Add( new SampleLoop( aByteArray, informationList ) );
			}
		}

		public RiffWaveSmpl( UInt32 aManufacturer, UInt32 aProduct, UInt32 aSamplePeriod, UInt32 aMidiUnityNote, UInt32 aMidiPitchFraction, UInt32 aSmpteFormat, UInt32 aSmpteOffset, UInt32 aSampleLoops, UInt32 aSamplerData, List<SampleLoop> aSampleLoopList )
			: base( ID, 4 * 9 + 24 * aSampleLoops, null, null )
		{
			manufacturer = aManufacturer;
			product = aProduct;
			samplePeriod = aSamplePeriod;
			midiUnityNote = aMidiUnityNote;
			midiPitchFraction = aMidiPitchFraction;
			smpteFormat = aSmpteFormat;
			smpteOffset = aSmpteOffset;
			sampleLoops = aSampleLoops;
			samplerData = aSamplerData;
			sampleLoopList = aSampleLoopList;

			informationList.Add( "Manufacturer:" + manufacturer );
			informationList.Add( "Product:" + product );
			informationList.Add( "Sample Period:" + samplePeriod );
			informationList.Add( "Midi Unity Note:" + midiUnityNote );
			informationList.Add( "Midi Pitch Fraction:" + midiPitchFraction );
			informationList.Add( "SMPTE Format:" + smpteFormat );
			informationList.Add( "SMPTE Offset:" + smpteOffset );
			informationList.Add( "Sample Loops:" + sampleLoops );
			informationList.Add( "Sampler Data:" + samplerData );
		}

		public RiffWaveSmpl( RiffWaveSmpl aSmplChunk, List<SampleLoop> aSampleLoopList )
			: base( ID, ( UInt32 )( 4 * 9 + 24 * aSampleLoopList.Count ), null, null )
		{
			manufacturer = aSmplChunk.manufacturer;
			product = aSmplChunk.product;
			samplePeriod = aSmplChunk.samplePeriod;
			midiUnityNote = aSmplChunk.midiUnityNote;
			midiPitchFraction = aSmplChunk.midiPitchFraction;
			smpteFormat = aSmplChunk.smpteFormat;
			smpteOffset = aSmplChunk.smpteOffset;
			sampleLoops = ( UInt32 )aSampleLoopList.Count;
			samplerData = aSmplChunk.samplerData;
			sampleLoopList = aSampleLoopList;

			informationList.Add( "Manufacturer:" + manufacturer );
			informationList.Add( "Product:" + product );
			informationList.Add( "Sample Period:" + samplePeriod );
			informationList.Add( "Midi Unity Note:" + midiUnityNote );
			informationList.Add( "Midi Pitch Fraction:" + midiPitchFraction );
			informationList.Add( "SMPTE Format:" + smpteFormat );
			informationList.Add( "SMPTE Offset:" + smpteOffset );
			informationList.Add( "Sample Loops:" + sampleLoops );
			informationList.Add( "Sampler Data:" + samplerData );
		}

		public override void WriteByteArray( ByteArray aByteArrayRead, ByteArray aByteArray )
		{
			for( int i = 0; i < id.Length; i++ )
			{
				aByteArray.WriteUByte( ( Byte )id[i] );
			}

			aByteArray.WriteUInt32( ( UInt32 )Size );

			aByteArray.WriteUInt32( manufacturer );
			aByteArray.WriteUInt32( product );
			aByteArray.WriteUInt32( samplePeriod );
			aByteArray.WriteUInt32( midiUnityNote );
			aByteArray.WriteUInt32( midiPitchFraction );
			aByteArray.WriteUInt32( smpteFormat );
			aByteArray.WriteUInt32( smpteOffset );
			aByteArray.WriteUInt32( sampleLoops );
			aByteArray.WriteUInt32( samplerData );

			for( int i = 0; i < sampleLoops; i++ )
			{
				sampleLoopList[i].WriteByteArray( aByteArray );
			}
		}
	}

	public class SampleLoop
	{
		public readonly UInt32 cuePointId;
		public readonly UInt32 type;
		public readonly UInt32 start;
		public readonly UInt32 end;
		public readonly UInt32 fraction;
		public readonly UInt32 playCount;

		public SampleLoop( ByteArray aByteArray, List<string> aInformationList )
		{
			cuePointId = aByteArray.ReadUInt32();
			type = aByteArray.ReadUInt32();
			start = aByteArray.ReadUInt32();
			end = aByteArray.ReadUInt32();
			fraction = aByteArray.ReadUInt32();
			playCount = aByteArray.ReadUInt32();

			aInformationList.Add( "Cue Point ID:" + cuePointId );
			aInformationList.Add( "Type:" + type );
			aInformationList.Add( "Start:" + start );
			aInformationList.Add( "End:" + end );
			aInformationList.Add( "Fraction:" + fraction );
			aInformationList.Add( "Play Count:" + playCount );
		}

		public SampleLoop( UInt32 aCuePointId, UInt32 aType, UInt32 aStart, UInt32 aEnd, UInt32 aFraction, UInt32 aPlayCount )
		{
			cuePointId = aCuePointId;
			type = aType;
			start = aStart;
			end = aEnd;
			fraction = aFraction;
			playCount = aPlayCount;
		}

		public void WriteByteArray( ByteArray aByteArray )
		{
			aByteArray.WriteUInt32( cuePointId );
			aByteArray.WriteUInt32( type );
			aByteArray.WriteUInt32( start );
			aByteArray.WriteUInt32( end );
			aByteArray.WriteUInt32( fraction );
			aByteArray.WriteUInt32( playCount );
		}
	}
}
