using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using System.IO;
using System.Net;

namespace LambdaFunction
{
    public class LambdaHandler
    {
        [LambdaSerializer(typeof(JsonSerializer))]
        public object handleRequest(Stream inputStream)
        {            
            uploadFile();
            var response = new
            {
                body = "You just called a dotnet Lambda handler",
                statusCode = 200
            };
            return response;
        }

        private void createLocalDummFile(string localFilePath)
        {
            System.Console.WriteLine($"currentDir=[{Directory.GetCurrentDirectory()}]");
            File.WriteAllText(localFilePath, "id;name;company\n100;Michael;AWS\n200;Jane;AWS");
        }

        private void uploadFile()
        {
            using (var client = new WebClient())
            {
                var localFilePath = System.Environment.GetEnvironmentVariable("localFilePath");
                var targetFTPPath = System.Environment.GetEnvironmentVariable("targetFTPPath");
                var ftpUsername = System.Environment.GetEnvironmentVariable("ftpUsername");
                var ftpPassword = System.Environment.GetEnvironmentVariable("ftpPassword");
                if (string.IsNullOrEmpty(localFilePath)) throw new System.Exception("Please define an environment variable for [localFilePath]");
                if (string.IsNullOrEmpty(targetFTPPath)) throw new System.Exception("Please define an environment variable for [targetFTPPath]");
                if (string.IsNullOrEmpty(ftpUsername)) throw new System.Exception("Please define an environment variable for [ftpUsername]");
                if (string.IsNullOrEmpty(ftpPassword)) throw new System.Exception("Please define an environment variable for [ftpPassword]");
                System.Console.WriteLine($"localFilePath={localFilePath}");
                System.Console.WriteLine($"targetFTPPath={targetFTPPath}");
                client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                createLocalDummFile(localFilePath);
                System.Console.WriteLine($"Local file created.");
                client.UploadFile(targetFTPPath, WebRequestMethods.Ftp.UploadFile, localFilePath);
                System.Console.WriteLine($"File uploaded to FTP server.");
            }
        }
    }
}
