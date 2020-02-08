using CsharpReflection.Infrastructure;
using System;

namespace CsharpReflection
{
    class Program
    {
        static void Main(string[] args)
        {
            var prefixes = new string[]
            {
                "http://localhost:5341/"
            };

            var webApplication = new WebApplication(prefixes);

            webApplication.Init();
        }
    }
}
