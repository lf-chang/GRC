using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.DataModel;

namespace GRC_AWS
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        

        protected void Page_Load(object sender, EventArgs e)
        {

            //crtTable();
            TestCRUDOperations();

        }

        public void PauseForDebugWindow()
        {
            // Keep the console open if in Debug mode...
            Response.Write("\n\n ...Press any key to continue");
            
            //Response.ReadKey();
            //Response.WriteLine();
        }

        private void crtTable() {

            AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
            //ddbConfig.ServiceURL = "http://localhost:8000";
            //ddbConfig.ServiceURL = "http://dynamodb.ap-southeast-1.amazonaws.com";
            ddbConfig.ServiceURL = "https://dynamodb.us-west-2.amazonaws";
            AmazonDynamoDBClient client;
            try { client = new AmazonDynamoDBClient(ddbConfig); }
            catch (Exception ex)
            {
                Response.Write("\n Error: failed to create a DynamoDB client; " + ex.Message);
                PauseForDebugWindow();
                return;
            }

            // Build a 'CreateTableRequest' for the new table
            CreateTableRequest createRequest = new CreateTableRequest
            {
                TableName = "Movies02",
                AttributeDefinitions = new List<AttributeDefinition>()
            {
                new AttributeDefinition
                {
                    AttributeName = "year02",
                    AttributeType = "N"
                },
                new AttributeDefinition
                {
                    AttributeName = "title02",
                    AttributeType = "S"
                }
            },
                KeySchema = new List<KeySchemaElement>()
            {
                new KeySchemaElement
                {
                    AttributeName = "year02",
                    KeyType = "HASH"
                },
                new KeySchemaElement
                {
                    AttributeName = "title02",
                    KeyType = "RANGE"
                }
            },
            };

            // Provisioned-throughput settings are required even though
            // the local test version of DynamoDB ignores them
            createRequest.ProvisionedThroughput = new ProvisionedThroughput(1, 1);

            // Using the DynamoDB client, make a synchronous CreateTable request
            CreateTableResponse createResponse;
            try { createResponse = client.CreateTable(createRequest);
                Response.Write("\n write ok!");
            }
            catch (Exception ex)
            {
                Response.Write("\n Error: failed to create the new table; " + ex.Message);
                PauseForDebugWindow();
                return;
            }
        }

        private  void TestCRUDOperations()
        {
            AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
            //ddbConfig.ServiceURL = "http://localhost:8000";
            //ddbConfig.ServiceURL = "http://dynamodb.ap-southeast-1.amazonaws.com";
            ddbConfig.ServiceURL = "https://dynamodb.us-west-2.amazonaws";
            AmazonDynamoDBClient client;
            try { client = new AmazonDynamoDBClient(ddbConfig); }
            catch (Exception ex)
            {
                Response.Write("\n Error: failed to create a DynamoDB client; " + ex.Message);
                PauseForDebugWindow();
                return;
            }

            DynamoDBContext context = new DynamoDBContext(client);


            int bookID = 1001; // Some unique value.
            Book myBook = new Book
            {
                Id = bookID,
                Title = "object persistence-AWS SDK for.NET SDK-Book 1001",
                ISBN = "111-1111111001",
                BookAuthors = new List<string> { "Author 1", "Author 2" },
            };

            // Save the book.
            context.Save(myBook);

            /*
            int bookID1 = 1002; // Some unique value.
            Book myBook1 = new Book
            {
                Id = bookID,
                Title = "object persistence-AWS SDK for.NET SDK-Book 1002",
                ISBN = "222-1111111001",
                BookAuthors = new List<string> { "Author 1", "Author 2", "Author 3", "Author 4" },
            };

            // Save the book.
            context.Save(myBook1);

            return;
            // Retrieve the book.
            Book bookRetrieved = context.Load<Book>(bookID);

            

            // Update few properties.
            bookRetrieved.ISBN = "222-2222221001";
            bookRetrieved.BookAuthors = new List<string> { " Author 1", "Author x" }; // Replace existing authors list with this.
            context.Save(bookRetrieved);

            // Retrieve the updated book. This time add the optional ConsistentRead parameter using DynamoDBContextConfig object.
            Book updatedBook = context.Load<Book>(bookID, new DynamoDBContextConfig
            {
                ConsistentRead = true
            });
            
            // Delete the book.
            context.Delete<Book>(bookID);
            // Try to retrieve deleted book. It should return null.
            Book deletedBook = context.Load<Book>(bookID, new DynamoDBContextConfig
            {
                ConsistentRead = true
            });
            if (deletedBook == null)
                Console.WriteLine("Book is deleted");

            */
        }

        [DynamoDBTable("ProductCatalog")]
        public class Book
        {
            [DynamoDBHashKey] //Partition key
            public int Id
            {
                get; set;
            }
            [DynamoDBProperty]
            public string Title
            {
                get; set;
            }
            [DynamoDBProperty]
            public string ISBN
            {
                get; set;
            }
            [DynamoDBProperty("Authors")] //String Set datatype
            public List<string> BookAuthors
            {
                get; set;
            }
        }

    }
}