using Xunit;

namespace FiapConnect.Tests.Integration
{
    // Define a coleção de testes de integração
    [CollectionDefinition("IntegrationTests")]
    public class ApiTestCollection : ICollectionFixture<WebAppFixture>
    {
    }
}