namespace Unmanaged
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.InteropServices;

	internal static class UnmanagedHelper
	{
		public static bool TryGetFunction(
			FieldInfo field,
			IntPtr libraryHandle,
			Expression<Func<UnmanagedCallback>> expression,
			out Delegate @delegate)
		{
			if (field == null)
			{
				throw new ArgumentNullException(nameof(field));
			}

			if (expression == null)
			{
				throw new ArgumentNullException(nameof(expression));
			}

			if (libraryHandle == IntPtr.Zero)
			{
				throw new ArgumentException("Invalid library handle.", nameof(libraryHandle));
			}

			LoadFunctionAttribute attribute = field.GetCustomAttribute<LoadFunctionAttribute>();

			IntPtr methodHandle = NativeLibrary.GetExport(libraryHandle, attribute.EntryPoint);

			if (methodHandle == IntPtr.Zero)
			{
				@delegate = field.FieldType.GetEmptyDebugDelegate(expression);
				return false;
			}
			else if (!attribute.DisableOnFunctionCall)
			{
				@delegate = methodHandle.GetDebugDelegate(field.FieldType, expression);
				return true;
			}
			else
			{
				@delegate = Marshal.GetDelegateForFunctionPointer(methodHandle, field.FieldType);
				return true;
			}
		}

		public static bool TryGetFunction(
			FieldInfo field,
			IntPtr libraryHandle,
			out Delegate @delegate)
		{
			if (field == null)
			{
				throw new ArgumentNullException(nameof(field));
			}

			if (libraryHandle == IntPtr.Zero)
			{
				throw new ArgumentException("Invalid library handle.", nameof(libraryHandle));
			}

			LoadFunctionAttribute attribute = field.GetCustomAttribute<LoadFunctionAttribute>();

			IntPtr methodHandle = NativeLibrary.GetExport(libraryHandle, attribute.EntryPoint);

			if (methodHandle != IntPtr.Zero)
			{
				@delegate = Marshal.GetDelegateForFunctionPointer(methodHandle, field.FieldType);
				return true;
			}

			@delegate = null;
			return false;
		}
	}
}
