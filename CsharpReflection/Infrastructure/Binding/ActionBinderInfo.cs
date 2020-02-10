using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CsharpReflection.Infrastructure.Binding
{
    public class ActionBinderInfo
    {
        public MethodInfo MethodInfo { get; private set; }
        public IReadOnlyCollection<NameValueArgument> TuplesNameValueArgument { get; private set; }

        public ActionBinderInfo(MethodInfo methodInfo, IEnumerable<NameValueArgument> nameValueArguments)
        {
            MethodInfo = methodInfo ?? throw new ArgumentException(nameof(methodInfo));

            if (nameValueArguments == null)
            {
                throw new ArgumentException(nameof(nameValueArguments));
            }

            TuplesNameValueArgument = new ReadOnlyCollection<NameValueArgument>(nameValueArguments.ToList());
        }

        public object Invoke(object controller)
        {
            var count = TuplesNameValueArgument.Count;
            var isEmptyArgument = count == 0;

            if (isEmptyArgument)
            {
                return MethodInfo.Invoke(controller, new object[0]);
            }

            var parametersInvoke = new object[count];

            var parametersMethodInfo = MethodInfo.GetParameters();

            for (int i = 0; i < count; i++)
            {
                var parameter = parametersMethodInfo[i];
                var parameterName = parameter.Name;

                var argument = TuplesNameValueArgument.Single(tuple => tuple.Name == parameterName);
                parametersInvoke[i] = Convert.ChangeType(argument.Value, parameter.ParameterType);
            }

            return MethodInfo.Invoke(controller, parametersInvoke);
        }
    }
}
