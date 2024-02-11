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
    ///Draws and moves snake and foods on canvas
    ///in the User Controls
    /// </summary>
    public partial class SnakeGame : UserControl, INotifyPropertyChanged
    {
        public enum GameState { ONGOING, PAUSED, OVER, PREPEARING }
        public enum GameLevel { ZERO, FIRST, SECOND, THIRD }

        public event PropertyChangedEventHandler? PropertyChanged;

        #region Fields
        private double unit;
        private static Random rnd = new Random();

        private int score;
        private GameState state;

        private Snake snake;
        private Polyline? body;
        private Ellipse? head;

        private FoodFarm farm;
        private FoodCollection foodStock;

        private List<Obstacle> obstacles;
        private List<Rectangle> rectangles; 
        #endregion

        #region Properties
        /// <summary>
        /// time for food to expire
        /// </summary>
        public int ExpiretionTime
        {
            get
            { return farm.ExpiretionTime; }
            set
            {
                farm.ExpiretionTime = value;

                OnPropertyChanged(nameof(ExpiretionTime));
            }
        }
        /// <summary>
        /// Game Score
        /// </summary>
        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value >= 0 ? value : 0;
                OnPropertyChanged(nameof(Score));
            }
        }
        /// <summary>
        /// Current game state
        /// </summary>
        public GameState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }
        /// <summary>
        /// Current game level
        /// </summary>
        public GameLevel Level { get; set; } 
        #endregion
        public SnakeGame()
        {
            InitializeComponent();
            unit = 20;
            State = GameState.PREPEARING;
            Level = GameLevel.ZERO;
            farm = new FoodFarm(unit);
            snake = new Snake(unit);
            body = new Polyline();
            head = new Ellipse();
            foodStock = new FoodCollection(farm);
            obstacles = new List<Obstacle>();
            rectangles = new List<Rectangle>();
            Score = 0;
            ExpiretionTime = 5;

            snakeCanvas.Children.Add(body);
            snakeCanvas.Children.Add(head);

            farm.inspectionTimer.Tick += DrawFood;
            farm.inspectionTimer.Tick += DeleteExpiredFoods;

            head.MouseLeftButtonDown += (x, y) => { GameStart(); };
            snake.PropertyChanged += (x, y) => { Score++; };
        }
        #region ClientMethods
        /// <summary>
        /// Initialize a new game  with given parameters
        /// </summary>
        public void NewGame()
        {
            if (State != GameState.OVER &&
                State != GameState.PREPEARING &&
                State != GameState.PAUSED) { return; }
            unit = snakeCanvas.ActualHeight / 20;
            Score = 0;
            farm.Stop();
            farm.FoodRadius = unit;
            ClearFood();
            snake.Reset(unit);
            DrawSnakeBody();
            DrawSnakeHead();
            TurnLevel(Level);
            DrawObstacles();
            State = GameState.PAUSED;
        }
        /// <summary>
        /// End Game
        /// </summary>
        public void GameOVER()
        {
            if (State != GameState.ONGOING) { return; }
             farm.Stop();
            State = GameState.OVER;
        }
        /// <summary>
        /// Pause Game
        /// </summary>
        public void GamePause()
        {
            if (State != GameState.ONGOING) { return; }
            farm.Stop();
            State = GameState.PAUSED;
        }
        /// <summary>
        /// Continue Game
        /// </summary>
        public void GameStart()
        {
            if (State != GameState.PAUSED) { return; }
            farm.Start();
            State = GameState.ONGOING;
        }
        #endregion
        #region DrawingMethods
        /// <summary>
        /// Draws the body of the snake given with class Snake
        /// </summary>
        private void DrawSnakeBody()
        {
            body!.Points = new PointCollection(snake.Body);
            body.StrokeThickness = snake.Segment;
            body.StrokeLineJoin = PenLineJoin.Round;
            body.StrokeEndLineCap = PenLineCap.Round;
            body.StrokeStartLineCap = PenLineCap.Triangle;
            body.Stroke = Brushes.Green;
        }
        /// <summary>
        ///  Draws the head of the snake given with class Snake
        /// </summary>
        private void DrawSnakeHead()
        {
            head!.Width = 2 * snake.Segment;
            head.Height = 2 * snake.Segment;

            head.Fill = Brushes.Green;

            Canvas.SetLeft(head, snake.HeadPoint.X - head.Width / 2);
            Canvas.SetTop(head, snake.HeadPoint.Y - head.Height / 2);
        }
        /// <summary>
        /// Draw Food for the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawFood(object? sender, EventArgs e)
        {
            if (farm.CurrentTime % farm.ProductionTime != 0) { return; }
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

                snakeCanvas.Children.Add(food);
            }
        }
        /// <summary>
        /// Draw Obstacles
        /// </summary>
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
        /// <summary>
        /// Delete food at a given index of foodStock and canvas
        /// </summary>
        /// <param name="index"></param>
        private void DeleteFoodAt(int index)
        {
            if (index < 0 || index >= foodStock.Count) return;

            snakeCanvas.Children.Remove(foodStock[index]);
            foodStock.RemoveAt(index);
        }
        /// <summary>
        /// Delete all expired foods in foodStock and canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteExpiredFoods(object? sender, EventArgs e)
        {
            //farm.CurrentTime++;
            while (farm.DeleteEpiredFood(DeleteFoodAt))
            { }
        }

        /// <summary>
        /// Clear all foods in foodStock and canvas
        /// </summary>
        private void ClearFood()
        {
            foreach (var food in foodStock)
            {
                snakeCanvas.Children.Remove(food);
            }
            foodStock.Clear();
            //farm.Clear();
        }
        /// <summary>
        /// Change level settings without redrawing
        /// </summary>
        /// <param name="level"></param>
        private void TurnLevel(GameLevel level)
        {
            double canvasWidth = snakeCanvas.ActualWidth;
            double canvasHeight = snakeCanvas.ActualHeight;

            obstacles.Clear();
            switch (level)
            {
                case GameLevel.ZERO:
                    obstacles = Obstacle.LevelZero(canvasWidth, canvasHeight, unit);
                    farm.Capasity = 6;
                    farm.ProductionTime = 1;
                    break;
                case GameLevel.FIRST:
                    obstacles = Obstacle.LevelOne(canvasWidth, canvasHeight, unit);
                    farm.Capasity = 5;
                    farm.ProductionTime = 2;
                    break;
                case GameLevel.SECOND:
                    obstacles = Obstacle.LevelTwo(canvasWidth, canvasHeight, unit);
                    farm.Capasity = 4;
                    farm.ProductionTime = 3;
                    break;
                case GameLevel.THIRD:
                    obstacles = Obstacle.LevelThree(canvasWidth, canvasHeight, unit);
                    farm.Capasity = 3;
                    farm.ProductionTime = 4;
                    break;
                default:
                    obstacles = Obstacle.LevelZero(canvasWidth, canvasHeight, unit);
                    farm.Capasity = 6;
                    farm.ProductionTime = 1;
                    //farm.ExpiretionTime = 2;
                    break;
            }


        }
        /// <summary>
        /// Get a random Point from canvas witch doesnt intersect with any obstacle at 
        /// a distance farm.FoodRadius
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Checks if point is valid for food position
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool IsValidPoint(Point point)
        {
            foreach (var obstacle in obstacles)
            {
                if (obstacle.IsCircleIntersecting(point, unit, 1))
                    return false;
            }
            return true;
        }

        #endregion
        #region MouseControls
        /// <summary>
        /// Moves the snake if game State is Ongoing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void snakeCanvas_MouseMove(object sender, MouseEventArgs e)
        {

            if (State != GameState.ONGOING) return;
            var currentPosition = e.GetPosition(snakeCanvas);
            snake.Move(currentPosition);
            DrawSnakeBody();
            DrawSnakeHead();
            if (farm.IsNerbyFoodEaten(currentPosition, out int foodIndex))
            {
                DeleteFoodAt(foodIndex);
                snake.Length++;
            }
            if (snake.IsEatingItself() || snake.IsHittingObstacle(obstacles, 0.9))
            {
                GameOVER();               
            }
        }
        /// <summary>
        /// If it leavs user control pause game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void snakeCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (State == GameState.ONGOING)
            {
                GamePause();
            }
        }
        /// <summary>
        /// Pauses game when ButtomLeft is UP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            if (propertyName.Equals(nameof(Score)) || propertyName.Equals(nameof(ExpiretionTime)) || propertyName.Equals(nameof(State)))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
