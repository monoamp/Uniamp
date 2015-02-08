using System;
using System.Collections.Generic;
using System.IO;

using Monoamp.Boundary;

namespace Monoamp.Common.Utility
{
	public struct Constructor
	{
		public delegate Object DConstructor( string aFilePath );
		
		private readonly Dictionary<string, DConstructor> constructorDictionary;

		public Constructor( Dictionary<string, DConstructor> aConstructorDictionary )
		{
			constructorDictionary = aConstructorDictionary;
		}

		public Object Construct( string aPathFile )
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
