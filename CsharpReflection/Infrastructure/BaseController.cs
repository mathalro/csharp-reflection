using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

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

        protected string View(object model, [CallerMemberName] string fileName = null)
        {
            var normalView = View(fileName);
            var allProperties = model.GetType().GetProperties();

            var regex = new Regex("\\{{(.*?)\\}}");

            var processedView = regex.Replace(normalView, (match) => 
            {
                var propertie = match.Groups[1].Value;

                var prop = allProperties.Single(prop => prop.Name.Equals(propertie));

                return prop.GetValue(model)?.ToString();
            });

            return processedView;
        }
    }
}
