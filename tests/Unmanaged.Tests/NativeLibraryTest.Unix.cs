namespace Unmanaged.Tests
{
	using System;
	using Xunit;
	using XUnit;

	public partial class NativeLibraryTest
	{
		[PlatformSpecificTheory(Platform.Unix)]
		[InlineData("libdl.so")]
		public void Test_NativeLibrary_Load_Unix(string library)
		{
			using (new NativeLibrary(library))
			{
			}
		}

		[PlatformSpecificTheory(Platform.Unix)]
		[InlineData("libdl.so", "dlopen")]
		[InlineData("libdl.so", "dlsym")]
		[InlineData("libdl.so", "dlclose")]
		public void Test_NativeLibrary_GetAddress_Unix(string windowsLibrary, string entryPoint)
		{
			using (var lib = new NativeLibrary(windowsLibrary))
			{
				IntPtr handle = lib.GetAddress(entryPoint);

				Assert.True(handle != IntPtr.Zero);
			}
		}

		[PlatformSpecificTheory(Platform.Unix)]
		[InlineData("libdl.so", "dlopen", typeof(LoadLibrary))]
		[InlineData("libdl.so", "dlsym", typeof(GetProcAddressUnix))]
		[InlineData("libdl.so", "dlclose", typeof(FreeLibraryUnix))]
		public void Test_NativeLibrary_GetDelegate_Unix(string windowsLibrary, string entryPoint, Type delegateType)
		{
			using (var lib = new NativeLibrary(windowsLibrary))
			{
				Delegate @delegate = lib.GetDelegate(delegateType, entryPoint);

				Assert.NotNull(@delegate);
			}
		}

		[PlatformSpecificFact(Platform.Unix)]
		public void Test_NativeLibrary_GetDelegateGeneric_Unix()
		{
			using (var lib = new NativeLibrary("libdl.so"))
			{
				GetProcAddressUnix @delegate = lib.GetDelegate<GetProcAddressUnix>("dlsym");
				FreeLibraryUnix @delegate1 = lib.GetDelegate<FreeLibraryUnix>("dlclose");

				Assert.NotNull(@delegate);
				Assert.NotNull(@delegate1);
			}
		}

		#region Delegates

		public const int RTLD_NOW = 2;

		private delegate IntPtr GetProcAddressUnix(IntPtr hModule, string procName);

		private delegate bool FreeLibraryUnix(IntPtr hModule);

		private delegate IntPtr LoadLibrary(string file, int mode);

		#endregion
	}
}
