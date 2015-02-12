using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Common.Data.Standard.Form.Aiff;
using Monoamp.Common.Data.Standard.Riff.Wave;
using Monoamp.Common.Data.Application.Music;
using Monoamp.Common.Data.Application.Sound;
using Monoamp.Common.Component.Sound.Player;

namespace Monoamp.Common.Utility
{
	public static class ConstructorCollection
	{
		private static readonly Constructor constructorMusic;
		private static readonly Constructor constructorWaveform;
		private static readonly Constructor constructorPlayer;

		static ConstructorCollection()
		{
			Dictionary<string, Constructor.DConstructor> constructorDictionaryMusic = new Dictionary<string, Constructor.DConstructor>();
			constructorDictionaryMusic.Add( ".aif", ( string aPathFile ) => { return new MusicAiff( aPathFile ); } );
			constructorDictionaryMusic.Add( ".aiff", ( string aPathFile ) => { return new MusicAiff( aPathFile ); } );
			constructorDictionaryMusic.Add( ".wav", ( string aPathFile ) => { return new MusicWave( aPathFile ); } );
			constructorDictionaryMusic.Add( ".wave", ( string aPathFile ) => { return new MusicWave( aPathFile ); } );
			constructorMusic = new Constructor( constructorDictionaryMusic );

			Dictionary<string, Constructor.DConstructor> constructorDictionaryWaveform = new Dictionary<string, Constructor.DConstructor>();
			constructorDictionaryWaveform.Add( ".aif", ( string aPathFile ) => { return new WaveformAiff( aPathFile ); } );
			constructorDictionaryWaveform.Add( ".aiff", ( string aPathFile ) => { return new WaveformAiff( aPathFile ); } );
			constructorDictionaryWaveform.Add( ".wav", ( string aPathFile ) => { return new WaveformWave( aPathFile ); } );
			constructorDictionaryWaveform.Add( ".wave", ( string aPathFile ) => { return new WaveformWave( aPathFile ); } );
			constructorWaveform = new Constructor( constructorDictionaryWaveform );
			
			Dictionary<string, Constructor.DConstructor> constructorDictionaryPlayer = new Dictionary<string, Constructor.DConstructor>();
			constructorDictionaryPlayer.Add( ".aif", ( string aPathFile ) => { return new PlayerPcm( aPathFile ); } );
			constructorDictionaryPlayer.Add( ".aiff", ( string aPathFile ) => { return new PlayerPcm( aPathFile ); } );
			constructorDictionaryPlayer.Add( ".wav", ( string aPathFile ) => { return new PlayerPcm( aPathFile ); } );
			constructorDictionaryPlayer.Add( ".wave", ( string aPathFile ) => { return new PlayerPcm( aPathFile ); } );
			constructorPlayer = new Constructor( constructorDictionaryPlayer );
		}
		
		public static IMusic ConstructMusic( string aPathFile )
		{
			return ( IMusic )constructorMusic.Construct( aPathFile );
		}

		public static WaveformReaderPcm ConstructWaveform( string aPathFile )
		{
			return ( WaveformReaderPcm )constructorWaveform.Construct( aPathFile );
		}

		public static IPlayer ConstructPlayer( string aPathFile )
		{
			return ( IPlayer )constructorPlayer.Construct( aPathFile );
		}
	}
}
