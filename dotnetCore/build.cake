var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");


Task("Restore")
    .Does(() =>
{
    DotNetCoreRestore();
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings
     {
         Configuration = configuration,
         //OutputDirectory = "./artifacts/"
     };

    DotNetCoreBuild("Solution.sln",settings);
});

RunTarget(target);