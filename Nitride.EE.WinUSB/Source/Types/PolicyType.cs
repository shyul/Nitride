using System;

namespace Nitride.EE.WinUSB
{
	public static class PolicyType
	{
		public const uint SHORT_PACKET_TERMINATE = 0x1;
		public const uint AUTO_CLEAR_STALL = 0x2;
		public const uint PIPE_TRANSFER_TIMEOUT = 0x3;
		public const uint IGNORE_SHORT_PACKETS = 0x4;
		public const uint ALLOW_PARTIAL_READS = 0x5;
		public const uint AUTO_FLUSH = 0x6;
		public const uint RAW_IO = 0x7;
		public const uint MAXIMUM_TRANSFER_SIZE = 0x8;
		public const uint RESET_PIPE_ON_RESUME = 0x9;
	}
}
