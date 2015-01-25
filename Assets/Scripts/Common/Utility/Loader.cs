using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Monoamp.Common.Utility
{
	public struct Loader
	{
		public delegate Object Constructor( string aFilePath );
		
		private readonly Dictionary<string, Constructor> constructorDictionary;

		public Loader( Dictionary<string, Constructor> aConstructorDictionary )
		{
			constructorDictionary = aConstructorDictionary;
		}

		public Object Load( string aPathFile )
		{
			Object lObject = null;

			string lExtension = Path.GetExtension( aPathFile ).ToLower();

			// 登録してある拡張子の場合はインスタンスを生成する.
			if( constructorDictionary.ContainsKey( lExtension ) )
			{
				lObject = constructorDictionary[lExtension]( aPathFile );
			}
			else
			{
				Logger.Debug( aPathFile + " is not supported type." );
			}

			return lObject;
		}
	}
}
