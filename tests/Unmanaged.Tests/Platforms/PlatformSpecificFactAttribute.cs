namespace Unmanaged.Tests.Platforms
{
	using System;
	using System.Runtime.InteropServices;
	using Xunit;
	using Xunit.Sdk;

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	[XunitTestCaseDiscoverer("Xunit.Sdk.FactDiscoverer", "xunit.execution.{Platform}")]
	public class PlatformSpecificFactAttribute : FactAttribute
	{
		public new string Skip { get; protected set; }

		public PlatformSpecificFactAttribute(Platform platform)
		{
			Skip = !IsOSPlatform(platform)
				? $"Test is `{platform}` platform specific."
				: null;
		}

		private static bool IsOSPlatform(Platform platform)
		{
			switch (platform)
			{
				case Platform.Linux:
					return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
				case Platform.OSX:
					return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
				case Platform.Windows:
					return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
				default:
					throw new ArgumentException($"Unsupported OS platform `{platform}`.");
			}
		}
	}
}
