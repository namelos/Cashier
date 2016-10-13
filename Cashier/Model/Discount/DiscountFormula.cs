using System.Collections.Generic;

namespace Cashier
{
    public class DiscountFormula
    {
        public IDiscount Discount;
        public DiscountFormula(List<DiscountType> discountTypes, int quantity)
        {
            if (discountTypes.Contains(DiscountType.BuyTwoGetOneFree) &&
                discountTypes.Contains(DiscountType.NintyFivePercentDiscount))
            {
                if (quantity > 2)
                    Discount = new BuyTwoGetOneFree();
                else
                    Discount =  new NinetyFivePercent();
            }
            else if (discountTypes.Contains(DiscountType.BuyTwoGetOneFree))
                Discount = new BuyTwoGetOneFree();
            else if (discountTypes.Contains(DiscountType.NintyFivePercentDiscount))
                Discount = new NinetyFivePercent();
            else
                Discount = new NoDiscount();
        }
    }
}