namespace Unmanaged.Tests
{
	using System;
	using Xunit;
	using XUnit;

	public partial class NativeLibraryTest
	{
		[PlatformSpecificTheory(Platform.Unix)]
		[InlineData("libc.so")]
		public void Test_NativeLibrary_Load_Unix(params string[] libraryNames)
		{
			using (new NativeLibrary(libraryNames))
			{
			}
		}

		[PlatformSpecificTheory(Platform.Unix)]
		[InlineData(new string[] { "libc.so" }, "getpid")]
		public void Test_NativeLibrary_GetAddress_Unix(string[] libraryNames, string entryPoint)
		{
			using (var lib = new NativeLibrary(libraryNames))
			{
				IntPtr handle = lib.GetAddress(entryPoint);

				Assert.True(handle != IntPtr.Zero);
			}
		}

		[PlatformSpecificTheory(Platform.Unix)]
		[InlineData(new string[] { "libc.so" }, "getpid", typeof(GetPIDUnix))]
		public void Test_NativeLibrary_GetDelegate_Unix(string[] libraryNames, string entryPoint, Type delegateType)
		{
			using (var lib = new NativeLibrary(libraryNames))
			{
				Delegate @delegate = lib.GetDelegate(delegateType, entryPoint);

				Assert.NotNull(@delegate);
			}
		}

		[PlatformSpecificFact(Platform.Unix)]
		public void Test_NativeLibrary_GetDelegateGeneric_Unix()
		{
			using (var lib = new NativeLibrary(new string[] { "libc.so" }))
			{
				GetPIDUnix @delegate = lib.GetDelegate<GetPIDUnix>("getpid");

				Assert.NotNull(@delegate);
			}
		}

		#region Delegates

		private delegate int GetPIDUnix();

		#endregion
	}
}
