using System.Reflection;
public class DIContext
{
    private readonly HashSet<object> serviceInstances = new HashSet<object>();

    public DIContext(IEnumerable<Type> serviceClasses)
    {
        // create an instance of each service class
        foreach (var serviceClass in serviceClasses)
        {
            var constructor = serviceClass.GetConstructor(Type.EmptyTypes);
            var serviceInstance = constructor.Invoke(null);
            this.serviceInstances.Add(serviceInstance);
        }

        // wire them together
        foreach (var serviceInstance in this.serviceInstances)
        {
            foreach (var field in serviceInstance.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                if (!field.CustomAttributes.Any(x => x.AttributeType == typeof(InjectAttribute)))
                {
                    continue;
                }
                Type fieldType = field.FieldType;
                // find a suitable matching service instance
                foreach (var matchPartner in this.serviceInstances)
                {
                    if (fieldType.IsAssignableFrom(matchPartner.GetType()))
                    {
                        field.SetValue(serviceInstance, matchPartner);
                    }
                }
            }
        }
    }

    public T GetServiceInstance<T>(Type type)
    {
        foreach (var serviceInstance in this.serviceInstances)
        {
            if (serviceInstance is T)
            {
                return (T)serviceInstance;
            }
        }
        return default;
    }
    public static DIContext CreateContextForPackage(string rootPackageName)
    {
        var allClassesInPackage = ClassPathScanner.GetAllClassesInPackage(rootPackageName);
        var serviceClasses = new HashSet<Type>();
        foreach (var aClass in allClassesInPackage)
        {
            if (!aClass.IsDefined(typeof(ServiceAttribute), false))
            {
                continue;
            }
            serviceClasses.Add(aClass);
        }
        return new DIContext(serviceClasses);
    }
}

