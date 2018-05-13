namespace Unmanaged.Tests
{
	using System;
	using Xunit;
	using XUnit;

	public class NativeLibraryTest
	{
		[PlatformSpecificTheory(Platform.Windows)]
		[InlineData("kernel32.dll")]
		public void Test_NativeLibrary_Load_Windows(string library)
		{
			using (new NativeLibrary(library))
			{
			}
		}

		[PlatformSpecificTheory(Platform.Unix)]
		[InlineData("libdl.so")]
		public void Test_NativeLibrary_Load_Unix(string library)
		{
			using (new NativeLibrary(library))
			{
			}
		}

		[Theory]
		[InlineData("randomasdasdas.dll")]
		[InlineData("randomasdasdas.so")]
		public void Test_NativeLibrary_NotFound(string library)
		{
			Assert.Throws<DllNotFoundException>(() => new NativeLibrary(library));
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
