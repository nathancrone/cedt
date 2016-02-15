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

namespace cedt.SegmentConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            string filesDirectory = ConfigurationManager.AppSettings["filesDirectory"];
            string templateFileName = ConfigurationManager.AppSettings["templateFileName"];
            string coordinatesFileName = ConfigurationManager.AppSettings["coordinatesFileName"];
            string segmentsFileName = ConfigurationManager.AppSettings["segmentsFileName"];

            //get the template
            XDocument templateDoc = XDocument.Load(Path.Combine(filesDirectory, templateFileName));

            //get the coordinates file
            XDocument coordinatesDoc = XDocument.Load(Path.Combine(filesDirectory, coordinatesFileName));
            
            // Read the file as one string
            string stringSegments = System.IO.File.ReadAllText(Path.Combine(filesDirectory, segmentsFileName));
            
            //parse the json
            JArray jsonSegments = JArray.Parse(stringSegments);

            //iterate through the territories
            foreach (JObject jsonTerritory in jsonSegments)
            {
                string territory = jsonTerritory["territory"].Value<string>();

                List<string> coordinatesList = new List<string>();

                //iterate throught the segments for the territory
                foreach (JObject jsonSegment in jsonTerritory["segments"])
                {
                    //put the segmentid in a variable
                    string segmentId = jsonSegment["id"].Value<string>();
                    bool reverse = jsonSegment["reverse"].Value<bool>();

                    //find the placemark node from the coordinates xml doc
                    XElement Placemark = coordinatesDoc.Root.Descendants().Where(
                        x => x.Name.LocalName == "Placemark" &&
                        x.Descendants().Any(
                            b => b.Name.LocalName == "name" &&
                            String.Concat(b.Nodes()) == segmentId)).First();

                    //extract the coordinates from the placemark
                    string coords = String.Concat(Placemark.Elements().Where(
                        a => a.Name.LocalName == "Point" || a.Name.LocalName == "LineString").First().Descendants().Where(
                        b => b.Name.LocalName == "coordinates").Nodes());

                    //add these coordinates to the coordinates list
                    if (reverse)
                    {
                        coordinatesList.AddRange(coords.Split(' ').Reverse());
                    }
                    else
                    {
                        coordinatesList.AddRange(coords.Split(' '));
                    }
                }


                XElement templatePlacemark = templateDoc.Root.Descendants().Where(
                    a => a.Name.LocalName == "Placemark").First();

                templatePlacemark.Descendants().Where(
                    b => b.Name.LocalName == "name").First().SetValue(territory);

                templatePlacemark.Descendants().Where(
                    b => b.Name.LocalName == "coordinates").First().SetValue(string.Join(" ", coordinatesList.ToArray()));


                templateDoc.Save(string.Format("{0}.kml", territory));
                //Console.WriteLine(territory);
                //Console.WriteLine(coordinatesList.Count.ToString());
            }

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
