using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FeedTheSnake
{
    public static class ExtensionHelper
    {
        /// <summary>
        /// Distance between points
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Distance(this Point a, Point b)
        {
            return Point.Subtract(a, b).Length;
        }
        /// <summary>
        /// moves point a towards point b by a given distance
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Point MoveTowards(this Point a, Point b, double distance)
        {
            var normal = Point.Subtract(b, a);
            normal.Normalize();
            
            return a + (normal * distance);
        }

    }
}
