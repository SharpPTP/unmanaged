namespace Unmanaged.Tests
{
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System;
	using System.Runtime.InteropServices;
	using Unmanaged.MSTest;

	[TestClass]
	[TestCategory("Unmanaged")]
	public class ReflectionExtensionsTest
	{
		[PlatformSpecificTestMethod(Platform.Windows)]
		public void Test_GetDebugDelegate_Throws()
		{
			IntPtr libraryHandle = IntPtr.Zero;

			try
			{
				libraryHandle = NativeLibrary.Load("kernel32.dll");

				IntPtr methodHandle = NativeLibrary.GetExport(libraryHandle, "GetTickCount");

				GetTickCountDelegate @delegate = (GetTickCountDelegate)methodHandle
					.GetDebugDelegate(typeof(GetTickCountDelegate), () => CallbackThrows);

				Assert.ThrowsException<UnmanagedCallbackTestException>(() => @delegate.Invoke());
			}
			finally
			{
				NativeLibrary.Free(libraryHandle);
			}
		}

		[PlatformSpecificTestMethod(Platform.Windows)]
		public void Test_GetDebugDelegate()
		{
			IntPtr libraryHandle = IntPtr.Zero;

			try
			{
				libraryHandle = NativeLibrary.Load("kernel32.dll");

				IntPtr methodHandle = NativeLibrary.GetExport(libraryHandle, "GetTickCount");

				GetTickCountDelegate @delegate = (GetTickCountDelegate)methodHandle
					.GetDebugDelegate(typeof(GetTickCountDelegate), () => Callback);

				uint result = @delegate.Invoke();

				Assert.IsTrue(result != default);
			}
			finally
			{
				NativeLibrary.Free(libraryHandle);
			}
		}

		[TestMethod]
		public void Test_GetEmptyDebugDelegate_Throws()
		{
			GetTickCountDelegate @delegate = (GetTickCountDelegate)typeof(GetTickCountDelegate)
				.GetEmptyDebugDelegate(() => CallbackThrows);

			Assert.ThrowsException<UnmanagedCallbackTestException>(() => @delegate.Invoke());
		}

		[TestMethod]
		public void Test_GetEmptyDebugDelegate()
		{
			GetTickCountDelegate @delegate = (GetTickCountDelegate)typeof(GetTickCountDelegate)
				.GetEmptyDebugDelegate(() => Callback);

			uint result = @delegate.Invoke();

			Assert.IsTrue(result == default);
		}

		[TestMethod]
		public void Test_GetEmptyDebugDelegate_AssertIsEmpty()
		{
			GetTickCountDelegate @delegate = (GetTickCountDelegate)typeof(GetTickCountDelegate)
				.GetEmptyDebugDelegate(() => CallbackEmptyTrue);

			uint result = @delegate.Invoke();

			Assert.IsTrue(result == default);
		}

		[PlatformSpecificTestMethod(Platform.Windows)]
		public void Test_GetEmptyDebugDelegate_AssertIsFalse()
		{
			IntPtr libraryHandle = IntPtr.Zero;

			try
			{
				libraryHandle = NativeLibrary.Load("kernel32.dll");

				IntPtr methodHandle = NativeLibrary.GetExport(libraryHandle, "GetTickCount");

				GetTickCountDelegate @delegate = (GetTickCountDelegate)methodHandle
					.GetDebugDelegate(typeof(GetTickCountDelegate), () => CallbackEmptyFalse);

				uint result = @delegate.Invoke();

				Assert.IsTrue(result != default);
			}
			finally
			{
				NativeLibrary.Free(libraryHandle);
			}
		}

		#region UnmanagedCallbackTestException, GetTickCountDelegate, Callback

		public static UnmanagedCallback CallbackThrows
			=> (methodName, isEmpty) => throw new UnmanagedCallbackTestException();

		public static UnmanagedCallback Callback
			=> (methodName, isEmpty) => { };

		public static UnmanagedCallback CallbackEmptyTrue
			=> (methodName, isEmpty) => Assert.IsTrue(isEmpty);

		public static UnmanagedCallback CallbackEmptyFalse
			=> (methodName, isEmpty) => Assert.IsFalse(isEmpty);

		private delegate uint GetTickCountDelegate();

		internal class UnmanagedCallbackTestException : Exception { }

		#endregion
	}
}
