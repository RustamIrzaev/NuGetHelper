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
        private readonly ProjectFileParser _projectFileParser;
        
        private string _projectPath;

        public Parser(ParserOptions options)
        {
            _parserOptions = options ?? ParserOptions.Default();
            _projectFileParser = new ProjectFileParser(options);
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
                _projectFileParser.SetCSProjectFile(csProjectFile);
                _projectFileParser.Parse();
                
                if (_parserOptions.LoadNuGetMetadata)
                    await _projectFileParser.ObtainNugetMetadataAsync();
                
                solutionDetails.AddProjectDetails(_projectFileParser.ProjectInfo);
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