using System.CommandLine;

namespace NuGetHelper.Tool.Helpers
{
    public static class CommandLineHelper
    {
        public static RootCommand BuildCommandLineCommands()
        {
            var optionSolutionFolder = new Option<string>(
                new []{"--solution-folder", "--folder"},
                "Specifies a folder where a project is located"
            );

            var optionGenerateLicense = new Option<bool>(
                new[] {"--generate-license", "--license"},
                getDefaultValue: () => true,
                "Generates LICENSE-DEPENDENCIES.md file"
            );

            var optionLoadMetadata = new Option<bool>(
                new []{"--load-metadata"},
                getDefaultValue: () => true,
                "Load NuGet information for each package"
            );

            var optionIgnoreCLITools = new Option<bool>(
                new []{"--ignore-cli-tools"},
                getDefaultValue: () => false,
                "Ignores CLITools packages"
            );

            var optionIgnorePackagesConfig = new Option<bool>(
                new []{"--ignore-packages-config"},
                getDefaultValue: () => false,
                "Ignores processing packages.config file"
            );

            var optionPrintResults = new Option<bool>(
                new []{"--print-results"},
                getDefaultValue: () => false,
                "Writes all information to console"
            );

            var optionShortOutput = new Option<bool>(
                new []{"--short-output", "--short"},
                getDefaultValue: () => false,
                "Shorten the information printed to the console"
            );

            var rootCommand = new RootCommand();
            
            rootCommand.AddOption(optionSolutionFolder);
            rootCommand.AddOption(optionGenerateLicense);
            rootCommand.AddOption(optionLoadMetadata);
            rootCommand.AddOption(optionIgnoreCLITools);
            rootCommand.AddOption(optionIgnorePackagesConfig);
            rootCommand.AddOption(optionPrintResults);
            rootCommand.AddOption(optionShortOutput);
            
            rootCommand.AddValidator(result =>
            {
                if (result.Children["--solution-folder"] is null)
                {
                    return "Option SolutionFolder is required";
                }

                return null;
            });
            rootCommand.TreatUnmatchedTokensAsErrors = true;

            return rootCommand;
        }
    }
}