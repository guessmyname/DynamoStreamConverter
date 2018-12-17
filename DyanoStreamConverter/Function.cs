using System;
using System.IO;
using System.Text;

using Newtonsoft.Json;

using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Amazon.DynamoDBv2.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AWSLambda.StreamTest
{
    public class Function
    {
        private static readonly JsonSerializer _jsonSerializer = new JsonSerializer();

        public void FunctionHandler(DynamoDBEvent dynamoEvent, ILambdaContext context)
        {
            context.Logger.LogLine($"Beginning to process {dynamoEvent.Records.Count} records...");

            foreach (var record in dynamoEvent.Records)
            {

                dynamic image = record.Dynamodb.NewImage;


                context.Logger.LogLine($"Event ID: {record.EventID}");
                context.Logger.LogLine($"Event Name: {record.EventName}");

                context.Logger.LogLine($"id:{image.id.S}");

                string streamRecordJson = SerializeStreamRecord(record);
                context.Logger.LogLine($"DynamoDB Record:");
                context.Logger.LogLine(streamRecordJson );
            }

            context.Logger.LogLine("Stream processing complete.");
        }

        private string SerializeStreamRecord(object streamRecord)
        {
            using (var writer = new StringWriter())
            {
                _jsonSerializer.Serialize(writer, streamRecord);
                return writer.ToString();
            }
        }
    }
}