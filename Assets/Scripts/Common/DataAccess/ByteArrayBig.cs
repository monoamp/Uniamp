using System;
using System.IO;

namespace Monoamp.Common.system.io
{
	public class ByteArrayBig : AByteArray
	{
		private Byte[] dataArray2;
		private Byte[] dataArray3;
		private Byte[] dataArray4;
		private Byte[] dataArray8;
		private Byte[] mask;

		public ByteArrayBig( Stream aStream )
			: base( aStream )
		{
			dataArray2 = new Byte[2];
			dataArray3 = new Byte[3];
			dataArray4 = new Byte[4];
			dataArray8 = new Byte[8];
			mask = new Byte[]{ 0x00, 0x01, 0x03, 0x07, 0x0F, 0x1F, 0x3F, 0x7F, 0xFF };
		}

		public override UInt16 ReadUInt16()
		{
			ReadBytes( dataArray2 );

			return ( UInt16 )System.Net.IPAddress.HostToNetworkOrder( ( Int16 )BitConverter.ToUInt16( dataArray2, 0 ) );
		}

		public override UInt32 ReadUInt24()
		{
			ReadBytes( dataArray3 );

			return ( UInt32 )System.Net.IPAddress.HostToNetworkOrder( ( Int32 )BitConverter.ToUInt32( dataArray3, 0 ) );
		}

		public override UInt32 ReadUInt32()
		{
			ReadBytes( dataArray4 );

			return ( UInt32 )System.Net.IPAddress.HostToNetworkOrder( ( Int32 )BitConverter.ToUInt32( dataArray4, 0 ) );
		}

		public override UInt64 ReadUInt64()
		{
			ReadBytes( dataArray8 );

			return ( UInt64 )System.Net.IPAddress.HostToNetworkOrder( ( Int64 )BitConverter.ToUInt64( dataArray8, 0 ) );
		}

		public override Int16 ReadInt16()
		{
			ReadBytes( dataArray2 );

			return System.Net.IPAddress.HostToNetworkOrder( BitConverter.ToInt16( dataArray2, 0 ) );
		}

		public override Int32 ReadInt24()
		{
			ReadBytes( dataArray3 );

			dataArray4[0] = 0x00;
			dataArray4[1] = dataArray3[0];
			dataArray4[2] = dataArray3[1];
			dataArray4[3] = dataArray3[2];
			
			if( dataArray3[0] >= 0x80 )
			{
				dataArray4[0] = 0xFF;
			}

			return System.Net.IPAddress.HostToNetworkOrder( BitConverter.ToInt32( dataArray4, 0 ) );
		}

		public override Int32 ReadInt32()
		{
			ReadBytes( dataArray4 );

			return System.Net.IPAddress.HostToNetworkOrder( BitConverter.ToInt32( dataArray4, 0 ) );
		}

		public override Int64 ReadInt64()
		{
			ReadBytes( dataArray8 );

			return System.Net.IPAddress.HostToNetworkOrder( BitConverter.ToInt64( dataArray8, 0 ) );
		}

		public override Single ReadSingle()
		{
			ReadBytes( dataArray4 );

			return BitConverter.ToSingle( dataArray4, 0 );
		}

		public override Double ReadDouble()
		{
			ReadBytes( dataArray8 );

			return BitConverter.ToDouble( dataArray8, 0 );
		}

		public UInt32 ReadBits( int aLengthBit )
		{
			int lRead = 0;
			int lPositionBit = PositionBit;
			UInt32 lData = 0;
			
			while( lRead < aLengthBit )
			{
				int lBit = ( lPositionBit + lRead ) & 7;
				int lRestBit = 8 - lBit;
				int lRestReadBit = aLengthBit - lRead;
				int lReadNow = Math.Min( lRestReadBit, lRestBit );

				UInt32 lDataRead = ( UInt32 )( ( stream.ReadByte() >> ( 8 - lBit - lReadNow ) ) & mask[lReadNow] );
				lData |= ( UInt32 )( lDataRead << ( aLengthBit - lRead - lReadNow ) );
				lRead += lReadNow;
			}
			
			AddBitPosition( aLengthBit );
			
			if( PositionBit <= LengthBit )
			{
				return lData;
			}
			else
			{
				return 0;
			}
		}
		
		public override Byte ReadBitsAsByte( int aLengthBit )
		{
			return ( Byte )ReadBits( aLengthBit );
		}
		
		public override UInt16 ReadBitsAsUInt16( int aLengthBit )
		{
			return ( UInt16 )ReadBits( aLengthBit );
		}
		
		public override UInt32 ReadBitsAsUInt32( int aLengthBit )
		{
			return ( UInt32 )ReadBits( aLengthBit );
		}
	}
}
