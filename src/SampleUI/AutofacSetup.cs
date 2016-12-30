using System.Reflection;
using System.Reflection.Emit;
using Autofac;
using Persistence.Azure;

namespace SampleUI
{
    internal static class AutofacSetup
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(Assembly.Load(new AssemblyName("Persistence.Azure")), 
                Assembly.Load(new AssemblyName("Persistence")));
            builder.RegisterType<TableStorageEventStoreConfig>().As<ITableStorageEventStoreConfig>();
            return builder.Build();
        }
    }
}
