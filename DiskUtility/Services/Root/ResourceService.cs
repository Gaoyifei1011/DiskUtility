using System.Reflection;
using System.Resources;

namespace DiskUtility.Services.Root
{
    /// <summary>
    /// 应用资源服务
    /// </summary>
    public static class ResourceService
    {
        private static Assembly CurrentAssembly { get; } = Assembly.GetExecutingAssembly();

        public static ResourceManager WindowResource { get; } = new("DiskUtility.Strings.Window", CurrentAssembly);
    }
}
