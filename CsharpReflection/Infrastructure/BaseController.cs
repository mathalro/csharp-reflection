using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CsharpReflection.Infrastructure
{
    public abstract class BaseController
    {
        protected string View([CallerMemberName] string filenName = null)
        {
            var type = GetType();
            var directoryName = type.Name.Replace("Controller", "");

            var referenceName = $"CsharpReflection.View.{directoryName}.{filenName}.html";

            var assembly = Assembly.GetExecutingAssembly();
            var fileStream = assembly.GetManifestResourceStream(referenceName);
            var streamReader = new StreamReader(fileStream);

            var content = streamReader.ReadToEnd();

            return content;
        }
    }
}
