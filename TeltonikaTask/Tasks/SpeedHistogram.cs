using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeltonikaTask.Models;

namespace TeltonikaTask.Tasks
{
    class SpeedHistogram
    {
        public void PrintSpeedHistogram(List<GpsData> gpsData)
        {
            int speedMaxNum = gpsData.Max(x => x.Speed); //302
            int speedIntervals = (int)Math.Ceiling((double)speedMaxNum / 10); //34

            int[] speedIntervalHits = new int[speedIntervals];

            for (int i = 0; i < speedIntervals; i++)
            {
                int minInterval = 10 * i;
                speedIntervalHits[i] = gpsData.Where(x => x.Speed < minInterval + 10 && x.Speed > minInterval).Count();
            }

            int maxSpeedHits = speedIntervalHits.Max(); //3360

            //Getting speed interval ratio's
            double[] speedIntervalHitRatio = new double[speedIntervals];

            for (int i = 0; i < speedIntervals; i++)
            {
                speedIntervalHitRatio[i] = (double)speedIntervalHits[i] / maxSpeedHits * 100;
                speedIntervalHitRatio[i] = (Math.Ceiling(speedIntervalHitRatio[i] / 5.0));
            }

            DrawingHistogram(speedIntervals, speedIntervalHits, speedIntervalHitRatio);

            Console.ReadLine();
        }

        private static void DrawingHistogram(int speedIntervals, int[] speedIntervalHits, double[] speedIntervalHitRatio)
        {
            Console.WriteLine();
            Console.WriteLine("Speed historgram -------------------: hits");
            for (int i = 0; i < speedIntervals; i++)
            {
                int minInterval = 10 * i;

                if (minInterval < 10)
                    Console.Write("[  " + minInterval + " -   " + (minInterval + 9) + "]  :  ");
                else if (minInterval <= 99)
                    Console.Write("[ " + minInterval + " -  " + (minInterval + 9) + "]  :  ");
                else
                    Console.Write("[" + minInterval + " - " + (minInterval + 9) + "]  :  ");

                int n = Convert.ToInt32(speedIntervalHitRatio[i]);

                Console.Write(new string('*', n));
                Console.Write(new string(' ', 20 - n));
                Console.Write(": " + speedIntervalHits[i]);

                Console.WriteLine();
            }

            Console.WriteLine();
            /*Data check
            for (int i = 0; i < speedIntervals; i++)
            {
                Console.WriteLine(9 * i + " - " + ((9 * i) + 9) + " === " + speedIntervalHits[i] + " === " + speedIntervalHitRatio[i]);
            } */
        }
    }
}
