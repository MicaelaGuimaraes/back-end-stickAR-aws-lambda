using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Amazon.Runtime;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ADDLikes_StickerAR
{
    public class ADDLikes
    {
        public string id { get; set; }
    }
    public class Function
    {
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private static string tableName = "Sticker_StickerAR";

        public async Task<ADDLikes> FunctionHandler(ADDLikes input)
        {
            try
            {
                Table Sticker_StickerAR = Table.LoadTable(client, tableName);

                var request = new UpdateItemRequest
                {
                    Key = new Dictionary<string, AttributeValue>() { { "id", new AttributeValue { S = input.id } } },
                    ExpressionAttributeNames = new Dictionary<string, string>()
                        {
                            {"#likes", "likes"}
                        },
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                        {
                            {":incr",new AttributeValue {N = "1"}}
                        },
                    UpdateExpression = "SET #likes = #likes + :incr",
                    TableName = tableName
                };

                var response = await client.UpdateItemAsync(request);
                return input;
            }
            catch (AmazonDynamoDBException ex)
            {
                throw;
            }
            catch (AmazonServiceException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private Dictionary<string, string> PrintDocument(Document document)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();

            foreach (var attribute in document.GetAttributeNames())
            {
                string stringValue = null;
                var value = document[attribute];
                if (value is Primitive)
                    stringValue = value.AsPrimitive().Value.ToString();
                else if (value is PrimitiveList)
                    stringValue = string.Join(",", (from primitive
                                    in value.AsPrimitiveList().Entries
                                                    select primitive.Value).ToArray());
                values.Add(attribute, stringValue);
            }

            return values;
        }
    }
}
