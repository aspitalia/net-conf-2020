using Microsoft.Extensions.DependencyInjection;
using EmailSenderTool.Services;
using EmailSenderTool.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandLine;
using System.IO;
using System;

namespace EmailSenderTool
{
    class Program
    {
        /// <summary>
        /// Gets or sets the list of services
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Represents the application entrypoint
        /// </summary>
        /// <param name="args">Startup parameters needed to configure the SMTP client and send the email</param>
        static async Task Main(string[] args)
        {
            // setup of the dependency injection engine
            RegisterDI();
            PrintArgs(args);
                        
            var result = Parser.Default.ParseArguments<Options>(args)
                .WithNotParsed<Options>((errs) => HandleParseError(errs));

            await result.WithParsedAsync(async options => await RunOptionsAndReturnExitCode(options));  
        }

        /// <summary>
        /// Prints all the arguments received when the application starts for debugging purposes
        /// </summary>
        /// <param name="args">The arguments received from the application on startup</param>
        private static void PrintArgs(string[] args)
        {
            Console.WriteLine($"Arguments: {string.Join(", ", args)}");
        }

        /// <summary>
        /// Validates input parameters and sends the required email
        /// </summary>
        /// <param name="options">The parameters parsed during application startup</param>
        private static async Task RunOptionsAndReturnExitCode(Options options)
        {
            // load all the services
            var logger = ServiceProvider.GetService<ILogger>();
            var mailService = ServiceProvider.GetService<IMailService>();

            // when sending an email where its content is stored within a file, 
            // then ensure that the path is specified in the body property
            if (options.MailFromFile && string.IsNullOrEmpty(options.Body))
            {
                Console.Error.WriteLine($"You need to use the '{nameof(options.Body)}' option property to specify the path of the file where the body of the email can be found and loaded.");
                throw new ArgumentOutOfRangeException();
            }

            // ensure that the file exists before proceeding
            if (options.MailFromFile && !File.Exists(options.Body))
            {
                Console.Error.WriteLine($"The file specified in the path {options.Body} cannot be found.");
                throw new FileNotFoundException();
            }

            // send the email
            logger.LogInformation("Initializing...");
            await mailService.SendEmailAsync(options);

            // inform the client the process is completed
            logger.LogInformation("Process completed.");
        }

        /// <summary>
        /// Registers all the services for the dependency injection
        /// </summary>
        private static void RegisterDI()
        {
            //setup our DI
            var services = new ServiceCollection();

            // register logging
            services
                .AddLogging()
                .AddSingleton<ILogger>(GetLogger());

            // register internal services
            services
                .AddSingleton<IMailService, MailService>();

            // create the service provider
            ServiceProvider = services.BuildServiceProvider(true);
        }

        /// <summary>
        /// Creates the logger service
        /// </summary>
        /// <returns>An instance of <see cref="ILogger"/></returns>
        private static ILogger GetLogger()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Critical)
                    .AddFilter("System", LogLevel.Critical)
                    .AddConsole();
            });

            ILogger logger = loggerFactory.CreateLogger("EmailSender");
            return logger;
        }

        /// <summary>
        /// Prints all the errors generated when parsing invalid input parameters
        /// </summary>
        /// <param name="errs">Parameters that cannot be parsed during application startup</param>
        private static int HandleParseError(IEnumerable<Error> errs)
        {
            Console.Error.WriteLine(string.Join("\r\n", errs));
            throw new ArgumentException("Please fill all the required parameters.");
        }
    }
}