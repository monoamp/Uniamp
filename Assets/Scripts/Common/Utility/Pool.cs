using System;
using System.Collections.Generic;
using System.IO;

namespace Monoamp.Common.Utility
{
	public struct Pool
	{
		public delegate object Constructor( FileStream aFileStream );

		private readonly Constructor constructor;
		private readonly Dictionary<string, object> dictionary;
		private readonly Dictionary<string, long> timeStampTicksDictionary;

		private object objectLock;

		public Pool( Constructor aConstructor )
		{
			constructor = aConstructor;
			dictionary = new Dictionary<string, object>();
			timeStampTicksDictionary = new Dictionary<string, long>();
			
			objectLock = new object();
		}

		public object Get( string aPathFile )
		{
			lock( objectLock )
			{
				if( dictionary.ContainsKey( aPathFile ) == false )
				{
					object l = Load( aPathFile );
					dictionary.Add( aPathFile, l );
					timeStampTicksDictionary.Add( aPathFile, File.GetLastWriteTime( aPathFile ).Ticks );
				}
				else if( File.GetLastWriteTime( aPathFile ).Ticks != timeStampTicksDictionary[aPathFile] )
				{
					object l = Load( aPathFile );
					dictionary[aPathFile] = l;
					timeStampTicksDictionary[aPathFile] = File.GetLastWriteTime( aPathFile ).Ticks;
				}
			}

			if( dictionary.ContainsKey( aPathFile ) == true )
			{
				return dictionary[aPathFile];
			}
			else
			{
				return null;
			}
		}
		
		private object Load( string aPathFile )
		{
			try
			{
				using( FileStream u = new FileStream( aPathFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) )
				{
					return constructor( u );
				}
			}
			catch( Exception aExpection )
			{
				UnityEngine.Debug.LogError( "Exception:" + aExpection );
				UnityEngine.Debug.LogError( "PathFile:" + aPathFile );
				return null;
			}
		}
	}
}
