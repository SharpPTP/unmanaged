namespace Unmanaged.XUnit
{
	using System;
	using Xunit.Sdk;

	/// <summary>
	/// Marks a test method as being a data theory on specific platform. Data theories are test which are fed
	/// various bits of data from a data source, mapping to parameters on the test method.
	/// If the data source contains multiple rows, then the test method is executed
	/// multiple times (once with each data row). Data is provided by attributes which
	/// derive from <see cref="T:Xunit.Sdk.DataAttribute" /> (notably, <see cref="T:Xunit.InlineDataAttribute" /> and
	/// <see cref="T:Xunit.MemberDataAttribute" />).
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	[XunitTestCaseDiscoverer("Xunit.Sdk.TheoryDiscoverer", "xunit.execution.{Platform}")]
	public class PlatformSpecificTheoryAttribute : PlatformSpecificFactAttribute
	{
		/// <summary>
		/// Creates a new instance of <see cref="PlatformSpecificTheoryAttribute"/>.
		/// </summary>
		/// <param name="platform"></param>
		public PlatformSpecificTheoryAttribute(Platform platform)
			: base(platform)
		{
		}
	}
}
