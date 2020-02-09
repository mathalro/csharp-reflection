namespace CsharpReflection.Services
{
    public interface IExchangeService
    {
        double GetCurrentExchange(string originCurrency, string targetCurrency, double value);
    }
}
