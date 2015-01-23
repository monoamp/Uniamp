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
	public static class LoaderCollection
	{
		private static readonly Loader loaderMusic;
		private static readonly Loader loaderWaveform;
		private static readonly Loader loaderPlayer;

		static LoaderCollection()
		{
			Dictionary<string, Loader.Constructor> constructorDictionaryMusic = new Dictionary<string, Loader.Constructor>();
			//constructorDictionaryMusic.Add( ".aif", ( string aPathFile ) => { return new MusicAiff( ( FormForm )PoolCollection.poolAif.Get( aPathFile ) ); } );
			//constructorDictionaryMusic.Add( ".aiff", ( string aPathFile ) => { return new MusicAiff( ( FormForm )PoolCollection.poolAif.Get( aPathFile ) ); } );
			//constructorDictionaryMusic.Add( ".wav", ( string aPathFile ) => { return new MusicWave( ( RiffRiff )PoolCollection.poolWav.Get( aPathFile ) ); } );
			//constructorDictionaryMusic.Add( ".wave", ( string aPathFile ) => { return new MusicWave( ( RiffRiff )PoolCollection.poolWav.Get( aPathFile ) ); } );
			constructorDictionaryMusic.Add( ".aif", ( string aPathFile ) => { return new MusicAiff( aPathFile ); } );
			constructorDictionaryMusic.Add( ".aiff", ( string aPathFile ) => { return new MusicAiff( aPathFile ); } );
			constructorDictionaryMusic.Add( ".wav", ( string aPathFile ) => { return new MusicWave( aPathFile ); } );
			constructorDictionaryMusic.Add( ".wave", ( string aPathFile ) => { return new MusicWave( aPathFile ); } );
			loaderMusic = new Loader( constructorDictionaryMusic );

			Dictionary<string, Loader.Constructor> constructorDictionaryWaveform = new Dictionary<string, Loader.Constructor>();
			//constructorDictionaryWaveform.Add( ".aif", ( string aPathFile ) => { return new WaveformAiff( PoolCollection.GetAif( aPathFile ) ); } );
			//constructorDictionaryWaveform.Add( ".aiff", ( string aPathFile ) => { return new WaveformAiff( PoolCollection.GetAif( aPathFile ) ); } );
			//constructorDictionaryWaveform.Add( ".wav", ( string aPathFile ) => { return new WaveformWave( PoolCollection.GetWav( aPathFile ) ); } );
			//constructorDictionaryWaveform.Add( ".wave", ( string aPathFile ) => { return new WaveformWave( PoolCollection.GetWav( aPathFile ) ); } );
			constructorDictionaryWaveform.Add( ".aif", ( string aPathFile ) => { return new WaveformAiff( aPathFile ); } );
			constructorDictionaryWaveform.Add( ".aiff", ( string aPathFile ) => { return new WaveformAiff( aPathFile ); } );
			constructorDictionaryWaveform.Add( ".wav", ( string aPathFile ) => { return new WaveformWave( aPathFile ); } );
			constructorDictionaryWaveform.Add( ".wave", ( string aPathFile ) => { return new WaveformWave( aPathFile ); } );
			loaderWaveform = new Loader( constructorDictionaryWaveform );
			
			Dictionary<string, Loader.Constructor> constructorDictionaryPlayer = new Dictionary<string, Loader.Constructor>();
			constructorDictionaryPlayer.Add( ".aif", ( string aFilePath ) => { return new PlayerPcm( aFilePath ); } );
			constructorDictionaryPlayer.Add( ".aiff", ( string aFilePath ) => { return new PlayerPcm( aFilePath ); } );
			constructorDictionaryPlayer.Add( ".wav", ( string aFilePath ) => { return new PlayerPcm( aFilePath ); } );
			constructorDictionaryPlayer.Add( ".wave", ( string aFilePath ) => { return new PlayerPcm( aFilePath ); } );
			loaderPlayer = new Loader( constructorDictionaryPlayer );
		}
		
		public static IMusic LoadMusic( string aPathFile )
		{
			return ( IMusic )loaderMusic.Load( aPathFile );
		}

		public static IWaveform LoadWaveform( string aPathFile )
		{
			return ( IWaveform )loaderWaveform.Load( aPathFile );
		}

		public static IPlayer LoadPlayer( string aPathFile )
		{
			return ( IPlayer )loaderPlayer.Load( aPathFile );
		}
	}
}
