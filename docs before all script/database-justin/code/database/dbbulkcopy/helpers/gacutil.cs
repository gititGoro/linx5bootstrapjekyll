using System;
using System.Runtime.InteropServices;

namespace Twenty57.Linx.Components.Database.DbBulkCopy.Helpers
{
	// Adapted from http://stackoverflow.com/questions/19456547/how-to-programmatically-determine-if-net-assembly-is-installed-in-gac
	public static class GacUtil
	{
		public static string GetAssemblyPath(string assemblyName)
		{
			var assemblyInfo = new AssemblyInfo { cchBuf = 512 };
			assemblyInfo.currentAssemblyPath = new string('\0', assemblyInfo.cchBuf);

			IAssemblyCache assemblyCache;
			var hr = CreateAssemblyCache(out assemblyCache, 0);
			if (hr == IntPtr.Zero)
			{
				hr = assemblyCache.QueryAssemblyInfo(1, assemblyName, ref assemblyInfo);
				return hr == IntPtr.Zero ? assemblyInfo.currentAssemblyPath : null;
			}

			Marshal.ThrowExceptionForHR(hr.ToInt32());
			return null;
		}


		[DllImport("fusion.dll")]
		private static extern IntPtr CreateAssemblyCache(
			out IAssemblyCache ppAsmCache,
			int reserved);

		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("e707dcde-d1cd-11d2-bab9-00c04f8eceae")]
		private interface IAssemblyCache
		{
			int Dummy1();

			[PreserveSig()]
			IntPtr QueryAssemblyInfo(
				int flags,
				[MarshalAs(UnmanagedType.LPWStr)] string assemblyName,
				ref AssemblyInfo assemblyInfo);

			int Dummy2();
			int Dummy3();
			int Dummy4();
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct AssemblyInfo
		{
			public int cbAssemblyInfo;
			public int assemblyFlags;
			public long assemblySizeInKB;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string currentAssemblyPath;

			public int cchBuf;
		}
	}
}
