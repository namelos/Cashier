using Xunit;
using static Xunit.Assert;

namespace Cashier.Test
{
    public class CategoryTest
    {
        public CategoryTest()
        {
            Category = new Category(Item, Config);
        }

        public Item Item { get; } = Fixture.Item;
        public Config Config { get; } = Fixture.Config;
        public Category Category { get; }

        [Fact]
        public void ShouldCalculateSaved() => Equal(Category.Saved, 1);

        [Fact]
        public void ShouldCalculateSubtotal() => Equal(Category.Subtotal, 4);

        [Fact]
        public void ShouldCalculateSubtotalWithoutDiscount() => Equal(Category.SubtotalWithOutDiscount, 5);
    }
}