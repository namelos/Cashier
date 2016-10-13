using System.Collections.Generic;
using Xunit;
using static Xunit.Assert;

namespace Cashier.Test
{
    public class DiscountTest
    {
        public class DiscountFormulaTest
        {
            [Fact]
            public void ShouldBuyTwoGetOneFreeWhenTheQuantityIsLargerThanTwoWhenBothDiscountApplys()
            {
                var discountTypes = new List<DiscountType>
                {
                    DiscountType.BuyTwoGetOneFree,
                    DiscountType.NintyFivePercentDiscount
                };
                var discountFormula = new DiscountFormula(discountTypes, 3).Discount;
                IsType<BuyTwoGetOneFree>(discountFormula);
            }

            [Fact]
            public void ShouldDiscountNinetyFiveWhenTheQuantityIsLessThanThreeWhenBothDiscountApplys()
            {
                var discountTypes = new List<DiscountType>
                {
                    DiscountType.BuyTwoGetOneFree,
                    DiscountType.NintyFivePercentDiscount
                };
                var discountFormula = new DiscountFormula(discountTypes, 2).Discount;
                IsType<NinetyFivePercent>(discountFormula);
            }

            [Fact]
            public void ShouldReturnBuyTwoGetOneFreeWhenDiscountIsBuyTwoGetOneFree()
            {
                var discountTypes = new List<DiscountType> { DiscountType.BuyTwoGetOneFree };
                var discountFormula = new DiscountFormula(discountTypes, 1).Discount;
                IsType<BuyTwoGetOneFree>(discountFormula);
            }

            [Fact]
            public void ShouldReturnNinetyFivePercentWhenDiscountIsNinetyFivePercent()
            {
                var discountTypes = new List<DiscountType> { DiscountType.NintyFivePercentDiscount };
                var discountFormula = new DiscountFormula(discountTypes, 1).Discount;
                IsType<NinetyFivePercent>(discountFormula);
            }

            [Fact]
            public void ShouldReturnNoDiscountWhenThereIsNoDiscount()
            {
                var discountTypes = new List<DiscountType>();
                var discountFormula = new DiscountFormula(discountTypes, 1).Discount;
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
    }
}