using System;
using System.IO;
using System.Threading.Tasks;
using NuGetHelper.Helpers;
using NuGetHelper.Models;

namespace NuGetHelper
{
    public class Parser
    {
        private string _projectPath;

        public async Task RunAsync(string projectPath, ParserOptions options = null)
        {
            ValidationHelper.ThrowIfStringIsNullOrEmpty(projectPath);
            
            if (options == null)
                options = ParserOptions.Default();
            
            var projectFileParser = new ProjectFileParser(options);
            
            _projectPath = projectPath;
            
            var solutionDetails = GetSolutionDetails();

            if (solutionDetails == null)
            {
                await Task.FromResult(new Exception("Can't find project files in the specified directory."));
                return;
            }

            foreach (var csProjectFile in solutionDetails.CSProjectFiles)
            {
                projectFileParser.SetCSProjectFile(csProjectFile);
                projectFileParser.Parse();
                
                if (options.LoadNuGetMetadata)
                    await projectFileParser.ObtainNugetMetadataAsync();
                
                solutionDetails.AddProjectDetails(projectFileParser.ProjectInfo);
            }
            
            if (options.PrintResults)
                solutionDetails.Print();
            
            if (options.GenerateLicenseDependencies)
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