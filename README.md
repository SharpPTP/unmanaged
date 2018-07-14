## UNMANAGED

### Build Status

|               |                                                                   Build Status                                                                   |
| ------------- | :----------------------------------------------------------------------------------------------------------------------------------------------: |
| **Linux/Mac** |             [![Build Status](https://travis-ci.org/SharpPTP/unmanaged.svg?branch=master)](https://travis-ci.org/SharpPTP/unmanaged)              |
| **Windows**   | [![Build Status](https://ci.appveyor.com/api/projects/status/d6n5dnvukkrrsli7?svg=true)](https://ci.appveyor.com/project/petarpetrovt/unmanaged) |

### Installation

| Package Name  | Release (NuGet)                                                                                     |
| ------------- | --------------------------------------------------------------------------------------------------- |
| **Unmanaged** | [![NuGet](https://img.shields.io/nuget/v/Unmanaged.svg)](https://www.nuget.org/packages/Unmanaged/) |

### API

Native library class example.

```csharp
using (var lib = new NativeLibrary(libraryName))
{
    IntPtr handle = lib.GetAddress(entryPoint);

    Assert.True(handle != IntPtr.Zero);
}
```

Load function attribute example.

```csharp
public static class TestKernel32Wrapper
{
    static TestKernel32Wrapper()
    {
        using (var lib = new NativeLibrary("kernel32.dll"))
        {
            ICollection<string> errors = typeof(TestKernel32Wrapper)
                .LoadFunctions(lib.GetAddress, (s, b) => { Console.WriteLine($"Function `{s}` called."); });
        }
    }

    public delegate uint GetTickCountDelegate();

    [LoadFunction("GetTickCount")]
    public static GetTickCountDelegate KernelGetTickCount;
}
```

### Deploy and test

Run `deploy.cmd`, `deploy.ps1`, or `deploy.sh`.

### Team

* Petar Petrov
