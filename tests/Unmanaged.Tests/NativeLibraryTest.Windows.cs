namespace Unmanaged.Tests
{
	using System;
	using Xunit;
	using XUnit;

	public partial class NativeLibraryTest
	{
		[PlatformSpecificTheory(Platform.Windows)]
		[InlineData("kernel32.dll")]
		public void Test_NativeLibrary_Load_Windows(params string[] libraryNames)
		{
			using (new NativeLibrary(libraryNames))
			{
			}
		}

		[PlatformSpecificTheory(Platform.Windows)]
		[InlineData("kernel32.dll", "GetTickCount")]
		[InlineData("kernel32.dll", "GetProcAddress")]
		[InlineData("kernel32.dll", "FreeLibrary")]
		public void Test_NativeLibrary_GetAddress_Windows(string windowsLibrary, string entryPoint)
		{
			using (var lib = new NativeLibrary(windowsLibrary))
			{
				IntPtr handle = lib.GetAddress(entryPoint);

				Assert.True(handle != IntPtr.Zero);
			}
		}

		[PlatformSpecificTheory(Platform.Windows)]
		[InlineData("kernel32.dll", "GetTickCount", typeof(GetTickCountWindows))]
		[InlineData("kernel32.dll", "GetProcAddress", typeof(GetProcAddressWindows))]
		[InlineData("kernel32.dll", "FreeLibrary", typeof(FreeLibraryWindows))]
		public void Test_NativeLibrary_GetDelegate_Windows(string windowsLibrary, string entryPoint, Type delegateType)
		{
			using (var lib = new NativeLibrary(windowsLibrary))
			{
				Delegate @delegate = lib.GetDelegate(delegateType, entryPoint);

				Assert.NotNull(@delegate);
			}
		}

		[PlatformSpecificFact(Platform.Windows)]
		public void Test_NativeLibrary_GetDelegateGeneric_Windows()
		{
			using (var lib = new NativeLibrary("kernel32.dll"))
			{
				GetProcAddressWindows @delegate = lib.GetDelegate<GetProcAddressWindows>("GetProcAddress");
				FreeLibraryWindows @delegate1 = lib.GetDelegate<FreeLibraryWindows>("FreeLibrary");

				Assert.NotNull(@delegate);
				Assert.NotNull(@delegate1);
			}
		}

		#region Delegates

		private delegate IntPtr GetProcAddressWindows(IntPtr hModule, string procName);

		private delegate bool FreeLibraryWindows(IntPtr hModule);

		private delegate uint GetTickCountWindows();

		#endregion
	}
}
