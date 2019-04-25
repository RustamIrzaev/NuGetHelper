using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using NuGetHelper.Helpers;
using NuGetHelper.Models;

namespace NuGetHelper
{
    internal class ProjectFileParser
    {
        private readonly ParserOptions _options;
        public ProjectDetails ProjectInfo { get; private set; }

        public ProjectFileParser(ParserOptions options)
        {
            _options = options;
            ProjectInfo = new ProjectDetails();
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
            ValidationHelper.ThrowIfStringIsNullOrEmpty(ProjectInfo.Config.CSProjectFilePath);
            
            ParseCSProjectFile();
            
            if (ProjectInfo.Config.PackagesConfigExists && _options.AlwaysIncludeResultsFromPackagesConfigFile)
                ParsePackagesConfigFile();
        }

        public async Task ObtainNugetMetadataAsync()
        {
            foreach (var package in ProjectInfo.Packages)
            {
                if (!string.IsNullOrEmpty(package.Version))
                {
                    var metadata = await NuGetTools.GetNugetPackageMetadataAsync(package.Name, package.Version);
                    package.Metadata = metadata;
                }
            }
        }
        
        private void ParseCSProjectFile()
        {
            var doc = new XmlDocument();
            doc.Load(ProjectInfo.Config.CSProjectFilePath);
            
            foreach (XmlNode node in doc)
            {
                if (node.NodeType == XmlNodeType.XmlDeclaration)
                {
                    doc.RemoveChild(node);
                }
            }
            
            var xRoot = doc.DocumentElement;

            ProjectInfo.IsCoreProject = xRoot.HasAttribute("Sdk");
            
            var targetFrameworkNode = xRoot.SelectSingleNode(
                ProjectInfo.IsCoreProject
                ? "//PropertyGroup/TargetFramework"
                : "//PropertyGroup/TargetFrameworkVersion"
                );
            var targetFramework = targetFrameworkNode?.InnerText ?? "n/a";

            ProjectInfo.TargetFramework = targetFramework;
            
            var packageReferences = new List<XmlNode>();
            packageReferences.AddRange(xRoot.SelectNodes("ItemGroup/PackageReference").Cast<XmlNode>());
            // probably, old MVC projects
            packageReferences.AddRange(xRoot.SelectNodes("*[local-name() = 'ItemGroup']/*[local-name() = 'Reference']").Cast<XmlNode>());
            // probably, Xamarin.* projects
            packageReferences.AddRange(xRoot.SelectNodes("*[local-name() = 'ItemGroup']/*[local-name() = 'PackageReference']").Cast<XmlNode>());
            
            foreach (XmlNode packageNode in packageReferences.Distinct())
            {
                var includeAttribute = packageNode.Attributes["Include"];
                string versionValue;
                
                if (packageNode.HasChildNodes)
                {
                    versionValue = packageNode.SelectSingleNode("*[local-name() = 'Version']")?.InnerText;
                }
                else
                {
                    versionValue = packageNode.Attributes["Version"]?.Value;
                }

                if (includeAttribute != null && !string.IsNullOrEmpty(versionValue))
                {
                    ProjectInfo.Packages.Add(NuGetPackageDetails.CreateFromCSProject(
                        includeAttribute.Value,
                        versionValue,
                        NuGetPackageType.NuGet));
                }
            }

            if (_options.IncludeCLITools && ProjectInfo.IsCoreProject)
            {
                var cliToolsReferencesNodes = xRoot.SelectNodes("ItemGroup/DotNetCliToolReference");
                foreach (XmlNode cliToolNode in cliToolsReferencesNodes)
                {
                    var includeAttribute = cliToolNode.Attributes["Include"];
                    var versionAttribute = cliToolNode.Attributes["Version"];

                    if (includeAttribute != null && versionAttribute != null)
                    {
                        ProjectInfo.Packages.Add(NuGetPackageDetails.CreateFromCSProject(
                            includeAttribute.Value,
                            versionAttribute.Value,
                            NuGetPackageType.CLITool));
                    }
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
                    ProjectInfo.Packages.Add(NuGetPackageDetails.CreateFromPackagesConfig(
                        idAttribute.Value,
                        versionAttribute.Value));
                }
            }
        }
    }
}