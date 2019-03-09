namespace Unmanaged.MSTest.Tests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;

	internal static class Extensions
	{
		public static void AssertOSPlatform(this Platform value)
		{
			OSPlatform[] platforms = GetOSPlatforms(value);

			bool result = false;

			foreach (OSPlatform platform in platforms)
			{
				result |= RuntimeInformation.IsOSPlatform(platform);
			}

			Assert.IsTrue(result, "Invalid OS platform.");
		}

		private static OSPlatform[] GetOSPlatforms(Platform value)
		{
			var result = new List<OSPlatform>();

			if (value.HasFlag(Platform.Linux))
			{
				result.Add(OSPlatform.Linux);
			}

			if (value.HasFlag(Platform.OSX))
			{
				result.Add(OSPlatform.OSX);
			}

			if (value.HasFlag(Platform.Windows))
			{
				result.Add(OSPlatform.Windows);
			}

			return result.ToArray();
		}
	}
}
