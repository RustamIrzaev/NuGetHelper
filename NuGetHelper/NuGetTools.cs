using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using NuGetHelper.Helpers;
using NuGetHelper.Models;

namespace NuGetHelper
{
    internal class NuGetTools
    {
        private const string NugetPackageSource = "https://api.nuget.org/v3/index.json";

        public static async Task<NuGetPackageMetadata> GetNugetPackageMetadataAsync(string packageName, string packageVersion)
        {
            ValidationHelper.ThrowIfStringIsNullOrEmpty(packageName);
            ValidationHelper.ThrowIfStringIsNullOrEmpty(packageVersion);
            
            var providers = new List<Lazy<INuGetResourceProvider>>();
            providers.AddRange(Repository.Provider.GetCoreV3());
            
            var sourceRepository = new SourceRepository(new PackageSource(NugetPackageSource), providers);
            var packageMetadataResource = await sourceRepository.GetResourceAsync<PackageMetadataResource>();
            var packageMetadata = await packageMetadataResource.GetMetadataAsync(
                new PackageIdentity(packageName, NuGetVersion.Parse(packageVersion)), 
                new SourceCacheContext(), 
                new NullLogger(), 
                CancellationToken.None);

            if (packageMetadata == null)
                return null;

            var metadata = NuGetPackageMetadata.Create(packageMetadata);

            return metadata;
        }
    }
}