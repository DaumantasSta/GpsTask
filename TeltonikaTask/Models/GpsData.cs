using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeltonikaTask.Models
{
    public class GpsData
    {
        public double Latitude;
        public double Longitude;
        public DateTime GpsTime;
        public int Speed;
        public int Angle;
        public int Altitude;
        public int Satellites;
    }
}
