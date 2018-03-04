namespace Unmanaged.Test
{
	using System;
	using Xunit;

	public class NativeLibraryTest
	{
		[Theory]
		[InlineData("kernel32.dll", "libdl.so")]
		[InlineData("opengl32.dll", "libgl.so")]
		public void Test_NativeLibrary_Load(string windowsLibrary, string unixLibrary)
		{
			using (new NativeLibrary(windowsLibrary, unixLibrary))
			{
			}
		}

		[Theory]
		[InlineData("randomasdasdas.dll", "randomasdasdas.so")]
		public void Test_NativeLibrary_NotFound(string windowsLibrary, string unixLibrary)
		{
			Assert.Throws<DllNotFoundException>(() => new NativeLibrary(windowsLibrary, unixLibrary));
		}

		[Theory]
		[InlineData("kernel32.dll", "GetTickCount")]
		[InlineData("kernel32.dll", "GetProcAddress")]
		[InlineData("kernel32.dll", "FreeLibrary")]
		// TODO: [PlatformSpesific(OSPlatform.Windows)]
		public void Test_NativeLibrary_GetAddress(string windowsLibrary, string entryPoint)
		{
			using (var lib = new NativeLibrary(windowsLibrary))
			{
				IntPtr handle = lib.GetAddress(entryPoint);

				Assert.True(handle != IntPtr.Zero);
			}
		}

		[Theory]
		[InlineData("kernel32.dll", "GetTickCount", typeof(GetTickCount))]
		[InlineData("kernel32.dll", "GetProcAddress", typeof(GetProcAddress))]
		[InlineData("kernel32.dll", "FreeLibrary", typeof(FreeLibrary))]
		// TODO: [PlatformSpesific(OSPlatform.Windows)]
		public void Test_NativeLibrary_GetDelegate(string windowsLibrary, string entryPoint, Type delegateType)
		{
			using (var lib = new NativeLibrary(windowsLibrary))
			{
				Delegate @delegate = lib.GetDelegate(delegateType, entryPoint);

				Assert.NotNull(@delegate);
			}
		}

		[Fact]
		// TODO: [PlatformSpesific(OSPlatform.Windows)]
		public void Test_NativeLibrary_GetDelegateGeneric()
		{
			using (var lib = new NativeLibrary("kernel32.dll"))
			{
				GetProcAddress @delegate = lib.GetDelegate<GetProcAddress>("GetProcAddress");
				FreeLibrary @delegate1 = lib.GetDelegate<FreeLibrary>("FreeLibrary");

				Assert.NotNull(@delegate);
				Assert.NotNull(@delegate1);
			}
		}


		#region Delegates

		private delegate IntPtr GetProcAddress(IntPtr hModule, string procName);

		private delegate bool FreeLibrary(IntPtr hModule);

		private delegate uint GetTickCount();

		#endregion
	}
}
