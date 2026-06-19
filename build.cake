#tool "nuget:?package=ReportGenerator&version=5.5.10"

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

using Cake.Common.Tools.ReportGenerator;

var target = Argument("target", "RunGui");
var configuration = Argument("configuration", "Release");

Task("InstallSdk").Does(() =>
{
    IEnumerable<FilePath> sdkFiles = GetFiles("./sdk/*.json").Distinct();

    foreach (FilePath sdkFile in sdkFiles)
    {
        if (IsRunningOnWindows())
        {
            StartProcess("pwsh", $"-ExecutionPolicy Bypass -File ./script/dotnet-install.ps1 --jsonfile {sdkFile}");
        }
        else
        {
            StartProcess("bash", $"./script/dotnet-install.sh --jsonfile {sdkFile}");
        }
    }
});

Task("Restore")
    .Does(() =>
    {
        DotNetRestore(".");
    });

Task("RestoreWorkloads")
    .Does(() =>
    {
        DotNetWorkloadRestore("./DynamicsEndpointDiscovery.slnx");
    });

Task("PackCli")
    .IsDependentOn("InstallSdk")
    .IsDependentOn("Restore")
    .IsDependentOn("RestoreWorkloads")
    .Does(() =>
    {
        DotNetPack("./src/Cli/Cli.csproj");
    });

Task("InstallCli")
    .IsDependentOn("PackCli")
    .Does(() =>
    {
        StartProcess("dotnet", "tool install -g --add-source ./src/Cli/bin/nupkg dynsvcdiscovery --allow-downgrade");
    });

Task("RunCli")
    .IsDependentOn("InstallSdk")
    .IsDependentOn("Restore")
    .IsDependentOn("RestoreWorkloads")
    .Does(() =>
    {
        DotNetRun("./src/Cli/Cli.csproj", new DotNetRunSettings
        {
            Configuration = configuration,
        });
    });

Task("PublishGui")
    .IsDependentOn("InstallSdk")
    .IsDependentOn("Restore")
    .IsDependentOn("RestoreWorkloads")
    .Does(() =>
    {
        StartProcess("pwsh.exe", "./Build-SignedMsix.ps1");
    });

Task("PackGui")
    .IsDependentOn("PublishGui")
    .Does(() =>
    {
        StartProcess("zip", "-r ./src/BlazorHybrid/bin/BlazorHybridMsix.zip ./src/BlazorHybrid/bin/Release/net10.0-windows10.0.19041.0/win-x64/AppPackages");
    });

Task("InstallGui")
    .IsDependentOn("PublishGui")
    .Does(() =>
    {
        const string root = "./src/BlazorHybrid/bin/Release/net10.0-windows10.0.19041.0/win-x64/AppPackages";
        var dir = GetDirectories($"{root}/*")
            .Select(d => d.GetDirectoryName())
            .OrderBy(d => d)
            .Last();

        StartProcess("pwsh.exe", $"{root}/{dir}/Install.ps1");
    });

Task("FastRunGui")
    .Does(() =>
    {
        DotNetRun("./src/BlazorHybrid/BlazorHybrid.csproj", new DotNetRunSettings
        {
            Configuration = configuration,
            Framework = "net10.0-windows10.0.19041.0"
        });
    });

Task("RunGui")
    .IsDependentOn("InstallSdk")
    .IsDependentOn("Restore")
    .IsDependentOn("RestoreWorkloads")
    .IsDependentOn("FastRunGui")
    .Does(() => { });

Task("Test")
    .IsDependentOn("InstallSdk")
    .IsDependentOn("Restore")
    .IsDependentOn("RestoreWorkloads")
    .Does(() =>
    {
        var projects = GetFiles("test/**/*.csproj");

        foreach (var proj in projects)
        {
            DotNetTest(proj.FullPath, new DotNetTestSettings
            {
                ArgumentCustomization = args => args.Append("--collect:\"XPlat Code Coverage\""),
            });
        }
    });

Task("GenerateCoverage")
    .IsDependentOn("Test")
    .Does(() =>
    {
        ReportGenerator(new GlobPattern("**/coverage.cobertura.xml"), Directory("./coveragereport"), new ReportGeneratorSettings
        {
            ReportTypes = [ReportGeneratorReportType.Html],
        });
    });

Task("ShowCoverage")
    .IsDependentOn("GenerateCoverage")
    .Does(() =>
    {
        var coverageReport = MakeAbsolute(File("./coveragereport/index.html")).FullPath;

        if (IsRunningOnWindows())
        {
            StartProcess("cmd", $"/c start \"\" \"{coverageReport}\"");
        }
        else if (IsRunningOnMacOs())
        {
            StartProcess("open", coverageReport);
        }
        else
        {
            StartProcess("xdg-open", coverageReport);
        }
    });

RunTarget(target);
