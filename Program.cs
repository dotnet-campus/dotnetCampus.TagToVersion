using System;
using System.IO;
using CommandLine;
using NuGet.Versioning;

namespace dotnetCampus.TagToVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("dotnetCampus.TagToVersion");
            Console.WriteLine($"Current directory {Environment.CurrentDirectory}");
            Console.WriteLine($"Args {Environment.CommandLine}");

            try
            {
                Parser.Default.ParseArguments<Program>(args)
                    .WithParsed(option =>
                    {
                        Console.WriteLine($"Read tag version. Tag name is {option.Tag}");
                        var version = ReadTagVersion(option.Tag);
                        Console.WriteLine($"Tag version is {version}");

                        Console.WriteLine($"Find the version.props file. ");
                        var versionFile = FindVersionFile(option.VersionFile);

                        WriteVersionToFile(version, versionFile);
                    })
                    .WithNotParsed(errors =>
                    {

                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Environment.Exit(-1);
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
                versionFile = Path.GetFullPath(@"build\Version.props");

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
            if (tag.StartsWith("v", StringComparison.OrdinalIgnoreCase))
            {
                tag = tag.Substring(1);
            }

            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentException("Could not find the tag version");
            }



            if (SemanticVersion.TryParse(tag, out var tagVersion))
            {
                return tagVersion;
            }
            else
            {
                throw new ArgumentException("Can not parse to version");
            }
        }

        [Option('t', "tag", Required = true, HelpText = "The Tag Name")]
        public string Tag { set; get; }

        [Option('f', "file", HelpText = "The version file path")]
        public string VersionFile { set; get; }
    }

}
