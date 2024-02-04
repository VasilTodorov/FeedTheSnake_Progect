using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FeedTheSnake
{
    /// <summary>
    /// A struct which gives the cordinates of an Rectangaler obstacle
    /// without rotation
    /// and also gives methods to work with them.
    /// </summary>
    public struct Obstacle
    {
        public Point Center;
        public double Width;
        public double Height;
        
        public Obstacle(Point center, double width, double height)
        {
            this.Center = center;
            this.Width = width;
            this.Height = height;
            
        }
        /// <summary>
        /// Checks if the obstacle is crossed by a circle with a center="circleCenter" and radius="circleRadius"
        /// where 0 < "coeficent" <= 1 is how much of the radius will be counted for crossed 
        /// 1 means the whole circleRadius, and 0.4 only 0.4 * circleRadius
        /// </summary>
        /// <param name="circleCenter"></param>
        /// <param name="circleRadius"></param>
        /// <param name="coeficent"></param>
        /// <returns></returns>
        public bool IsCircleIntersecting(Point circleCenter, double circleRadius, double coeficent)
        {
            coeficent = coeficent <= 0 || coeficent > 1 ? 1 : coeficent;
            circleRadius = coeficent * circleRadius;
            // Calculate half-width and half-height of the rectangle
            double halfRectWidth = Math.Abs(this.Width) / 2;
            double halfRectHeight = Math.Abs(this.Height) / 2;

            // Calculate the distance between the centers of the rectangle and the circle
            double dx = Math.Abs(circleCenter.X - this.Center.X);
            double dy = Math.Abs(circleCenter.Y - this.Center.Y);

            // Check if the distance is within the rectangle's half-width and half-height
            if ((dx <= halfRectWidth + circleRadius && dy <= halfRectHeight) ||
                (dx <= halfRectWidth && dy <= halfRectHeight + circleRadius)) 
            {
                // The rectangle and circle intersect
                return true;
            }

            // Check if the circle is inside the rectangle
            if (dx <= halfRectWidth && dy <= halfRectHeight)
            {
                // The circle is inside the rectangle
                return true;
            }

            // Check if the circle intersects with any of the rectangle's corners
            double cornerDistanceSquared = Math.Pow(dx - halfRectWidth, 2) + Math.Pow(dy - halfRectHeight, 2);

            return cornerDistanceSquared <= Math.Pow(circleRadius, 2);
        }
        
        #region LevelObstacles
        /// <summary>
        /// Level zero obstacles for the snake, which are only the walls
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static List<Obstacle> LevelZero(double width, double height, double unit)
        {

            Obstacle a1 = new Obstacle(new Point(width / 2, 0 + unit / 4), width, unit / 2);
            Obstacle a2 = new Obstacle(new Point((width / 2), height - unit / 4), width, unit / 2);
            Obstacle a3 = new Obstacle(new Point(0 + unit / 4, height / 2), unit / 2, height);
            Obstacle a4 = new Obstacle(new Point(width - unit / 4, height / 2), unit / 2, height);

            var zero = new List<Obstacle>();
            zero.Add(a1);
            zero.Add(a2);
            zero.Add(a3);
            zero.Add(a4);

            return zero;
        }
        /// <summary>
        /// Level one obstacles for the snake
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static List<Obstacle> LevelOne(double width, double height, double unit)
        {

            Obstacle a1 = new Obstacle(new Point(width * (1.0 / 3), height / 2), unit, unit * 3);
            Obstacle a2 = new Obstacle(new Point(width * (2.0 / 3), height / 2), unit, unit * 3);
            //Obstacle a3 = new Obstacle(new Point(0, height / 2), unit, height);
            //Obstacle a4 = new Obstacle(new Point(width, height / 2), unit, height);

            var one = new List<Obstacle>(LevelZero(width, height, unit));//LevelZero(width, height, unit)
            one.Add(a1);
            one.Add(a2);

            return one;
        }
        /// <summary>
        /// Level two obstacles for the snake
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static List<Obstacle> LevelTwo(double width, double height, double unit)
        {

            Obstacle a1 = new Obstacle(new Point(width * (1.0 / 5), height * (1.0 / 4)), unit * 2, unit * 2);
            Obstacle a2 = new Obstacle(new Point(width * (1.0 / 5), height * (3.0 / 4)), unit * 2, unit * 2);
            Obstacle a3 = new Obstacle(new Point(width * (4.0 / 5), height * (1.0 / 4)), unit * 2, unit * 2);
            Obstacle a4 = new Obstacle(new Point(width * (4.0 / 5), height * (3.0 / 4)), unit * 2, unit * 2);

            var two = new List<Obstacle>(LevelOne(width, height, unit));//LevelZero(width, height, unit)
            two.Add(a1);
            two.Add(a2);
            two.Add(a3);
            two.Add(a4);

            return two;
        }
        /// <summary>
        /// Level three obstacles for the snake
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static List<Obstacle> LevelThree(double width, double height, double unit)
        {

            Obstacle a1 = new Obstacle(new Point(width / 2, height / 2), unit * 4, unit * 4);

            var three = new List<Obstacle>(LevelTwo(width, height, unit));//LevelZero(width, height, unit)
            three.Add(a1);


            return three;
        } 
        #endregion
    }
    
}
