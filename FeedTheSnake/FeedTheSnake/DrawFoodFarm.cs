using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using System.Drawing; ask for similar sounding names
using System.Linq;
using System.Numerics;
using System.Printing;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FeedTheSnake
{
    class DrawFoodFarm
    {
        private const double FOOD_DIAMETER = 40;

        private Collection<SnakeFood> foodCollection;
        private DispatcherTimer timer;
        private int foodProductionInterval;
        private int capacity;
        private Canvas paintCanvas;

        public DrawFoodFarm(Canvas canvas)
        {
            paintCanvas = canvas;
            foodProductionInterval = 2;
            capacity = 3;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, foodProductionInterval);
            timer.Tick += AddFood;
            foodCollection = new Collection<SnakeFood>();

        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public bool IsNearbyFoodEaten(Point positonPredator)
        {
            for(int i = 0; i < foodCollection.Count; i++)
            {
                if (positonPredator.Distance(foodCollection[i].Position) < FOOD_DIAMETER)
                {
                    DelateFoodAt(i);
                    return true;
                }
            }
            return false;
        }
        private void AddFood(object? sender, EventArgs e)
        {           
            if (capacity > foodCollection.Count)
            {
                Point foodCoords = GetRandomFoodPosition();
                SnakeFood snakeFood = new SnakeFood(foodCoords);
                foodCollection.Add(snakeFood);

                Ellipse food = new Ellipse();
                food.Width = FOOD_DIAMETER;
                food.Height = FOOD_DIAMETER;
                food.Fill = snakeFood.Color;

                Canvas.SetLeft(food, foodCoords.X - FOOD_DIAMETER/2);
                Canvas.SetTop(food, foodCoords.Y - FOOD_DIAMETER / 2);

                Panel.SetZIndex(food, -5);

                paintCanvas.Children.Add(food);
            }          
        }

        private void DelateFoodAt(int pos)
        {
            int foodCount = foodCollection.Count;
            if (pos < 0 || pos >= foodCount) return;

            foodCollection.RemoveAt(pos);
          
            int countElements = paintCanvas.Children.Count;
            paintCanvas.Children.RemoveAt((countElements-1)-(foodCount-1) + pos);

        }

        private Point GetRandomFoodPosition()
        {
            Random rnd = new Random();

            double canvasWidth = paintCanvas.Width;
            double canvasHeight = paintCanvas.Height;

            return new Point(rnd.NextDouble() * canvasWidth, rnd.NextDouble() * canvasHeight);
        }
       
        
        public int FoodProductionInterval 
        {
            get => foodProductionInterval;
           set=> foodProductionInterval = value >= 1? value : 1;
        }

        public int Capacity
        {
            get => Capacity;
            set => Capacity = value >= 1 ? value : 1;
        }

    }
}
