using System;

namespace CsharpReflection.Infrastructure.Binding
{
    public class NameValueArgument
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public NameValueArgument(string name, string value)
        {
            Name = name ?? throw new ArgumentException(nameof(name));
            Value = value ?? throw new ArgumentException(nameof(value));
        }
    }
}
