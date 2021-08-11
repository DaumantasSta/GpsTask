using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeltonikaTask.Models;

namespace TeltonikaTask.Tasks
{
    public class SatellitesHistogram
    {
        public void PrintSatellitesHistogram(List<GpsData> gpsData)
        {

            var satNumComb = gpsData.Select(x => x.Satellites).Distinct().ToArray();
            Array.Sort(satNumComb);

            //Geting hits quantity of certain satellites
            int[] satNumHits = new int[satNumComb.Count()];

            for (int i = 0; i < satNumComb.Count(); i++)
            {
                satNumHits[i] = gpsData.Count(x => x.Satellites == satNumComb[i]);
            }

            int maxSatHitsValue = satNumHits.Max();

            double[] satNumHitRatio = new double[satNumHits.Count()];
            for (int i = 0; i < satNumComb.Count(); i++)
            {
                satNumHitRatio[i] = (double)satNumHits[i] / maxSatHitsValue * 100;
                satNumHitRatio[i] = Math.Round(satNumHitRatio[i] / 5.0)+1;
            }

            //Printing histogram
            Console.WriteLine("                                                              " + maxSatHitsValue + "hits");
            
            Histogram(satNumHitRatio.ToList());
            //Bottom of the histogram
            foreach (int i in satNumComb)
            {
                if (i < 10) Console.Write("0" + i + " ");
                else
                    Console.Write(i + " ");
            }

            Console.WriteLine();
            Console.ReadLine();
        }

        private static void Histogram(List<double> list)
        {
            if (list.All(x => x <= 0))
                return;
            else
            {
                var _list =
                    list.Select(x => x - 1).ToList();
                Histogram(_list);

                var toPrint = "";
                foreach (var n in list)
                {
                    if (n > 0)
                        toPrint += " - ";
                    else
                        toPrint += "   ";
                }

                Console.WriteLine(toPrint);
            }
        }
    }
}
