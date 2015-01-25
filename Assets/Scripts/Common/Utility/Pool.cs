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

		private object objectLock;

		public Pool( Constructor aConstructor )
		{
			constructor = aConstructor;
			dictionary = new Dictionary<string, object>();
			
			objectLock = new object();
		}

		public object Get( string aPathFile )
		{
			lock( objectLock )
			{
				if( dictionary.ContainsKey( aPathFile ) == false )
				{
					try
					{
						using( FileStream u = new FileStream( aPathFile, FileMode.Open, FileAccess.Read ) )
						{
							object l = constructor( u );

							dictionary.Add( aPathFile, l );
						}
					}
					catch( Exception aExpection )
					{
						UnityEngine.Debug.LogError( "Exception:" + aExpection );
						UnityEngine.Debug.LogError( "PathFile:" + aPathFile );
					}
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
	}
}
