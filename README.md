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

And We can specialize the version file path.

```csharp
dontet TagToVersion -t 1.0.2 -f build/Version.props
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

## GitHub Action

Pushing a tag to GitHub and then publish the nuget package with this tag version.

The first step:

```yaml
on:
  push:
    tags:
    - '*' 
```

The second step:

```yaml
    - name: Install dotnet tool
      run: dotnet tool install -g dotnetCampus.TagToVersion

    - name: Set tag to version  
      run: dotnet TagToVersion -t ${{ github.ref }}
```

The thrid step:

```yaml
# Build and publish

    - name: Build with dotnet
      run: dotnet build --configuration Release

    - name: Install Nuget
      uses: nuget/setup-nuget@v1
      with:        
        nuget-version: '5.x'

    - name: Add private GitHub registry to NuGet
      run: |
        nuget sources add -name github -Source https://nuget.pkg.github.com/ORGANIZATION_NAME/index.json -Username ORGANIZATION_NAME -Password ${{ secrets.GITHUB_TOKEN }}
    - name: Push generated package to GitHub registry
      run: |
        nuget push .\bin\release\*.nupkg -Source github -SkipDuplicate
        nuget push .\bin\release\*.nupkg -Source https://api.nuget.org/v3/index.json -SkipDuplicate -ApiKey ${{ secrets.NugetKey }} -NoSymbols 
```

For more detailed code, please find in this repo.