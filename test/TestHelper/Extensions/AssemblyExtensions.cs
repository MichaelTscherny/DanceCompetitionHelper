using System.Reflection;

namespace TestHelper.Extensions
{
    public static class AssemblyExtensions
    {
        public static string? GetAssemblyPath()
        {
            return Path.GetDirectoryName(
                (Assembly.GetEntryAssembly()
                ?? Assembly.GetExecutingAssembly()
                ?? Assembly.GetCallingAssembly())
                .Location);
        }
    }
}
