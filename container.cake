#addin nuget:?package=Cake.Git
#tool "nuget:?package=xunit.runner.console"
using System.Text.RegularExpressions;

var configuration = Argument("configuration", "Release");
var outputDirectory = Directory("packages");

Task("Clean")
    .Does(() =>
    {
        CleanDirectories(GetDirectories("./src/**/bin")
            .Concat(GetDirectories("./src/**/obj")));
    });

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetCoreRestore();
    });

Task("Compile")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        var buildSettings = new ProcessSettings
        {
            Arguments = $"/property:Configuration={configuration}",
        };

        using(var process = StartAndReturnProcess("msbuild", buildSettings))
        {
            process.WaitForExit();
            var exitCode = process.GetExitCode();
            if(exitCode != 0)
                throw new Exception("Build Failed.");
        }
    });

Task("ResolvePermissions")
    .IsDependentOn("Compile")
    .Does(() => {
        var chmodSettings = new ProcessSettings
        {
            Arguments = $"-c 'chmod -R 777 ./src'",
        };

        using(var process = StartAndReturnProcess("bash", chmodSettings))
        {
            process.WaitForExit();
            var exitCode = process.GetExitCode();
            if(exitCode != 0)
                throw new Exception("Resolve permissions failed.");
        }
    });

Task("Test")
    .IsDependentOn("ResolvePermissions")
    .Does(() => 
    {
        Warning("Tests disabled because Mono's System.Net.Http isn't fully baked");
        // XUnit2(GetFiles($"./src/**/bin/{configuration}/**/*.Tests.dll"), new XUnit2Settings
        // {
        //     Parallelism = ParallelismOption.Assemblies,
        // });
    });

Task("ContainerBuild")
    .IsDependentOn("Test");

Task("CleanPackages")
    .Does(() => {
        CleanDirectory(outputDirectory);
    });

Task("PackageNuget")
    .IsDependentOn("CleanPackages")
    .IsDependentOn("AssignVersion")
    .IsDependentOn("ContainerBuild")
    .Does(() =>
    {
        var assemblyInfo = ParseAssemblyInfo(File("./src/CloudMQTT.Client/AssemblyInfo.cs"));
        var versionInfo = assemblyInfo.AssemblyInformationalVersion;

        var packageSettings = new DotNetCorePackSettings
        {
            Configuration =  configuration,
            OutputDirectory = outputDirectory,
            ArgumentCustomization = args => args.Append($"/p:Version={versionInfo}")
        };

        DotNetCorePack(File("./src/CloudMQTT.Client/CloudMQTT.Client.csproj"), packageSettings);
    });
