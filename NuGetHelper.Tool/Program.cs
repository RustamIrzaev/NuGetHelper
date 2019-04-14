using System;
using System.CommandLine.Invocation;
using System.Reflection;
using NuGetHelper.Tool.Helpers;
using NuGetHelper.Tool.Models;
using static NuGetHelper.Tool.Helpers.ColoredConsole;

namespace NuGetHelper.Tool
{
    public class Program
    {
        private static readonly (string tool, string library) VersionsInfo = GetVersions();
        
        public static int Main(string[] args)
        {
            var rootCommand = CommandLineHelper.BuildCommandLineCommands();
            rootCommand.Handler = CommandHandler.Create<Options>(options => Execute(options));
            
            return rootCommand.InvokeAsync(args).Result;
        }

        private static void Execute(Options options)
        {
            Print($"Welcome to NuGetHelper [tool: v.{VersionsInfo.tool}, " +
                  $"library: v.{VersionsInfo.library}]");
            PrintEmptyLine();

            Print("Processing, please wait...");

            try
            {
                new Parser(OptionsConverter.Convert(options)).RunAsync(options.SolutionFolder).Wait();
            }
            catch (Exception ex)
            {
                PrintError(ex);
                Terminate();
            }
            
            PrintEmptyLine();
            PrintSuccess("Done ❤️");
            PrintEmptyLine();
            PrintGray("Don't forget to visit https://github.com/RustamIrzaev/NuGetHelper");
        }
        
        private static (string, string) GetVersions()
        {
            var getVersion = new Func<Assembly, string>(assembly => assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion);

            var toolVersion = getVersion(Assembly.GetExecutingAssembly());
            var libraryVersion = getVersion(typeof(Parser).Assembly);
            
            return (toolVersion, libraryVersion);
        }

        private static void Terminate()
        {
            Environment.Exit(-1);
        }
    }
}