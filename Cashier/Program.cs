using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashier
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    public class Item
    {
        public string Code { get; set; }
        public int Amount { get; set; }
    }

    public class Config
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public List<Discount> Discounts { get; set; }
    }

    public enum Discount
    {
        BuyTwoGetOneFree,
        NintyFivePercentDiscount
    }
}
