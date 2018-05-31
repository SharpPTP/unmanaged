namespace Unmanaged
{
	/// <summary>
	/// Represents a native function callback.
	/// </summary>
	/// <param name="name">the function name</param>
	/// <param name="isEmpty">a flag indicating whether the function was empty or not</param>
	public delegate void UnmanagedCallback(string name, bool isEmpty);
}