using Autofac;
using EgosaToolAPI.Models.Chatwork;
using EgosaToolAPI.Models.Db;
using EgosaToolAPI.Models.Twitter;
using Microsoft.Extensions.Configuration;


namespace EgosaToolAPI.Modules
{
    public class ApplicationModule : Module
    {
        private readonly IConfiguration config = null;

        public ApplicationModule(IConfiguration config)
        {
            this.config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ApplicationDbContext>()
                .WithParameter("options", ApplicationDbContextOptionsFactory.Get(config))
                .InstancePerLifetimeScope();

            // TODO: scopeが適切か確認
            builder
                .RegisterType<TwitterApiClient>()
                .WithParameter("config", config)
                .SingleInstance();

            // TODO: scopeが適切か確認
            builder
                .RegisterType<ChatworkApiClient>()
                .WithParameter("config", config)
                .SingleInstance();
        }
    }
}
