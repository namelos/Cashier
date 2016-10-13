using System.Collections.Generic;

namespace Cashier.Test
{
    public class Fixture
    {
        public static Item Item => new Item("ITEM00001", 5);

        public static List<Item> Items => new List<Item>
        {
            new Item("ITEM00001", 5),
            new Item("ITEM00003", 2),
            new Item("ITEM00005", 3),
        };

        public static Config Config
            => new Config("羽毛球", 1, "个", new List<DiscountType> { DiscountType.BuyTwoGetOneFree });

        public static Config ConfigWithNinetyFiveDiscount
            => new Config("羽毛球", 1, "个", new List<DiscountType> { DiscountType.NintyFivePercentDiscount });

        public static Dictionary<string, Config> Configs => new Dictionary<string, Config>
        {
            { "ITEM00001", new Config("羽毛球", 1m, "个", new List<DiscountType> { DiscountType.BuyTwoGetOneFree }) },
            { "ITEM00003", new Config("苹果", 5.5m, "斤", new List<DiscountType>()) },
            { "ITEM00005", new Config("羽毛球", 3m, "瓶", new List<DiscountType> { DiscountType.BuyTwoGetOneFree }) }
        };

        public static string SingularItemInput => "ITEM00001";
        public static string PluralItemInput => "ITEM00003-2";

        public static string[] Input => new[]
        {
            "ITEM00001",
            "ITEM00001",
            "ITEM00001",
            "ITEM00001",
            "ITEM00001",
            "ITEM00003-2",
            "ITEM00005",
            "ITEM00005",
            "ITEM00005"
        };
    }
}