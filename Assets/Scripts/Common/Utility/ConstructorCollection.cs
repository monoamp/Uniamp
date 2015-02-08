using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Common.Data.Standard.Form.Aiff;
using Monoamp.Common.Data.Standard.Riff.Wave;
using Monoamp.Common.Data.Application.Music;
using Monoamp.Common.Data.Application.Waveform;
using Monoamp.Common.Component.Sound.Player;

namespace Monoamp.Common.Utility
{
	public static class ConstructorCollection
	{
		private static readonly Constructor loaderMusic;
		private static readonly Constructor loaderWaveform;
		private static readonly Constructor loaderPlayer;

		static ConstructorCollection()
		{
			Dictionary<string, Constructor.DConstructor> constructorDictionaryMusic = new Dictionary<string, Constructor.DConstructor>();
			//constructorDictionaryMusic.Add( ".aif", ( string aPathFile ) => { return new MusicAiff( ( FormForm )PoolCollection.poolAif.Get( aPathFile ) ); } );
			//constructorDictionaryMusic.Add( ".aiff", ( string aPathFile ) => { return new MusicAiff( ( FormForm )PoolCollection.poolAif.Get( aPathFile ) ); } );
			//constructorDictionaryMusic.Add( ".wav", ( string aPathFile ) => { return new MusicWave( ( RiffRiff )PoolCollection.poolWav.Get( aPathFile ) ); } );
			//constructorDictionaryMusic.Add( ".wave", ( string aPathFile ) => { return new MusicWave( ( RiffRiff )PoolCollection.poolWav.Get( aPathFile ) ); } );
			constructorDictionaryMusic.Add( ".aif", ( string aPathFile ) => { return new MusicAiff( aPathFile ); } );
			constructorDictionaryMusic.Add( ".aiff", ( string aPathFile ) => { return new MusicAiff( aPathFile ); } );
			constructorDictionaryMusic.Add( ".wav", ( string aPathFile ) => { return new MusicWave( aPathFile ); } );
			constructorDictionaryMusic.Add( ".wave", ( string aPathFile ) => { return new MusicWave( aPathFile ); } );
			loaderMusic = new Constructor( constructorDictionaryMusic );

			Dictionary<string, Constructor.DConstructor> constructorDictionaryWaveform = new Dictionary<string, Constructor.DConstructor>();
			//constructorDictionaryWaveform.Add( ".aif", ( string aPathFile ) => { return new WaveformAiff( PoolCollection.GetAif( aPathFile ) ); } );
			//constructorDictionaryWaveform.Add( ".aiff", ( string aPathFile ) => { return new WaveformAiff( PoolCollection.GetAif( aPathFile ) ); } );
			//constructorDictionaryWaveform.Add( ".wav", ( string aPathFile ) => { return new WaveformWave( PoolCollection.GetWav( aPathFile ) ); } );
			//constructorDictionaryWaveform.Add( ".wave", ( string aPathFile ) => { return new WaveformWave( PoolCollection.GetWav( aPathFile ) ); } );
			constructorDictionaryWaveform.Add( ".aif", ( string aPathFile ) => { return new WaveformAiff( aPathFile ); } );
			constructorDictionaryWaveform.Add( ".aiff", ( string aPathFile ) => { return new WaveformAiff( aPathFile ); } );
			constructorDictionaryWaveform.Add( ".wav", ( string aPathFile ) => { return new WaveformWave( aPathFile ); } );
			constructorDictionaryWaveform.Add( ".wave", ( string aPathFile ) => { return new WaveformWave( aPathFile ); } );
			loaderWaveform = new Constructor( constructorDictionaryWaveform );
			
			Dictionary<string, Constructor.DConstructor> constructorDictionaryPlayer = new Dictionary<string, Constructor.DConstructor>();
			constructorDictionaryPlayer.Add( ".aif", ( string aFilePath ) => { return new PlayerPcm( aFilePath ); } );
			constructorDictionaryPlayer.Add( ".aiff", ( string aFilePath ) => { return new PlayerPcm( aFilePath ); } );
			constructorDictionaryPlayer.Add( ".wav", ( string aFilePath ) => { return new PlayerPcm( aFilePath ); } );
			constructorDictionaryPlayer.Add( ".wave", ( string aFilePath ) => { return new PlayerPcm( aFilePath ); } );
			loaderPlayer = new Constructor( constructorDictionaryPlayer );
		}
		
		public static IMusic LoadMusic( string aPathFile )
		{
			return ( IMusic )loaderMusic.Construct( aPathFile );
		}

		public static WaveformPcm LoadWaveform( string aPathFile )
		{
			return ( WaveformPcm )loaderWaveform.Construct( aPathFile );
		}

		public static IPlayer LoadPlayer( string aPathFile )
		{
			return ( IPlayer )loaderPlayer.Construct( aPathFile );
		}
	}
}
