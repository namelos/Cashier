using System;

namespace Cashier
{
    public class BuyTwoGetOneFree : IDiscount
    {
        public decimal Discount(decimal price, int quantity) => 
            (quantity - Math.Floor((decimal)quantity / 3)) * price;
    }
}