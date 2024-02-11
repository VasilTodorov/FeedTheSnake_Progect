using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static System.Formats.Asn1.AsnWriter;

namespace FeedTheSnake
{
    /// <summary>
    /// a basic struct to keep information for food
    /// </summary>
    struct SnakeFood
    {
        public Point Position;
        public int TimeOfProduction;       

        public SnakeFood(Point position, int timeOfProduction)
        {
            Position = position;
            TimeOfProduction = timeOfProduction;            
        }

        
    }
    /// <summary>
    /// class FoodFarm demonstrates the logic behind food production in
    /// the game feed the snake
    /// </summary>
    class FoodFarm
    {       
        public DispatcherTimer inspectionTimer;

        #region Fields
        private double foodRadius;
        private int capasity;
        private int productionTime;
        private List<SnakeFood> foodCollection;
        private int expiretionTime;

        #endregion
        #region Constructors
        public FoodFarm(double foodRadius, int capasity, int productionTime)
        {
            ExpiretionTime = 5;
            CurrentTime = 0;
            FoodRadius = foodRadius;
            Capasity = capasity;
            ProductionTime = productionTime;
            foodCollection = new List<SnakeFood>();

            inspectionTimer = new DispatcherTimer();
            inspectionTimer.Interval = TimeSpan.FromSeconds(1);
            inspectionTimer.Tick += (x, y) => { CurrentTime++; };
        }

        public FoodFarm(double foodRadius) : this(foodRadius, 3, 2)
        {

        }

        public FoodFarm() : this(20, 3, 2)
        {

        } 
        #endregion
        #region Properties
        /// <summary>
        /// the current time of the game
        /// </summary>
        public int CurrentTime { get; set; }
        /// <summary>
        /// time for each food produced to expire
        /// </summary>
        public int ExpiretionTime
        {
            get { return expiretionTime; }
            set
            {
                expiretionTime = value >= 1 ? value : 1;
            }
        }
        /// <summary>
        /// the position of the foods
        /// </summary>
        public List<SnakeFood> FoodCollection
        {
            get
            {
                return foodCollection;
            }
        }
        /// <summary>
        /// the radius of each food
        /// </summary>
        public double FoodRadius
        {
            get { return foodRadius; }
            set { foodRadius = value >= 1 ? value : 1; }
        }
        /// <summary>
        /// the time to needed to produce food
        /// </summary>
        public int ProductionTime
        {
            get { return productionTime; }
            set
            {
                productionTime = value >= 1 ? value : 1;
            }
        }
        /// <summary>
        /// max number of food that can be
        /// </summary>
        public int Capasity
        {
            get { return capasity; }
            set { capasity = value >= 1 ? value : 1; }
        } 
        #endregion
        #region FarmMethods
        /// <summary>
        /// the start of the timer
        /// </summary>
        public void Start()
        {
            inspectionTimer.Start();
        }
        /// <summary>
        /// the end of the timer
        /// </summary>
        public void Stop()
        {
            inspectionTimer.Stop();
        }
        /// <summary>
        /// finds and deletes an expired food using a given
        /// fuction as argument which deletes food at agiven index
        /// in FoodCollection
        /// </summary>
        /// <param name="deleteAt"></param>
        /// <returns></returns>
        public bool DeleteEpiredFood(Action<int> deleteAt)
        {
            for (int i = 0; i < FoodCollection.Count; i++)
            {
                if (FoodCollection[i].TimeOfProduction + ExpiretionTime <= CurrentTime)
                {
                    deleteAt(i);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// checks if there is is a food that should be deleted if it is at distance
        /// FoodRadius from a given predatory point. If a point should be deleted returns true
        /// and gives index of point as out parameter, otherwise false and index = ind.MinValue
        /// </summary>
        /// <param name="preadatorPosotion"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsNerbyFoodEaten(Point preadatorPosotion, out int index)
        {
            for (int i = 0; i < foodCollection.Count; i++)
            {
                if (foodCollection[i].Position.Distance(preadatorPosotion) <= foodRadius)
                {
                    index = i;
                    //foodCollection.RemoveAt(i);
                    return true;
                }
            }
            index = int.MinValue;
            return false;
        }

        #endregion


    }
}
