using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceListService.Models
{
    public class RaceCalc
    {

        public static double CalculatePredicion(double distances, double oldDistance, int oldtime)
        {
            return (calcPredictedTime(oldDistance, Convert.ToDouble(distances), oldtime) + cameron(oldDistance, Convert.ToDouble(distances), oldtime)) / 2;
        }

        public static string formatTime(int result)
        {
            TimeSpan t = TimeSpan.FromSeconds(result);

            string resultString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);


            return resultString;
        }

        public static double cameron(double old_dist, double next_distance, double old_time)
        {
            //a = 13.49681 - (0.000030363 * old_dist) + (835.7114 / (old_dist ^ 0.7905))
            //b = 13.49681 - (0.000030363 * new_dist) + (835.7114 / (new_dist ^ 0.7905))
            //new_time = (old_time / old_dist) * (a / b) * new_dist

            // x = (0.000030363 * old_dist)
            double x = (0.000030363 * old_dist);
            // y = (old_dist ^ 0.7905)
            double y = Math.Pow(old_dist, 0.7905);
            // z = (835.7114 / y )
            double z = 835.7114 / y;
            double a = 13.49681 - x + z;

            double w = Math.Pow(next_distance, 0.7905);
            double v = 835.7114 / w;
            double t = 0.000030363 * next_distance;


            double b = 13.49681 - t + v;

            double new_time = (old_time / old_dist) * (a / b) * next_distance;
            return new_time;

        }

        // T2=T1×(D2÷D1)1.06
        //T1 is the time achieved for D1.
        //T2 is the time predicted for D2.
        //D1 is the distance over which the initial time is achieved.
        //D2 is the distance for which the time is to be predicted.

        public static double calcPredictedTime(double old_distance, double next_distance, int old_time)
        {
            double con = 1.06;
            //t2 = t1 * (d2 / d1) ^ 1.06
            // d3 = (d2 / d1)
            double d3 = next_distance / old_distance;
            // w = d3^ 1.06
            double w = Math.Pow(d3, con);
            // t2 = t1 * w
            double new_time = old_time * w;
            return new_time;
        }
    }
}