using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.system.io;

using Monoamp.Boundary;

namespace Monoamp.Common.Data.Standard.Riff.Wave
{
	public class RiffWaveList : RiffChunkList
	{
		public const string ID = "LIST";
		
		public static readonly Dictionary<string, Type> chunkTypeDictionaryAdtl;
		public static readonly Dictionary<string, Type> chunkTypeDictionaryInfo;
		public static readonly Dictionary<string, Dictionary<string,Type>> chunkTypeDictionaryDictionary;
		public override Dictionary<string, Dictionary<string,Type>> ChunkTypeDictionaryDictionary{ get{ return chunkTypeDictionaryDictionary; } }

		static RiffWaveList()
		{
			chunkTypeDictionaryAdtl = new Dictionary<string, Type>();
			chunkTypeDictionaryAdtl.Add( RiffWaveLabl.ID, typeof( RiffWaveLabl ) );
			chunkTypeDictionaryAdtl.Add( RiffWaveNote.ID, typeof( RiffWaveNote ) );
			chunkTypeDictionaryAdtl.Add( RiffWaveLtxt.ID, typeof( RiffWaveLtxt ) );
			chunkTypeDictionaryAdtl.Add( RiffWaveFile.ID, typeof( RiffWaveFile ) );

			chunkTypeDictionaryInfo = new Dictionary<string, Type>();
			chunkTypeDictionaryInfo.Add( RiffInfoIart.ID, typeof( RiffInfoIart ) );
			chunkTypeDictionaryInfo.Add( RiffInfoIcms.ID, typeof( RiffInfoIcms ) );
			chunkTypeDictionaryInfo.Add( RiffInfoIcmt.ID, typeof( RiffInfoIcmt ) );
			chunkTypeDictionaryInfo.Add( RiffInfoIcop.ID, typeof( RiffInfoIcop ) );
			chunkTypeDictionaryInfo.Add( RiffInfoIcrd.ID, typeof( RiffInfoIcrd ) );
			chunkTypeDictionaryInfo.Add( RiffInfoIeng.ID, typeof( RiffInfoIeng ) );
			chunkTypeDictionaryInfo.Add( RiffInfoIgnr.ID, typeof( RiffInfoIgnr ) );
			chunkTypeDictionaryInfo.Add( RiffInfoIkey.ID, typeof( RiffInfoIkey ) );
			chunkTypeDictionaryInfo.Add( RiffInfoInam.ID, typeof( RiffInfoInam ) );
			chunkTypeDictionaryInfo.Add( RiffInfoIprd.ID, typeof( RiffInfoIprd ) );
			chunkTypeDictionaryInfo.Add( RiffInfoIsbj.ID, typeof( RiffInfoIsbj ) );
			chunkTypeDictionaryInfo.Add( RiffInfoIsft.ID, typeof( RiffInfoIsft ) );
			chunkTypeDictionaryInfo.Add( RiffInfoIsrc.ID, typeof( RiffInfoIsrc ) );
			chunkTypeDictionaryInfo.Add( RiffInfoItch.ID, typeof( RiffInfoItch ) );
			
			chunkTypeDictionaryDictionary = new Dictionary<string, Dictionary<string,Type>>();
			chunkTypeDictionaryDictionary.Add( "adtl", chunkTypeDictionaryAdtl );
			chunkTypeDictionaryDictionary.Add( "INFO", chunkTypeDictionaryInfo );
		}
		
		public RiffWaveList( string aId, UInt32 aSize, ByteArray aByteArray, RiffChunkList aParent )
			: base( aId, aSize, aByteArray, aParent )
		{

		}
	}
}
