public interface IServiceB
{
    string JobB();
    void SetServiceA(IServiceA serviceA);
    IServiceA GetServiceA();
}
[Service]
public class ServiceB : IServiceB
{
    [Inject]
    private IServiceA serviceA;
    public IServiceA GetServiceA()
    {
        return serviceA;
    }
    public string JobB()
    {
        return "JobB()";
    }
    public void SetServiceA(IServiceA serviceA)
    {
        this.serviceA = serviceA;
    }
}