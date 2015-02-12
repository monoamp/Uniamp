using System;
using System.IO;
using System.Collections.Generic;

namespace Monoamp.Common.system.io
{
	public abstract class AByteArray
	{
		protected Stream stream;

		public int Position{ get; private set; }
		public int PositionBit{ get; private set; }
		public int Length{ get{ return ( int )stream.Length - start / 8; } }
		public int LengthBit{ get{ return ( int )stream.Length * 8 - start; } }

		private int start;

		public int GetByInt( int len )
		{
			if( len < 0 || len >= 32 )
			{
				UnityEngine.Debug.LogError( "Out Range" + len );
			}

			return ( int )ReadBitsAsUInt32( len );
		}

		public bool AddByteArray( byte[] bytes )
		{
			stream.Seek( Length + start / 8, SeekOrigin.Begin );
			stream.Write( bytes, 0, bytes.Length );
			stream.Seek( Position + start / 8, SeekOrigin.Begin );
			return true;
		}
		
		public void RemoveRange( int range )
		{
			start += range * 8;

			if( PositionBit > 0 )
			{
				if( PositionBit > range * 8 )
				{
					SubBitPosition( range * 8 );
				}
				else
				{
					SetBitPosition( 0 );
				}
			}
		}

		protected AByteArray( Stream aStream )
		{
			stream = aStream;

			start = 0;
			Position = 0;
			PositionBit = 0;
		}

		public string GetName()
		{
			FileStream lFileStream = ( FileStream )stream;

			return lFileStream.Name;
		}

		public void Open()
		{
			stream = new FileStream( GetName(), FileMode.Open, FileAccess.Read );
		}

		public void Close()
		{
			stream.Close();
		}

		public int GetBitPositionInByte()
		{
			return PositionBit - Position * 8;
		}

		public void SetPosition( int aPosition )
		{
			Position = aPosition;
			PositionBit = Position * 8;

			stream.Seek( Position + start / 8, SeekOrigin.Begin );
		}

		public void AddPosition( int aOffset )
		{
			SetPosition( Position + aOffset );
		}

		public void SubPosition( int aOffset )
		{
			SetPosition( Position - aOffset );
		}

		public void SetBitPosition( int aPositionBit )
		{
			Position = aPositionBit / 8;
			PositionBit = aPositionBit;
			
			stream.Seek( Position + start / 8, SeekOrigin.Begin );
		}

		public void AddBitPosition( int aOffsetBit )
		{
			SetBitPosition( PositionBit + aOffsetBit );
		}

		public void SubBitPosition( int aOffsetBit )
		{
			SetBitPosition( PositionBit - aOffsetBit );
		}

		public Byte ReadByte()
		{
			Position++;
			PositionBit += 8;

			return ( Byte )stream.ReadByte();
		}

		public Byte ReadByte( int aPosition )
		{
			stream.Seek( aPosition, SeekOrigin.Begin );

			return ( Byte )stream.ReadByte();
		}

		public Byte[] ReadBytes( int aLength )
		{
			byte[] lDataArray = new byte[aLength];

			stream.Read( lDataArray, 0, aLength );

			Position += ( int )aLength;
			PositionBit = ( int )Position * 8;

			return lDataArray;
		}
        
        public void ReadBytes( Byte[] aDataArray )
        {
            stream.Read( aDataArray, 0, aDataArray.Length );
            
            Position += ( int )aDataArray.Length;
            PositionBit = ( int )Position * 8;
        }

		public Byte[] ReadBytes( UInt32 aLength )
		{
			return ReadBytes( ( int )aLength );
		}

		public string ReadString( int length )
		{
			string lString = System.Text.Encoding.ASCII.GetString( ReadBytes( length ) );

			return lString.Split( '\0' )[0];
		}

		public string ReadShiftJisString( int length )
		{
			return System.Text.Encoding.GetEncoding( "shift_jis" ).GetString( ReadBytes( length ) );
		}

		public Byte ReadUByte()
		{
			return ( Byte )ReadByte();
		}

		public abstract UInt16 ReadUInt16();
		public abstract UInt32 ReadUInt24();
		public abstract UInt32 ReadUInt32();
		public abstract UInt64 ReadUInt64();

		public SByte ReadSByte()
		{
			return ( SByte )ReadByte();
		}

		public abstract Int16 ReadInt16();
		public abstract Int32 ReadInt24();
		public abstract Int32 ReadInt32();
		public abstract Int64 ReadInt64();

		public abstract Single ReadSingle();
		public abstract Double ReadDouble();

