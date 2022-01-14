using System.Collections.Generic;
using System.Diagnostics;
using LambdaFunction;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing.Parsers;
using System.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing;
using System.Linq;
using System.IO;

public class Console {
    public static void Main(string[] args) {
        //setup our DI
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddSingleton<LambdaHandler, LambdaHandler>()
            .BuildServiceProvider();

        /*var configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", false, true)
                            .Build();*/

        //configure console logging
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Trace).AddFilter("System.Net", LogLevel.Trace));
        //serviceProvider.GetService<ILoggerFactory>().AddConsole(null);

        var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Console>();
        logger.LogDebug("Starting application");

        var lambdaHandler = serviceProvider.GetService<LambdaHandler>();
        var classLogger = serviceProvider.GetService<ILogger<LambdaHandler>>();

        logger.LogInformation("Started");
        string path = "../local/sample-sqs-message.json";
        object response = lambdaHandler.handleRequest(new FileStream(path, FileMode.Open), null).ConfigureAwait(false).GetAwaiter().GetResult();
        logger.LogInformation(response.ToString());
        logger.LogInformation("Ended.");
    }
}