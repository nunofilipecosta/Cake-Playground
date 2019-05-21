var target = Argument("Target", "Build");
var configuration = Argument("Configuration", "Release");

Task("Restore")
	.Does(() =>
{
	NuGetRestore("Solution.sln");
});

Task("Build")
	.IsDependentOn("Restore")
    .Does(() =>
{
    MSBuild("Solution.sln", settings => settings
		.SetConfiguration(configuration)
		.SetVerbosity(Verbosity.Minimal)
        .UseToolVersion(MSBuildToolVersion.VS2019));
});

RunTarget(target);


