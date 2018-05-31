using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyModel;
using PLATFORMS = Microsoft.DotNet.PlatformAbstractions;

namespace Unmanaged
{
	/// <summary>
	/// Enumerates possible library load targets. This default implementation returns the following load targets:
	/// First: The library contained in the applications base folder.
	/// Second: The simple name, unchanged.
	/// Third: The library as resolved via the default DependencyContext, in the default nuget package cache folder.
	/// </summary>
	public class DefaultPathResolver : IPathResolver
	{
		#region IPathResolver Members

		/// <summary>
		/// Returns an enumerator which yields possible library load paths, in priority order.
		/// </summary>
		/// <param name="name">The name of the library to load.</param>
		/// <returns>An enumerator yielding load targets.</returns>
		public IEnumerable<string> EnumeratePaths(string name)
		{
			yield return Path.Combine(AppContext.BaseDirectory, name);
			yield return name;

			if (TryLocateNativeAssetFromDeps(name, out string depsResolvedPath))
			{
				yield return depsResolvedPath;
			}
		}

		#endregion

		private bool TryLocateNativeAssetFromDeps(string name, out string depsResolvedPath)
		{
			DependencyContext context = DependencyContext.Default;

			if (context == null)
			{
				depsResolvedPath = null;
				return false;
			}

			string runtimeIdentifier = PLATFORMS.RuntimeEnvironment.GetRuntimeIdentifier();

			var allRIDs = new List<string>
			{
				runtimeIdentifier
			};

			if (!AddFallbacks(allRIDs, runtimeIdentifier, context.RuntimeGraph))
			{
				string guessedFallbackRID = GuessFallbackRID(runtimeIdentifier);
				if (guessedFallbackRID != null)
				{
					allRIDs.Add(guessedFallbackRID);
					AddFallbacks(allRIDs, guessedFallbackRID, context.RuntimeGraph);
				}
			}

			foreach (string rid in allRIDs)
			{
				foreach (var runtimeLib in context.RuntimeLibraries)
				{
					foreach (var nativeAsset in runtimeLib.GetRuntimeNativeAssets(context, rid))
					{
						if (Path.GetFileName(nativeAsset) == name || Path.GetFileNameWithoutExtension(nativeAsset) == name)
						{
							string fullPath = Path.Combine(
								GetNugetPackagesRootDirectory(),
								runtimeLib.Name.ToLowerInvariant(),
								runtimeLib.Version, nativeAsset);
							fullPath = Path.GetFullPath(fullPath);
							depsResolvedPath = fullPath;
							return true;
						}
					}
				}
			}

			depsResolvedPath = null;
			return false;
		}

		private string GuessFallbackRID(string actualRuntimeIdentifier)
		{
			if (actualRuntimeIdentifier == "osx.10.13-x64")
			{
				return "osx.10.12-x64";
			}
			else if (actualRuntimeIdentifier.StartsWith("osx"))
			{
				return "osx-x64";
			}

			return null;
		}

		private bool AddFallbacks(List<string> fallbacks, string rid, IReadOnlyList<RuntimeFallbacks> allFallbacks)
		{
			foreach (RuntimeFallbacks fallback in allFallbacks)
			{
				if (fallback.Runtime == rid)
				{
					fallbacks.AddRange(fallback.Fallbacks);
					return true;
				}
			}

			return false;
		}

		private string GetNugetPackagesRootDirectory()
		{
			// TODO: Handle alternative package directories, if they are configured.
			return Path.Combine(GetUserDirectory(), ".nuget", "packages");
		}

		private string GetUserDirectory()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				return Environment.GetEnvironmentVariable("USERPROFILE");
			}
			else
			{
				return Environment.GetEnvironmentVariable("HOME");
			}
		}
	}
}
