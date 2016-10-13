using System.Collections.Generic;
using System.Linq;

namespace Cashier
{
    public class Parser
    {
        public List<ItemParser> ParsedItems;

        public Parser(IEnumerable<string> inputs)
        {
            ParsedItems = inputs
                .Select(input => new ItemParser(input))
                .GroupBy(parsedItem => parsedItem.Code)
                .Select(groupedItem =>
                {
                    return groupedItem.Aggregate((item1, item2) =>
                    {
                        item1.AddQuantity(item2.Quantity);
                        return item1;
                    });
                }).ToList();
        }

        public List<Item> Items => ParsedItems
            .Select(parsedItem => new Item(parsedItem.Code, parsedItem.Quantity)).ToList();
    }
}