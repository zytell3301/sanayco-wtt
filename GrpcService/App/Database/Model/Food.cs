using System;
using System.Collections.Generic;

namespace GrpcService1.App.Database.Model
{
    public partial class Food
    {
        public Food()
        {
            FoodOrders = new HashSet<FoodOrder>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Price { get; set; }
        public bool IsAvailable { get; set; }

        public virtual ICollection<FoodOrder> FoodOrders { get; set; }
    }
}
