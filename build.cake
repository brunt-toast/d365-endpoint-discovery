using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

var target = Argument("target", "RunGui");
var configuration = Argument("configuration", "Release");

Task("InstallSdk").Does(() =>
{
    if (IsRunningOnWindows())
    {
        StartProcess("pwsh", "-Command \"Invoke-WebRequest -Uri https://dot.net/v1/dotnet-install.ps1 -OutFile ./dotnet-install.ps1\"");
        StartProcess("pwsh", "-ExecutionPolicy Bypass -File ./dotnet-install.ps1 --jsonfile ./global.json");
        StartProcess("pwsh", "-Command \"Remove-Item -Path ./dotnet-install.ps1\"");
    }
    else
    {
        StartProcess("sh", "-c \"curl -sSL 'https://dot.net/v1/dotnet-install.sh' > ./dotnet-install.sh.tmp\"");

        if (Context.Tools.Resolve("gpg") != null)
        {
            StartProcess("sh", "-c \"curl -sSL 'https://dot.net/v1/dotnet-install.asc' > ./dotnet-install.asc.tmp\"");
            StartProcess("sh", "-c \"curl -sSL 'https://dot.net/v1/dotnet-install.sig' > ./dotnet-install.sig.tmp\"");
            StartProcess("sh", "-c \"gpg --import ./dotnet-install.asc.tmp\"");
            int gpgExitCode = StartProcess("sh", "-c \"gpg --verify ./dotnet-install.sig.tmp ./dotnet-install.sh.tmp\"");
            if (gpgExitCode != 0)
            {
                throw new CakeException("The dotnet install script failed the GPG integrity check.");
            }

            StartProcess("sh", "-c \"rm ./dotnet-install.asc.tmp\"");
            StartProcess("sh", "-c \"rm ./dotnet-install.sig.tmp\"");
        }

        StartProcess("sh", "-c \"chmod +x ./dotnet-install.sh.tmp\"");
        StartProcess("sh", "-c \"./dotnet-install.sh.tmp --jsonfile ./global.json\"");

        StartProcess("sh", "-c \"rm ./dotnet-install.sh.tmp\"");
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

Task("RunGui")
    .IsDependentOn("InstallSdk")
    .IsDependentOn("Restore")
    .IsDependentOn("RestoreWorkloads")
    .Does(() =>
    {
        DotNetRun("./src/BlazorHybrid/BlazorHybrid.csproj", new DotNetRunSettings
        {
            Configuration = configuration,
            Framework = "net10.0-windows10.0.19041.0"
        });
    });

Task("Test")
    .IsDependentOn("InstallSdk")
    .IsDependentOn("Restore")
    .IsDependentOn("RestoreWorkloads")
    .Does(() =>
    {
        var projects = GetFiles("test/**/*.csproj");

        foreach (var proj in projects)
        {
            DotNetTest(proj.FullPath);
        }
    });

RunTarget(target);
