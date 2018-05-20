using System;
using System.Runtime.InteropServices;

namespace Unmanaged.XUnit
{
	/// <summary>
	/// Represents an enumeration of platforms defined in <see cref="OSPlatform"/>.
	/// </summary>
	[Flags]
	public enum Platform
	{
		/// <summary>
		/// Linux
		/// </summary>
		Linux = 1,

		/// <summary>
		/// OSX
		/// </summary>
		OSX = 2,

		/// <summary>
		/// Windows
		/// </summary>
		Windows = 4,

		/// <summary>
		/// Unix (Linux and OSX)
		/// </summary>
		Unix = Linux | OSX,
	}
}
