using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FeedTheSnake
{
    //Comment: I would like an interface. ISnake

    /// <summary>
    /// class Snake illustrates the drawing
    /// and fuctionality of the snake in the game
    /// feed the snake
    /// </summary>
    class Snake : INotifyPropertyChanged
    {      

        private double segment;
        private int length;
        private Queue<Point>? body;
        private Point bodyEndPoint;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Snake(double segment)
        {
            Segment = segment;
            Body = new Queue<Point>();
            Reset();
        }

        public Snake() : this(20)
        { }
       

        public Point HeadPoint { get;private set; }
        
        public double Segment 
        {
            get => segment;
            private set => segment = value>=1?value:1;
        }
        public int Length 
        {
            get => length;
            // comment: I don't like having a public setter for length. Snake should have a method eat
            set 
            { 
                length = value >= 3 ? value : 3;
                OnPropertyChanged("Length");
            }
        }

        public Queue<Point> Body
        {
            get
            {              
                return new Queue<Point>(body!);
            }
            private set
            {
                body = value!=null?value:new Queue<Point>();
            }
            
        }


        
      
        
        public void Reset()
        {
            Segment = segment;
            body!.Clear();

            body.Enqueue(new Point(Segment, Segment*2));
            body.Enqueue(new Point(Segment * 2, Segment*2));
            body.Enqueue(new Point(Segment * 3, Segment*2));
            length = Body.Count;
            HeadPoint = this.body!.Last();
            bodyEndPoint = HeadPoint;
        }
        
        /// <summary>
        /// Moves DrawSnake to a given point
        /// </summary>
        /// <param name="goal"></param>
        public void Move(Point goal)
        {            
                     
            
            //the snake moves only one SEGMENT at a time
            //by adding point at the end and removing at the beginig of body
            while (bodyEndPoint.Distance(goal) > Segment)
            {
                //the added new point is at a distance "SEGMENT" from "headBodyPoint"
                //towards goal point
                bodyEndPoint = bodyEndPoint.MoveTowards(goal, Segment);
                body!.Enqueue(bodyEndPoint);

                
                //removes points from snake tail untill length == body.Points.Count
                //this way we show the new lemgth of the snake
                while (body.Count > length)
                { body.Dequeue(); }

                //And repeat untill the distance is less than SEGMENT
               //if(!isValidMove())
               // {
               //     susspended = true;
               //     return;
               // }
            }
            HeadPoint = goal;
            
        }


        public bool IsEatingItself()
        {
            

            Queue<Point> examPoints = new Queue<Point>(body!);
            while (examPoints.Count > 3) 
            {
                var currentPoint = examPoints.Dequeue();
                if(currentPoint.Distance(HeadPoint) < Segment)
                {
                    return true;
                }
            }
            //examPoints.Dequeue();
            return false;
        }

        // Comments: Don't like this coeficient here, other people are not expecting to know stuf like that. I would add it as a constant
        public bool IsHittingObstacle(List<Obstacle> obstacles, double coeficent) {

            foreach(var obstacle in obstacles)
            {
                if (obstacle.IsCircleIntersecting(HeadPoint, Segment, coeficent))
                    return true;
            }
            return false;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if(propertyName.Equals(nameof(Length)))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
    }
}
