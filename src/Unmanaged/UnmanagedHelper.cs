namespace Unmanaged
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.InteropServices;

	internal static class UnmanagedHelper
	{
		public static bool TryGetFunction(FieldInfo field, Expression<Func<object>> onFunctionCall, out Delegate @delegate)
		{
			if (field == null)
			{
				throw new ArgumentNullException(nameof(field));
			}

			LoadFunctionAttribute attribute = field.GetCustomAttribute<LoadFunctionAttribute>();

			IntPtr methodHandle = attribute.GetFunctionPointer.Invoke(attribute.EntryPoint);

			if (methodHandle == IntPtr.Zero && onFunctionCall != null)
			{
				@delegate = field.FieldType.GetEmptyDebugDelegate(onFunctionCall);
				return false;
			}
			else if (!attribute.DisableOnFunctionCall)
			{
				// TODO:
				//	@delegate = ptr.GetDebugDelegate(field.FieldType, onFunctionCall);
				//	return true;
			}

			@delegate = Marshal.GetDelegateForFunctionPointer(methodHandle, field.FieldType);
			return true;
		}

		public static bool TryGetFunction(FieldInfo field, out Delegate @delegate)
		{
			if (field == null)
			{
				throw new ArgumentNullException(nameof(field));
			}

			LoadFunctionAttribute attribute = field.GetCustomAttribute<LoadFunctionAttribute>();

			IntPtr methodHandle = attribute.GetFunctionPointer.Invoke(attribute.EntryPoint);

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
