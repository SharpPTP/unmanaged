namespace Unmanaged.Tests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;
	using Unmanaged.MSTest;

	[TestClass]
	[TestCategory("Unmanaged")]
	public class LoadFunctionsTest
	{

		[PlatformSpecificTestMethod(Platform.Windows)]
		public void Test_LoadFunctions()
		{
			IntPtr libraryHandle = IntPtr.Zero;

			try
			{
				libraryHandle = NativeLibrary.Load("kernel32.dll");

				ICollection<string> errors = typeof(TestKernel32Wrapper)
					.LoadFunctions(libraryHandle);

				Assert.IsTrue(errors.Count <= 0);
				Assert.IsNotNull(TestKernel32Wrapper.KernelGetProcAddress);
				Assert.IsNotNull(TestKernel32Wrapper.KernelFreeLibrary);
				Assert.IsNotNull(TestKernel32Wrapper.KernelGetTickCount);
			}
			finally
			{
				NativeLibrary.Free(libraryHandle);
			}
		}

		[PlatformSpecificTestMethod(Platform.Windows)]
		public void Test_LoadFunctions_OnCall()
		{
			IntPtr libraryHandle = IntPtr.Zero;

			try
			{
				libraryHandle = NativeLibrary.Load("kernel32.dll");

				ICollection<string> errors = typeof(TestKernel32Wrapper)
					.LoadFunctions(libraryHandle, () => Callback);

				Assert.IsTrue(errors.Count <= 0);
				Assert.IsNotNull(TestKernel32Wrapper.KernelGetProcAddress);
				Assert.IsNotNull(TestKernel32Wrapper.KernelFreeLibrary);
				Assert.IsNotNull(TestKernel32Wrapper.KernelGetTickCount);
			}
			finally
			{
				NativeLibrary.Free(libraryHandle);
			}
		}

		#region Nested type: TestClass, Delegates

		public static UnmanagedCallback Callback
			=> (a, b) => { };

		private static class TestKernel32Wrapper
		{
			public static GetProcAddressDelegate KernelGetProcAddress = null;

			[LoadFunction("FreeLibrary")]
			public static FreeLibraryDelegate KernelFreeLibrary = null;

			[LoadFunction("GetTickCount")]
			public static GetTickCountDelegate KernelGetTickCount = null;
		}

		private delegate IntPtr GetProcAddressDelegate(IntPtr hModule, string procName);

		private delegate bool FreeLibraryDelegate(IntPtr hModule);

		private delegate uint GetTickCountDelegate();

		#endregion
	}
}
