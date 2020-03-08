# TagToVersion

The dotnetCampus.TagToVersion that can help we to write the tag name to version file is a dotnet tool.

This tool can use to CI to help we push tag and build the special version nuget file.

## Install 

```csharp
dotnet install -g dotnetCampus.TagToVersion
```

## Usage

The tool can auto find the Version.props file and write the tag name to version file.

```csharp
dotnet TagToVersion -t 1.0.2
```

