using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;
using NuGetHelper.Tool.Models;

namespace NuGetHelper.Tool
{
    public class Program
    {
        private static readonly (string tool, string library) VersionsInfo = GetVersions();
        
        public static int Main(string[] args)
        {
            var rootCommand = BuildCommands();
            rootCommand.Handler = CommandHandler.Create<Options>(options => Execute(options));
            
            return rootCommand.InvokeAsync(args).Result;
        }

        private static void Execute(Options options)
        {
            Console.WriteLine($"Welcome to NuGetHelper [tool: {VersionsInfo.tool}, library: {VersionsInfo.library}]");
            Console.WriteLine();

            Console.WriteLine("Processing, please wait...");
            
            new Parser().RunAsync(options.SolutionFolder, new ParserOptions
            {
                GenerateLicenseDependencies = options.GenerateLicense,
                LoadNuGetMetadata = options.LoadMetadata,
                AlwaysIncludeResultsFromPackagesConfigFile = !options.IgnorePackagesConfig,
                IncludeCLITools = !options.IgnoreCLITools,
                PrintResults = options.PrintResults
            }).Wait();
            
            Console.WriteLine("Done ❤️");
            Console.WriteLine();
            Console.WriteLine("Don't forget to visit https://github.com/RustamIrzaev/NuGetHelper");
        }
        
        private static RootCommand BuildCommands()
        {
            var optionSolutionFolder = new Option(
                new []{"--solution-folder", "--folder"},
                "Specifies a folder where a project is located",
                new Argument<string>()
                );
            
            var optionGenerateLicense = new Option(
                new []{"--generate-license", "--license"},
                "Generates LICENSE-DEPENDENCIES.md file",
                new Argument<bool>(false)
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
                new Argument<bool>(true)
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
        
        private static (string, string) GetVersions()
        {
            var toolVersion = Assembly.GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;
            
            var libraryVersion = typeof(Parser).Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;
            
            return (toolVersion, libraryVersion);
        }
    }
}