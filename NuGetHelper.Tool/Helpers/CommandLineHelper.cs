using System.CommandLine;

namespace NuGetHelper.Tool.Helpers
{
    public static class CommandLineHelper
    {
        public static RootCommand BuildCommandLineCommands()
        {
            var optionSolutionFolder = new Option(
                new []{"--solution-folder", "--folder"},
                "Specifies a folder where a project is located",
                new Argument<string>()
                );
            
            var optionGenerateLicense = new Option(
                new []{"--generate-license", "--license"},
                "Generates LICENSE-DEPENDENCIES.md file",
                new Argument<bool>(true)
            );
            
            var optionLoadMetadata = new Option(
                new []{"--load-metadata"},
                "Load NuGet information for each package",
                new Argument<bool>(true)
            );
            
            var optionIgnoreCLITools = new Option(
                new []{"--ignore-cli-tools"},
                "Ignores CLITools packages",
                new Argument<bool>(false)
            );
            
            var optionIgnorePackagesConfig = new Option(
                new []{"--ignore-packages-config"},
                "Ignores processing packages.config file",
                new Argument<bool>(false)
            );
            
            var optionPrintResults = new Option(
                new []{"--print-results"},
                "Writes all information to console",
                new Argument<bool>(false)
            );
            
            var rootCommand = new RootCommand();
            rootCommand.AddOption(optionSolutionFolder);
            rootCommand.AddOption(optionGenerateLicense);
            rootCommand.AddOption(optionLoadMetadata);
            rootCommand.AddOption(optionIgnoreCLITools);
            rootCommand.AddOption(optionIgnorePackagesConfig);
            rootCommand.AddOption(optionPrintResults);
            
            rootCommand.Argument.AddValidator(result =>
            {
                if (result.Children["--solution-folder"] is null)
                {
                    return "Option SolutionFolder is required";
                }

                return null;
            });

            return rootCommand;
        }
    }
}