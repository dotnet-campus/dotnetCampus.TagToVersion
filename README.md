# TagToVersion

The dotnetCampus.TagToVersion that can helps us write tag names into version files is a dotnet tool.

This tool can be used in CI to help us push tag and build the special version of nuget file.

| Build | NuGet |
|--|--|
|![](https://github.com/dotnet-campus/dotnetCampus.TagToVersion/workflows/.NET%20Core/badge.svg)|[![](https://img.shields.io/nuget/v/dotnetCampus.TagToVersion.svg)](https://www.nuget.org/packages/dotnetCampus.TagToVersion)|

## Install 

```csharp
dotnet install -g dotnetCampus.TagToVersion
```

## Usage

The tool can automatically find the Version.props file and write the tag name to version file.

```csharp
dotnet TagToVersion -t 1.0.2
```

