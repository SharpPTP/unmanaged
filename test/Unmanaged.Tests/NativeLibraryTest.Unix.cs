namespace Unmanaged.Tests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System;
	using Unmanaged.MSTest;

	public partial class NativeLibraryTest
	{
		[PlatformSpecificDataTestMethod(Platform.Unix)]
		[DataRow(new string[] { "libc.so", "libc.so.6" })]
		public void Test_NativeLibrary_Load_Unix(params string[] libraryNames)
		{
			using (new NativeLibrary(libraryNames))
			{
			}
		}

		[PlatformSpecificDataTestMethod(Platform.Unix)]
		[DataRow(new string[] { "libc.so", "libc.so.6" }, "getpid")]
		public void Test_NativeLibrary_GetAddress_Unix(string[] libraryNames, string entryPoint)
		{
			using (var lib = new NativeLibrary(libraryNames))
			{
				IntPtr handle = lib.GetAddress(entryPoint);

				Assert.IsTrue(handle != IntPtr.Zero);
			}
		}

		[PlatformSpecificDataTestMethod(Platform.Unix)]
		[DataRow(new string[] { "libc.so", "libc.so.6" }, "getpid", typeof(GetPIDUnix))]
		public void Test_NativeLibrary_GetDelegate_Unix(string[] libraryNames, string entryPoint, Type delegateType)
		{
			using (var lib = new NativeLibrary(libraryNames))
			{
				Delegate @delegate = lib.GetDelegate(delegateType, entryPoint);

				Assert.IsNotNull(@delegate);
			}
		}

		[PlatformSpecificTestMethod(Platform.Unix)]
		public void Test_NativeLibrary_GetDelegateGeneric_Unix()
		{
			using (var lib = new NativeLibrary(new string[] { "libc.so", "libc.so.6" }))
			{
				GetPIDUnix @delegate = lib.GetDelegate<GetPIDUnix>("getpid");

				Assert.IsNotNull(@delegate);
			}
		}

		#region Delegates

		private delegate int GetPIDUnix();

		#endregion
	}
}
