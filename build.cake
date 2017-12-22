#addin nuget:?package=Cake.Git
#addin "Cake.Docker"
#load "container.cake"

void RunTargetInContainer(string target, string arguments, params string[] includeEnvironmentVariables) {
    var cwd = MakeAbsolute(Directory("./"));
    var env = includeEnvironmentVariables
        .Select(key => new { Key = key, Value = EnvironmentVariable(key) })
        .ToArray();

    var missingEnv = env.Where(x => string.IsNullOrEmpty(x.Value)).ToList();
    if (missingEnv.Any()) {
        throw new Exception($"The following environment variables are required to be set: {string.Join(", ", missingEnv.Select(x => x.Key))}");
    }

    var settings = new DockerContainerRunSettings
    {
        Volume = new string[] { $"{cwd}:/artifacts"},
        Workdir = "/artifacts",
        Rm = true,
        Env = env
            .OrderBy(x => x.Key)
            .Select((x) => $"{x.Key}=\"{x.Value}\"")
            .ToArray(),
    };

    Information(string.Join(Environment.NewLine, settings.Env));

    var command = $"cake -t {target} {arguments}";
    Information(command);
    var buildBoxImage = "syncromatics/build-box";
    DockerPull(buildBoxImage);
    DockerRun(settings, buildBoxImage, command);
}

var version = "";
var semVersion = "";
Task("GetVersion")
    .Does(() =>
    {
        var repositoryPath = Directory(".");
        var branch = GitBranchCurrent(repositoryPath).FriendlyName;
        var prereleaseTag = Regex.Replace(branch, @"\W+", "-");
        var describe = GitDescribe(repositoryPath, GitDescribeStrategy.Tags);

        var isMaster = prereleaseTag == "master" || prereleaseTag == "-no-branch-";
        version = string.Join(".", describe.Split(new[] { '-' }, 3).Take(2));
        semVersion = version + (isMaster ? "" : $"-{prereleaseTag}");
    });

Task("AssignVersion")
    .IsDependentOn("GetVersion")
    .Does(() => {
        CreateAssemblyInfo(File("./src/CloudMQTT.Client/AssemblyInfo.cs"), new AssemblyInfoSettings {
            Product = "CloudMQTT.Client",
            Version = version,
            FileVersion = version,
            InformationalVersion = semVersion,
            Copyright = "Copyright 2017",
        });

    });


Task("Build")
    .IsDependentOn("AssignVersion")
    .Does(() => RunTargetInContainer("ContainerBuild", "--verbosity Diagnostic"));

Task("Package")
    .Does(() => RunTargetInContainer("PackageNuget", "--verbosity Diagnostic"));

Task("Publish")
    .IsDependentOn("Package")
    .Does(() =>
    {
        var packageDir = Directory("packages");
        var package = GetFiles($"{packageDir}/*.nupkg").Single();

        NuGetPush(package, new NuGetPushSettings
        {
            Source = "https://www.nuget.org/api/v2/package",
            ApiKey = EnvironmentVariable("NUGET_API_KEY"),
        });
    });

Task("Default").IsDependentOn("Build");
RunTarget(Argument("target", "Build"));
