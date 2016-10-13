namespace Cashier
{
    public class Item
    {
        public Item(string code, int quantity)
        {
            Code = code;
            Quantity = quantity;
        }

        public string Code { get; }
        public int Quantity { get; }
    }
}