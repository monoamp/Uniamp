using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Monoamp.Common.Data.Application;
using Monoamp.Common.Data.Application.Sound;
using Monoamp.Common.system.io;
using Monoamp.Common.Data.Standard.Riff;
using Monoamp.Common.Data.Standard.Riff.Wave;
using Monoamp.Common.Struct;
using Monoamp.Common.Utility;

using Monoamp.Boundary;

namespace Monoamp.Common.Component.Sound.LoopTool
{
	public static class LoopSearch
	{
		public static void Execute( string aFilePathInput, string aFilePathOutput, InputMusicInformation aData )
		{
			RiffWaveRiff lRiffWaveRiff = ( RiffWaveRiff )PoolCollection.GetRiffWave( aFilePathInput );

			WaveformPcm lWaveform = new WaveformPcm( lRiffWaveRiff );

			SByte[] lSampleArray = new SByte[lWaveform.format.samples];
			
			lSampleArray = lWaveform.data.sampleByteArray[0];

			/*
			for( int i = 0; i < lWaveform.format.samples; i++ )
			{
				lSampleArray[i] = lWaveform.data.GetSampleByte( 0, i );
			}*/

			List<LoopInformation> lLoopList = null;

			try
			{
				lLoopList = LoopSearchTool.Execute( lSampleArray, aData, aFilePathInput );
			}
			catch( Exception aExpection )
			{
				Logger.BreakError( aExpection.ToString() + ":LoopTool Exception" );
			}

			for( int i = 0; i < lLoopList.Count; i++ )
			{
				AddCuePoint( lRiffWaveRiff, ( int )lLoopList[i].start.sample, ( int )lLoopList[i].end.sample );
				AddSampleLoop( lRiffWaveRiff, ( int )lLoopList[i].start.sample, ( int )lLoopList[i].end.sample );
			}

			Byte[] lDataArrayRead = null;
			RiffWaveData dataChunk = ( RiffWaveData )lRiffWaveRiff.GetChunk( RiffWaveData.ID );
			
			using ( FileStream u = new FileStream( lRiffWaveRiff.name, FileMode.Open, FileAccess.Read ) )
			{
				AByteArray l = new ByteArrayLittle( u );
				
				int bytePosition = ( int )dataChunk.position;

				l.SetPosition( bytePosition );
				
				lDataArrayRead = l.ReadBytes( dataChunk.Size );
			}

			SetDataArray( lRiffWaveRiff, lDataArrayRead );
			
			Logger.BreakDebug( "lMemoryStreamWrite" );

			MemoryStream lMemoryStreamWrite = new MemoryStream( ( int )lRiffWaveRiff.Size + 8 );
			ByteArrayLittle lByteArray = new ByteArrayLittle( lMemoryStreamWrite );

			lRiffWaveRiff.WriteByteArray( lByteArray );
			
			Logger.BreakDebug( "WriteByteArray" );
			Logger.BreakDebug( "Out:" + aFilePathOutput );
			
			try
			{
				using( FileStream u = new FileStream( aFilePathOutput, FileMode.Create, FileAccess.Write, FileShare.Read ) )
				{
					u.Write( lMemoryStreamWrite.GetBuffer(), 0, ( int )lMemoryStreamWrite.Length );
				}
			}
			catch( Exception aExpection )
			{
				Logger.BreakError( "Write Exception:" + aExpection );
			}
		}

		public static void AddCuePoint( RiffWaveRiff lRiffWaveRiff, int aStart, int aEnd )
		{
			Logger.BreakDebug( "AddCuePoint" );

			RiffWaveCue_ lRiffWaveCue_ = ( RiffWaveCue_ )lRiffWaveRiff.GetChunk( RiffWaveCue_.ID );

			if( lRiffWaveCue_ == null )
			{
				List<CuePoint> lCuePointList = new List<CuePoint>();
				lCuePointList.Add( new CuePoint( 1, ( UInt32 )aStart, "data", 0, 0, ( UInt32 )aStart ) );
				lCuePointList.Add( new CuePoint( 2, ( UInt32 )aEnd, "data", 0, 0, ( UInt32 )aEnd ) );
				lRiffWaveCue_ = new RiffWaveCue_( lCuePointList );
				lRiffWaveRiff.AddChunk( lRiffWaveCue_ );
			}
			else
			{
				List<CuePoint> lCuePointList = lRiffWaveCue_.cuePoints;
				lCuePointList.Add( new CuePoint( lRiffWaveCue_.points + 1, ( UInt32 )aStart, "data", 0, 0, ( UInt32 )aStart ) );
				lCuePointList.Add( new CuePoint( lRiffWaveCue_.points + 2, ( UInt32 )aEnd, "data", 0, 0, ( UInt32 )aEnd ) );
				lRiffWaveCue_ = new RiffWaveCue_( lCuePointList );
				lRiffWaveRiff.OverrideChunk( lRiffWaveCue_ );
			}
		}

		public static void AddSampleLoop( RiffWaveRiff lRiffWaveRiff, int aStart, int aEnd )
		{
			Logger.BreakDebug( "AddSampleLoop" );

			RiffWaveSmpl lRiffWaveSmpl = ( RiffWaveSmpl )lRiffWaveRiff.GetChunk( RiffWaveSmpl.ID );

			if( lRiffWaveSmpl == null )
			{
				List<SampleLoop> lSampleLoopList = new List<SampleLoop>();
				lSampleLoopList.Add( new SampleLoop( 0, 0, ( UInt32 )aStart, ( UInt32 )aEnd, 0, 0 ) );
				lRiffWaveSmpl = new RiffWaveSmpl( 0, 0, 0, 60, 0, 0, 0, 1, 0, lSampleLoopList );
				lRiffWaveRiff.AddChunk( lRiffWaveSmpl );
			}
			else
			{
				List<SampleLoop> lSmplLoopList = lRiffWaveSmpl.sampleLoopList;
				lSmplLoopList.Add( new SampleLoop( 0, 0, ( UInt32 )aStart, ( UInt32 )aEnd, 0, 0 ) );
				lRiffWaveRiff.OverrideChunk( new RiffWaveSmpl( lRiffWaveSmpl, lSmplLoopList ) );
			}
		}

		public static void SetDataArray( RiffWaveRiff lRiffWaveRiff, Byte[] aSampleData )
		{
			Logger.BreakDebug( "SetDataArray" );

			RiffWaveData dataChunk = ( RiffWaveData )lRiffWaveRiff.GetChunk( RiffWaveData.ID );

			if( dataChunk == null )
			{
				// Error.
			}
			else
			{
				MemoryStream lMemoryStream = new MemoryStream( aSampleData );
				AByteArray lByteArray = new ByteArrayLittle( lMemoryStream );
				lByteArray.WriteBytes( new byte[dataChunk.position] );
				lRiffWaveRiff.OverrideChunk( new RiffWaveData( RiffWaveData.ID, ( UInt32 )aSampleData.Length, lByteArray, lRiffWaveRiff ) );
			}
		}
	}
}
