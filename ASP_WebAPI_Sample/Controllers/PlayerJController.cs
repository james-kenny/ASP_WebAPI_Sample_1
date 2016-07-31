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
    public class PlayerJController : ApiController
    {
        private static string _connectionString = "DefaultEndpointsProtocol=https;AccountName=ACCOUNTNAME;AccountKey=ACCOUNTKEY";

        // This controller uses JSON to pass parameters.
        [HttpPost]
        public string Post([FromBody] Player record)
        {
            string sResponse = "";

            // To make sure the ROW is random we use the random to generarte a unique ID, you can do this anyway you like.
            Random rnd = new Random();
            int rowId = rnd.Next(1, 450);
            string sRow = rowId.ToString();


            // Create the entity with a partition key for sport and a row
            // Row should be unique within that partition
            PlayerEntity _record = new PlayerEntity(record.Sport_VC, sRow);

            _record.Sport_VC = record.Sport_VC;
            _record.First_Name_VC = record.First_Name_VC;
            _record.Last_Name_VC = record.Last_Name_VC;
            _record.Club_VC = record.Club_VC;
            _record.Position_VC = record.Position_VC;

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
