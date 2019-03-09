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
			Platform.Windows.AssertOSPlatform();
		}

		[PlatformSpecificTestMethod(Platform.Linux)]
		public void TestMethod_Linux()
		{
			Platform.Linux.AssertOSPlatform();
		}

		[PlatformSpecificTestMethod(Platform.OSX)]
		public void TestMethod_OSX()
		{
			Platform.OSX.AssertOSPlatform();
		}

		[PlatformSpecificTestMethod(Platform.Unix)]
		public void TestMethod_Unix()
		{
			Platform.Unix.AssertOSPlatform();
		}
	}
}
