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
    struct SnakeFood//:INotifyPropertyChanged
    {
        public Point Position;
        public int TimeOfProduction;
        //DateTimeOffset a;

        public SnakeFood(Point position, int timeOfProduction)
        {
            Position = position;
            TimeOfProduction = timeOfProduction;
            //a = DateTime.Now;
            //double h = a.Offset;
        }

        //public event PropertyChangedEventHandler? PropertyChanged;
    }
    class FoodFarm : INotifyPropertyChanged
    {
        //public DispatcherTimer timer;
        public DispatcherTimer inspectionTimer;

       
        private double foodRadius;
        private int capasity;
		private int productionTime;
		private List<SnakeFood> foodCollection;
        private int expiretionTime;
        //link SnakeGame expiretionTime with FoodFarm it doesnt need property change!!!
        //check if there is a more efficent method for expiration time of food
        //use seperated threds
        //make deleteExpiredFood work insted bool IsExpired(out int index) in while then eat food
        //help menu
        //game state display
        //changed background when game over
        //fix default ExpirationTime , TurnLevel
        //resize UNIT private double
        public event PropertyChangedEventHandler? PropertyChanged;

        public FoodFarm(double foodRadius, int capasity, int productionTime)
        {
            ExpiretionTime = 5;
            CurrentTime = 0;
            FoodRadius = foodRadius;
			Capasity = capasity;
			ProductionTime = productionTime;
			foodCollection = new List<SnakeFood>();

            //timer = new DispatcherTimer();
            //timer.Interval = new TimeSpan(0, 0, ProductionTime);

            inspectionTimer = new DispatcherTimer();
            inspectionTimer.Interval = new TimeSpan(0, 0, 1);
            //inspectionTimer.Interval = new TimeSpan(0, 0, 1);
            //double seconds = inspectionTimer.Interval.TotalSeconds;
            //TimeSpan.FromSeconds(0.4);//TODO with miliseconnds!!!!!!!!!!

            inspectionTimer.Tick += (x, y) => { CurrentTime++; };
        }

        public FoodFarm(double foodRadius) : this(foodRadius, 3, 2)
        {
            
        }

        public FoodFarm() : this(20, 3, 2)
        {
            
        }

        public void Start()
        {
            //timer.Start();
            inspectionTimer.Start();
        }

        public void Stop()
        {
            //timer.Stop();
            inspectionTimer.Stop();
            //inspectionTimer.Interval = new TimeSpan(0, 0, 3);
        }
        public int CurrentTime { get; set; }

        public int ExpiretionTime
        {
            get { return expiretionTime; }
            set 
            { 
                expiretionTime = value >= 1 ? value : 1; 
                OnPropertyChanged(nameof(ExpiretionTime));
            }
        }
        public bool DeleteEpiredFood(Action<int> deleteAt,int expire)
        {
            for (int i = 0; i < FoodCollection.Count; i++)
            {
                if (FoodCollection[i].TimeOfProduction + expire <= CurrentTime)//ExpiretionTime
                { 
                    deleteAt(i); 
                    return true;
                }
            }
            return false;
        }
        public bool IsNerbyFoodEaten(Point preadatorPosotion, out int index)
        {
            for(int i = 0; i < foodCollection.Count; i++)
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
        public List<SnakeFood> FoodCollection 
		{
			get
			{				
				return foodCollection;
			}
		}

		public double FoodRadius
        {
			get { return foodRadius; }
			set { foodRadius = value >= 1 ? value: 1 ; }
		}

		public int ProductionTime
        {
			get { return productionTime; }
			set { 
                productionTime = value>=1?value:1;
            }
		}

		public int Capasity
		{
			get { return capasity; }
			set { capasity = value>=1?value:1; }
		}

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (propertyName.Equals(nameof(ExpiretionTime)))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
