namespace Cashier
{
    public interface IDiscount
    {
        decimal Discount(decimal price, int quantity);
    }
}