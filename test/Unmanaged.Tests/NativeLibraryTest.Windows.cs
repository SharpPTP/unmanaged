namespace Unmanaged.Tests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System;
	using Unmanaged.MSTest;

	public partial class NativeLibraryTest
	{
		[PlatformSpecificDataTestMethod(Platform.Windows)]
		[DataRow(new string[] { "kernel32.dll" })]
		public void Test_NativeLibrary_Load_Windows(params string[] libraryNames)
		{
			using (new NativeLibrary(libraryNames))
			{
			}
		}

		[PlatformSpecificDataTestMethod(Platform.Windows)]
		[DataRow("kernel32.dll", "GetTickCount")]
		[DataRow("kernel32.dll", "GetProcAddress")]
		[DataRow("kernel32.dll", "FreeLibrary")]
		public void Test_NativeLibrary_GetAddress_Windows(string windowsLibrary, string entryPoint)
		{
			using (var lib = new NativeLibrary(windowsLibrary))
			{
				IntPtr handle = lib.GetAddress(entryPoint);

				Assert.IsTrue(handle != IntPtr.Zero);
			}
		}

		[PlatformSpecificDataTestMethod(Platform.Windows)]
		[DataRow("kernel32.dll", "GetTickCount", typeof(GetTickCountWindows))]
		[DataRow("kernel32.dll", "GetProcAddress", typeof(GetProcAddressWindows))]
		[DataRow("kernel32.dll", "FreeLibrary", typeof(FreeLibraryWindows))]
		public void Test_NativeLibrary_GetDelegate_Windows(string windowsLibrary, string entryPoint, Type delegateType)
		{
			using (var lib = new NativeLibrary(windowsLibrary))
			{
				Delegate @delegate = lib.GetDelegate(delegateType, entryPoint);

				Assert.IsNotNull(@delegate);
			}
		}

		[PlatformSpecificTestMethod(Platform.Windows)]
		public void Test_NativeLibrary_GetDelegateGeneric_Windows()
		{
			using (var lib = new NativeLibrary("kernel32.dll"))
			{
				GetProcAddressWindows @delegate = lib.GetDelegate<GetProcAddressWindows>("GetProcAddress");
				FreeLibraryWindows @delegate1 = lib.GetDelegate<FreeLibraryWindows>("FreeLibrary");

				Assert.IsNotNull(@delegate);
				Assert.IsNotNull(@delegate1);
			}
		}

		#region Delegates

		private delegate IntPtr GetProcAddressWindows(IntPtr hModule, string procName);

		private delegate bool FreeLibraryWindows(IntPtr hModule);

		private delegate uint GetTickCountWindows();

		#endregion
	}
}
