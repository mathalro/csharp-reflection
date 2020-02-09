using System;

namespace CsharpReflection.Services
{
    public class ExchangeService : IExchangeService
    {
        private readonly Random random = new Random();

        public double GetCurrentExchange(string originCurrency, string targetCurrency, double value) => value * random.NextDouble();
    }
}
