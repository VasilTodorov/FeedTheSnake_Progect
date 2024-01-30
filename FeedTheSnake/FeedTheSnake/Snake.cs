using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// class DrawSnake illustrates the drawing
    /// and fuctionality of the snake in the game
    /// feed the snake
    /// </summary>
    public class DrawSnake
    {
        private const double SEGMENT = 20;//TODO can make it const
        private bool susspended = true;

        private Canvas paintCanvas;
        private Polyline body;
        private Ellipse head;

        private int length;
		

        /// <summary>
        /// Default constructor 
        /// </summary>
        /// <param name="paintCanvas"></param>
        public DrawSnake(Canvas paintCanvas)
        {          
            //init variables
            this.paintCanvas = paintCanvas;            
            body = new Polyline();
            head = new Ellipse();
        
            head.MouseLeftButtonDown += (x, y) => { susspended = false; };

            DefaultSnake();
         
            paintCanvas.Children.Add(body);
            paintCanvas.Children.Add(head);
        }
      
        //public void HeadSelection(MouseButtonEventHandler a)
        //{
        //    head.MouseLeftButtonDown += a;
        //}
        /// <summary>
        /// The length of the snake cant never be less than 3
        /// </summary>
        public int Length 
        {
            get => length;
            set => length = value >= 3 ? value : 3 ; 
        }
        /// <summary>
        /// Moves DrawSnake to a given point
        /// </summary>
        /// <param name="goal"></param>
        public void Move(Point goal)
        {            
            if (susspended) { return; }           
            
            //the end of the snake body weher the head will be
            Point headBodyPoint = body.Points.Last();           

            //the snake moves only one SEGMENT at a time
            //by adding point at the end and removing at the beginig of body
            if (headBodyPoint.Distance(goal) > SEGMENT)
            {
                //the added new point is at a distance "SEGMENT" from "headBodyPoint"
                //towards goal point
                body.Points.Add(headBodyPoint.MoveTowards(goal, SEGMENT));

                //removes points from snake tail untill length == body.Points.Count
                //this way we show the new lemgth of the snake
                while (body.Points.Count > length)
                { body.Points.RemoveAt(0); }

                //And repeat untill the distance is less than SEGMENT
                
                Move(goal);
            }

            Canvas.SetTop(head, goal.Y - head.Height / 2);
            Canvas.SetLeft(head, goal.X - head.Width / 2);
        }

        public bool isValidMove()
        {
            return !(this.IsEatingItself() || this.isHittingTheWall());
        }

        private bool IsEatingItself()
        {
            //eating itself moves 
            //back move
            //body eat
            //tail eat


            //Point headPoint = new Point(Canvas.GetLeft(head), Canvas.GetTop(head));
            ////Point headPoint = body.Points.Last();
            ////Bug when it its
            ///
            // body
            //for (int i = 1; i < body.Points.Count - 3; i++)
            //{
            //    if (body.Points[i].Distance(headPoint) < SEGMENT)
            //        return true;
            //}

            //tail 
            //if (body.Points[0].Distance(headPoint) < SEGMENT)
            //    return true;
            return false;
        }

        private bool isHittingTheWall() {
            return false;
        }
        /// <summary>
        /// Sets DrawSnake body and head default values
        /// also the length of body
        /// </summary>
        public void DefaultSnake()
        {
            //clears old body
            body.Points.Clear();

            //sets default cordinates for body
            body.Points.Add(new Point(0, 0 + 40));
            body.Points.Add(new Point(SEGMENT, SEGMENT + 40));
            body.Points.Add(new Point(SEGMENT * 2, 0 + 40));
            body.Points.Add(new Point(SEGMENT * 3, SEGMENT + 40));
            body.Points.Add(new Point(SEGMENT * 4, SEGMENT + 40));
            body.Points.Add(new Point(SEGMENT * 5, SEGMENT + 40));
            body.Points.Add(new Point(SEGMENT * 6, SEGMENT + 40));

            //sets field length of body new value
            length = body.Points.Count;

            //sets default properties for body 
            body.Stroke = Brushes.Green;          
            body.StrokeThickness = SEGMENT;
            //body.StrokeStartLineCap = PenLineCap.Round;
            body.StrokeEndLineCap = PenLineCap.Round;

            //sets default properties for head           
            head.Width = SEGMENT * 2;
            head.Height = SEGMENT * 2;
            head.Fill = Brushes.Green;
            
            //sets head position
            Point headPoint = body.Points.Last();
            Canvas.SetTop(head, headPoint.Y - head.Height / 2);
            Canvas.SetLeft(head, headPoint.X - head.Width / 2);

        }
    }
}
