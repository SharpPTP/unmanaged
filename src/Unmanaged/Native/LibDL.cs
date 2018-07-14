namespace Unmanaged.Native
{
	using System;
	using System.Runtime.InteropServices;

	internal static class Libdl
	{
		private const string SO_NAME = "libdl.so";

		public const int RTLD_NOW = 2;

		[DllImport(SO_NAME, EntryPoint = "dlopen")]
		public static extern IntPtr LoadLibrary(string file, int mode);

		[DllImport(SO_NAME, EntryPoint = "dlsym")]
		public static extern IntPtr GetProcAddress(IntPtr handle, string name);

		[DllImport(SO_NAME, EntryPoint = "dlclose")]
		public static extern int FreeLibrary(IntPtr handle);
	}
}
