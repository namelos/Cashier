using System.Linq;
using Xunit;
using static Xunit.Assert;

namespace Cashier.Test
{
    public class ParserTest
    {
        public class ItemParserTest
        {
            [Fact]
            public void ShouldParsePluralItemInput()
            {
                var parsed = new ItemParser(Fixture.PluralItemInput);
                Equal(parsed.Code, "ITEM00003");
                Equal(parsed.Quantity, 2);
            }

            [Fact]
            public void ShouldParseSingularItemInput()
            {
                var parsed = new ItemParser(Fixture.SingularItemInput);
                Equal(parsed.Code, "ITEM00001");
                Equal(parsed.Quantity, 1);
            }
        }

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
    }
}