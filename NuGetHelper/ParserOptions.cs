namespace NuGetHelper
{
    public class ParserOptions
    {
        public bool LoadNuGetMetadata { get; set; }
        public bool IncludeCLITools { get; set; }
        public bool AlwaysIncludeResultsFromPackagesConfigFile { get; set; }
        public bool GenerateLicenseDependencies { get; set; }
        public bool PrintResults { get; set; }

        public static ParserOptions Default()
        {
            return new ParserOptions
            {
                LoadNuGetMetadata = true,
                IncludeCLITools = true,
                AlwaysIncludeResultsFromPackagesConfigFile = true,
                GenerateLicenseDependencies = true,
                PrintResults = true
            };
        }
    }
}