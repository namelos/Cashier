using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
        public List<Category> Categories;
        public Model(List<Item> items, Dictionary<string, Config> configs)
        {
            Categories = items.Select(item => {
                var config = configs[item.Code];
                return new Category(item, config);
            }).ToList();
        }

        public decimal Total => Categories
            .Select(c => c.Subtotal)
            .Aggregate((x, y) => x + y);
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
        public string QuantityWithUnit => $"{Item.Quantity}{Config.Unit}";
        public decimal SubtotalWithOutDiscount => Config.Price*Item.Quantity;
        public decimal Subtotal => Config.Price*Item.Quantity;
        public override string ToString() => 
            $"名称: {Config.Name}, 数量: {Item.Quantity}, 单价: {Config.Price}(元), 小计: {Subtotal}(元)";
    }
    public class Item
    {
        public Item(string code, int quantity)
        {
            Code = code;
            Quantity = quantity;
        }

        public string Code { get; }
        public int Quantity { get; }
    }

    public class Config
    {
        public Config(string name, decimal price, string unit, List<Discount> discounts)
        {
            Name = name;
            Price = price;
            Unit = unit;
            Discounts = discounts;
        }

        public string Name { get; }
        public decimal Price { get; }
        public string Unit { get; }
        public List<Discount> Discounts { get; }
    }

    public class View
    {
        public Model Model { get; set; }
        public View(Model model)
        {
            Model = model;
        }
        public string Header => "***<没钱赚商店>购物清单***";
        public string Splitter => "----------------------";
        public string Footer => "**********************";
    }

    public enum Discount
    {
        BuyTwoGetOneFree,
        NintyFivePercentDiscount
    }
}
