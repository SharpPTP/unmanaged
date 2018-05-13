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
			Func<string, IntPtr> handleFactory,
			Expression<Func<object>> callback,
			out Delegate @delegate)
		{
			if (field == null)
			{
				throw new ArgumentNullException(nameof(field));
			}

			if (handleFactory == null)
			{
				throw new ArgumentNullException(nameof(handleFactory));
			}

			LoadFunctionAttribute attribute = field.GetCustomAttribute<LoadFunctionAttribute>();

			IntPtr methodHandle = handleFactory.Invoke(attribute.EntryPoint);

			if (methodHandle == IntPtr.Zero && callback != null)
			{
				@delegate = field.FieldType.GetEmptyDebugDelegate(callback);
				return false;
			}
			else if (!attribute.DisableOnFunctionCall)
			{
				@delegate = methodHandle.GetDebugDelegate(field.FieldType, callback);
				return true;
			}

			@delegate = Marshal.GetDelegateForFunctionPointer(methodHandle, field.FieldType);
			return true;
		}

		public static bool TryGetFunction(
			FieldInfo field,
			Func<string, IntPtr> handleFactory,
			out Delegate @delegate)
		{
			if (field == null)
			{
				throw new ArgumentNullException(nameof(field));
			}

			if (handleFactory == null)
			{
				throw new ArgumentNullException(nameof(handleFactory));
			}

			LoadFunctionAttribute attribute = field.GetCustomAttribute<LoadFunctionAttribute>();

			IntPtr methodHandle = handleFactory.Invoke(attribute.EntryPoint);

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
