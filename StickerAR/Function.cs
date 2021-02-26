using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace StickerAR
{
    public class StickerModel
    {
        public string id { get; set; }
        public string created_at { get; set; }
        public string description_sticker { get; set; }
        public string name_owner { get; set; }
        public string name_sticker { get; set; }
        public string url_sticker { get; set; }
        public int likes { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Function
    {
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private static string tableName = "Sticker_StickerAR";

        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public async Task<StickerModel> FunctionHandler(StickerModel input)
        {
            try
            {
                Table Sticker_StickerAR = Table.LoadTable(client, tableName);

                input.id = Guid.NewGuid().ToString();
                input.created_at = DateTime.Now.ToString();
                input.likes = 0;

                var sticker = new Document();

                sticker["id"] = input.id;
                sticker["created_at"] = input.created_at;
                sticker["description_sticker"] = input.description_sticker;
                sticker["name_owner"] = input.name_owner;
                sticker["name_sticker"] = input.name_sticker; ;
                sticker["url_sticker"] = input.url_sticker;
                sticker["likes"] = 0;
                sticker["width"] = input.width;
                sticker["height"] = input.height;

                await Sticker_StickerAR.PutItemAsync(sticker);
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
    }
}
