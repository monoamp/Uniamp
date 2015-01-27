using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

using Monoamp.Common.Data.Application;
using Monoamp.Common.Data.Application.Waveform;
using Monoamp.Common.system.io;
using Monoamp.Common.Data.Standard.Riff;
using Monoamp.Common.Data.Standard.Riff.Wave;
using Monoamp.Common.Struct;
using Monoamp.Common.Utility;

using Monoamp.Boundary;

namespace Curan.Common.ApplicationComponent.Sound.LoopTool
{
	public static class LoopSearchExecutor
	{
		public static bool IsCutLast{ get; set; }

		static LoopSearchExecutor()
		{
			IsCutLast = false;
		}

		public static void Execute( string aFilePathInput, string aFilePathOutput, List<double> aProgressList, int aIndex )
		{
			RiffWaveRiff lRiffWaveRiff = ( RiffWaveRiff )PoolCollection.GetRiffWave( aFilePathInput );
			List<RiffChunk> lChunkList = lRiffWaveRiff.chunkList;

			WaveformPcm waveform = new WaveformPcm( lRiffWaveRiff );
			//waveform = Loader..Load( aFileInput.FullName );

			SByte[] lSampleArray = new SByte[waveform.format.samples];

			for( int i = 0; i < waveform.format.samples; i++ )
			{
				lSampleArray[i] = ( SByte )( waveform.data.GetSampleData( 0, i ) >> 8 );
			}

			List<LoopInformation> lLoopList = null;

			try
			{
				lLoopList = LoopSearchTool.Execute( lSampleArray, aProgressList, aIndex );
			}
			catch( Exception aExpection )
			{
				UnityEngine.Debug.Log( aExpection.ToString() + ":LoopTool Exception" );
			}

			//for( int i = 0; i < lLoopList.Count; i++ )
			if ( lLoopList.Count >= 1 )
			{
				//lRiffChunkListWave.AddCuePoint( ( int )lLoopList[i].start.sample, ( int )lLoopList[i].end.sample );
				//lRiffChunkListWave.AddSampleLoop( ( int )lLoopList[i].start.sample, ( int )lLoopList[i].end.sample );
				AddCuePoint( lRiffWaveRiff, ( int )lLoopList[0].start.sample, ( int )lLoopList[0].end.sample );
				AddSampleLoop( lRiffWaveRiff, ( int )lLoopList[0].start.sample, ( int )lLoopList[0].end.sample );
			}

			Byte[] lDataArrayRead = null;
			RiffWaveData dataChunk = ( RiffWaveData )lRiffWaveRiff.GetChunk( RiffWaveData.ID );
			
			using ( FileStream u = new FileStream( lRiffWaveRiff.name, FileMode.Open, FileAccess.Read ) )
			{
				ByteArray l = new ByteArrayLittle( u );
				
				int bytePosition = ( int )dataChunk.position;

				l.SetPosition( bytePosition );
				
				lDataArrayRead = l.ReadBytes( dataChunk.Size );
			}

			Byte[] lDataArrayWrite = lDataArrayRead;

			if( IsCutLast == true )
			{
				lDataArrayWrite = new Byte[( ( int )lLoopList[0].end.sample + 1 ) * 4];

				for( int i = 0; i < ( lLoopList[0].end.sample + 1 ) * 4; i++ )
				{
					lDataArrayWrite[i] = lDataArrayRead[i];
				}
			}
			
			for( int i = 0; i < 64; i++ )
			{
				Logger.BreakDebug( i.ToString() + ":" + lDataArrayWrite[i] );
			}

			SetDataArray( lRiffWaveRiff, lDataArrayWrite );

			MemoryStream lMemoryStreamWrite = new MemoryStream( ( int )lRiffWaveRiff.Size + 8 );
			ByteArrayLittle lByteArray = new ByteArrayLittle( lMemoryStreamWrite );

			//lByteArrayRead.Open();
			lRiffWaveRiff.WriteByteArray( null, lByteArray );
			//lByteArrayRead.Close();

			using( FileStream u = new FileStream( aFilePathOutput, FileMode.Create, FileAccess.Write ) )
			{
				u.Write( lMemoryStreamWrite.GetBuffer(), 0, ( int )lMemoryStreamWrite.Length );
			}
		}
		
