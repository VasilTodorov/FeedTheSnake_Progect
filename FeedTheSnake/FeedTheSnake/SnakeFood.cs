using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace FeedTheSnake
{
    class SnakeFood
    {
        private static Random ran = new Random();
        public SnakeFood(Brush color, Point position)
        {
            Color = color;           
            Position = position;
        }

        public SnakeFood(Point position) 
            : this(new SolidColorBrush(System.Windows.Media.Color.FromArgb(
                    (byte)255, (byte)(ran.NextDouble()*255),
                    (byte)(ran.NextDouble() * 255), (byte)(ran.NextDouble() * 255))), position)
            {}
        public Point Position { get; set; }
        public Brush Color { get; set; }
    }
}
