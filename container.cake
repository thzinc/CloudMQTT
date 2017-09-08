#tool "nuget:?package=xunit.runner.console"
var configuration = Argument("configuration", "Release");

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
