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

        //do the actual work here
        var lambdaHandler = serviceProvider.GetService<LambdaHandler>();
        var classLogger = serviceProvider.GetService<ILogger<LambdaHandler>>();

        Trace.Listeners.Add(new TextWriterTraceListener("../local/TextWriterOutput.log", "myListener"));        
        Trace.TraceInformation("Test message.");
        // You must close or flush the trace to empty the output buffer.        

        logger.LogInformation("Started");

        /*var providers = new List<EventPipeProvider>()
        {
            new EventPipeProvider("Microsoft-DotNETCore-SampleProfiler",
                EventLevel.Informational, (long)ClrTraceEventParser.Keywords.All),    
            new EventPipeProvider("Microsoft-Windows-DotNETRuntime",
                EventLevel.Informational, (long)ClrTraceEventParser.Keywords.Default) 
        };*/
       var providers = new List<EventPipeProvider>()
        {
            new EventPipeProvider("System.Net",
                EventLevel.Informational, arguments: new Dictionary<string, string>
            {
                //{"EventCounterIntervalSec", "1"}
            })
        };
        
        // Create client        
        var diagClient = new DiagnosticsClient(Process.GetCurrentProcess().Id);
        // Create session
        using (var eventPipeSession = diagClient.StartEventPipeSession(providers)) {
            var source = new EventPipeEventSource(eventPipeSession.EventStream);

            source.Dynamic.All += obj =>
            {
                lambdaHandler.handleRequest(null, null);
                //if (obj.EventName == "EventCounters")
                //{
                    var payload = (IDictionary<string, object>)obj.PayloadValue(0);
                    System.Console.WriteLine(string.Join(", ", payload.Select(p => $"{p.Key}: {p.Value}")));
                //}
            };

            source.Process();
        }

        //var eventPipeSession = diagClient.StartEventPipeSession(providers);        
        //diagClient.WriteDump(DumpType.Normal, "../local/dump.txt", true);
        lambdaHandler.handleRequest(null, null);
        //eventPipeSession.Stop();
        //Trace.Flush();
        logger.LogInformation("Ended.");
    }
}