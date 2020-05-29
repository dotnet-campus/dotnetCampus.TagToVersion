using System;
using System.Diagnostics;
using System.IO;
using CommandLine;
using NuGet.Versioning;

namespace dotnetCampus.TagToVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Parser.Default.ParseArguments<Program>(args)
                    .WithParsed(option =>
                    {
                        option.ParseTagVersion();
                    })
                    .WithNotParsed(errors =>
                    {
                        Console.WriteLine("dotnetCampus.TagToVersion");
                        Console.WriteLine($"Current directory {Environment.CurrentDirectory}");
                        Console.WriteLine($"Args {Environment.CommandLine}");
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Environment.Exit(-1);
            }
        }

        private void ParseTagVersion()
        {
            Log("dotnetCampus.TagToVersion");
            Log($"Current directory {Environment.CurrentDirectory}");

            Log($"Read tag version. Tag name is {Tag}");
            var version = ReadTagVersion(Tag);
            Log($"Tag version is {version}");

            if (ToConsole)
            {
                Console.WriteLine(version);
            }
            else
            {
                Log($"Find the version.props file. ");
                var versionFile = FindVersionFile(VersionFile);

                WriteVersionToFile(version, versionFile);
            }
        }

        private void Log(string message)
        {
            if (ToConsole)
            {
                Debug.WriteLine(message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }

        private static void WriteVersionToFile(SemanticVersion version, FileInfo versionFile)
        {
            Console.WriteLine($"Start writing version {version} to file {versionFile.FullName}");

            var text = $@"<Project>
  <PropertyGroup>
    <Version>{version}</Version>
  </PropertyGroup>
</Project>";

            File.WriteAllText(versionFile.FullName, text);

            Console.WriteLine("Finish write");
        }

        private static FileInfo FindVersionFile(string versionFile)
        {
            if (string.IsNullOrEmpty(versionFile))
            {
                Console.WriteLine($"Auto finding version file");
                var buildFolder = Path.GetFullPath("build");
                if (!Directory.Exists(buildFolder))
                {
                    buildFolder = Path.GetFullPath("Build");
                    if (!Directory.Exists(buildFolder))
                    {
                        throw new ArgumentException($"Can not find build folder {buildFolder}");
                    }
                }

                versionFile = Path.Combine(buildFolder, "Version.props");

                if (!File.Exists(versionFile))
                {
                    throw new ArgumentException($"Can not find {versionFile}");
                }
            }
            else
            {
                versionFile = Path.GetFullPath(versionFile);
                if (!File.Exists(versionFile))
                {
                    throw new ArgumentException($"Can not find {versionFile}");
                }
            }

            Console.WriteLine($"Version file is {versionFile}");

            return new FileInfo(versionFile);
        }

        private static SemanticVersion ReadTagVersion(string tag)
        {
            const string githubRef = "refs/tags/";
            if (tag.StartsWith(githubRef))
            {
                // github
                tag = tag.Substring(githubRef.Length);
            }

            if (tag.StartsWith("v", StringComparison.OrdinalIgnoreCase))
            {
                tag = tag.Substring(1);
            }

            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentException("Could not find the tag version");
            }


            // 使用 NuGetVersion 而不是 SemanticVersion 是因为不支持 rc 版本号
            if (NuGetVersion.TryParse(tag, out NuGetVersion tagVersion))
            {
                return tagVersion;
            }
            else
            {
                throw new ArgumentException("Can not parse to version");
            }
        }

        [Option('c', "toconsole", Required = false, HelpText = "Output the tag version to console")]
        public bool ToConsole { set; get; }

        [Option('t', "tag", Required = true, HelpText = "The Tag Name")]
        public string Tag { set; get; }

        [Option('f', "file", HelpText = "The version file path")]
        public string VersionFile { set; get; }
    }

}
