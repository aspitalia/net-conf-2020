using System;

namespace HelloWorldTool
{
    class Program
    {
        // test the tool
        // dotnet tool install --tool-path . Hello --add-source .\nupkg\ --version 1.0.0
        // dotnet tool uninstall hello --tool-path .

        static void Main(string[] args)
        {
            var name = "world";

            if (args.Length > 0)
            {
                name = args[0];
            }

            Console.WriteLine($"Hello, {name}!");

            // IntegrateLogging();
        }

        public static void IntegrateLogging()
        {
            Console.WriteLine($"##[group]Beginning of a group");
            Console.WriteLine($"##[warning]Warning message");
            Console.WriteLine($"##[error]Error message");
            Console.WriteLine($"##[debug]Debug text");
            Console.WriteLine($"##[command]Command-line being run");
            Console.WriteLine($"##[endgroup]");
        }

        public static void AdvancedLogging()
        {
            // Log issues
            Console.WriteLine("##vso[task.logissue type=warning;sourcepath=Program.cs;linenumber=1;columnnumber=1;code=10;]Found something that could be a problem.");
            
            // Show progress
            Console.WriteLine("Starting very long process...");

            for (var i = 0; i < 100; i++)
            {
                Console.WriteLine($"##vso[task.setprogress value={i};]Sample Progress Indicator");
            }
            
            Console.WriteLine("Process completed.");

            // Create environment variables
            Console.WriteLine("##vso[task.setvariable variable=myEnvVariable;]netconf2020");

            // Upload a markdown file to be shown into the build summary
            Console.WriteLine("##vso[task.uploadsummary]c:\\testsummary.md");

            // Add build tag
            Console.WriteLine("##vso[build.addbuildtag]build tag");
        }
    }
}