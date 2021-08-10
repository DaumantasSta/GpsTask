using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TeltonikaTask.Models;

namespace TeltonikaTask.DataRepository
{
    public class CsvDataRepository
    {

        public List<GpsData> LoadData(string fileName)
        {
                List<GpsData> gpsData;
                gpsData = File.ReadAllLines(fileName).Select(v => CsvForm(v))
                                               .ToList();

                return gpsData;
        }

        public static GpsData CsvForm(string csvLine)
        {
            string[] values = csvLine.Split(',');
            GpsData csvData = new GpsData();

            csvData.Latitude = Convert.ToDouble(values[0]);
            csvData.Longitude = Convert.ToDouble(values[1]);
            csvData.GpsTime = Convert.ToDateTime(values[2]);
            csvData.Speed = Convert.ToInt32(values[3]);
            csvData.Angle = Convert.ToInt32(values[4]);
            csvData.Altitude = Convert.ToInt32(values[5]);
            csvData.Satellites = Convert.ToInt32(values[6]);

            return csvData;
        }
        
    }
}

