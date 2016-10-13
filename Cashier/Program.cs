using System;
using Cashier.Test;

namespace Cashier
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var inputs = Fixture.Input;
            var configs = Fixture.Configs;
            var parsedResult = new Parser(inputs);
            var model = new Model(parsedResult.Items, configs);
            var view = new View(model);
            view.Render();
            Console.ReadLine();
        }
    }
}