namespace Unmanaged.MSTest.Tests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	[TestCategory("Unmanaged.MSTest")]
	public class PlatformSpecificDataTestMethodAttributeTests
	{
		[PlatformSpecificDataTestMethod(Platform.Windows)]
		[DataRow(Platform.Windows)]
		public void DataTestMethod_Windows(Platform platform)
		{
			platform.AssertOSPlatform();
		}

		[PlatformSpecificDataTestMethod(Platform.Linux)]
		[DataRow(Platform.Linux)]
		public void DataTestMethod_Linux(Platform platform)
		{
			platform.AssertOSPlatform();
		}

		[PlatformSpecificDataTestMethod(Platform.OSX)]
		[DataRow(Platform.OSX)]
		public void DataTestMethod_OSX(Platform platform)
		{
			platform.AssertOSPlatform();
		}

		[PlatformSpecificDataTestMethod(Platform.Unix)]
		[DataRow(Platform.Unix)]
		public void DataTestMethod_Unix(Platform platform)
		{
			platform.AssertOSPlatform();
		}
	}
}
