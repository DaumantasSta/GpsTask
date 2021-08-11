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

        public void FindFasterRoadSection(List<GpsData> gpsData)
        {
            List<RoadSection> roadSection = new List<RoadSection>();

            double startLatitude = 0.0;
            double startLongitude = 0.0;
            double endLatitude = 0.0;
            double endLongitude = 0.0;
            double distanceDriven = 0.0;
            double avgSpeed = 0.0;
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
                        startLatitude = gpsData[i].Latitude;
                        startLongitude = gpsData[i].Longitude;
                        startGpsTime = gpsData[i].GpsTime;
                    }
                    else
                    {
                        distanceDriven = distanceDriven + GeoCalculator.GetDistance(gpsData[i - 1].Latitude, gpsData[i - 1].Longitude, gpsData[i].Latitude, gpsData[i].Longitude, 2, DistanceUnit.Kilometers);
                        avgSpeed = avgSpeed + gpsData[i].Speed;
                    }
                    sequenceNum++;
                }
                else if (sequenceNum > 0 && gpsData[i].Satellites == 0)
                {
                    if (gpsData[i].GpsTime.Subtract(gpsData[i - 1].GpsTime).TotalSeconds > sequenceMin)
                    {
                        //Ending sequence
                        endLatitude = gpsData[i - 1].Latitude;
                        endLongitude = gpsData[i - 1].Longitude;
                        endGpsTime = gpsData[i - 1].GpsTime;
                        avgSpeed = avgSpeed / sequenceNum;
                        DateTime dateee = startGpsTime;
                        double time = endGpsTime.Subtract(startGpsTime).TotalSeconds;

                        if (distanceDriven > 100)
                            SelectedRoads(roadSection, startLatitude, startLongitude, avgSpeed, startGpsTime, endGpsTime, time, distanceDriven);

                        //Reseting values for next sequence
                        distanceDriven = 0;
                        sequenceNum = 0;
                    }
                }
            }

            PrintFasterRoadSectionData(roadSection);

            Console.ReadLine();

        }

        private static void PrintFasterRoadSectionData(List<RoadSection> roadSection)
        {
            int index = roadSection.IndexOf(roadSection.First(x => x.totalTime == roadSection.Min(prop => prop.totalTime)));

            Console.WriteLine("Fastest road section of at least 100km was driven over " + roadSection[index].totalTime + ",000s and was " + roadSection[index].totalDistanceDriven + "km long.");
            Console.WriteLine("Start position " + roadSection[index].startPosition);
            Console.WriteLine("Start gps time " + roadSection[index].startGpsTimeDate);
            Console.WriteLine("End position " + roadSection[index].endPosition);
            Console.WriteLine("End gps time " + roadSection[index].endGpsTimeDate);
            Console.WriteLine("Average speed: " + roadSection[index].avgRoadSpeed + "km/h");
            Console.WriteLine();
        }

        private static void SelectedRoads(List<RoadSection> roadSection, double startLatitude, double startLongitude, double avgSpeed, DateTime? startGpsTime, DateTime endGpsTime, double timePassed, double distance)
        {
            roadSection.Add(new RoadSection { startPosition = startLatitude + "; " + startLongitude, startGpsTimeDate = startGpsTime.ToString(), endPosition = startLatitude + "; " + startLongitude, endGpsTimeDate = endGpsTime.ToString(), avgRoadSpeed = avgSpeed.ToString(), totalTime = timePassed, totalDistanceDriven = distance });
        }

    }
}
