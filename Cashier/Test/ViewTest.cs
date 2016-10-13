using Xunit;
using static Xunit.Assert;

namespace Cashier.Test
{
    public class ViewTest
    {
        public class CategoryViewTest
        {
            public CategoryViewTest()
            {
                Category = new Category(Fixture.Item, Fixture.Config);
                CategoryView = new CategoryView(Category);
            }

            public Category Category { get; set; }
            public CategoryView CategoryView { get; set; }

            [Fact]
            public void ShouldShowCategory()
            {
                Equal(CategoryView.Show,
                    $"名称:{Category.Config.Name}, 数量:{Category.Item.Quantity}{Category.Config.Unit}, 单价:{Category.Config.Price}(元), 小计:{Category.Subtotal}(元)\n");
            }

            [Fact]
            public void ShouldShowCategoryWithNinetyFivePercentDiscount()
            {
                var category = new Category(Fixture.Item, Fixture.ConfigWithNinetyFiveDiscount);
                var categoryView = new CategoryView(category);
                Equal(categoryView.Show, $"名称:{category.Config.Name}, " +
                                         $"数量:{category.Item.Quantity}{category.Config.Unit}, " +
                                         $"单价:{category.Config.Price}(元), " +
                                         $"小计:{category.Subtotal}(元), " +
                                         $"节省:{category.Saved}(元)\n");
            }
        }
    }
}