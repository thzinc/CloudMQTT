#addin nuget:?package=Cake.Git
using System.Text.RegularExpressions;

var outputDirectory = Directory("packages");

Task("CleanPackages")
    .Does(() => {
        CleanDirectory(outputDirectory);
    });

Task("PackageNuget")
    .IsDependentOn("CleanPackages")
    .IsDependentOn("GetVersion")
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

Task("PublishNuget")
    .IsDependentOn("PackageNuget")
    .Does(() =>
    {
        var package = GetFiles($"{outputDirectory}/*.nupkg").Single();

        NuGetPush(package, new NuGetPushSettings
        {
            Source = "https://www.nuget.org/api/v2/package",
            ApiKey = EnvironmentVariable("NUGET_API_KEY"),
        });
    });
