namespace Cashier
{
    public class NoDiscount : IDiscount
    {
        public decimal Discount(decimal price, int quantity) =>
            price*quantity;
    }
}