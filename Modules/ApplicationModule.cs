using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EgosaToolAPI.Models.Db;
using Microsoft.Extensions.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;


namespace EgosaToolAPI.Modules
{
    public class ApplicationModule : Module
    {
        private readonly IConfiguration _configuration = null;

        public ApplicationModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ApplicationDbContext>()
                .WithParameter("options", ApplicationDbContextOptionsFactory.Get(_configuration))
                .InstancePerLifetimeScope();
        }
    }
}
