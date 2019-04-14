using System.IO;
using System.Linq;
using System.Text;
using NuGetHelper.Models;

namespace NuGetHelper
{
    public class ReportBroker
    {
        public static void GenerateLicenseDependencies(SolutionDetails solutionDetails, string filePath)
        {
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                if (!solutionDetails.Projects.Any(q => q.Packages.Any()))
                {
                    writer.WriteLine("No packages found");
                    return;
                }
               
                writer.WriteLine("# LICENSE DEPENDENCIES");
                writer.WriteLine();
                
                foreach (var project in solutionDetails.Projects)
                {
                    writer.WriteLine($"## Project {project.Config.ProjectName}");
                    writer.WriteLine();
                    writer.WriteLine("|Package Name|Type|Version|License url|");
                    writer.WriteLine("|---|---|---|---|");
                    
                    foreach (var package in project.Packages.OrderBy(q => q.Name).ThenBy(q => q.PackageType))
                    {
                        var license = package.Metadata?.LicenseUrl?.ToString();
                        var licenseUrlData = "n/a";

                        if (!string.IsNullOrEmpty(license))
                        {
                            licenseUrlData = $"[{license}]({license})";
                        }
                        
                        
                        writer.WriteLine($"|{package.Name}|{package.PackageType}|{package.Version}|{licenseUrlData}|");
                    }
                    
                    writer.WriteLine();
                }
                
                writer.WriteLine("---");
                writer.WriteLine("Made with ❤️ by [NuGet Helper](https://github.com/RustamIrzaev/NuGetHelper)");
            }
        }
    }
}