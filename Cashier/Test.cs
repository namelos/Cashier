using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Xunit.Assert;

namespace Cashier
{
    public class Test
    {
        public class ParserTest
        {
            [Fact]
            public void ShouldGroupInputsToItems()
            {
                var parsedItems = new Parser(Fixture.Input).ParsedItems.ToList();
                var item00001 = parsedItems.Find(x => x.Code == "ITEM00001");
                Equal(item00001.Quantity, 5);
                var item00003 = parsedItems.Find(x => x.Code == "ITEM00003");
                Equal(item00003.Quantity, 2);
                var item00005 = parsedItems.Find(x => x.Code == "ITEM00005");
                Equal(item00005.Quantity, 3);
            }

            public class ItemParserTest
            {
                [Fact]
                public void ShouldParseSingularItemInput()
                {
                    var parsed = new ItemParser(Fixture.SingularItemInput);
                    Equal(parsed.Code, "ITEM00001");
                    Equal(parsed.Quantity, 1);
                }

                [Fact]
                public void ShouldParsePluralItemInput()
                {
                    var parsed = new ItemParser(Fixture.PluralItemInput);
                    Equal(parsed.Code, "ITEM00003");
                    Equal(parsed.Quantity, 2);
                }
            }
        }

        public class CategoryTest
        {
            public Item Item { get; } = Fixture.Item;
            public Config Config { get; } = Fixture.Config;
            public Category Category { get; }
            public CategoryTest()
            {
                Category = new Category(Item, Config);
            }
            [Fact]
            public void ShouldCalculateSubtotal() =>
                Equal(Category.Subtotal, 4);
            [Fact]
            public void ShouldCalculateSubtotalWithoutDiscount() => 
                Equal(Category.SubtotalWithOutDiscount, 5);
            [Fact]
            public void ShouldCalculateSaved() => Equal(Category.Saved, 1);
        }

        public class ModelTest
        {
            public Model Model { get; }
            public List<Item> Items { get; } = Fixture.Items;
            public Dictionary<string, Config> Configs { get; } = Fixture.Configs;
            public ModelTest()
            {
                Model = new Model(Items, Configs);
            }
            [Fact]
            public void ShouldInitCategoriesWithItemAndConfigs()
            {
                Equal(Model.Categories[0].Item, Items[0]);    
                Equal(Model.Categories[0].Config, Configs[Items[0].Code]);    
                Equal(Model.Categories[1].Item, Items[1]);    
                Equal(Model.Categories[1].Config, Configs[Items[1].Code]);    
                Equal(Model.Categories[2].Item, Items[2]);    
                Equal(Model.Categories[2].Config, Configs[Items[2].Code]);    
            }
            [Fact]
            public void ShouldCalculateTotal() => Equal(Model.Total, 21);
            [Fact]
            public void ShouldCalculateTotalSaved() => Equal(Model.TotalSaved, 4);
        }

        public class DiscountTest
        {
            public class DiscountFormulaTest
            {
                [Fact]
                public void ShouldReturnBothDiscountWhenThereAreBothOfDiscounts()
                {
                    var discountTypes = new List<DiscountType>
                    {
                        DiscountType.BuyTwoGetOneFree,
                        DiscountType.NintyFivePercentDiscount
                    };
                    var discountFormula = new DiscountFormula(discountTypes).Discount;
                    IsType<BothDiscount>(discountFormula);
                }
                [Fact]
                public void ShouldReturnBuyTwoGetOneFreeWhenDiscountIsBuyTwoGetOneFree()
                {
                    var discountTypes = new List<DiscountType> { DiscountType.BuyTwoGetOneFree };
                    var discountFormula = new DiscountFormula(discountTypes).Discount;
                    IsType<BuyTwoGetOneFree>(discountFormula);
                }
                [Fact]
                public void ShouldReturnNinetyFivePercentWhenDiscountIsNinetyFivePercent()
                {
                    var discountTypes = new List<DiscountType> { DiscountType.NintyFivePercentDiscount };
                    var discountFormula = new DiscountFormula(discountTypes).Discount;
                    IsType<NinetyFivePercent>(discountFormula);
                }
                [Fact]
                public void ShouldReturnNoDiscountWhenThereIsNoDiscount()
                {
                    var discountTypes = new List<DiscountType>();
                    var discountFormula = new DiscountFormula(discountTypes).Discount;
                    IsType<NoDiscount>(discountFormula);
                }
            }
            public class BuyTwoGetOneFreeTest
            {
                [Fact]
                public void ShouldGiveOneFreeWhenBuyThree() =>
                    Equal(new BuyTwoGetOneFree().Discount(1, 3), 2);

                [Fact]
                public void ShouldNotGiveAnyWhenBuyTwo() =>
                    Equal(new BuyTwoGetOneFree().Discount(1, 2), 2);
            }
            public class NinetyFivePercentTest
            {
                [Fact]
                public void ShouldCalculateNinetyFivePercent() =>
                    Equal(new NinetyFivePercent().Discount(10, 10), 95);
            }
            public class BothDiscountTest
            {
                [Fact]
                public void ShouldBuyTwoGiveOneFreeWhenBuyMoreThanTwo() =>
                    Equal(new BothDiscount().Discount(1, 3), 2);

                [Fact]
                public void ShouldCalculateNinetyFivePercentDiscountWhenBuyLessThanThree() =>
                    Equal(new BothDiscount().Discount(1, 2), 1.9m);
            }
        }

        public class ViewTest
        {
            public class CategoryViewTest
            {
                public Category Category { get; set; }
                public CategoryView CategoryView { get; set; }
                public CategoryViewTest()
                {
                    Category = new Category(Fixture.Item, Fixture.Config);
                    CategoryView = new CategoryView(Category);
                }
                [Fact]
                public void ShouldShowAmountWithUnit() => 
                    Equal(CategoryView.AmountWithUnit, $"{Category.Item.Quantity}{Category.Config.Unit}");
            }
        }
    }

    public class Fixture
    {
        public static Item Item => new Item("ITEM00001", 5);
        public static List<Item> Items => new List<Item>
        {
            new Item ("ITEM00001", 5),
            new Item ("ITEM00003", 2),
            new Item ("ITEM00005", 3),
        };
        public static Config Config => new Config("羽毛球", 1, "个", new List<DiscountType> { DiscountType.BuyTwoGetOneFree } );
        public static Dictionary<string, Config> Configs => new Dictionary<string, Config>
        {
            { "ITEM00001", new Config("羽毛球", 1m, "个", new List<DiscountType> { DiscountType.BuyTwoGetOneFree} ) },
            { "ITEM00003", new Config("苹果",  5.5m, "斤", new List<DiscountType>()) },
            { "ITEM00005", new Config("羽毛球",  3m, "瓶", new List<DiscountType> { DiscountType.BuyTwoGetOneFree} ) }
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
