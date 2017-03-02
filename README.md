[<img src="http://teamcity.jetbrains.com/app/rest/builds/buildType:(id:TeamCityServiceMessages_TeamCityServiceMessages)/statusIcon"/>](http://tcavs2015.cloudapp.net/viewType.html?buildTypeId=DevTeam_IoC_Build) [<img src="https://www.nuget.org/Content/Logos/nugetlogo.png" height="18">](https://www.nuget.org/packages/TeamCity.ServiceMessages/)

TeamCity.ServiceMessages .NET library. 
======================================

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
License:
========
[Apache 2.0.](https://github.com/JetBrains/TeamCity.ServiceMessages/blob/master/LICENSE.txt)
