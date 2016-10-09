using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Cashier
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    public class Model
    {
    }

    public class Category
    {
        public Category(Item item, Config config)
        {
            Item = item;
            Config = config;
        }

        public Item Item { get; }
        public Config Config { get; }
    }

    public class Item
    {
        public Item(string code, int amount)
        {
            Code = code;
            Amount = amount;
        }

        public string Code { get; }
        public int Amount { get; }
    }

    public class Config
    {
        public Config(string name, double price, string unit, List<Discount> discounts)
        {
            Name = name;
            Price = price;
            Unit = unit;
            Discounts = discounts;
        }

        public string Name { get; }
        public double Price { get; }
        public string Unit { get; }
        public List<Discount> Discounts { get; }
    }

    public enum Discount
    {
        BuyTwoGetOneFree,
        NintyFivePercentDiscount
    }
}
