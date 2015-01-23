using System;
using System.Collections.Generic;
using System.IO;

namespace Monoamp.Common.Utility
{
    public static class PoolFilePath
    {
        private static Dictionary<string, string[]> pathArrayDictionary;
        private static Dictionary<string, DateTime> dateTimeDictionary;
        private static object objectLock;

        static PoolFilePath()
        {
			pathArrayDictionary = new Dictionary<string, string[]>();
            dateTimeDictionary = new Dictionary<string, DateTime>();
            objectLock = new object();
        }
        
		public static string[] Get( DirectoryInfo aDirectoryInfo )
        {
            lock( objectLock )
            {
				if( pathArrayDictionary.ContainsKey( aDirectoryInfo.FullName ) == false )
                {
					pathArrayDictionary.Add( aDirectoryInfo.FullName, Directory.GetFiles( aDirectoryInfo.FullName, "*.*", SearchOption.TopDirectoryOnly ) );
					dateTimeDictionary.Add( aDirectoryInfo.FullName, Directory.GetLastWriteTime( aDirectoryInfo.FullName ) );
                }

				if( dateTimeDictionary[aDirectoryInfo.FullName].Ticks != Directory.GetLastWriteTime( aDirectoryInfo.FullName ).Ticks )
                {
					pathArrayDictionary[aDirectoryInfo.FullName] = Directory.GetFiles( aDirectoryInfo.FullName, "*.*", SearchOption.TopDirectoryOnly );
					dateTimeDictionary[aDirectoryInfo.FullName] = Directory.GetLastWriteTime( aDirectoryInfo.FullName );
				}
            }
            
			return pathArrayDictionary[aDirectoryInfo.FullName];
		}
	}
}
