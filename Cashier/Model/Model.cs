using System.Collections.Generic;
using System.Linq;

namespace Cashier
{
    public class Model
    {
        public List<Category> Categories;
        public Model(List<Item> items, Dictionary<string, Config> configs)
        {
            Categories = items.Select(item => {
                var config = configs[item.Code];
                return new Category(item, config);
            }).ToList();
        }
        public decimal Total => Categories
            .Select(c => c.Subtotal)
            .Aggregate((x, y) => x + y);
        public decimal TotalSaved => Categories
            .Select(c => c.Saved)
            .Aggregate((x, y) => x + y);
    }
}