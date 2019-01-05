namespace Unmanaged.Tests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System;

	[TestClass]
	[TestCategory("Unmanaged")]
	public partial class NativeLibraryTest
	{
		[TestMethod]
		[DataRow("randomasdasdas.dll")]
		[DataRow("randomasdasdas.so")]
		public void Test_NativeLibrary_NotFound(string library)
		{
			Assert.ThrowsException<DllNotFoundException>(() => new NativeLibrary(library));
		}
	}
}
