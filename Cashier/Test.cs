using System;
using System.Collections.Generic;
using System.Linq;
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
            public Item Item { get; } = Fixture.Item;
            public Config Config { get; } = Fixture.Config;
            public Category Category { get; }
            public CategoryTest()
            {
                Category = new Category(Item, Config);
            }
            [Fact]
            public void ShouldGetQuantityWithUnit() 
                => Equal(Category.QuantityWithUnit, "5个");
            [Fact]
            public void ShouldCalculateSubtotal() =>
                Equal(Category.Subtotal, 4);
            [Fact]
            public void ShouldCalculateSubtotalWithoutDiscount() => 
                Equal(Category.SubtotalWithOutDiscount, 5);
            [Fact]
            public void ShouldCalculateSaved() => Equal(Category.Saved, 1);
            [Fact]
            public void ShouldPrintCategorySummary() => Equal(Category.Show, 
                $"名称: {Config.Name}, 数量: {Item.Quantity}{Config.Unit}, 单价: {Config.Price}(元), 小计: {Category.Subtotal}(元)\n");
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
    }
}
