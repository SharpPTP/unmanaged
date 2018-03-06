namespace Unmanaged
{
	using System;

	/// <summary>
	/// Class that provides a description for an unmanaged function.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class LoadFunctionAttribute : Attribute
	{
		/// <summary>
		/// Gets or sets the function entry point.
		/// </summary>
		public string EntryPoint { get; }

		/// <summary>
		/// Gets or sets the flag indicating if log on function call is enabled.
		/// </summary>
		public bool DisableOnFunctionCall { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="LoadFunctionAttribute"/> class.
		/// </summary>
		/// <param name="entryPoint">The value of the entryPoint.</param>
		public LoadFunctionAttribute(string entryPoint)
		{
			EntryPoint = entryPoint ?? throw new ArgumentNullException(nameof(entryPoint));
		}
	}
}
