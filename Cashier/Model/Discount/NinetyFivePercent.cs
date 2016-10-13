namespace Cashier
{
    public class NinetyFivePercent : IDiscount
    {
        public decimal Discount(decimal price, int quantity) =>
            0.95m*price*quantity;
    }
}