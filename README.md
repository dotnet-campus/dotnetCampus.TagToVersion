# TagToVersion

The dotnetCampus.TagToVersion that can helps us write tag names into version files is a dotnet tool.

This tool can be used in CI to help us push tag and build the special version of nuget file.

| Build | NuGet |
|--|--|
|![](https://github.com/dotnet-campus/dotnetCampus.TagToVersion/workflows/.NET%20Core/badge.svg)|[![](https://img.shields.io/nuget/v/dotnetCampus.TagToVersion.svg)](https://www.nuget.org/packages/dotnetCampus.TagToVersion)|

## Install 

```csharp
dotnet tool install -g dotnetCampus.TagToVersion
```

## Usage

The tool can automatically find the Version.props file and write the tag name to version file.

```csharp
dotnet TagToVersion -t 1.0.2
```

## Principle

The tool will read the command line args and get the tag name. The tool then parses the tag name to version and writes it to the Version.props file. If you send a version file path to the tool, the tool will use your path. And if you send nothing, the tool will find the Version.props file. The default version file path is `build\Version.props` file.

The tool will write this code to the version file.

```xml
<Project>
  <PropertyGroup>
    <Version>1.0.3</Version>
  </PropertyGroup>
</Project>
```

The version in the code will be replaced with the version that parsed in previous step.