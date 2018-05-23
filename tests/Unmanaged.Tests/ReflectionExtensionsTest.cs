namespace Unmanaged.Tests
{
	using System;
	using Unmanaged.XUnit;
	using Xunit;

	public class ReflectionExtensionsTest
	{
		[PlatformSpecificFact(Platform.Windows)]
		public void Test_GetDebugDelegate_Throws()
		{
			using (var lib = new NativeLibrary("kernel32.dll"))
			{
				IntPtr methodHandle = lib.GetAddress("GetTickCount");

				GetTickCountDelegate @delegate = (GetTickCountDelegate)methodHandle
					.GetDebugDelegate(typeof(GetTickCountDelegate), () => CallbackThrows);

				Assert.Throws<UnmanagedCallbackTestException>(() => @delegate.Invoke());
			}
		}

		[PlatformSpecificFact(Platform.Windows)]
		public void Test_GetDebugDelegate()
		{
			using (var lib = new NativeLibrary("kernel32.dll"))
			{
				IntPtr methodHandle = lib.GetAddress("GetTickCount");

				GetTickCountDelegate @delegate = (GetTickCountDelegate)methodHandle
					.GetDebugDelegate(typeof(GetTickCountDelegate), () => Callback);

				uint result = @delegate.Invoke();

				Assert.True(result != default);
			}
		}

		[Fact]
		public void Test_GetEmptyDebugDelegate_Throws()
		{
			GetTickCountDelegate @delegate = (GetTickCountDelegate)typeof(GetTickCountDelegate)
				.GetEmptyDebugDelegate(() => CallbackThrows);

			Assert.Throws<UnmanagedCallbackTestException>(() => @delegate.Invoke());
		}

		[Fact]
		public void Test_GetEmptyDebugDelegate()
		{
			GetTickCountDelegate @delegate = (GetTickCountDelegate)typeof(GetTickCountDelegate)
				.GetEmptyDebugDelegate(() => Callback);

			uint result = @delegate.Invoke();

			Assert.True(result == default);
		}

		#region UnmanagedCallbackTestException, GetTickCountDelegate, Callback

		public static UnmanagedCallback CallbackThrows
			=> (a, b) => throw new UnmanagedCallbackTestException();

		public static UnmanagedCallback Callback
			=> (a, b) => { };

		private delegate uint GetTickCountDelegate();

		internal class UnmanagedCallbackTestException : Exception { }

		#endregion
	}
}
