using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashier
{
    public class Test
    {
        
    }

    public class Fixture
    {
        public static List<Item> Data() => new List<Item>
        {
            new Item { Code = "ITEM00001", Amount = 5 },
            new Item { Code = "ITEM00003", Amount = 2 },
            new Item { Code = "ITEM00005", Amount = 3 },
        };

        public static Dictionary<string, Config> Configs() => new Dictionary<string, Config>
        {
            {
                "ITEM00001", new Config
                {
                    Name = "羽毛球", Price = 1, Unit = "个",
                    Discounts = new List<Discount> { Discount.BuyTwoGetOneFree }
                }
            },
            {
                "ITEM00003", new Config
                {
                    Name = "苹果", Price = 5.5, Unit = "斤",
                    Discounts = new List<Discount>()
                }
            },
            {
                "ITEM00005", new Config
                {
                    Name = "羽毛球", Price = 3, Unit = "瓶",
                    Discounts = new List<Discount> { Discount.BuyTwoGetOneFree }
                }
            }
        };
    }
}
