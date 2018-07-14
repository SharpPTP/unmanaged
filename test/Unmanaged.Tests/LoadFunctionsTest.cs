namespace Unmanaged.Tests
{
	using System;
	using System.Collections.Generic;
	using Unmanaged.XUnit;
	using Xunit;

	public class LoadFunctionsTest
	{
		[PlatformSpecificFact(Platform.Windows)]
		public void Test_LoadFunctions()
		{
			using (var lib = new NativeLibrary("kernel32.dll"))
			{
				ICollection<string> errors = typeof(TestKernel32Wrapper)
					.LoadFunctions(lib.GetAddress);

				Assert.True(errors.Count <= 0);
				Assert.NotNull(TestKernel32Wrapper.KernelGetProcAddress);
				Assert.NotNull(TestKernel32Wrapper.KernelFreeLibrary);
				Assert.NotNull(TestKernel32Wrapper.KernelGetTickCount);
			}
		}

		[PlatformSpecificFact(Platform.Windows)]
		public void Test_LoadFunctions_OnCall()
		{
			using (var lib = new NativeLibrary("kernel32.dll"))
			{
				ICollection<string> errors = typeof(TestKernel32Wrapper)
					.LoadFunctions(lib.GetAddress, () => Callback);

				Assert.True(errors.Count <= 0);
				Assert.NotNull(TestKernel32Wrapper.KernelGetProcAddress);
				Assert.NotNull(TestKernel32Wrapper.KernelFreeLibrary);
				Assert.NotNull(TestKernel32Wrapper.KernelGetTickCount);
			}
		}

		#region Nested type: TestClass, Delegates

		public static UnmanagedCallback Callback
			=> (a, b) => { };

		private static class TestKernel32Wrapper
		{
			[LoadFunction("GetProcAddress")]
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