		public Double ReadExtendedFloatPoint()
		{
			byte[] lDataRead = ReadBytes( 10 );
			byte[] lDataArray = new byte[8];

			lDataArray[7] = lDataRead[0];
			lDataArray[6] = ( byte )( ( ( lDataRead[1] << 4 ) & 0xF0 ) | ( ( lDataRead[2] >> 3 ) & 0x0F ) );
			lDataArray[5] = ( byte )( ( ( lDataRead[2] << 5 ) & 0xD0 ) | ( ( lDataRead[3] >> 3 ) & 0x1F ) );
			lDataArray[4] = ( byte )( ( ( lDataRead[3] << 5 ) & 0xD0 ) | ( ( lDataRead[4] >> 3 ) & 0x1F ) );
			lDataArray[3] = ( byte )( ( ( lDataRead[4] << 5 ) & 0xD0 ) | ( ( lDataRead[5] >> 3 ) & 0x1F ) );
			lDataArray[2] = ( byte )( ( ( lDataRead[5] << 5 ) & 0xD0 ) | ( ( lDataRead[6] >> 3 ) & 0x1F ) );
			lDataArray[1] = ( byte )( ( ( lDataRead[6] << 5 ) & 0xD0 ) | ( ( lDataRead[7] >> 3 ) & 0x1F ) );
			lDataArray[0] = ( byte )( ( ( lDataRead[7] << 5 ) & 0xD0 ) | ( ( lDataRead[8] >> 3 ) & 0x1F ) );

			/*
			lDataArray[0] = lDataRead[0];
			lDataArray[1] = ( byte )( ( ( lDataRead[1] << 4 ) & 0xF0 ) | ( ( lDataRead[2] >> 3 ) & 0x0F ) );
			lDataArray[2] = ( byte )( ( ( lDataRead[2] << 5 ) & 0xD0 ) | ( ( lDataRead[3] >> 3 ) & 0x1F ) );
			lDataArray[3] = ( byte )( ( ( lDataRead[3] << 5 ) & 0xD0 ) | ( ( lDataRead[4] >> 3 ) & 0x1F ) );
			lDataArray[4] = ( byte )( ( ( lDataRead[4] << 5 ) & 0xD0 ) | ( ( lDataRead[5] >> 3 ) & 0x1F ) );
			lDataArray[5] = ( byte )( ( ( lDataRead[5] << 5 ) & 0xD0 ) | ( ( lDataRead[6] >> 3 ) & 0x1F ) );
			lDataArray[6] = ( byte )( ( ( lDataRead[6] << 5 ) & 0xD0 ) | ( ( lDataRead[7] >> 3 ) & 0x1F ) );
			lDataArray[7] = ( byte )( ( ( lDataRead[7] << 5 ) & 0xD0 ) | ( ( lDataRead[8] >> 3 ) & 0x1F ) );
			*/

			return BitConverter.ToDouble( lDataArray, 0 );
		}

		public void WriteBytes( Byte[] data )
		{
			for( int i = 0; i < data.Length; i++ )
			{
				WriteUByte( data[i] );
			}
		}

		public void WriteBytes( AByteArray aByteArray, int aPosition, UInt32 aLength )
		{
			aByteArray.SetPosition( aPosition );
			Byte[] lDataArray = aByteArray.ReadBytes( aLength );

			for( int i = 0; i < lDataArray.Length; i++ )
			{
				WriteUByte( lDataArray[i] );
			}
		}

		public void WriteUByte( Byte data )
		{
			//dataArray[Position] = data;
			stream.Seek( Position, SeekOrigin.Begin );
			stream.WriteByte( data );

			Position++;
			PositionBit = Position * 8;
		}

		public void WriteUInt16( UInt16 data )
		{
			WriteBytes( BitConverter.GetBytes( data ) );
		}

		public void WriteUInt24( UInt16 data )
		{

		}

		public void WriteUInt32( UInt32 data )
		{
			WriteBytes( BitConverter.GetBytes( data ) );
		}

		public void WriteString( string data )
		{
			for( int i = 0; i < data.Length; i++ )
			{
				WriteUByte( ( byte )data[i] );
			}
		}

		public abstract Byte ReadBitsAsByte( int aLength );
		public abstract UInt16 ReadBitsAsUInt16( int aLength );
		public abstract UInt32 ReadBitsAsUInt32( int aLength );

		public UInt16 ReadBitsAsUInt16Little( int aLength )
		{
			return ( UInt16 )System.Net.IPAddress.HostToNetworkOrder( ( Int16 )ReadBitsAsUInt16( aLength ) );
		}

		public UInt32 ReadBitsAsUInt32Little( int aLength )
		{
			return ( ( UInt32 )System.Net.IPAddress.HostToNetworkOrder( ( Int32 )ReadBitsAsUInt32( aLength ) ) >> 8 );
		}

		public int Find( Byte[] aPatternArray )
		{
			for( int i = 0; i < stream.Length - aPatternArray.Length; i++ )
			{
				for( int j = 0; j < aPatternArray.Length; j++ )
				{
					if( ReadByte( ( int )( Position + i + j ) ) != aPatternArray[j] )
					{
						break;
					}

					if( j == aPatternArray.Length - 1 )
					{
						return Position + i;
					}
				}
			}

			return -1;
		}

		public int Find( Byte[] aPatternArray, int aCount )
		{
			for( int i = 0; i < stream.Length - aPatternArray.Length && i <= aCount; i++ )
			{
				for( int j = 0; j < aPatternArray.Length; j++ )
				{
					if( ReadByte( ( int )( Position + i + j ) ) != aPatternArray[j] )
					{
						break;
					}

					if( j == aPatternArray.Length - 1 )
					{
						return Position + i;
					}
				}
			}

			return -1;
		}
	}
}
