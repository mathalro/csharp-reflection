using System;
using System.Linq;

namespace CsharpReflection.Infrastructure
{
    public static class Utilities
    {
        public static bool IsFile(string path)
        {
            var pieces = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var lastPiece = pieces.Last();

            return lastPiece.Contains('.');
        }

        public static string ConvertPathToAssemblyName(string path)
        {
            var dotPaht = path.Replace('/', '.');
            var prefixAssembly = "CsharpReflection";

            var completeName = $"{prefixAssembly}{dotPaht}";

            return completeName;
        }

        public static string GetContentExtension(string path)
        {
            if (path.EndsWith(".css"))
            {
                return "text/css; charset=utf-8";
            }

            if (path.EndsWith(".js"))
            {
                return "application/js; charset=utf-8";
            }

            if (path.EndsWith(".html"))
            {
                return "text/html; charset=utf-8";
            }

            return "Invalid content";
        }
    }
}
