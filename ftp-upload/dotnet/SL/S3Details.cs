using Newtonsoft.Json;

namespace LambdaFunction
{
    public class S3Details
    {
        public S3BucketInfo bucket { get; set; }
        [JsonProperty("object")]
        public S3ObjectInfo s3object { get; set; }
    }
}