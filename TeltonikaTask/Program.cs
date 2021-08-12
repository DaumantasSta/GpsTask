using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using TeltonikaTask.DataRepository;
using TeltonikaTask.Models;
using TeltonikaTask.Tasks;

namespace TeltonikaTask
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileNameJson = "2019-07.json";
            string fileNameCsv = "2019-08.csv";
            string fileNameBin = "2019-09.bin";

            List<GpsData> gpsData = new List<GpsData>();
            bool showMenu = true;
            
            JsonDataRepository json = new JsonDataRepository();
            CsvDataRepository csv = new CsvDataRepository();
            BinDataRepository bin = new BinDataRepository();

            //gpsData = json.LoadData(fileNameJson);
            //gpsData = csv.LoadData(fileNameCsv);
            gpsData = bin.LoadData(fileNameBin);

            //Adding all records together
            //gpsData.AddRange(json.LoadData(fileNameJson));
            //gpsData.AddRange(csv.LoadData(fileNameCsv));
            
            showMenu = CheckDataExist(showMenu, gpsData);

            while (showMenu == true)
            {
                Console.Clear();
                Console.WriteLine("Parsed gps records: " + gpsData.Count);
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1) Print Satellites Histogram");
                Console.WriteLine("2) Print Speed Historgram");
                Console.WriteLine("3) Calculate distance");
                Console.WriteLine("4) Exit");
                Console.Write("\r\nSelect an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        SatellitesHistogram s = new SatellitesHistogram();
                        s.PrintSatellitesHistogram(gpsData);
                        break;
                    case "2":
                        SpeedHistogram sp = new SpeedHistogram();
                        sp.PrintSpeedHistogram(gpsData);
                        break;
                    case "3":
                        RoadSections sh = new RoadSections();
                        sh.FindFasterRoadSection(gpsData);
                        break;
                    case "4":
                        showMenu = false;
                        break;
                }
            }
        }

        private static bool CheckDataExist(bool showMenu, List<GpsData> gpsData)
        {
            if (gpsData.Count < 1)
            {
                showMenu = false;
                Console.WriteLine("No data");
            }

            return showMenu;
        }
    }
}
