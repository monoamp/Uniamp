using System;
using System.IO;
using System.Collections.Generic;

using Monoamp.Common.Struct;

namespace Monoamp.Common.Component.Sound.LoopTool
{
	public class LoopSet
	{
		public FileInfo fileInfo;
		public bool isSelected;
		public float progress;

		public LoopSet( FileInfo aFileInfo, bool aIsSelected, float aProgress )
		{
			fileInfo = aFileInfo;
			isSelected = aIsSelected;
			progress = aProgress;
		}
	}

	public static class LoopSearchTool
	{
		private static int LOOP_POINTS;
		private static int POINTS_SAMPLE;
		private static int FEADOUT_SAMPLES;
		private static int SEARCH_WIDTH;

		private static LoopSearchPoint[] searchPointArray;

		public static double Progress{ get; private set; }

		static LoopSearchTool()
		{
			LOOP_POINTS = 120;
			POINTS_SAMPLE = 16;
			FEADOUT_SAMPLES = 44100 * 8;
			SEARCH_WIDTH = 4410;

			searchPointArray = new LoopSearchPoint[LOOP_POINTS];

			Progress = 0.0d;
		}

		public static List<LoopInformation> Execute( SByte[] aDataArray, Dictionary<string, double> aProgressList, string aFilePath )
		{
			List<LoopInformation> lSamePointList = SearchLoopPoints( aDataArray, aProgressList, aFilePath );

			return SortLoopCount( lSamePointList );
		}

		private static List<LoopInformation> SearchLoopPoints( SByte[] aDataArray, Dictionary<string, double> aProgressList, string aFilePath )
		{
			List<LoopInformation> lLoopPointList = new List<LoopInformation>();

			int lLength = aDataArray.Length - FEADOUT_SAMPLES;
			int lMinLoopLength = lLength / 4;//SEARCH_WIDTH

			aProgressList[aFilePath] = 0.0d;

			object l = new object();

            for( int i = 0; i < LOOP_POINTS; i++ )
            {
				searchPointArray[i] = new LoopSearchPoint( aDataArray, lLength, SEARCH_WIDTH, ( lLength - SEARCH_WIDTH - lMinLoopLength ) / 120 * i, POINTS_SAMPLE, lMinLoopLength );

				lock( l )
				{
					aProgressList[aFilePath] += 1.0d / LOOP_POINTS;
				}
			}

			aProgressList[aFilePath] = 0.0d;

			for( int i = 0; i < LOOP_POINTS; i++ )
			{
				for( int j = 0; j < searchPointArray[i].samePointList.Count; j++ )
				{
					lLoopPointList.Add( new LoopInformation( 44100, searchPointArray[i].basePoint, searchPointArray[i].samePointList[j] ) );
				}
			}

			return lLoopPointList;
		}

		private static List<LoopInformation> SortLoopCount( List<LoopInformation> aSamePointArray )
		{
			Dictionary<int, List<LoopInformation>> lLoopListDictionary = CreateLoopDictionary( aSamePointArray );
			List<LoopInformation> lLoopList = new List<LoopInformation>();

			int lCountMax = 1;

			foreach( KeyValuePair<int, List<LoopInformation>> lKeyValuePair in lLoopListDictionary )
			{
				if( lKeyValuePair.Value.Count > lCountMax )
				{
					lCountMax = lKeyValuePair.Value.Count;
					Console.WriteLine( "Count Max:" + lCountMax.ToString() );
				}
			}

			for( int i = lCountMax; i > 0; i-- )
			{
				foreach( KeyValuePair<int, List<LoopInformation>> lKeyValuePair in lLoopListDictionary )
				{
					if( i == lKeyValuePair.Value.Count )
					{
						for( int j = 0; j < lKeyValuePair.Value.Count; j++ )
						{
							lLoopList.Add( lKeyValuePair.Value[j] );
						}
					}
				}
			}

			return lLoopList;
		}

		private static Dictionary<int, List<LoopInformation>> CreateLoopDictionary( List<LoopInformation> aSamePointArray )
		{
			Dictionary<int, List<LoopInformation>> lDictionary = new Dictionary<int, List<LoopInformation>>();

			for( int i = 0; i < aSamePointArray.Count; i++ )
			{
				int lSampleLength = ( int )aSamePointArray[i].length.sample;

				if( lDictionary.ContainsKey( lSampleLength ) == false )
				{
					lDictionary.Add( lSampleLength, new List<LoopInformation>() );
				}
				else
				{
						
				}

				lDictionary[lSampleLength].Add( aSamePointArray[i] );
			}

			return lDictionary;
		}
	}
}
