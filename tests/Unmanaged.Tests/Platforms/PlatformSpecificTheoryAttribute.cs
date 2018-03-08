namespace Unmanaged.Tests.Platforms
{
	using System;
	using Xunit.Sdk;

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	[XunitTestCaseDiscoverer("Xunit.Sdk.TheoryDiscoverer", "xunit.execution.{Platform}")]
	public class PlatformSpecificTheoryAttribute : PlatformSpecificFactAttribute
	{
		public PlatformSpecificTheoryAttribute(Platform platform)
			: base(platform)
		{
		}
	}
}
