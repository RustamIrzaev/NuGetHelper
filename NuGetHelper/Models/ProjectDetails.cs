using System.Collections.Generic;

namespace NuGetHelper.Models
{
    public class ProjectDetails
    {
        public bool IsCoreProject { get; set; }
        public string TargetFramework { get; set; }
        public ProjectConfig Config { get; set; }
        public List<NuGetPackageDetails> Packages { get; set; }

        public ProjectDetails()
        {
            Config = new ProjectConfig();
            Packages = new List<NuGetPackageDetails>();
        }
    }
}