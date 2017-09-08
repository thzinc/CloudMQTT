#addin nuget:?package=Cake.Git
#addin "Cake.Docker"
#load "container.cake"
#load "package.cake"

void RunTargetInContainer(string target, string arguments, params string[] includeEnvironmentVariables) {
    var cwd = MakeAbsolute(Directory("./"));
    var env = includeEnvironmentVariables.ToDictionary(key => key, key => EnvironmentVariable(key));

    var missingEnv = env.Where(x => string.IsNullOrEmpty(x.Value)).ToList();
    if (missingEnv.Any()) {
        throw new Exception($"The following environment variables are required to be set: {string.Join(", ", missingEnv.Select(x => x.Key))}");
    }

    var settings = new DockerRunSettings
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

    var command = $"{settings.Workdir}/build.sh -t {target} {arguments}";
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

Task("Publish")
    .IsDependentOn("Build")
    .IsDependentOn("PublishNuget");

Task("Default").IsDependentOn("Build");
RunTarget(Argument("target", "Build"));
