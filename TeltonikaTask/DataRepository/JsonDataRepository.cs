using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeltonikaTask.Models;

namespace TeltonikaTask.DataRepository
{
    public class JsonDataRepository
    {
        public List<GpsData> LoadData(string fileName)
        {
            List<GpsData> gpsData;
            using (var reader = new StreamReader(fileName))
            {
                var jsonString = reader.ReadToEnd();
                gpsData = JsonConvert.DeserializeObject<List<GpsData>>(jsonString);
            }
            return gpsData;
        }
    }
}
