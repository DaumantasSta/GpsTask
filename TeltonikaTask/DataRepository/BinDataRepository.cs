using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeltonikaTask.Models;

namespace TeltonikaTask.DataRepository
{
    class BinDataRepository
    {
        public List<GpsData> LoadData(string fileName)
        {
            List<GpsData> gpsData = new List<GpsData>();

            using var fileStream = File.OpenRead(fileName);
            using var binaryReader = new BinaryReader(fileStream);

            double records = fileStream.Length / (4 + 4 + 8 + 2 + 2 + 2 + 1);

            for (int i = 0; i < records; i++)
            {
                var lat = BinaryPrimitives.ReadInt32BigEndian(binaryReader.ReadBytes(4)) / 10000000.0;
                var lon = BinaryPrimitives.ReadInt32BigEndian(binaryReader.ReadBytes(4)) / 10000000.0;

                var unixTime = BinaryPrimitives.ReadInt64BigEndian(binaryReader.ReadBytes(8));
                DateTime date = DateTimeOffset.FromUnixTimeMilliseconds(unixTime).UtcDateTime;

                var speed = BinaryPrimitives.ReadInt16BigEndian(binaryReader.ReadBytes(2));
                var angle = BinaryPrimitives.ReadInt16BigEndian(binaryReader.ReadBytes(2));
                var alt = BinaryPrimitives.ReadInt16BigEndian(binaryReader.ReadBytes(2));
                var satellites = binaryReader.Read();

                gpsData.Add(new GpsData
                {
                    Latitude = lat,
                    Longitude = lon,
                    GpsTime = date,
                    Speed = speed,
                    Angle = angle,
                    Altitude = alt,
                    Satellites = satellites
                }) ;

            }

            fileStream.Close();
            return gpsData;
        }
    }
}
