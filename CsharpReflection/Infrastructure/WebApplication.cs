using CsharpReflection.Controllers;
using System;
using System.Net;
using System.Reflection;
using System.Text;

namespace CsharpReflection.Infrastructure
{
    public class WebApplication
    {
        private readonly string[] _prefixes;

        public WebApplication(string[] prefixes)
        {
            _prefixes = prefixes ?? throw new ArgumentException(nameof(prefixes));
        }

        public void Init()
        {
            while (true)
                HandleRequest();
        }

        private void HandleRequest()
        {
            var httpListener = new HttpListener();

            foreach (var p in _prefixes)
            {
                httpListener.Prefixes.Add(p);
            }

            httpListener.Start();

            var context = httpListener.GetContext();

            var request = context.Request;
            var response = context.Response;

            var path = request.Url.AbsolutePath;

            if (Utilities.IsFile(path))
            {
                var fileHandler = new RequestFileHandler();
                fileHandler.Handle(response, path);
            }
            else
            {
                var handler = new RequestControllerHandler();
                handler.Handle(response, path);
            }
            
            httpListener.Stop();
        }
    }
}
