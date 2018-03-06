namespace Unmanaged.Test
{
	using System;
	using System.Collections.Generic;
	using Xunit;

	public class LoadFunctionsTest
	{
		[Fact]
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

		[Fact]
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

		#region Nested type: TestClass

		private static class TestKernel32Wrapper
		{
			[LoadFunction("GetProcAddress")]
			public static GetProcAddressDelegate KernelGetProcAddress;

			[LoadFunction("FreeLibrary")]
			public static FreeLibraryDelegate KernelFreeLibrary;

			[LoadFunction("GetTickCount")]
			public static GetTickCountDelegate KernelGetTickCount;
		}

		#endregion

		#region Delegates

		private delegate IntPtr GetProcAddressDelegate(IntPtr hModule, string procName);

		private delegate bool FreeLibraryDelegate(IntPtr hModule);

		private delegate uint GetTickCountDelegate();

		#endregion
	}
}
