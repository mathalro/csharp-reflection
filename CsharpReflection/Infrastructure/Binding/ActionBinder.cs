using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CsharpReflection.Infrastructure.Binding
{
    public class ActionBinder
    {
        public ActionBinderInfo GetActionBindInfo(object controller, string path)
        {
            var index = path.IndexOf('?');
            var isQueryString = index >= 0;


            if (!isQueryString)
            {
                var pieces = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                var actionName = pieces[1];

                var methodInfo = controller.GetType().GetMethod(actionName);

                return new ActionBinderInfo(methodInfo, Enumerable.Empty<NameValueArgument>());
            }
            else
            {
                var controllerName = path.Substring(0, index);
                var pieces = controllerName.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                var actionName = pieces[1];

                var queryString = path.Substring(index+1);
                var tuples = GetNameValueArguments(queryString);
                var argumentName = tuples.Select(tuple => tuple.Name).ToArray();

                var methodInfo = GetMethodInfoFromArguments(actionName, argumentName, controller);

                return new ActionBinderInfo(methodInfo, tuples); 
            }
        }

        private IEnumerable<NameValueArgument> GetNameValueArguments(string queryString)
        {
            var tuples = queryString.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var tuple in tuples)
            {
                var pieces = tuple.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                yield return new NameValueArgument(pieces[0], pieces[1]);
            }
        }

        private MethodInfo GetMethodInfoFromArguments(string name, string[] arguments, object controller)
        {
            var count = arguments.Length;

            var bindingFlags =
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.Instance |
                BindingFlags.DeclaredOnly;

            var methods = controller.GetType().GetMethods(bindingFlags);

            var overloads = methods.Where(method => method.Name == name);

            foreach (var overload in overloads)
            {
                var parameters = overload.GetParameters();

                if (parameters.Length != count)
                {
                    continue;
                }

                var match = parameters.All(param => arguments.Contains(param.Name));

                if (match)
                {
                    return overload;
                }
            }

            throw new ArgumentException($"A sobrecarga do método {name} não foi encontrada!");
        }
    }
}
