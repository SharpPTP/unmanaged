namespace Unmanaged
{
	using System;
	using System.Runtime.InteropServices;
	using Unmanaged.Native;

	/// <summary>
	/// Represents a helper class for working with unmanaged assemblies.
	/// </summary>
	/// <remarks>
	/// TODO: Replace with COREFX's version when its released.
	/// </remarks>
	public class NativeLibrary : INativeLibrary
	{
		private readonly IntPtr _handle;
		private bool _disposed = false;

		/// <summary>
		/// Initializes the <see cref="NativeLibrary"/> instance.
		/// </summary>
		/// <param name="libraryName">the library name</param>
		public NativeLibrary(string libraryName)
		{
			if (libraryName == null)
			{
				throw new ArgumentNullException(nameof(libraryName));
			}

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				_handle = Kernel32.LoadLibraryEx(libraryName, IntPtr.Zero, 0);
			}
			else
			{
				_handle = LibDL.LoadLibrary(libraryName, LibDL.RTLD_NOW);
			}

			if (_handle == IntPtr.Zero)
			{
				throw new DllNotFoundException($"Failed to load library with name `{libraryName}`.");
			}
		}

		/// <summary>
		/// Initializes the <see cref="NativeLibrary"/> instance.
		/// </summary>
		/// <param name="winLibraryName">Windows DLL name</param>
		/// <param name="unixLibraryName">UNIX SO name</param>
		public NativeLibrary(string winLibraryName, string unixLibraryName)
			: this(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? winLibraryName : unixLibraryName)
		{
		}

		/// <summary>
		/// Destroys the object.
		/// </summary>
		~NativeLibrary()
		{
			Dispose(false);
		}

		#region INativeLibrary Members

		/// <inheritdoc />
		public IntPtr GetAddress(string entryPoint)
		{
			if (entryPoint == null)
			{
				throw new ArgumentNullException(nameof(entryPoint));
			}

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				return Kernel32.GetProcAddress(_handle, entryPoint);
			}
			else
			{
				return LibDL.GetProcAddress(_handle, entryPoint);
			}
		}

		/// <inheritdoc />
		public TDelegate GetDelegate<TDelegate>(string entryPoint)
		{
			if (entryPoint == null)
			{
				throw new ArgumentNullException(nameof(entryPoint));
			}

			IntPtr handle = GetAddress(entryPoint);

			if (handle != IntPtr.Zero)
			{
				TDelegate @delegate = Marshal.GetDelegateForFunctionPointer<TDelegate>(handle);

				return @delegate;
			}

			return default;
		}

		/// <inheritdoc />
		public Delegate GetDelegate(Type delegateType, string entryPoint)
		{
			if (delegateType == null)
			{
				throw new ArgumentNullException(nameof(delegateType));
			}

			if (entryPoint == null)
			{
				throw new ArgumentNullException(nameof(entryPoint));
			}

			IntPtr handle = GetAddress(entryPoint);

			if (handle != IntPtr.Zero)
			{
				Delegate @delegate = Marshal.GetDelegateForFunctionPointer(handle, delegateType);

				return @delegate;
			}

			return default;
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Releases the library handle.
		/// </summary>
		/// <param name="disposing">a flag indicating whether object is being disposed or destroyed</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
				}

				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					Kernel32.FreeLibrary(_handle);
				}
				else
				{
					LibDL.FreeLibrary(_handle);
				}

				_disposed = true;
			}
		}

		/// <summary>
		/// Releases the library handle.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		#endregion
	}
}
