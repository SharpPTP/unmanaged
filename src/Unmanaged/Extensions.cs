namespace Unmanaged
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using System.Runtime.InteropServices;

	/// <summary>
	/// Provides extensions methods.
	/// </summary>
	public static class Extensions
	{
		private const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

		/// <summary>
		/// Copies all characters up to the first null character from an unmanaged ANSI string
		/// to a managed System.String, and widens each ANSI character to Unicode.
		/// </summary>
		/// <param name="ptr">The address of the first character of the unmanaged string.</param>
		/// <returns>A managed string that holds a copy of the unmanaged ANSI string. If handle is null,
		/// the method returns a null string.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetAnsiString(this IntPtr ptr)
		{
			return ptr != IntPtr.Zero
				? Marshal.PtrToStringAnsi(ptr)
				: null;
		}

		/// <summary>
		/// Loads all unmanaged functions of provided type.
		/// </summary>
		/// <param name="type">The type</param>
		/// <param name="methodHandleFactory">The function that retrieves the handle</param>
		/// <param name="expression">The on function call event</param>
		/// <returns>returns a collection of error string</returns>
		public static ICollection<string> LoadFunctions(this Type type,
			Func<string, IntPtr> methodHandleFactory,
			Expression<Func<UnmanagedCallback>> expression)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type));
			}

			if (methodHandleFactory == null)
			{
				throw new ArgumentNullException(nameof(methodHandleFactory));
			}

			if (expression == null)
			{
				throw new ArgumentNullException(nameof(expression));
			}

			var errors = new List<string>();

			IEnumerable<FieldInfo> fields = type
				.GetFields(DefaultBindingFlags)
				.Where(i => i.IsDefined(typeof(LoadFunctionAttribute)));

			foreach (FieldInfo field in fields)
			{
				if (!UnmanagedHelper.TryGetFunction(field, methodHandleFactory, expression, out Delegate @delegate))
				{
					LoadFunctionAttribute attribute = field.GetCustomAttribute<LoadFunctionAttribute>();

					errors.Add($"Failed to load `{type.Name}` function `{attribute.EntryPoint}`.");
				}

				field.SetValue(null, @delegate);
			}

			return errors;
		}

		/// <summary>
		/// Loads all unmanaged functions of provided type and binding flags.
		/// </summary>
		/// <param name="type">The type</param>
		/// <param name="methodHandleFactory">The function that retrieves the handle</param>
		/// <returns>returns a collection of error string</returns>
		public static ICollection<string> LoadFunctions(
			this Type type,
			Func<string, IntPtr> methodHandleFactory)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type));
			}

			if (methodHandleFactory == null)
			{
				throw new ArgumentNullException(nameof(methodHandleFactory));
			}

			var errors = new List<string>();

			IEnumerable<FieldInfo> fields = type
				.GetFields(DefaultBindingFlags)
				.Where(i => i.IsDefined(typeof(LoadFunctionAttribute)));

			foreach (FieldInfo field in fields)
			{
				if (!UnmanagedHelper.TryGetFunction(field, methodHandleFactory, out Delegate @delegate))
				{
					LoadFunctionAttribute attribute = field.GetCustomAttribute<LoadFunctionAttribute>();

					errors.Add($"Failed to load `{type.Name}` function `{attribute.EntryPoint}`.");
				}

				field.SetValue(null, @delegate);
			}

			return errors;
		}
	}
}
