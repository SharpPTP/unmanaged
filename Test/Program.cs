using System;
using Unmanaged;

namespace Test
{
	public class Program
	{
		public static void Main(string[] args)
		{
			using (var lib = new NativeLibrary(new string[] { "libc.so", "libc.so.6" }))
			{
				IntPtr handle = lib.GetAddress("getpid");

				Console.WriteLine(handle);
			}
		}
	}
}
