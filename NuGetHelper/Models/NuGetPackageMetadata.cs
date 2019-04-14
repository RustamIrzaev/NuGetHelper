using System;
using NuGet.Protocol.Core.Types;

namespace NuGetHelper.Models
{
    public class NuGetPackageMetadata
    {
        public string Tags { get; set; }
        public Uri ProjectUrl { get; set; }
        public Uri LicenseUrl { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Authors { get; set; }
        public string Owners { get; set; }
        public string Title { get; set; }
        
        public static NuGetPackageMetadata Create(IPackageSearchMetadata packageMetadata)
        {
            return new NuGetPackageMetadata
            {
                Tags = packageMetadata.Tags,
                Title = packageMetadata.Title,
                Owners = packageMetadata.Owners,
                Authors = packageMetadata.Authors,
                Summary = packageMetadata.Summary,
                Description = packageMetadata.Description,
                LicenseUrl = packageMetadata.LicenseUrl,
                ProjectUrl = packageMetadata.ProjectUrl
            };
        }
    }
}