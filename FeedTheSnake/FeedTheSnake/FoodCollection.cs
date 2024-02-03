using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FeedTheSnake
{
    internal class FoodCollection : Collection<Ellipse>
    {
        private FoodFarm foodfarm;
        public FoodCollection(FoodFarm farm) : base()
        {
            foodfarm = farm;
        }
        protected override void InsertItem(int index, Ellipse item)
        {
            base.InsertItem(index, item);
            foodfarm.FoodCollection.Add(new SnakeFood(new Point(Canvas.GetLeft(item) + item.Width / 2, Canvas.GetTop(item) + item.Height / 2), foodfarm.CurrentTime));
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            foodfarm.FoodCollection.RemoveAt(index);
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            foodfarm.FoodCollection.Clear();
        }
    }
    
    
}
