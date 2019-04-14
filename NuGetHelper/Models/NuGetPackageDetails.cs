namespace NuGetHelper.Models
{
    public class NuGetPackageDetails
    {
        public string Version { get; set; }
        public string Name { get; set; }
        public NuGetPackageLocation Source { get; set; }
        public NuGetPackageType PackageType { get; set; }
        public NuGetPackageMetadata Metadata { get; set; }
        
        public static NuGetPackageDetails Create(string package, string version, 
            NuGetPackageLocation location, NuGetPackageType type)
        {
            return new NuGetPackageDetails
            {
                Name = package,
                Version = version,
                Source = location,
                PackageType = type
            };
        }

        public static NuGetPackageDetails CreateFromPackagesConfig(string package, string version)
        {
            return Create(package, version, 
                NuGetPackageLocation.PackagesConfigFile, 
                NuGetPackageType.NuGet);
        }
        
        public static NuGetPackageDetails CreateFromCSProject(string package, string version, 
            NuGetPackageType type)
        {
            return Create(package, version, 
                NuGetPackageLocation.CSProjectFile, 
                type);
        }
    }
}