namespace Unmanaged.Native
{
	using System;
	using System.Runtime.InteropServices;

	internal static class Kernel32
	{
		private const string DLL_NAME = "kernel32.dll";

		[DllImport(DLL_NAME, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport(DLL_NAME, SetLastError = true)]
		public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, uint dwFlags);

		[DllImport(DLL_NAME, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FreeLibrary(IntPtr hModule);
	}
}
