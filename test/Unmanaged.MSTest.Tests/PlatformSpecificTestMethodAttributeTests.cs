namespace Unmanaged.MSTest.Tests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	[TestCategory("Unmanaged.MSTest")]
	public class PlatformSpecificTestMethodAttributeTests
	{
		[PlatformSpecificTestMethod(Platform.Windows)]
		public void TestMethod_Windows()
		{
		}

		[PlatformSpecificTestMethod(Platform.Linux)]
		public void TestMethod_Linux()
		{
		}

		[PlatformSpecificTestMethod(Platform.OSX)]
		public void TestMethod_OSX()
		{
		}

		[PlatformSpecificTestMethod(Platform.Unix)]
		public void TestMethod_Unix()
		{
		}
	}
}