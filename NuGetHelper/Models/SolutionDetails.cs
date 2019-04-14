using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NuGetHelper.Models
{
    public class SolutionDetails
    {
        private readonly List<ProjectDetails> _projects;
        
        public string SolutionPath { get; private set; }
        public List<string> SolutionFiles { get; private set; }
        public List<string> CSProjectFiles { get; private set; }

        public ReadOnlyCollection<ProjectDetails> Projects => _projects.AsReadOnly();
        
        public SolutionDetails()
        {
            _projects = new List<ProjectDetails>();
            SolutionFiles = new List<string>();
            CSProjectFiles = new List<string>();
        }

        public SolutionDetails(string solutionPath, IEnumerable<string> solutionFiles, 
            IEnumerable<string> csProjectFiles) : this()
        {
            SolutionPath = solutionPath;
            SolutionFiles = solutionFiles.ToList();
            CSProjectFiles = csProjectFiles.ToList();
        }

        public void AddProjectDetails(ProjectDetails projectDetails)
        {
            _projects.Add(projectDetails);
        }
        
        public void Print()
        {
            Console.WriteLine();
            Console.WriteLine($"Origin solution path: {SolutionPath}");
            Console.WriteLine($"Solution files found: {SolutionFiles.Count}");
            Console.WriteLine($"CSProject files found: {CSProjectFiles.Count}");
            Console.WriteLine($"Projects processed: {Projects.Count}");
            Console.WriteLine();
            Console.WriteLine();

            foreach (var project in Projects)
            {
                Console.WriteLine($" Project {project.Config.ProjectName}");
                Console.WriteLine();
                Console.WriteLine($"  Is .Net Core project? {project.IsCoreProject}");
                Console.WriteLine($"  Target framework: {project.TargetFramework}");
                Console.WriteLine($"  Has packages.config file - {project.Config.PackagesConfigExists}");
                Console.WriteLine();

                foreach (var package in project.Packages.OrderBy(q => q.Name).ThenBy(q => q.PackageType))
                {
                    Console.WriteLine($"    NuGet name: {package.Name}");
                    Console.WriteLine($"    NuGet version: {package.Version}");
                    Console.WriteLine($"    NuGet location: {package.Source}");
                    Console.WriteLine($"    NuGet package type: {package.PackageType}");
                    Console.WriteLine($"    Has metadata: {package.Metadata != null}");

                    if (package.Metadata != null)
                    {
                        Console.WriteLine($"      License url: {package.Metadata.LicenseUrl}");
                        Console.WriteLine($"      Project url: {package.Metadata.ProjectUrl}");
                        Console.WriteLine($"      Tags: {package.Metadata.Tags}");
                        Console.WriteLine($"      Title: {package.Metadata.Title}");
                        Console.WriteLine($"      Owners: {package.Metadata.Owners}");
                        Console.WriteLine($"      Authors: {package.Metadata.Authors}");
                        Console.WriteLine($"      Summary: {package.Metadata.Summary}");
                        Console.WriteLine($"      Description: {package.Metadata.Description}");
                    }
                    
                    Console.WriteLine();
                }
                
                Console.WriteLine();
            }
            
            Console.WriteLine();
        }
    }
}