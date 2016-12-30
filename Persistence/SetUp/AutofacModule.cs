using Autofac;

namespace Persistence.SetUp
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Repository<>)).AsImplementedInterfaces();
        }
    }
}
