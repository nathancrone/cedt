using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using System.Configuration;
using System.IO;

using System.Xml;
using System.Xml.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace cedt.CreateBlobStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the account
            CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureStorage"].ConnectionString);

            // Create a blob client.
            CloudBlobClient blobClient = account.CreateCloudBlobClient();

            // Get a reference to a blob container.
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("iocb");

            //the blob format
            string formatString = "us752/en/CNT/street/{0}/";

            string[] colors = new string[] { "#ead945", "#6f0989", "#da712d", "#97cae5", "#b82035" };

            string filesDirectory = ConfigurationManager.AppSettings["filesDirectory"];
            //string templateFileName = ConfigurationManager.AppSettings["templateFileName"];
            //string coordinatesFileName = ConfigurationManager.AppSettings["coordinatesFileName"];
            //string segmentsFileName = ConfigurationManager.AppSettings["segmentsFileName"];

            ////get the template
            //XDocument templateDoc = XDocument.Load(Path.Combine(filesDirectory, templateFileName));

            ////get the coordinates file
            //XDocument coordinatesDoc = XDocument.Load(Path.Combine(filesDirectory, coordinatesFileName));

            //// Read the file as one string
            //string stringSegments = System.IO.File.ReadAllText(Path.Combine(filesDirectory, segmentsFileName));

            ////parse the json
            //JArray jsonSegments = JArray.Parse(stringSegments);

            ////iterate through the territories
            //int index = 0;
            //foreach (JObject jsonTerritory in jsonSegments)
            //{
            //    string territory = jsonTerritory["territory"].Value<string>();

            //    List<string> coordinatesList = new List<string>();

            //    //iterate throught the segments for the territory
            //    foreach (JObject jsonSegment in jsonTerritory["segments"])
            //    {
            //        //put the segmentid in a variable
            //        string segmentId = jsonSegment["id"].Value<string>();
            //        bool reverse = jsonSegment["reverse"].Value<bool>();

            //        //find the placemark node from the coordinates xml doc
            //        XElement Placemark = coordinatesDoc.Root.Descendants().Where(
            //            x => x.Name.LocalName == "Placemark" &&
            //            x.Descendants().Any(
            //                b => b.Name.LocalName == "name" &&
            //                String.Concat(b.Nodes()) == segmentId)).First();

            //        //extract the coordinates from the placemark
            //        string coords = String.Concat(Placemark.Elements().Where(
            //            a => a.Name.LocalName == "Point" || a.Name.LocalName == "LineString").First().Descendants().Where(
            //            b => b.Name.LocalName == "coordinates").Nodes());

            //        //add these coordinates to the coordinates list
            //        if (reverse)
            //        {
            //            coordinatesList.AddRange(coords.Split(' ').Reverse());
            //        }
            //        else
            //        {
            //            coordinatesList.AddRange(coords.Split(' '));
            //        }
            //    }

            //    CloudBlockBlob blobColor = blobContainer.GetBlockBlobReference(string.Format(formatString + "color", territory));
            //    blobColor.UploadText(colors[index % (colors.Length - 1)]);

            //    CloudBlockBlob blobMidpoint = blobContainer.GetBlockBlobReference(string.Format(formatString + "midpoint", territory));
            //    blobMidpoint.UploadText("0,0");

            //    CloudBlockBlob blobCoords = blobContainer.GetBlockBlobReference(string.Format(formatString + "coordinates", territory));
            //    blobCoords.UploadText(string.Join(" ", coordinatesList.ToArray()));

            //    CloudBlockBlob blobInaccessible = blobContainer.GetBlockBlobReference(string.Format(formatString + "inaccessible", territory));
            //    blobInaccessible.UploadText("[ ]");

            //    CloudBlockBlob blobDoNotCall = blobContainer.GetBlockBlobReference(string.Format(formatString + "donotcall", territory));
            //    blobDoNotCall.UploadText("[ ]");

            //    index++;
            //}





            string[] files = new string[] { "t01", "t02", "t03", "t04", "t05", "t06" };
            
            int index = 0;
            foreach (string file in files)
            {
                //get the file
                XDocument tDoc = XDocument.Load(Path.Combine(filesDirectory, string.Format("{0}.kml", file)));

                foreach (var folder in tDoc.Root.Descendants().Where(a => a.Name.LocalName == "Folder"))
                {
                    string territory = folder.Descendants().Where(a => a.Name.LocalName == "name").FirstOrDefault().Value.ToLower();

                    //string coords = folder.Descendants().Where(a => a.Name.LocalName == "coordinates").FirstOrDefault().Value;

                    //CloudBlockBlob blobColor = blobContainer.GetBlockBlobReference(string.Format(formatString + "color", territory));
                    //blobColor.UploadText(colors[index % (colors.Length - 1)]);

                    //CloudBlockBlob blobMidpoint = blobContainer.GetBlockBlobReference(string.Format(formatString + "midpoint", territory));
                    //blobMidpoint.UploadText("0,0");

                    //CloudBlockBlob blobCoords = blobContainer.GetBlockBlobReference(string.Format(formatString + "coordinates", territory));
                    //blobCoords.UploadText(coords);

                    //CloudBlockBlob blobInaccessible = blobContainer.GetBlockBlobReference(string.Format(formatString + "inaccessible", territory));
                    //blobInaccessible.UploadText("[ ]");

                    //CloudBlockBlob blobDoNotCall = blobContainer.GetBlockBlobReference(string.Format(formatString + "donotcall", territory));
                    //blobDoNotCall.UploadText("[ ]");

                    index++;
                }
            }







            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
