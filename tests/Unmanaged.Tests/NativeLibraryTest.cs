namespace Unmanaged.Tests
{
	using System;
	using Unmanaged.Tests.Platforms;
	using Xunit;

	public class NativeLibraryTest
	{
		[Theory]
		[InlineData("kernel32.dll", "libdl.so")]
		// TODO: investigate libGL.so missing
		//[InlineData("opengl32.dll", "libGL.so")]
		public void Test_NativeLibrary_Load(string windowsLibrary, string unixLibrary)
		{
			using (new NativeLibrary(windowsLibrary, unixLibrary))
			{
			}
		}

		[Theory]
		[InlineData("randomasdasdas.dll", "randomasdasdas.so")]
		[InlineData("randomasdasdas.so", "randomasdasdas.dll")]
		public void Test_NativeLibrary_NotFound(string windowsLibrary, string unixLibrary)
		{
			Assert.Throws<DllNotFoundException>(() => new NativeLibrary(windowsLibrary, unixLibrary));
		}

		[PlatformSpecificTheory(Platform.Windows)]
		[InlineData("kernel32.dll", "GetTickCount")]
		[InlineData("kernel32.dll", "GetProcAddress")]
		[InlineData("kernel32.dll", "FreeLibrary")]
		public void Test_NativeLibrary_GetAddress(string windowsLibrary, string entryPoint)
		{
			using (var lib = new NativeLibrary(windowsLibrary))
			{
				IntPtr handle = lib.GetAddress(entryPoint);

				Assert.True(handle != IntPtr.Zero);
			}
		}

		[PlatformSpecificTheory(Platform.Windows)]
		[InlineData("kernel32.dll", "GetTickCount", typeof(GetTickCount))]
		[InlineData("kernel32.dll", "GetProcAddress", typeof(GetProcAddress))]
		[InlineData("kernel32.dll", "FreeLibrary", typeof(FreeLibrary))]
		public void Test_NativeLibrary_GetDelegate(string windowsLibrary, string entryPoint, Type delegateType)
		{
			using (var lib = new NativeLibrary(windowsLibrary))
			{
				Delegate @delegate = lib.GetDelegate(delegateType, entryPoint);

				Assert.NotNull(@delegate);
			}
		}

		[PlatformSpecificFact(Platform.Windows)]
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
