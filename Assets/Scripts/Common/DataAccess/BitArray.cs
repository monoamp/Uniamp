using System;

namespace Monoamp.Common.system.io
{
	public abstract class BitArray
	{
		public readonly byte[] dataArray;

		public int Position {
			get;
			protected set;
		}

		protected BitArray( byte[] aData )
		{
			Position = 0;

			dataArray = aData;
		}

		public int GetLength()
		{
			return dataArray.Length;
		}

		public void SetPosition( int aPosition )
		{
			Position = aPosition;
		}

		public void AddPosition( int length )
		{
			Position += length;
		}

		public void SubPosition( int length )
		{
			Position -= length;
		}

		public byte ReadBit()
		{
			byte tempData = ( byte )( ( dataArray[Position / 8] >> ( 7 - Position % 8 ) ) & 0x01 );

			Position++;

			return tempData;
		}

		public byte[] ReadBits( int length )
		{
			byte[] tempDataArray = new byte[( length - length % 8 ) / 8 + 1];

			for( int i = 0; i < length % 8; i++ )
			{
				tempDataArray[0] <<= 1;
				tempDataArray[0] |= ReadBit();
			}

			int baseIndex = 0;

			if( length % 8 != 0 )
			{
				baseIndex = 1;
			}

			for( int i = 0; i < ( length - length % 8 ) / 8; i++ )
			{
				for( int j = 0; j < 8; j++ )
				{
					tempDataArray[baseIndex + i] <<= 1;
					tempDataArray[baseIndex + i] |= ReadBit();
				}
			}

			return tempDataArray;
		}

		public Byte ReadBits8( int length )
		{
			byte[] tempDataArray = ReadBits( length );

			return ( Byte )tempDataArray[0];
		}

		public UInt16 ReadBits16( int length )
		{
			byte[] tempDataArray = ReadBits( length );

			return ( UInt16 )( ( tempDataArray[0] << 8 ) | tempDataArray[1] );
		}

		public UInt32 ReadBits32( int length )
		{
			byte[] tempDataArray = ReadBits( length );

			return ( UInt32 )( ( tempDataArray[0] << 24 ) | ( tempDataArray[1] << 16 ) | ( tempDataArray[2] << 8 ) | tempDataArray[3] );
		}
	}
}
