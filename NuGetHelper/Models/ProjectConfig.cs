namespace NuGetHelper.Models
{
    public class ProjectConfig
    {
        public string CSProjectFilePath { get; set; }
        public string ProjectName { get; set; }
        public string PackagesConfigFilePath { get; set; }
        public bool PackagesConfigExists { get; set; }
    }
}