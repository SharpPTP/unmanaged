namespace Unmanaged
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Reflection.Emit;
	using System.Runtime.InteropServices;

	/// <summary>
	/// Provides extensions methods for reflection.
	/// </summary>
	public static class ReflectionExtensions
	{
		/// <summary>
		/// Converts by ref type to pointer type.
		/// </summary>
		/// <param name="type">the type</param>
		/// <returns>return a new type</returns>
		public static Type GetPointerType(this Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type));
			}

			return type.IsByRef
				? Type.GetType(type.FullName.Replace("&", "*"))
				: type;
		}

		/// <summary>
		/// Enumerates parameters types.
		/// </summary>
		/// <param name="parameters">the parameters</param>
		/// <returns>returns a array of <see cref="Type"/></returns>
		public static Type[] GetTypes(this ParameterInfo[] parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException(nameof(parameters));
			}

			return parameters
				.Select(i => i.ParameterType)
				.ToArray();
		}

		/// <summary>
		/// Gets the invoke method info of a invokable type.
		/// </summary>
		/// <param name="type">the type</param>
		/// <returns>returns a <see cref="MethodInfo"/></returns>
		public static MethodInfo GetInvokeMethod(this Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type));
			}

			return type.GetMethod("Invoke");
		}

		/// <summary>
		/// Puts a <see cref="IntPtr"/> handle in IL generator instructions.
		/// </summary>
		/// <param name="il">the IL generator</param>
		/// <param name="handle">the handle</param>
		public static void EmitMethodHandle(this ILGenerator il, IntPtr handle)
		{
			if (il == null)
			{
				throw new ArgumentNullException(nameof(il));
			}

			if (handle == IntPtr.Zero)
			{
				throw new ArgumentException($"Invalid method handle `{handle}`.", nameof(handle));
			}

			if (IntPtr.Size == 4)
			{
				il.Emit(OpCodes.Ldc_I4, handle.ToInt32());
			}
			else if (IntPtr.Size == 8)
			{
				il.Emit(OpCodes.Ldc_I8, handle.ToInt64());
			}
			else
			{
				throw new PlatformNotSupportedException();
			}
		}

		/// <summary>
		/// Puts a <see cref="MethodInfo"/> of expression in IL generator instructions.
		/// </summary>
		/// <param name="il">the IL generator</param>
		/// <param name="expression">the expression</param>
		/// <param name="invoke">the expression method info</param>
		public static bool EmitExpressionCall(this ILGenerator il, Expression<Func<UnmanagedCallback>> expression, out MethodInfo invoke)
		{
			if (il == null)
			{
				throw new ArgumentNullException(nameof(il));
			}

			if (expression == null)
			{
				throw new ArgumentNullException(nameof(expression));
			}

			if (expression.Body is MemberExpression body)
			{
				if (body.Member is PropertyInfo propertyInfo)
				{
					// TODO: ensure static

					invoke = propertyInfo.PropertyType.GetInvokeMethod();

					il.Emit(OpCodes.Call, propertyInfo.GetGetMethod());

					return true;
				}
				else if (body.Member is FieldInfo fieldInfo)
				{
					// TODO: ensure static

					invoke = fieldInfo.FieldType.GetInvokeMethod();

					il.Emit(OpCodes.Ldsfld, fieldInfo);

					return true;
				}
				else
				{
					invoke = null;

					return false;
				}
			}
			else
			{
				invoke = null;

				return false;
			}
		}

		/// <summary>
		/// Constructs a empty DEBUG delegate by type and expression.
		/// </summary>
		/// <param name="delegateType">The delegate type.</param>
		/// <param name="expression">The expression</param>
		/// <returns>returns a <see cref="Delegate"/> of the same type</returns>
		public static Delegate GetEmptyDebugDelegate(this Type delegateType, Expression<Func<UnmanagedCallback>> expression)
		{
			if (delegateType == null)
			{
				throw new ArgumentNullException(nameof(delegateType));
			}

			if (expression == null)
			{
				throw new ArgumentNullException(nameof(expression));
			}

			MethodInfo methodInfo = delegateType.GetInvokeMethod();
			ParameterInfo[] parameterInfos = methodInfo.GetParameters();
			Type returnType = methodInfo.ReturnType;

			var method = new DynamicMethod($"{delegateType.Name}Calli", returnType, parameterInfos.GetTypes(), delegateType, true);

			ILGenerator il = method.GetILGenerator();

			if (!il.EmitExpressionCall(expression, out MethodInfo expressionInfo))
			{
				throw new Exception("Invalid expression.");
			}

			il.Emit(OpCodes.Ldstr, delegateType.Name);
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Callvirt, expressionInfo);

			if (returnType != typeof(void))
			{
				il.Emit(OpCodes.Ldloc, il.DeclareLocal(returnType));
			}

			il.Emit(OpCodes.Ret);

			return method.CreateDelegate(delegateType);
		}

		/// <summary>
		/// Constructs a DEBUG delegate by type and expression.
		/// </summary>
		/// <param name="ptr">the delegate handle</param>
		/// <param name="delegateType">the delegate type</param>
		/// <param name="expression">the expression</param>
		/// <returns>returns a <see cref="Delegate"/> of the same type</returns>
		public static Delegate GetDebugDelegate(this IntPtr ptr, Type delegateType, Expression<Func<UnmanagedCallback>> expression)
		{
			if (delegateType == null)
			{
				throw new ArgumentNullException(nameof(delegateType));
			}

			if (expression == null)
			{
				throw new ArgumentNullException(nameof(expression));
			}

			MethodInfo methodInfo = delegateType.GetInvokeMethod();

			Type returnType = methodInfo.ReturnType;
			Type returnPointerType = returnType.GetPointerType();
			ParameterInfo[] parameters = methodInfo.GetParameters();

			var dm = new DynamicMethod($"{delegateType.Name}Calli", returnType, parameters.GetTypes(), delegateType, true);

			ILGenerator il = dm.GetILGenerator();

			bool hasReturnType = returnType != typeof(void);
			LocalBuilder local = hasReturnType
				? il.DeclareLocal(returnType)
				: null;

			for (int i = 0; i < parameters.Length; i++)
			{
				il.Emit(OpCodes.Ldarg, i);
			}

			il.EmitMethodHandle(ptr);
			il.EmitCalli(OpCodes.Calli, CallingConvention.Cdecl, returnPointerType, parameters.GetTypes());

			if (hasReturnType)
			{
				il.Emit(OpCodes.Stloc, local);
			}

			if (!il.EmitExpressionCall(expression, out MethodInfo expressionInfo))
			{
				throw new Exception("Invalid expression.");
			}

			il.Emit(OpCodes.Ldstr, delegateType.Name);
			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Callvirt, expressionInfo);

			if (hasReturnType)
			{
				il.Emit(OpCodes.Ldloc, local);
			}

			il.Emit(OpCodes.Ret);

			return dm.CreateDelegate(delegateType);
		}
	}
}
