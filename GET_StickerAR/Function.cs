using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.Core;
using Amazon.Runtime;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace GET_StickerAR
{
    public class Function
    {
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        private static string tableName = "Sticker_StickerAR";

        public async Task<List<Dictionary<string, string>>> FunctionHandler()
        {
            try
            {
                Table Sticker_StickerAR = Table.LoadTable(client, tableName);

                ScanFilter scanFilter = new ScanFilter();

                Search search = Sticker_StickerAR.Scan(scanFilter);


                List<Document> documentList = new List<Document>();
                documentList = await search.GetNextSetAsync();

                List<Dictionary<string, string>> keyValuePairs = new List<Dictionary<string, string>>();

                foreach (var document in documentList)
                {
                    keyValuePairs.Add(PrintDocument(document));
                }

                return keyValuePairs;

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
