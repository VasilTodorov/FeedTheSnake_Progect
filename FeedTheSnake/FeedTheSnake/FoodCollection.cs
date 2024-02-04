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
    /// <summary>
    /// illustrates a collection of ellipses for food to be drawn
    /// when we change the collection we also change a given FoodFarm
    /// it is expected FoodFarm.FoodCollection to be empty otherwise
    /// it will be emptied
    /// </summary>
    internal class FoodCollection : Collection<Ellipse>
    {
        private FoodFarm foodfarm;
        public FoodCollection(FoodFarm farm) : base()
        {
            if(farm.FoodCollection.Count > 0)
                farm.FoodCollection.Clear();
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
