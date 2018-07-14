namespace Unmanaged.Tests
{
	using System;
	using Xunit;

	public partial class NativeLibraryTest
	{
		[Theory(Skip = "Not working in UNIX.")]
		[InlineData("randomasdasdas.dll")]
		[InlineData("randomasdasdas.so")]
		public void Test_NativeLibrary_NotFound(string library)
		{
			Assert.Throws<DllNotFoundException>(() => new NativeLibrary(library));
		}
	}
}
