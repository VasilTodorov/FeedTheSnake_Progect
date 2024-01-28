using System.Collections.ObjectModel;
using System.Net;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FeedTheSnake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point headPoint;
        
        Ellipse head;
        Polyline body;

        int length = 7;
        double segment;
        //Snake snake;
        public MainWindow()
        {
            InitializeComponent();

            segment = 20;
            body = new Polyline();

            body.Points.Add(new Point(0, 0 + 40));
            body.Points.Add(new Point(segment, segment+40));
            body.Points.Add(new Point(segment*2, 0 + 40));
            body.Points.Add(new Point(segment * 3, segment + 40));
            body.Points.Add(new Point(segment * 4, 0 + 40));
            body.Points.Add(new Point(segment * 5, segment + 40));
            body.Points.Add(new Point(segment * 6, 0 + 40));

            body.Stroke = Brushes.Green;
            body.StrokeThickness = 20;
            body.StrokeStartLineCap = PenLineCap.Round;
            body.StrokeEndLineCap = PenLineCap.Round;
            

            headPoint = body.Points.Last();
            head = new Ellipse();
            head.Width = segment * 2;
            head.Height = segment*2;
            head.Fill = Brushes.Green;
            
            Canvas.SetTop(head, headPoint.Y-head.Height/2);
            Canvas.SetLeft(head, headPoint.X-head.Width/2);

            //Rectangle rec = new Rectangle();
            

            paintCanvas.Children.Add(body);
            paintCanvas.Children.Add(head);
        }

        private void paintCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            length++;
            
        }

        private void paintCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            moveSnakeBody(e.GetPosition(paintCanvas));


        }

        private double Distance(Point a, Point b)
        {
            return Math.Sqrt( (a.X - b.X)* (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        private void moveSnakeBody(Point goal)
        {
            headPoint = goal;
            Canvas.SetTop(head, headPoint.Y - head.Height / 2);
            Canvas.SetLeft(head, headPoint.X - head.Width / 2);

            if (Distance(body.Points.Last(), headPoint) > segment)
            {
                body.Points.Add(headPoint);
                if (body.Points.Count > length)
                { body.Points.RemoveAt(0); }

            }
        }
    }
}