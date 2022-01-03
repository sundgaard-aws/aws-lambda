using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using System.IO;
using System.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LambdaFunction
{
    public class LambdaHandler
    {
        private ILambdaLogger logger;
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public object handleRequest(Stream inputStream, ILambdaContext context)
        {  
            if(context != null) logger = context.Logger;
            else logger = new LocalLambdaLogger();
            logger.LogLine("This is information");
            FTPFacade.uploadFile();
            var response = new
            {
                body = "You just called a dotnet Lambda handler",
                statusCode = 200
            };
            return response;
        }       
    }
}
