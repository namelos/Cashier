using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
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
        public DiscountFormula DiscountFormula;
        public Item Item { get; }
        public Config Config { get; }
        public Category(Item item, Config config)
        {
            Item = item;
            Config = config;
            DiscountFormula = new DiscountFormula(config.Discounts);
        }
        public string QuantityWithUnit => $"{Item.Quantity}{Config.Unit}";
        public decimal SubtotalWithOutDiscount =>
            new NoDiscount().Discount(Config.Price, Item.Quantity);
        public decimal Subtotal =>
            DiscountFormula.Discount.Discount(Config.Price, Item.Quantity);
        public decimal Saved => SubtotalWithOutDiscount - Subtotal;
        public string Show => 
            $"名称: {Config.Name}, 数量: {QuantityWithUnit}, 单价: {Config.Price}(元), 小计: {Subtotal}(元)\n";
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

    public class View
    {
        public Model Model { get; set; }
        public View(Model model)
        {
            Model = model;
        }
        public string Header => "***<没钱赚商店>购物清单***\n";
        public string Splitter => "----------------------\n";
        public string Footer => "**********************\n";
        public string RenderSummary => Model.Categories
            .Select(c => c.Show)
            .Aggregate((x, y) => x + y);
    }

    public enum DiscountType
    {
        BuyTwoGetOneFree,
        NintyFivePercentDiscount
    }

    public class DiscountFormula
    {
        public IDiscount Discount;
        public DiscountFormula(List<DiscountType> discountTypes)
        {
            if (discountTypes.Contains(DiscountType.BuyTwoGetOneFree) &&
                discountTypes.Contains(DiscountType.NintyFivePercentDiscount))
            {
                Discount = new BothDiscount();
            }
            else if (discountTypes.Contains(DiscountType.BuyTwoGetOneFree))
            {
                Discount = new BuyTwoGetOneFree();
            }
            else if (discountTypes.Contains(DiscountType.NintyFivePercentDiscount))
            {
                Discount = new NinetyFivePercent();
            }
            else
            {
                Discount = new NoDiscount();
            }
        }
    }

    public interface IDiscount
    {
        decimal Discount(decimal price, int quantity);
    }

    public class BuyTwoGetOneFree : IDiscount
    {
        public decimal Discount(decimal price, int quantity) => 
            (quantity - Math.Floor((decimal)quantity / 3)) * price;
    }

    public class NinetyFivePercent : IDiscount
    {
        public decimal Discount(decimal price, int quantity) =>
            0.95m*price*quantity;
    }

    public class BothDiscount : IDiscount
    {
        public decimal Discount(decimal price, int quantity) => quantity > 2 ? 
            new BuyTwoGetOneFree().Discount(price, quantity) :
            new NinetyFivePercent().Discount(price, quantity);
    }

    public class NoDiscount: IDiscount
    {
        public decimal Discount(decimal price, int quantity) =>
            price*quantity;
    }
}
