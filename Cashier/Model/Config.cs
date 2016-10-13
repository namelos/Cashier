using System.Collections.Generic;

namespace Cashier
{
    public class Config
    {
        public Config(string name, decimal price, string unit, List<DiscountType> discounts)
        {
            Name = name;
            Price = price;
            Unit = unit;
            Discounts = discounts;
        }

        public string Name { get; }
        public decimal Price { get; }
        public string Unit { get; }
        public List<DiscountType> Discounts { get; }
    }
}