		public static void AddCuePoint( RiffWaveRiff lRiffWaveRiff, int aStart, int aEnd )
		{
			RiffWaveCue_ cue_Chunk = ( RiffWaveCue_ )lRiffWaveRiff.GetChunk( RiffWaveCue_.ID );

			if( cue_Chunk == null )
			{
				List<CuePoint> lCuePointList = new List<CuePoint>();
				
				lCuePointList.Add( new CuePoint( 1, ( UInt32 )aStart, "data", 0, 0, ( UInt32 )aStart ) );
				lCuePointList.Add( new CuePoint( 2, ( UInt32 )aEnd, "data", 0, 0, ( UInt32 )aEnd ) );
				
				RiffWaveCue_ lCue_Body = new RiffWaveCue_( lCuePointList );

				lRiffWaveRiff.AddChunk( lCue_Body );
				cue_Chunk = lCue_Body;
			}
			else
			{
				List<CuePoint> lCuePointList = cue_Chunk.cuePoints;
				
				lCuePointList.Add( new CuePoint( cue_Chunk.points + 1, ( UInt32 )aStart, "data", 0, 0, ( UInt32 )aStart ) );
				lCuePointList.Add( new CuePoint( cue_Chunk.points + 2, ( UInt32 )aEnd, "data", 0, 0, ( UInt32 )aEnd ) );

				RiffWaveCue_ lCue_Body = new RiffWaveCue_( lCuePointList );
				lRiffWaveRiff.OverrideChunk( lCue_Body );
			}
		}

		public static void AddSampleLoop( RiffWaveRiff lRiffWaveRiff, int aStart, int aEnd )
		{
			RiffWaveSmpl smplChunk = ( RiffWaveSmpl )lRiffWaveRiff.GetChunk( RiffWaveSmpl.ID );

			if( smplChunk == null )
			{
				List<SampleLoop> lSampleLoopList = new List<SampleLoop>();
				
				lSampleLoopList.Add( new SampleLoop( 0, 0, ( UInt32 )aStart, ( UInt32 )aEnd, 0, 0 ) );
				
				RiffWaveSmpl lChunkSmpl = new RiffWaveSmpl( 0, 0, 0, 60, 0, 0, 0, 1, 0, lSampleLoopList );
				lRiffWaveRiff.AddChunk( lChunkSmpl );
				smplChunk = lChunkSmpl;
			}
			else
			{
				List<SampleLoop> lSmplLoopList = smplChunk.sampleLoopList;
				
				lSmplLoopList.Add( new SampleLoop( 0, 0, ( UInt32 )aStart, ( UInt32 )aEnd, 0, 0 ) );
				
				lRiffWaveRiff.OverrideChunk( new RiffWaveSmpl( smplChunk, lSmplLoopList ) );
			}
		}
		
		public static void SetDataArray( RiffWaveRiff lRiffWaveRiff, Byte[] aSampleData )
		{
			RiffWaveData dataChunk = ( RiffWaveData )lRiffWaveRiff.GetChunk( RiffWaveData.ID );

			if( dataChunk == null )
			{
				// Error.
			}
			else
			{
				MemoryStream lMemoryStream = new MemoryStream( aSampleData );
				ByteArray lByteArray = new ByteArrayLittle( lMemoryStream );
				lByteArray.WriteBytes( new byte[dataChunk.position] );
				lRiffWaveRiff.OverrideChunk( new RiffWaveData( RiffWaveData.ID, ( UInt32 )aSampleData.Length, lByteArray, lRiffWaveRiff ) );
			}
		}
	}
}
