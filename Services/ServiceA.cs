public interface IServiceA
{
    string JobA();
    IServiceB GetServiceB();
    void SetServiceB(IServiceB serviceB);


}
[Service]
public class ServiceA : IServiceA
{
    [Inject]
    private IServiceB serviceB;
    public IServiceB GetServiceB()
    {
        return serviceB;
    }
    public string JobA()
    {
        return $"JobA({serviceB.JobB()})";
    }
    public void SetServiceB(IServiceB serviceB)
    {
        this.serviceB = serviceB;
    }
}