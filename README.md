# TagToVersion

The dotnetCampus.TagToVersion that can helps us write tag names into version files is a dotnet tool.

This tool can be used in CI to help us push tag and build the special version of nuget file.

## Install 

```csharp
dotnet install -g dotnetCampus.TagToVersion
```

## Usage

The tool can automatically find the Version.props file and write the tag name to version file.

```csharp
dotnet TagToVersion -t 1.0.2
```

