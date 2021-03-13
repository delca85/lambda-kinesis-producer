using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.S3;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace KinesisProducer
{
    public class Function
    {
        private AmazonS3Client _client;
        public Function()
        {
            _client = new AmazonS3Client(RegionEndpoint.EUWest3);
        }

        private const string BUCKET_NAME = "my-bucket-for-playing";

        /// <summary>
        /// A simple function
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<List<GSLCompetition>> FunctionHandler(ILambdaContext context)
        {
            using (var competitionsObject = await _client.GetObjectAsync(BUCKET_NAME, "input/competitions.json"))
            using (var competitionsStream = competitionsObject.ResponseStream)
            using (var competitionsReader = new StreamReader(competitionsStream))
            using (var worldRegionsObject = await _client.GetObjectAsync(BUCKET_NAME, "input/worldRegions.json"))
            using (var worldRegionsStream = worldRegionsObject.ResponseStream)
            using (var worldRegionsReader = new StreamReader(worldRegionsStream))
            {
                var competitionsString = await competitionsReader.ReadToEndAsync();
                var competitionsResponse = JsonConvert.DeserializeObject<List<GSLCompetition>>(competitionsString);

                return competitionsResponse;
            }
        }
    }
}
