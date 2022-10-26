namespace Hermes.Models;

public interface IService
{
    public string ServiceName { get; }
    public Task InitializeAsync();
}
