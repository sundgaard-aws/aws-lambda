using Amazon.Lambda.Core;

namespace LambdaFunction
{
    internal class LocalLambdaLogger : ILambdaLogger
    {
        public void Log(string message)
        {
            System.Console.WriteLine(message);
        }

        public void LogLine(string message)
        {
            System.Console.WriteLine($"{message}\n");
        }
    }
}