using System.Collections.Generic;

namespace Unmanaged
{
	/// <summary>
	/// Represents a helper class for resolving native library paths.
	/// </summary>
	public interface IPathResolver
	{
		/// <summary>
		/// Enumerates the possible target paths.
		/// </summary>
		/// <param name="libraryName">the library name</param>
		/// <returns>returns a enumeration of possible paths.</returns>
		IEnumerable<string> EnumeratePaths(string libraryName);
	}
}