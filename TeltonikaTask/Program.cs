using System;
using System.Collections.Generic;
using TeltonikaTask.DataRepository;
using TeltonikaTask.Models;
using TeltonikaTask.Tasks;

namespace TeltonikaTask
{
    class Program
    {
        static void Main(string[] args)
        {
            String fileNameJson = "2019-07.json";
            String fileNameCsv = "2019-08.csv";
            String fileNameBin = "2019-09.bin";
            bool showMenu = true;

            List<GpsData> gpsData = new List<GpsData>();

            JsonDataRepository json = new JsonDataRepository();
            gpsData = json.LoadData(fileNameJson);

            Console.WriteLine("Parsed gps records: " + gpsData.Count);

            showMenu = CheckDataExist(showMenu, gpsData);

            while (showMenu == true)
            {
                Console.Clear();
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
                        //Todo
                        break;
                    case "3":
                        //Todo
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
