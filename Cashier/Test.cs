using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Xunit.Assert;

namespace Cashier
{
    public class Test
    {
        public class CategoryTest
        {
            public Category Category { get; }
            public CategoryTest()
            {
                Category = new Category(Fixture.Item, Fixture.Config);
            }
        }
    }

    public class Fixture
    {
        public static Item Item => new Item("ITEM00001", 5);
        public static List<Item> Data => new List<Item>
        {
            new Item ("ITEM00001", 5),
            new Item ("ITEM00003", 2),
            new Item ("ITEM00005", 3),
        };
        public static Config Config => new Config("羽毛球", 1, "个", new List<Discount> { Discount.BuyTwoGetOneFree } );
        public static Dictionary<string, Config> Configs() => new Dictionary<string, Config>
        {
            { "ITEM00001", new Config("羽毛球", 1, "个", new List<Discount> { Discount.BuyTwoGetOneFree} ) },
            { "ITEM00003", new Config("苹果",  5.5, "斤", new List<Discount>()) },
            { "ITEM00005", new Config("羽毛球",  3, "瓶", new List<Discount> { Discount.BuyTwoGetOneFree} ) }
        };
    }
}
