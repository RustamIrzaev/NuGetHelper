namespace NuGetHelper.Tool.Models
{
    public class Options
    {
        public string SolutionFolder { get; set; }
        public bool GenerateLicense { get; set; }
        public bool LoadMetadata { get; set; }
        public bool IgnoreCLITools { get; set; }
        public bool IgnorePackagesConfig { get; set; }
        public bool PrintResults { get; set; }
    }
}