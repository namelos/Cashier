using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cashier
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputs = Fixture.Input;
            var configs = Fixture.Configs;
            var parsedResult = new Parser(inputs);
            var model = new Model(parsedResult.Items, configs);
            var view = new View(model);
            view.Render();
            Console.ReadLine();
        }
    }

    public class Parser
    {
        public Parser(IEnumerable<string> inputs)
        {
            ParsedItems = inputs
                .Select(input => new ItemParser(input))
                .GroupBy(parsedItem => parsedItem.Code)
                .Select(groupedItem =>
                {
                    return groupedItem.Aggregate((item1, item2) =>
                    {
                        item1.AddQuantity(item2.Quantity);
                        return item1;
                    });
                }).ToList();
        }
        public List<ItemParser> ParsedItems;
        public List<Item> Items => ParsedItems
                .Select(parsedItem => new Item(parsedItem.Code, parsedItem.Quantity)).ToList();
    }

    public class ItemParser
    {
        public string Code { get; set; }
        public int Quantity { get; set; }
        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }
        public ItemParser(string inputItem)
        {
            var pair = inputItem.Split('-').ToList();
            Code = pair[0];
            Quantity = pair.Count == 1 ? 1 : int.Parse(pair[1]);
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
        public decimal TotalSaved => Categories
            .Select(c => c.Saved)
            .Aggregate((x, y) => x + y);
    }

    public class Category
    {
        public Item Item { get; }
        public Config Config { get; }
        public DiscountFormula DiscountFormula { get; }
        public Category(Item item, Config config)
        {
            Item = item;
            Config = config;
            DiscountFormula = new DiscountFormula(Config.Discounts, Item.Quantity);
        }
        public decimal SubtotalWithOutDiscount =>
            new NoDiscount().Discount(Config.Price, Item.Quantity);
        public decimal Subtotal =>
            DiscountFormula.Discount.Discount(Config.Price, Item.Quantity);
        public decimal Saved => SubtotalWithOutDiscount - Subtotal;
        public decimal BuyTwoGetOneFreeQuantity =>
            DiscountFormula.Discount is BuyTwoGetOneFree ? 
            Math.Floor((decimal) Item.Quantity/3) : 0;
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
        public List<CategoryView> CategoryViews;
        public View(Model model)
        {
            Model = model;
            CategoryViews = Model.Categories
                .Select(category => new CategoryView(category)).ToList();
        }
        public string Categories => CategoryViews
            .Select(c => c.Show)
            .Aggregate((x, y) => x + y);
        public string Header => "***<没钱赚商店>购物清单***\n";
        public string Splitter => "----------------------\n";
        public string Footer => "**********************\n";
        public string BuyTwoGetOneFreeSummarys => CategoryViews
            .Select(c => c.BuyTwoGetOneFreeSummary)
            .Aggregate((x, y) => x + y);
        public string BuyTwoGetOneFreeSummarysWithSpliter =>
            BuyTwoGetOneFreeSummarys + Splitter;
        public string Total => $"总计:{Model.Total}(元)\n";
        public string TotalSaved => Model.TotalSaved > 0 ? $"节省:{Model.TotalSaved}(元)\n" : string.Empty ;
        public void Render()
        {
            Console.Write(Header + Categories + Splitter +
                BuyTwoGetOneFreeSummarysWithSpliter + Total + TotalSaved + Footer);
        }
    }

    public class CategoryView
    {
        public Category Category;

        public CategoryView(Category category)
        {
            Category = category;
        }
        public string NinetyFivePercentBonus => 
            Category.DiscountFormula.Discount is NinetyFivePercent ? 
            $", 节省:{Category.Saved}(元)" : string.Empty;
        public string Show => $"名称:{Category.Config.Name}, " +
                              $"数量:{Category.Item.Quantity}{Category.Config.Unit}, " +
                              $"单价:{Category.Config.Price}(元), " +
                              $"小计:{Category.Subtotal}(元)" +
                              $"{NinetyFivePercentBonus}\n";

        public string BuyTwoGetOneFreeSummary => 
            Category.DiscountFormula.Discount is BuyTwoGetOneFree ?
            $"名称:{Category.Config.Name}, " + $"数量:{Category.BuyTwoGetOneFreeQuantity}{Category.Config.Unit}\n" :
            string.Empty;
    }

    public enum DiscountType
    {
        None,
        BuyTwoGetOneFree,
        NintyFivePercentDiscount
    }

    public class DiscountFormula
    {
        public IDiscount Discount;
        public DiscountFormula(List<DiscountType> discountTypes, int quantity)
        {
            if (discountTypes.Contains(DiscountType.BuyTwoGetOneFree) &&
                discountTypes.Contains(DiscountType.NintyFivePercentDiscount))
            {
                if (quantity > 2)
                    Discount = new BuyTwoGetOneFree();
                else
                    Discount =  new NinetyFivePercent();
            }
            else if (discountTypes.Contains(DiscountType.BuyTwoGetOneFree))
                Discount = new BuyTwoGetOneFree();
            else if (discountTypes.Contains(DiscountType.NintyFivePercentDiscount))
                Discount = new NinetyFivePercent();
            else
                Discount = new NoDiscount();
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

    public class NoDiscount: IDiscount
    {
        public decimal Discount(decimal price, int quantity) =>
            price*quantity;
    }
}
