namespace Unmanaged.Native
{
	using System;
	using System.Diagnostics;
	using System.Runtime.InteropServices;

	internal static class Libdl
	{
		private const string SO_NAME = "libdl.so";

		public const int RTLD_NOW = 2;

		[DllImport(SO_NAME, EntryPoint = "dlopen", SetLastError = true)]
		public static extern IntPtr LoadLibrary(string file, int mode);

		[DllImport(SO_NAME, EntryPoint = "dlsym", SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr handle, string name);

		[DllImport(SO_NAME, EntryPoint = "dlclose", SetLastError = true)]
		public static extern int FreeLibrary(IntPtr handle);

		[DllImport(SO_NAME, EntryPoint = "dlerror", SetLastError = true)]
		public static extern IntPtr GetLastError();

		public static void PrintErrors(string context = null)
		{
			IntPtr handle = IntPtr.Zero;

			while ((handle = GetLastError()) != IntPtr.Zero)
			{
				string value = Marshal.PtrToStringUTF8(handle);

				if (context != null)
				{
					Debug.WriteLine($"{context}:{value}");
				}
				else
				{
					Debug.WriteLine(value);
				}
			}
		}
	}
}
