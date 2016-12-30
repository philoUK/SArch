using Autofac;

namespace Persistence.Azure.SetUp
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TableStorageEventStore>().AsImplementedInterfaces();
        }
    }
}
