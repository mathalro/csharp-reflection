using CsharpReflection.Services;
using System.IO;
using System.Reflection;

namespace CsharpReflection.Controllers
{
    public class ExchangeController
    {
        private IExchangeService _exchangeService;

        public ExchangeController()
        {
            _exchangeService = new ExchangeService();
        }

        public string MXN()
        {
            var finalValue = _exchangeService.GetCurrentExchange("MXN", "BRL", 1);
            var referenceName = "CsharpReflection.View.Cambio.MXN.html";

            var assembly = Assembly.GetExecutingAssembly();
            var fileStream = assembly.GetManifestResourceStream(referenceName);
            var streamReader = new StreamReader(fileStream);

            var content = streamReader.ReadToEnd();

            var resultText = content.Replace("{{value}}", finalValue.ToString());

            return resultText;
        }

        public string USD()
        {
            return string.Empty;
        }
    }
}
