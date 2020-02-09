using System.Net;
using System.Reflection;

namespace CsharpReflection.Infrastructure
{
    public class RequestFileHandler
    {
        public void Handle(HttpListenerResponse response, string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = Utilities.ConvertPathToAssemblyName(path);

            using var resourceStream = assembly.GetManifestResourceStream(resourceName);

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
        }
    }
}
