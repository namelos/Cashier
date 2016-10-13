using System.Linq;

namespace Cashier
{
    public class ItemParser
    {
        public ItemParser(string inputItem)
        {
            var pair = inputItem.Split('-').ToList();
            Code = pair[0];
            Quantity = pair.Count == 1 ? 1 : int.Parse(pair[1]);
        }

        public string Code { get; set; }
        public int Quantity { get; set; }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }
    }
}