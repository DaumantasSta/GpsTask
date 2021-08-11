using Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeltonikaTask.Models;

namespace TeltonikaTask.Tasks
{
    class RoadSection
    {

        string startPosition;
        string startGpsTimeDate;
        string endPosition;
        string endGpsTimeDate;
        double totalTime;
        double totalDistanceDriven;
        string avgRoadSpeed;

        List<RoadSection> roadSection = new List<RoadSection>();

        public void FindFasterRoadSection(List<GpsData> gpsData)
        {

            double sequenceStartLat = 0.0;
            double sequenceStartLon = 0.0;
            double sequenceEndLat = 0.0;
            double sequenceEndLon = 0.0;
            double sequenceDistanceDriven = 0.0;
            double sequenceAvgSpeed = 0.0;
            DateTime startGpsTime = gpsData[0].GpsTime;
            DateTime endGpsTime;
            int sequenceNum = 0;
            int sequenceMin = 300; //If theres record with no satelites, how much time of error (in seconds) can it be given before making it end of the sequence

            for (int i = 0; i < gpsData.Count(); i++)
            {

                if (gpsData[i].Satellites > 0)
                {
                    if (sequenceNum == 0)
                    {
                        sequenceStartLat = gpsData[i].Latitude;
                        sequenceStartLon = gpsData[i].Longitude;
                        startGpsTime = gpsData[i].GpsTime;
                    }
                    else
                    {
                        sequenceDistanceDriven = sequenceDistanceDriven + GeoCalculator.GetDistance(gpsData[i - 1].Latitude, gpsData[i - 1].Longitude, gpsData[i].Latitude, gpsData[i].Longitude, 2, DistanceUnit.Kilometers);
                        sequenceAvgSpeed = sequenceAvgSpeed + gpsData[i].Speed;
                    }
                    sequenceNum++;
                }
                else if (sequenceNum > 0 && gpsData[i].Satellites == 0 && gpsData[i].GpsTime.Subtract(gpsData[i - 1].GpsTime).TotalSeconds > sequenceMin)
                {
                    //Ending sequence
                    sequenceEndLat = gpsData[i - 1].Latitude;
                    sequenceEndLon = gpsData[i - 1].Longitude;
                    endGpsTime = gpsData[i - 1].GpsTime;
                    sequenceAvgSpeed = sequenceAvgSpeed / sequenceNum;
                    double sequenceTotalTime = endGpsTime.Subtract(startGpsTime).TotalSeconds;
                    
                    CollectAndFilterSequences(roadSection, sequenceStartLat, sequenceStartLon, sequenceAvgSpeed, startGpsTime, endGpsTime, sequenceTotalTime, sequenceDistanceDriven);

                    //Reseting values for next sequence
                    sequenceDistanceDriven = 0;
                    sequenceNum = 0;
                }
            }

            PrintFastestRoadSectionData(roadSection);
            Console.ReadLine();

        }

        private static void PrintFastestRoadSectionData(List<RoadSection> roadSection)
        {
            int index = roadSection.IndexOf(roadSection.First(x => x.totalTime == 
                roadSection.Min(prop => prop.totalTime)));

            Console.WriteLine();
            Console.WriteLine("Fastest road section of at least 100km was driven over " + roadSection[index].totalTime + ",000s and was " + roadSection[index].totalDistanceDriven + "km long.");
            Console.WriteLine("Start position " + roadSection[index].startPosition);
            Console.WriteLine("Start gps time " + roadSection[index].startGpsTimeDate);
            Console.WriteLine("End position " + roadSection[index].endPosition);
            Console.WriteLine("End gps time " + roadSection[index].endGpsTimeDate);
            Console.WriteLine("Average speed: " + roadSection[index].avgRoadSpeed + "km/h");
            Console.WriteLine();
        }

        private static void CollectAndFilterSequences(List<RoadSection> roadSection, double startLatitude, double startLongitude, double avgSpeed, DateTime? startGpsTime, DateTime endGpsTime, double timePassed, double distance)
        {
            if (distance > 100)
                roadSection.Add(new RoadSection { startPosition = startLatitude + "; " + startLongitude, startGpsTimeDate = startGpsTime.ToString(), endPosition = startLatitude + "; " + startLongitude, endGpsTimeDate = endGpsTime.ToString(), avgRoadSpeed = avgSpeed.ToString(), totalTime = timePassed, totalDistanceDriven = distance });
        }

    }
}
