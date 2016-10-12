﻿using System;
using System.Collections.Generic;
using System.Linq;

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
        public DiscountFormula DiscountFormula;
        public Item Item { get; }
        public Config Config { get; }
        public Category(Item item, Config config)
        {
            Item = item;
            Config = config;
            DiscountFormula = new DiscountFormula(config.Discounts);
        }
        public decimal SubtotalWithOutDiscount =>
            new NoDiscount().Discount(Config.Price, Item.Quantity);
        public decimal Subtotal =>
            DiscountFormula.Discount.Discount(Config.Price, Item.Quantity);
        public decimal Saved => SubtotalWithOutDiscount - Subtotal;
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
    }

    public class CategoryView
    {
        public Category Category;

        public CategoryView(Category category)
        {
            Category = category;
        }

        public string AmountWithUnit => $"{Category.Item.Quantity}{Category.Config.Unit}";
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
                Discount = new BothDiscount();
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
