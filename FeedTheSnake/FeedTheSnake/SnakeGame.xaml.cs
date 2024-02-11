using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for SnakeGame.xaml
    /// </summary>
    public partial class SnakeGame : UserControl,INotifyPropertyChanged
    {
        public enum GameState { ONGOING , PAUSED, OVER, PREPEARING}
        public enum GameLevel { ZERO, FIRST, SECOND, THIRD}
        
        

        private const double UNIT = 20;
        private static Random rnd = new Random();

        //private bool susspended;
        // Comment: expiration not expiretion
        private int expiretionTime;
        private int score;
        private Snake snake;
        private Polyline? body;
        private Ellipse? head;

        private FoodFarm farm;
        private FoodCollection foodStock;

        private List<Obstacle> obstacles;
        private List<Rectangle> rectangles;

        public event PropertyChangedEventHandler? PropertyChanged;

        //public bool susspended { get; private set; }
        //public bool Ongoing { get; set; }
        public int ExpiretionTime
        {
            get
            { return expiretionTime; }
            set
            { 
                expiretionTime = value >= 1 ? value : 1;
                OnPropertyChanged(nameof(ExpiretionTime));
            }
        }
        public int Score 
        { 
          get
            {
                return score;
            }
          set
            {
                score = value>=0?value:0;
                OnPropertyChanged(nameof(Score));
            }
        }
        public GameState State { get; set; }
        public GameLevel Level { get; set; }
        public SnakeGame()
        {
            InitializeComponent();

            State = GameState.PREPEARING;           
            Level = GameLevel.ZERO;           
            farm = new FoodFarm(UNIT);
            snake = new Snake(UNIT);
            // comment: why cant you body!.Points = new PointCollection(snake.Body);
            // or better yet use something like drawSnake
            body = new Polyline(); 
            head = new Ellipse();
            foodStock = new FoodCollection(farm);
            obstacles = new List<Obstacle>();
            //obstacles = Obstacle.LevelZero(snakeCanvas.ActualWidth, snakeCanvas.ActualHeight, UNIT);           
            rectangles = new List<Rectangle>();
            Score = 0;
            ExpiretionTime = 1;

            //DrawSnakeBody();
            //DrawSnakeHead();
            //DrawObstacles();

            snakeCanvas.Children.Add(body);
            snakeCanvas.Children.Add(head);

            farm.inspectionTimer.Tick += DrawFood;
            farm.inspectionTimer.Tick += EatExpireFoods;
            
            // GamePause();
            head.MouseLeftButtonDown += (x, y) => { GameStart(); };

            // Comment: This code is brittle, if we add a new property or the length lowers the score will still go up
            // I don't like that the score auto increments when length evnet comes. The scrore should just take length as the one bellow 
            // I want to do stuff only when the property length has changes
            snake.PropertyChanged += (x, y) => { Score++; };
            //Comment: this is better but still I don't like that we don't check whiich property change. I want to do stuff only when the property is ExpirationTime has changes
            farm.PropertyChanged += (x, y) => { ExpiretionTime=farm.ExpiretionTime; };
        }
        #region ClientMethods
        public void NewGame()
        {
            if (State != GameState.OVER &&
                State != GameState.PREPEARING &&
                State != GameState.PAUSED) { return; }
            
            Score = 0;
            farm.Stop();
            ClearFood();
            snake.Reset();
            DrawSnakeBody();
            DrawSnakeHead();
            TurnLevel(Level);
            DrawObstacles();
            State = GameState.PAUSED;
        }
        public void GameOver()
        {
            if (State != GameState.ONGOING) { return; }
            //susspended = true;
            farm.Stop();
            State = GameState.OVER;
        }
        public void GamePause()
        {
            if (State != GameState.ONGOING) { return; }
            //susspended = true;
            farm.Stop();
            State = GameState.PAUSED;
        }

        public void GameStart()
        {
            if (State != GameState.PAUSED) { return; }
            //susspended = false;
            farm.Start();
            State = GameState.ONGOING;
        }
        #endregion

        #region DrawingMethods

        // comment I would like a single method draw snake. The snake mkight have eyes, sunglasses, mouth. I don't wantyou to think about it
        // comment I don't like that this method has access to all the snake game objects
        private void DrawSnakeBody()
        {
            // Comment: it is a bad practice to use !, if body is null you would still get an error, you are just hiding it
            body!.Points = new PointCollection(snake.Body);
            body.StrokeThickness = snake.Segment;
            body.StrokeLineJoin = PenLineJoin.Round;
            body.StrokeEndLineCap = PenLineCap.Round;
            body.StrokeStartLineCap = PenLineCap.Triangle;
            body.Stroke = Brushes.Green;
        }

        private void DrawSnakeHead()
        {
            // Comment: it is a bad practice to use !, if Width
            head!.Width = 2 * snake.Segment;
            head.Height = 2 * snake.Segment;

            head.Fill = Brushes.Green;

            Canvas.SetLeft(head, snake.HeadPoint.X - head.Width / 2);
            Canvas.SetTop(head, snake.HeadPoint.Y - head.Height / 2);
        }

        private void DrawFood(object? sender, EventArgs e)
        {
            if(farm.CurrentTime % farm.ProductionTime != 0) { return; }
            if (farm.Capasity > foodStock.Count)
            {
                var food = new Ellipse();
                food.Width = 2 * farm.FoodRadius;
                food.Height = 2 * farm.FoodRadius;
                food.Fill = new SolidColorBrush(Color.FromArgb(
                    (byte)255, (byte)(rnd.NextDouble() * 255),
                    (byte)(rnd.NextDouble() * 255), (byte)(rnd.NextDouble() * 255)));

                var foodPosition = GetRandomCanvasPoint();

                Canvas.SetLeft(food, foodPosition.X - food.Width / 2);
                Canvas.SetTop(food, foodPosition.Y - food.Height / 2);

                Panel.SetZIndex(food, -1);

                foodStock.Add(food);
                //farm.AddFoodCenterPosition(foodPosition);

                snakeCanvas.Children.Add(food);
            }
        }

        private void DrawObstacles()
        {

            foreach (var rectangle in rectangles)
            {
                snakeCanvas.Children.Remove(rectangle);
            }
            rectangles.Clear();

            foreach (var obstacle in obstacles)
            {
                var rec = new Rectangle();
                rec.Width = obstacle.Width;
                rec.Height = obstacle.Height;
                rec.Fill = Brushes.Brown;

                Panel.SetZIndex(rec, -1);

                Canvas.SetLeft(rec, obstacle.Center.X - rec.Width / 2.0);//- rec.Width / 2
                Canvas.SetTop(rec, obstacle.Center.Y - rec.Height / 2.0);//- rec.Height / 2

                rectangles.Add(rec);
                snakeCanvas.Children.Add(rec);
            }
        }

        #endregion
        #region UtilityMethods
        private void EatFood(int index)
        {
            if (index < 0 || index >= foodStock.Count) return;

            snakeCanvas.Children.Remove(foodStock[index]);
            foodStock.RemoveAt(index);
        }

        private void EatExpireFoods(object? sender, EventArgs e)
        {
            //farm.CurrentTime++;
            while(farm.DeleteEpiredFood(EatFood, ExpiretionTime))
            { }
        }


        private void ClearFood()
        {
            foreach(var food in foodStock)
            {
                snakeCanvas.Children.Remove(food);
            }
            foodStock.Clear();
            //farm.Clear();
        }

        private void TurnLevel(GameLevel level)
        {
            double canvasWidth = snakeCanvas.ActualWidth;
            double canvasHeight = snakeCanvas.ActualHeight;

            //obstacles = Obstacle.LevelZero(canvasWidth, canvasHeight, UNIT);
            obstacles.Clear();
            switch (level)
            {
                case GameLevel.ZERO:
                    obstacles = Obstacle.LevelZero(canvasWidth, canvasHeight, UNIT);
                    farm.Capasity = 6;
                    farm.ProductionTime = 1;
                    farm.ExpiretionTime = 4;
                    break;
                case GameLevel.FIRST:
                    obstacles = Obstacle.LevelOne(canvasWidth, canvasHeight, UNIT);
                    farm.Capasity = 5;
                    farm.ProductionTime = 2;
                    farm.ExpiretionTime = 6;
                    break;
                case GameLevel.SECOND:
                    obstacles = Obstacle.LevelTwo(canvasWidth, canvasHeight, UNIT);
                    farm.Capasity = 4;
                    farm.ProductionTime = 3;
                    farm.ExpiretionTime = 3;
                    break;
                case GameLevel.THIRD:
                    obstacles = Obstacle.LevelThree(canvasWidth, canvasHeight, UNIT);
                    farm.Capasity = 3;
                    farm.ProductionTime = 4;
                    farm.ExpiretionTime = 1;
                    break;
                default:
                    obstacles = Obstacle.LevelZero(canvasWidth, canvasHeight, UNIT);
                    farm.Capasity = 5;
                    farm.ProductionTime = 1;
                    farm.ExpiretionTime = 2;
                    break;
            }


        }
        private Point GetRandomCanvasPoint()
        {


            double canvasWidth = snakeCanvas.ActualWidth;
            double canvasHeight = snakeCanvas.ActualHeight;

            Point randomPoint;
            do
            {
                randomPoint = new Point(rnd.NextDouble() * canvasWidth, rnd.NextDouble() * canvasHeight);
            } while (!IsValidPoint(randomPoint));

            return randomPoint;

        }

        private bool IsValidPoint(Point point)
        {
            foreach (var obstacle in obstacles)
            {
                if (obstacle.IsCircleIntersecting(point, UNIT, 1))
                    return false;
            }
            return true;
        }

        #endregion
        #region MouseControls
        private void snakeCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (State!=GameState.ONGOING) return;
            var currentPosition = e.GetPosition(snakeCanvas);
            snake.Move(currentPosition);
            DrawSnakeBody();
            DrawSnakeHead();
            if (farm.IsNerbyFoodEaten(currentPosition, out int foodIndex))
            {
                EatFood(foodIndex);
                // comment: why isn't snake.lenght++ part of eat food
                // comment: i don't like ++ better to have a method snake eat which increases length
                snake.Length++;
            }
            if (snake.IsEatingItself() || snake.IsHittingObstacle(obstacles, 0.9))
            {
                GameOver();
                //Ongoing = false;
            }
        }

        private void snakeCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //snake.Length++;
            //snake.Reset();
            //susspended = false;
            //farm.Stop();
            //TurnLevel(Level);

            //DrawObstacles();
            ////level++;
            //if (State == GameState.ONGOING)
            //{
            //    GamePause();
            //}
            //Score++;
            //farm.inspectionTimer = (System.Windows.Threading.DispatcherTimer)TimeSpan.FromSeconds(0.4);
        }

        private void snakeCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            //snake.Reset();
            //DrawSnakeBody();
            //DrawSnakeHead();
            //susspended = false
            if(State == GameState.ONGOING)
            {
                GamePause();
            }            
        }
        private void snakeCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (State == GameState.ONGOING)
            {
                GamePause();
            }          

        }
        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (propertyName.Equals(nameof(Score)) || propertyName.Equals(nameof(ExpiretionTime)))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
