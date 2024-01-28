using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FeedTheSnake
{
    public class Snake
    {
		//const double HEAD_DIAMETER = 15;
        //const double BODY_THICKNNSS = 4;

        private int length;
		private double segment;
        private double headDiameter;
        private PointCollection body;
        public Snake(PointCollection body, double segment)
        {
            Body = body;
            Segment = segment;
            HeadDiameter = Segment*2;
            
            //PaintCanvas = paintCanvas;
            Length = body.Count;
            if(body.Count == 0)
            {
                HeadPoint = new Point(10, 10);
            }
            else
            {
                HeadPoint = body.Last();
            }
        }

        public Snake(PointCollection body):this(body, 12)
        {
            
        }


        //public Canvas PaintCanvas { get; init; }
        //public Ellipse Head { get; set; }
        public PointCollection Body { get; set; }
        public Point HeadPoint { get; set; }          
        public double Segment 
		{ 
			get
			{
				return segment;
			}
			init
			{
				segment = value >= 1 ? value : 1;
            }
		} 
        public double HeadDiameter
        {
            get { return headDiameter; }
            set { headDiameter = value >= Segment * 2 ? value : Segment * 2; }
        }
        public int Length
		{
			get { return length; }
			private set { length = value >= 3 ? value : 3; }
		}
        
        public void Move(Point goal)
        {

        }

        private bool isValidMove()
        {
            return !(this.IsEatingItself() || this.isHittingTheWall());
        }

        private bool IsEatingItself()
        {
            return false;
        }

        private bool isHittingTheWall() {
            return false;
        }
    }
}
