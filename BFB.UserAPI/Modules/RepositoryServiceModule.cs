using Autofac;
using BFB.Core.Repositories;
using BFB.Core.Services;
using BFB.Core.UnitOfWorks;
using BFB.Repository;
using BFB.Repository.Repositories;
using BFB.Repository.UnitOfWorks;
using BFB.Service;
using BFB.Service.Services;
using BFB.UserAPI.Filters;
using System.Reflection;

namespace BFB.UserAPI.Modules
{
	public class RepositoryServiceModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			var apiAssembly = Assembly.GetExecutingAssembly();
			var repoAssembly = Assembly.GetAssembly(typeof(AppDbContext));
			var serviceAssembly = Assembly.GetAssembly(typeof(Service<>));
			builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
			builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();
			builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
			builder.RegisterType<EmailService>().As<IEmailService>().InstancePerLifetimeScope();
			builder.RegisterType(typeof(IsCreatedProfileFilterAttribute));
			builder.RegisterGeneric(typeof(NotFoundFilter<>));
			builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
			builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, serviceAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
		}
	}
}
