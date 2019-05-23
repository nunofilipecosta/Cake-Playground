#tool "nuget:?package=GitVersion.CommandLine&version=4.0.0"


var target = Argument("target", "CI-Local");
var configuration = Argument("configuration", "Release");



var version = "1.0.0";
var packageVersion = "0.1.0";
var webProjectPath = "./src/web/web.csproj";


Task("Clean")
    .Does(() =>
{
    CleanDirectory("./artifacts");
});

Task("Restore")
    .Does(() =>
{
    DotNetCoreRestore("./Solution.sln");
});

Task("Build")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings
     {
         Configuration = configuration,
     };

    DotNetCoreBuild("Solution.sln",settings);
});

// Does not work because it's not msbuild
Task("Package-WebDeploy")
    .Does(() =>
{
    EnsureDirectoryExists("./artifacts");
    var packagePath = $"./artifacts/Linker{version}.zip";
    MSBuild("./src/web/web.csproj", settings => settings.SetConfiguration(configuration).WithTarget("Package").WithProperty("PackageLocation", packagePath) );
});


// does not seem to work
Task("Package-Nuget")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCorePack(webProjectPath, new DotNetCorePackSettings {
        Configuration = configuration,
        OutputDirectory = "./artifacts/"
    });
});


Task("Package-Zip")
    .Does(() =>
{
    DotNetCorePublish(webProjectPath, new DotNetCorePublishSettings {
        Configuration = configuration,
        OutputDirectory = "./artifacts/web"
    });

    Zip("./artifacts/web", $"./artifacts/web.{version}.zip");
});

Task("Version")
    .Does(() =>
{
    var version = GitVersion();
    Information($"Calculated semVer {version.SemVer} : {version.BranchName}");
    // dumn comment /// /// // /// //
    //GitVersion(new GitVErs)
});

Task("CI-Local")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build");

RunTarget(target);