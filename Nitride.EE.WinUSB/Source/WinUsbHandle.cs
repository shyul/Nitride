using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Nitride.EE.WinUSB
{
	/// <summary>
	/// Create a SafeHandle, informing the base class
	/// that this SafeHandle instance "owns" the handle,
	/// and therefore SafeHandle should call
	/// our ReleaseHandle method when the SafeHandle
	/// is no longer in use.
	/// </summary>
	public class WinUsbHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		public WinUsbHandle() : base(true)
		{
			SetHandle(handle);
			handle = IntPtr.Zero;
		}

		public override bool IsInvalid => handle == IntPtr.Zero || handle == (IntPtr)(-1);

		public void Release() => ReleaseHandle();

		protected override bool ReleaseHandle()
		{
			if (!IsInvalid) handle = IntPtr.Zero;
			return NativeMethods.WinUsb_Free(handle);
		}

		public IntPtr GetHandle()
		{
			if (IsInvalid)
			{
				throw new Exception("The handle is invalid.");
			}
			return handle;
		}
	}
}
