namespace Unmanaged
{
	using System;

	/// <summary>
	/// Represents a helper class for working with unmanaged assemblies.
	/// </summary>
	public interface INativeLibrary : IDisposable
	{
		/// <summary>
		/// Gets the address to the corresponding function.
		/// </summary>
		/// <param name="entryPoint">The function name</param>
		/// <returns>returns a <see cref="IntPtr"/> handle</returns>
		IntPtr GetAddress(string entryPoint);

		/// <summary>
		/// Gets the delegate of address to the corresponding function.
		/// </summary>
		/// <typeparam name="TDelegate">The delegate type</typeparam>
		/// <param name="entryPoint">The function name</param>
		/// <returns>returns a the delegate</returns>
		TDelegate GetDelegate<TDelegate>(string entryPoint);

		/// <summary>
		/// Gets the delegate of address to the corresponding function.
		/// </summary>
		/// <param name="delegateType">The delegate type</param>
		/// <param name="entryPoint">The function name</param>
		/// <returns>returns a <see cref="Delegate"/> object</returns>
		Delegate GetDelegate(Type delegateType, string entryPoint);
	}
}