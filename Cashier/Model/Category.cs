using System;

namespace Cashier
{
    public class Category
    {
        public Category(Item item, Config config)
        {
            Item = item;
            Config = config;
            DiscountFormula = new DiscountFormula(Config.Discounts, Item.Quantity);
        }

        public Item Item { get; }
        public Config Config { get; }
        public DiscountFormula DiscountFormula { get; }

        public decimal SubtotalWithOutDiscount =>
            new NoDiscount().Discount(Config.Price, Item.Quantity);

        public decimal Subtotal =>
            DiscountFormula.Discount.Discount(Config.Price, Item.Quantity);

        public decimal Saved => SubtotalWithOutDiscount - Subtotal;

        public decimal BuyTwoGetOneFreeQuantity =>
            DiscountFormula.Discount is BuyTwoGetOneFree
                ? Math.Floor((decimal) Item.Quantity/3)
                : 0;
    }
}