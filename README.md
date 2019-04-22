# NuGet Helper

NuGet Helper is a powerful tool that may generate a file with NuGet licenses from your projects and much more!

Here what you may achive using this tool:

* Generate [LICENSE-DEPENDENCIES.md](assets/LICENSE-DEPENDENCIES.MD) ([example](assets/LICENSE-DEPENDENCIES.MD)) file that contains information about all NuGet packages used in a whole solution (including version, link to a license file and more)
* Supports both: .Net Core and .Net Framework projects
* Show information about each *.csproj file (detect framework version, check if the project is .Net Core or .Net Framework project)
* Check and parse packages.config file

---

## Download

Get the latest binaries [from here](https://github.com/RustamIrzaev/NuGetHelper/releases).

There are two kind of archives per each platform: with and without **__full_** suffix.

Packages with **__full** suffix are [self-contained applications](https://docs.microsoft.com/en-us/dotnet/core/deploying/#self-contained-deployments-scd), that **do not** need dotnet framework to be installed, while packages without that suffix are [framework-dependent](https://docs.microsoft.com/en-us/dotnet/core/deploying/#framework-dependent-deployments-fdd) applications and they do need standalone dotnet framework installed.

## How to use (easy way)

All parameters will be set to their default values as specified in Parameters section.

### Mac\Linux

* Change permissions for executing the script

   ```bash
   chmod +x ./run.sh
   ```

* Execute the script

   ```bash
   # PROJECT_FOLDER is a full folder path to your .Net project
   ./run.sh "<PROJECT_FOLDER>"
   ```

### Windows

* Execute the script

   ```powershell
   .\run.ps1 "<PROJECT_FOLDER>"
   ```

_And do not forget to visit this page to check if a new version is available._

## How to use (advanced way)

Run the tool with parameters you need directly using `dotnet cli`.

For example

```bash
dotnet NuGetHelper.Tool.dll --solution-folder "<PROJECT_FOLDER>" --generate-license
```

## Parameters

|Parameter|Required?|Description|Default value|
|---|---|---|---|
|**--solution-folder** _or_ **--folder**|**yes**|A path to a folder where the projects is located|**-**|
|**-generate-license** _or_ **--license**|_no_|Generates LICENSE-DEPENDENCIES.md file|**true**|
|**--load-metadata**|_no_|Loads package information from [NuGet](http://nuget.org) _(Tags, Summary, Description and more)_|**true**|
|**--ignore-cli-tools**|_no_|Ignores CLITools<br>_(Works only on .Net Core projects)_|false|
|**--ignore-packages-config**|_no_|Ignores packages.config file processing|false|
|**--print-results**|_no_|Writes all information to console|**true**|

## Bulding the tool

If you want to create a build on your own, just follow those simple steps:

* Clone the repository

* Run a build script depending on your OS:

   ### Mac\Linux

   ```bash
   chmod +x ./build.sh
   ./build.sh
   ```

   ### Windows

   ```powershell
   .\build.ps1
   ```

* When the script completes, you will found a ready-to-use build in `build` folder.

## Contributing

Feel free to contribute to this amazing project.

## License

This project is licensed under terms of the MIT license. See the [LICENSE.md](LICENSE.md) file.

* For [System.CommandLine](https://github.com/dotnet/command-line-api), see [https://github.com/dotnet/command-line-api/blob/master/LICENSE.md](https://github.com/dotnet/command-line-api/blob/master/LICENSE.md)

* For [NuGet.Protocol](https://github.com/NuGet/NuGet.Client), see [https://github.com/NuGet/NuGet.Client/blob/dev/LICENSE.txt](https://github.com/NuGet/NuGet.Client/blob/dev/LICENSE.txt)

---
Made with ❤️ by Rustam Irzaev
