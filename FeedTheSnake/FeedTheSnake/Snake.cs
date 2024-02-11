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
    /// <summary>
    /// class Snake illustrates the logic
    /// behind the snake in the game feedTheSnake
    /// </summary>
    class Snake : INotifyPropertyChanged
    {

        #region Fields
        private double segment;
        private int length;
        private Queue<Point>? body;
        private Point bodyEndPoint;
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        #region Consructors
        public Snake(double segment)
        {
            Segment = segment;
            Body = new Queue<Point>();
            Reset(20);
        }

        public Snake() : this(20)
        { }

        #endregion

        #region Properties
        /// <summary>
        /// The actual position of the snake head
        /// </summary>
        public Point HeadPoint { get; private set; }
        /// <summary>
        /// The distance between the Points of the snake which makes up its body
        /// </summary>
        public double Segment
        {
            get => segment;
            private set => segment = value >= 1 ? value : 1;
        }
        /// <summary>
        /// The number of Points which make up the body
        /// </summary>
        public int Length
        {
            get => length;
            set
            {
                length = value >= 3 ? value : 3;
                OnPropertyChanged("Length");
            }
        }
        /// <summary>
        /// The Points in the body structurate in Queue 
        /// </summary>
        public Queue<Point> Body
        {
            get
            {
                return new Queue<Point>(body!);
            }
            private set
            {
                body = value != null ? value : new Queue<Point>();
            }

        }
        #endregion

        #region Methods
        /// <summary>
        /// Resets all Points in default Positons relative to unit, where Segment = unit
        /// </summary>
        /// <param name="unit"></param>
        public void Reset(double unit)
        {
            Segment = unit;
            body!.Clear();

            body.Enqueue(new Point(Segment, Segment * 2));
            body.Enqueue(new Point(Segment * 2, Segment * 2));
            body.Enqueue(new Point(Segment * 3, Segment * 2));
            length = Body.Count;
            HeadPoint = this.body!.Last();
            bodyEndPoint = HeadPoint;
        }

        /// <summary>
        /// Moves Snake to a given point
        /// </summary>
        /// <param name="goal"></param>
        public void Move(Point goal)
        {
            while (bodyEndPoint.Distance(goal) > Segment)
            {
                bodyEndPoint = bodyEndPoint.MoveTowards(goal, Segment);
                body!.Enqueue(bodyEndPoint);

                while (body.Count > length)
                { body.Dequeue(); }


            }
            HeadPoint = goal;
        }
        /// <summary>
        /// Checks if snake is bitting its own tail
        /// </summary>
        /// <returns></returns>
        public bool IsEatingItself()
        {


            Queue<Point> examPoints = new Queue<Point>(body!);
            while (examPoints.Count > 3)
            {
                var currentPoint = examPoints.Dequeue();
                if (currentPoint.Distance(HeadPoint) < Segment)
                {
                    return true;
                }
            }
            //examPoints.Dequeue();
            return false;
        }
        /// <summary>
        /// Checks if head of snake is hitting an obstacle
        /// </summary>
        /// <param name="obstacles"></param>
        /// <param name="coeficent"></param>
        /// <returns></returns>
        public bool IsHittingObstacle(List<Obstacle> obstacles, double coeficent)
        {

            foreach (var obstacle in obstacles)
            {
                if (obstacle.IsCircleIntersecting(HeadPoint, Segment, coeficent))
                    return true;
            }
            return false;
        }

        #endregion
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (propertyName.Equals(nameof(Length)))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
