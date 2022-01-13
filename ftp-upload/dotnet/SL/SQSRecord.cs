using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using System.IO;
using System.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LambdaFunction
{
    public class SQSRecord
    {
        public string messageId { get; set; }
        public string body { get; set; }
    }
}
