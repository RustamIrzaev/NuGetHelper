using NuGetHelper.Tool.Models;

namespace NuGetHelper.Tool.Helpers
{
    public static class OptionsConverter
    {
        public static ParserOptions Convert(Options options)
        {
            return new ParserOptions
            {
                GenerateLicenseDependencies = options.GenerateLicense,
                LoadNuGetMetadata = options.LoadMetadata,
                AlwaysIncludeResultsFromPackagesConfigFile = !options.IgnorePackagesConfig,
                IncludeCLITools = !options.IgnoreCLITools,
                PrintResults = options.PrintResults
            };
        }
    }
}