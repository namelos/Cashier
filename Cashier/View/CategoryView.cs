namespace Cashier
{
    public class CategoryView
    {
        public Category Category;

        public CategoryView(Category category)
        {
            Category = category;
        }

        public string NinetyFivePercentBonus =>
            Category.DiscountFormula.Discount is NinetyFivePercent
                ? $", 节省:{Category.Saved}(元)"
                : string.Empty;

        public string Show => $"名称:{Category.Config.Name}, " +
                              $"数量:{Category.Item.Quantity}{Category.Config.Unit}, " +
                              $"单价:{Category.Config.Price}(元), " +
                              $"小计:{Category.Subtotal}(元)" +
                              $"{NinetyFivePercentBonus}\n";

        public string BuyTwoGetOneFreeSummary =>
            Category.DiscountFormula.Discount is BuyTwoGetOneFree
                ? $"名称:{Category.Config.Name}, " + $"数量:{Category.BuyTwoGetOneFreeQuantity}{Category.Config.Unit}\n"
                : string.Empty;
    }
}