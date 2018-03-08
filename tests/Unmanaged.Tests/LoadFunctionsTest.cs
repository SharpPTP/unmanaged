namespace Unmanaged.Tests
{
	using System;
	using System.Collections.Generic;
	using Unmanaged.Tests.Platforms;
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
					.LoadFunctions(lib.GetAddress, (s, b) => { });

				Assert.True(errors.Count <= 0);
				Assert.NotNull(TestKernel32Wrapper.KernelGetProcAddress);
				Assert.NotNull(TestKernel32Wrapper.KernelFreeLibrary);
				Assert.NotNull(TestKernel32Wrapper.KernelGetTickCount);
			}
		}

		#region Nested type: TestClass, Delegates

		public static class TestKernel32Wrapper
		{
			[LoadFunction("GetProcAddress")]
			public static GetProcAddressDelegate KernelGetProcAddress = null;

			[LoadFunction("FreeLibrary")]
			public static FreeLibraryDelegate KernelFreeLibrary = null;

			[LoadFunction("GetTickCount")]
			public static GetTickCountDelegate KernelGetTickCount = null;
		}

		public delegate IntPtr GetProcAddressDelegate(IntPtr hModule, string procName);

		public delegate bool FreeLibraryDelegate(IntPtr hModule);

		public delegate uint GetTickCountDelegate();

		#endregion
	}
}
