using System.Reflection;

var context = CreateContextForPackage();
DoBussinessLogic(context);

static DIContext CreateContext()
{
    var serviceClasses = new HashSet<Type>();
    serviceClasses.Add(typeof(ServiceA));
    serviceClasses.Add(typeof(ServiceB));
    return new DIContext(serviceClasses);
}
static DIContext CreateContextForPackage()
{
    var rootPackageName = Assembly.GetExecutingAssembly().GetName().Name;
    return DIContext.CreateContextForPackage(rootPackageName);
}
static void DoBussinessLogic(DIContext context)
{
    var a = context.GetServiceInstance<ServiceA>(typeof(ServiceA));
    var b = context.GetServiceInstance<ServiceB>(typeof(ServiceB));
    Console.WriteLine(a.JobA());
    Console.WriteLine(b.JobB());
}