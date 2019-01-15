## Service Messages .NET library for [<img src="https://cdn.worldvectorlogo.com/logos/teamcity.svg" height="20" align="center"/>](https://www.jetbrains.com/teamcity/)

[<img src="http://jb.gg/badges/official.svg" height="20"/>](https://confluence.jetbrains.com/display/ALL/JetBrains+on+GitHub) [![NuGet TeamCity.Dotnet.Integration](https://buildstats.info/nuget/TeamCity.ServiceMessages?includePreReleases=false)](https://www.nuget.org/packages/TeamCity.ServiceMessages) [![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0) [<img src="http://teamcity.jetbrains.com/app/rest/builds/buildType:(id:TeamCityServiceMessages_TeamCityServiceMessages)/statusIcon"/>](http://teamcity.jetbrains.com/viewType.html?buildTypeId=TeamCityServiceMessages_TeamCityServiceMessages)


This library provides read/write access to TeamCity Service messages.
Take a look at the description of service messages at this [page](
http://confluence.jetbrains.net/display/TCDL/Build+Script+Interaction+with+TeamCity#BuildScriptInteractionwithTeamCity-ServiceMessages).

Usage:
======

Most use cases are covered in tests.

To create service message, use: 
```csharp
JetBrains.TeamCity.ServiceMessages.Write.ServiceMessageFormatter.FormatMessage
```	
To parse service messages, use: 
```csharp
JetBrains.TeamCity.ServiceMessages.Read.ServiceMessageParser.ParseServiceMessages
```
There is an API to generate TeamCity specific service messages, use: 
```csharp
JetBrains.TeamCity.ServiceMessages.Write.Special.ITeamCityWriter
```	
to get the instance of the object create an instance of the factory and get it by:
```csharp
new JetBrains.TeamCity.ServiceMessages.Write.Special.TeamCityServiceMessages().CreateWriter()
```

for [example](https://dotnetfiddle.net/4SoKKt):
```csharp
// Creating the root writer
using (var writer = new TeamCityServiceMessages().CreateWriter(Console.WriteLine))
// Creating the build log block "Tests"
using (var block = writer.OpenBlock("Tests"))
// Creating the test suite "Tests"
using (var testClass = block.OpenTestSuite("TestClass"))
{
    // Creating the successful test
    using (var test = testClass.OpenTest("Successful"))
    {
        test.WriteStdOutput("Some output");
        test.WriteDuration(TimeSpan.FromSeconds(1));
    }

    // Creating the ignored test
    using (var test = testClass.OpenTest("Ignored"))
    {
        test.WriteIgnored();
    }

    // Creating the failed test
    using (var test = testClass.OpenTest("Failed"))
    {
        test.WriteFailed("Some message", "Details");
    }

    // Attaching an image to test
    using (var test = testClass.OpenTest("Image"))
    {
        writer.PublishArtifact(Path.GetFullPath("TeamCity.png") + " => TestData");
        test.WriteImage("TestData/TeamCity.png", "Team City Logo");
    }

    // Attaching a value to test
    using (var test = testClass.OpenTest("Value"))
    {
        test.WriteValue(1234.56.ToString(CultureInfo.InvariantCulture), "Some Value");
    }

    // Attaching a file to test
    using (var test = testClass.OpenTest("File"))
    {
        writer.PublishArtifact(Path.GetFullPath("TeamCity.png") + " => TestData");
        test.WriteFile("TestData/TeamCity.png", "Team City Logo file");
    }

    // Attaching a link to test
    using (var test = testClass.OpenTest("Link"))
    {
        test.WriteLink("https://www.jetbrains.com/", "JetBrains");
    }
}
```
