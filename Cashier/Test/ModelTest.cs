using System.Collections.Generic;
using Xunit;
using static Xunit.Assert;

namespace Cashier.Test
{
    public class ModelTest
    {
        public ModelTest()
        {
            Model = new Model(Items, Configs);
        }

        public Model Model { get; }
        public List<Item> Items { get; } = Fixture.Items;
        public Dictionary<string, Config> Configs { get; } = Fixture.Configs;

        [Fact]
        public void ShouldCalculateTotal() => Equal(Model.Total, 21);

        [Fact]
        public void ShouldCalculateTotalSaved() => Equal(Model.TotalSaved, 4);

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
    }
}