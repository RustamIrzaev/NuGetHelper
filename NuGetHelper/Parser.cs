using System;
using System.IO;
using System.Threading.Tasks;
using NuGetHelper.Helpers;
using NuGetHelper.Models;

namespace NuGetHelper
{
    public class Parser
    {
        private readonly ParserOptions _parserOptions;
        
        private string _projectPath;

        public Parser(ParserOptions options)
        {
            _parserOptions = options ?? ParserOptions.Default();
        }
        
        public async Task RunAsync(string projectPath)
        {
            ValidationHelper.ThrowIfStringIsNullOrEmpty(projectPath);
            
            _projectPath = projectPath;
            
            var solutionDetails = GetSolutionDetails();

            if (solutionDetails == null)
                throw new Exception("Can't find project files in the specified directory.");

            foreach (var csProjectFile in solutionDetails.CSProjectFiles)
            {
                var projectFileParser = new ProjectFileParser(_parserOptions);
                
                projectFileParser.SetCSProjectFile(csProjectFile);
                projectFileParser.Parse();
                
                if (_parserOptions.LoadNuGetMetadata)
                    await projectFileParser.ObtainNugetMetadataAsync();
                
                solutionDetails.AddProjectDetails(projectFileParser.ProjectInfo);
            }
            
            if (_parserOptions.PrintResults)
                solutionDetails.Print();
            
            if (_parserOptions.GenerateLicenseDependencies)
                ReportBroker.GenerateLicenseDependencies(solutionDetails, "LICENSE-DEPENDENCIES.MD");
        }

        private SolutionDetails GetSolutionDetails()
        {
            var rootProjectFolderExists = Directory.Exists(_projectPath);

            if (!rootProjectFolderExists)
                return null;
            
            var solutionFiles = Directory.GetFiles(_projectPath, "*.sln");

            if (solutionFiles.Length == 0)
                return null;

            var csProjectFiles = Directory.GetFileSystemEntries(_projectPath,
                "*.csproj",
                SearchOption.AllDirectories);

            if (csProjectFiles.Length == 0)
                return null;
            
            return new SolutionDetails(_projectPath, solutionFiles, csProjectFiles);
        }
    }
}