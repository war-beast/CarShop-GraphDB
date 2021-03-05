using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using CarShop.BLL.Interfaces;

namespace CarShop.Extensions
{
	public static class ConfigureServicesExtension
	{
		public static void RegisterAllTypes<T>(this IServiceCollection services, Assembly[] assemblies,
			ServiceLifetime lifetime = ServiceLifetime.Singleton)
		{
			var types = assemblies.SelectMany(a => a.DefinedTypes.Where(t => t.GetInterfaces().Contains(typeof(T)))).ToArray();
			var interfaces = types.Where(t => t.IsInterface).ToArray();

			foreach (var cl in types.Where(t => t.IsClass))
			{
				var inter = interfaces.FirstOrDefault(i => cl.ImplementedInterfaces.Contains(i));
				if (inter != null)
				{
					services.Add(new ServiceDescriptor(inter.AsType(), cl.AsType(), lifetime));
				}
			}
		}

		public static IServiceCollection AddServices(
			this IServiceCollection services,
			ServiceLifetime lifetime = ServiceLifetime.Singleton)
		{
			services.RegisterAllTypes<IService>(new[]
			{
				typeof(IService).Assembly
			}, lifetime);

			return services;
		}
	}
}
