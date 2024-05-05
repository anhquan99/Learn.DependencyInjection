using System.Reflection;

public static class Helper
{
    public static ServiceA CreateServiceA(HashSet<Type> serviceClasses)
    {
        HashSet<Object> serviceInstances = new HashSet<Object>();
        foreach (var serviceClass in serviceClasses)
        {
            ConstructorInfo constructor = serviceClass.GetConstructor(Type.EmptyTypes);
            serviceInstances.Add(constructor.Invoke(null));
        }
        foreach (var serviceInstance in serviceInstances)
        {
            foreach (var field in serviceInstance.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                Type fieldType = field.FieldType;
                foreach (object matchPartner in serviceInstances)
                {
                    if (fieldType.IsAssignableFrom(matchPartner.GetType()))
                    {
                        field.SetValue(serviceInstance, matchPartner);
                    }
                }
            }
        }
        foreach (object serviceInstance in serviceInstances)
        {
            if (serviceInstance is ServiceA)
            {
                return (ServiceA)serviceInstance;
            }
        }
        return null;
    }
}