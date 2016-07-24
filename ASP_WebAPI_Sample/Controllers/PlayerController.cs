using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ASP_WebAPI_Sample.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace ASP_WebAPI_Sample.Controllers
{
    public class PlayerController : ApiController
    {
        private static string _connectionString = "DefaultEndpointsProtocol=https;AccountName=ACCOUNTNAME;AccountKey=ACCOUNTKEY";

        // GET api/player
        public List<PlayerEntity> Get(string sport)
        {
            List<PlayerEntity> _records = new List<PlayerEntity>();

            //Create a storage account object.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("player");

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            // Get All Players for a sport
            TableQuery<PlayerEntity> query = new TableQuery<PlayerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, sport));
            
            foreach (PlayerEntity entity in table.ExecuteQuery(query))
            {
                _records.Add(entity);
            }

            return _records;
        }

        // POST api/Player
        public string Post(string sSport, string sRow, string sFirstName, string sLastName, string sClub, string sPostition)
        {
            string sResponse = "";

            // Create our player

            // Create the entity with a partition key for sport and a row
            // Row should be unique within that partition
            PlayerEntity _record = new PlayerEntity(sSport, sRow);

            _record.Sport_VC = sSport;
            _record.First_Name_VC = sFirstName;
            _record.Last_Name_VC = sLastName;
            _record.Club_VC = sClub;
            _record.Position_VC = sPostition;

            //Create a storage account object.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("player");

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(_record);


            try
            {
                // Execute the insert operation.
                table.Execute(insertOperation);

                sResponse = "OK";
            }
            catch (Exception ex)
            {
                sResponse = "Failed: " + ex.ToString();
            }



            return sResponse;
        }
    }
}
