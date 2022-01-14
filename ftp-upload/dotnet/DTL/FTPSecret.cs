using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;
using System.IO;
using System.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DTL
{
    public class FTPSecret
    {
        [JsonProperty("ftp-login")]
        public string Login { get; set; }
        [JsonProperty("ftp-pw")]
        public string Password { get; set; }
        [JsonProperty("ftp-host")]
        public string Host { get; set; }
    }
}
