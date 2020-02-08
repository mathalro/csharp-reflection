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

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = Utilities.ConvertPathToAssemblyName(path);

            var resourceStream = assembly.GetManifestResourceStream(resourceName);
            
            if (resourceStream == null)
            {
                response.StatusCode = 404;
                response.OutputStream.Close();
            }
            else
            {
                var resourceBytes = new byte[resourceStream.Length];

                resourceStream.Read(resourceBytes, 0, (int)resourceStream.Length);

                response.ContentType = Utilities.GetContentExtension(path);
                response.StatusCode = 200;
                response.ContentLength64 = resourceBytes.Length;

                response.OutputStream.Write(resourceBytes, 0, resourceBytes.Length);
                response.OutputStream.Close();
            
            }
            
            httpListener.Stop();
        }
    }
}
