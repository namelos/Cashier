using System;
using System.Collections.Generic;
using System.Linq;

namespace Cashier
{
    public class View
    {
        public Model Model { get; set; }
        public List<CategoryView> CategoryViews;
        public View(Model model)
        {
            Model = model;
            CategoryViews = Model.Categories
                .Select(category => new CategoryView(category)).ToList();
        }
        public string Categories => CategoryViews
            .Select(c => c.Show)
            .Aggregate((x, y) => x + y);
        public string Header => "***<没钱赚商店>购物清单***\n";
        public string Splitter => "----------------------\n";
        public string Footer => "**********************\n";
        public string BuyTwoGetOneFreeSummarys => CategoryViews
            .Select(c => c.BuyTwoGetOneFreeSummary)
            .Aggregate((x, y) => x + y);
        public string BuyTwoGetOneFreeSummarysWithSpliter =>
            BuyTwoGetOneFreeSummarys + Splitter;
        public string Total => $"总计:{Model.Total}(元)\n";
        public string TotalSaved => Model.TotalSaved > 0 ? $"节省:{Model.TotalSaved}(元)\n" : string.Empty ;
        public void Render()
        {
            Console.Write(Header + Categories + Splitter +
                          BuyTwoGetOneFreeSummarysWithSpliter + Total + TotalSaved + Footer);
        }
    }
}