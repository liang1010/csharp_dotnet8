using ht_csharp_dotnet8.Attributes;
using ht_csharp_dotnet8.Helpers;

namespace ht_csharp_dotnet8.Extensions
{
    public static class ServiceDependencies
    {
        public static IServiceCollection AddCustomServiceDependencies(this IServiceCollection services)
        {
            var AllTypes = ServiceTypeLocator.GetAllTypes();
            var AllContracts = ServiceTypeLocator.GetAllContracts();
            var ServiceDependencies = AllTypes
                .Where(t => t.GetCustomAttributes(typeof(ServiceDependenciesAttribute), true).FirstOrDefault() != null)
               .Select(t => new KeyValuePair<Type, Type>(
                   AllContracts.FirstOrDefault(c => c.Name == "I" + t.Name) == null
                   ? t
                   : AllContracts.FirstOrDefault(c => c.Name == "I" + t.Name)
                   , t))
               .ToDictionary(e => e.Key, e => e.Value);

            foreach (var keyValue in ServiceDependencies)
            {
                try
                {
                    if (services.Any(x => x.ServiceType.Name == keyValue.Key.Name))
                        continue;

                    if (keyValue.Value == null)
                    {
                        Console.WriteLine("can not find implimentation type for " + keyValue.Key.Name);
                        continue;
                    }

                    if (keyValue.Value.IsAbstract)
                        continue;

                    var serviceDependenciesAttr = keyValue.Value.GetCustomAttributes(typeof(ServiceDependenciesAttribute), true).FirstOrDefault() as ServiceDependenciesAttribute;

                    var InterfaceType = keyValue.Key;
                    var ClassType = keyValue.Value;
                    switch (serviceDependenciesAttr.LifeCycle)
                    {
                        case ServiceDependenciesLifeCycle.Singleton:
                            services.AddSingleton(InterfaceType, ClassType);
                            break;
                        case ServiceDependenciesLifeCycle.Transient:
                            services.AddTransient(InterfaceType, ClassType);
                            break;
                        case ServiceDependenciesLifeCycle.Scoped:
                            services.AddScoped(InterfaceType, ClassType);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return services;
        }
    }
}
