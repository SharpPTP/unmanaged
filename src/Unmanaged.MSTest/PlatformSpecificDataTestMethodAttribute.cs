using System;

namespace Unmanaged.MSTest
{
	/// <summary>
	/// Attribute that is applied to a method to indicate that it is a fact that should be run
	/// by the test runner only on a specific platform.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class PlatformSpecificDataTestMethodAttribute : PlatformSpecificTestMethodAttribute
	{
		/// <summary>
		/// Creates a new instance of <see cref="PlatformSpecificDataTestMethodAttribute"/>.
		/// </summary>
		/// <param name="platform">the platform</param>
		public PlatformSpecificDataTestMethodAttribute(Platform platform)
			: base(platform)
		{
		}
	}
}
