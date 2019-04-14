using System.IO;
using System.Threading.Tasks;
using System.Xml;
using NuGetHelper.Models;

namespace NuGetHelper.Helpers
{
    internal class ProjectFileParser
    {
        public ProjectDetails ProjectInfo { get; private set; }

        public ProjectFileParser()
        {
            ProjectInfo = new ProjectDetails();
        }

        public ProjectFileParser(string csProjectFilePath) : this()
        {
            SetCSProjectFile(csProjectFilePath);
        }

        public void SetCSProjectFile(string csProjectFilePath)
        {
            ValidationHelper.ThrowIfStringIsNullOrEmpty(csProjectFilePath);
            ProjectInfo.Config.CSProjectFilePath = csProjectFilePath;
            ProjectInfo.Config.ProjectName = new FileInfo(csProjectFilePath).Name;
            CheckIfPackagesConfigExists();
        }

        public void Parse()
        {
            ParseCSProjectFile();
            
            if (ProjectInfo.Config.PackagesConfigExists)
                ParsePackagesConfigFile();
        }

        public async Task ObtainNugetMetadataAsync()
        {
            foreach (var package in ProjectInfo.Packages)
            {
                var metadata = await NuGetTools.GetNugetPackageMetadataAsync(package.Name, package.Version);
                package.Metadata = metadata;
            }
        }
        
        private void ParseCSProjectFile()
        {
            ValidationHelper.ThrowIfStringIsNullOrEmpty(ProjectInfo.Config.CSProjectFilePath);
            
            var doc = new XmlDocument();
            doc.Load(ProjectInfo.Config.CSProjectFilePath);
            
            var xRoot = doc.DocumentElement;
            var targetFrameworkNode = xRoot.SelectSingleNode("//PropertyGroup/TargetFramework");
            var targetFramework = targetFrameworkNode?.InnerText ?? "n/a";

            ProjectInfo.TargetFramework = targetFramework;
            
            var packageReferencesNodes = xRoot.SelectNodes("ItemGroup/PackageReference");
            foreach (XmlNode packageNode in packageReferencesNodes)
            {
                var includeAttribute = packageNode.Attributes["Include"];
                var versionAttribute = packageNode.Attributes["Version"];
                
                if (includeAttribute != null && versionAttribute != null)
                {
                    ProjectInfo.Packages.Add(NuGetPackageDetails.Create(
                        includeAttribute.Value,
                        versionAttribute.Value,
                        NuGetPackageLocation.CSProjectFile,
                        NuGetPackageType.NuGet));
                }
            }

            var cliToolsReferencesNodes = xRoot.SelectNodes("ItemGroup/DotNetCliToolReference");
            foreach (XmlNode cliToolNode in cliToolsReferencesNodes)
            {
                var includeAttribute = cliToolNode.Attributes["Include"];
                var versionAttribute = cliToolNode.Attributes["Version"];
                
                if (includeAttribute != null && versionAttribute != null)
                {
                    ProjectInfo.Packages.Add(NuGetPackageDetails.Create(
                        includeAttribute.Value,
                        versionAttribute.Value,
                        NuGetPackageLocation.CSProjectFile,
                        NuGetPackageType.CLITool));
                }
            }
        }

        private void CheckIfPackagesConfigExists()
        {
            var rootDirectory = Directory.GetParent(ProjectInfo.Config.CSProjectFilePath).FullName;
            var packagesConfigPath = Path.Combine(rootDirectory, "packages.config");

            ProjectInfo.Config.PackagesConfigExists = File.Exists(packagesConfigPath);

            if (ProjectInfo.Config.PackagesConfigExists)
                ProjectInfo.Config.PackagesConfigFilePath = packagesConfigPath;
        }
        
        private void ParsePackagesConfigFile()
        {
            ValidationHelper.ThrowIfStringIsNullOrEmpty(ProjectInfo.Config.CSProjectFilePath);
            
            if (!ProjectInfo.Config.PackagesConfigExists)
                return;
            
            var packagesDoc = new XmlDocument();
            packagesDoc.Load(ProjectInfo.Config.PackagesConfigFilePath);

            var packagesXRoot = packagesDoc.DocumentElement;
            var packageNodes = packagesXRoot.SelectNodes("//packages/package");

            foreach (XmlNode packageNode in packageNodes)
            {
                var idAttribute = packageNode.Attributes["id"];
                var versionAttribute = packageNode.Attributes["version"];
                
                if (idAttribute != null && versionAttribute != null)
                {
                    ProjectInfo.Packages.Add(NuGetPackageDetails.Create(
                        idAttribute.Value,
                        versionAttribute.Value,
                        NuGetPackageLocation.PackagesConfigFile,
                        NuGetPackageType.NuGet));
                }
            }
        }
    }
}