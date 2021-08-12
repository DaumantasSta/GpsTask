using Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeltonikaTask.Models;

namespace TeltonikaTask.Tasks
{
    class RoadSections
    {

        private string _startPosition;
        private string _startGpsTimeDate;
        private string _endPosition;
        private string _endGpsTimeDate;
        private double _totalTime;
        private double _totalDistanceDriven;
        private string _avgRoadSpeed;

        List<RoadSections> roadSections = new List<RoadSections>();

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
                        //Start of sequence
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
                    
                    CollectAndFilterSequences(roadSections, sequenceStartLat, sequenceStartLon, sequenceAvgSpeed, startGpsTime, endGpsTime, sequenceTotalTime, sequenceDistanceDriven);

                    //Reseting values for next sequence
                    sequenceDistanceDriven = 0;
                    sequenceNum = 0;
                }
            }

            PrintFastestRoadSectionData(roadSections);
            Console.ReadLine();

        }

        private static void PrintFastestRoadSectionData(List<RoadSections> roadSection)
        {
            int index = roadSection.IndexOf(roadSection.First(x => x._totalTime == 
                roadSection.Min(prop => prop._totalTime)));

            Console.WriteLine();
            Console.WriteLine("Fastest road section of at least 100km was driven over " + roadSection[index]._totalTime +
                ",000s and was " + roadSection[index]._totalDistanceDriven + "km long.");
            Console.WriteLine("Start position " + roadSection[index]._startPosition);
            Console.WriteLine("Start gps time " + roadSection[index]._startGpsTimeDate);
            Console.WriteLine("End position " + roadSection[index]._endPosition);
            Console.WriteLine("End gps time " + roadSection[index]._endGpsTimeDate);
            Console.WriteLine("Average speed: " + roadSection[index]._avgRoadSpeed + "km/h");
            Console.WriteLine();
        }

        private static void CollectAndFilterSequences(ICollection<RoadSections> roadSection, double startLatitude, double startLongitude, double avgSpeed, DateTime? startGpsTime, DateTime endGpsTime, double timePassed, double distance)
        {
            if (distance > 100)
            {
                roadSection.Add(new RoadSections
                {
                    _startPosition = startLatitude + "; " + startLongitude,
                    _startGpsTimeDate = startGpsTime.ToString(),
                    _endPosition = startLatitude + "; " + startLongitude,
                    _endGpsTimeDate = endGpsTime.ToString(),
                    _avgRoadSpeed = Math.Round(avgSpeed, 1).ToString(),
                    _totalTime = timePassed,
                    _totalDistanceDriven = Math.Round(distance, 3)
                });
            }
        }
    }
}
